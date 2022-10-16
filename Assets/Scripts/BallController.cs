using System;
using Configs;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class BallController : MonoBehaviour
{
    public BallConfig ballConfig;

    public Transform view;

    public Stair currentStair;

    public UnityEvent ballAtZenith;
    public UnityEvent jumpEnded;
    public UnityEvent onDeath;

    public bool isFalling;
    public bool isSideJumping;

    private bool hasDied;

    private Sequence _jumpTween;
    private Transform _transform;
    private AudioSource _audioSource;

    public bool IsJumping => _jumpTween.IsActive() && _jumpTween.IsPlaying();

    private void Awake()
    {
        _transform = transform;

        _audioSource = GetComponent<AudioSource>();

        onDeath ??= new UnityEvent();
        ballAtZenith ??= new UnityEvent();
        jumpEnded ??= new UnityEvent();
    }

    private void Start()
    {
        view.localScale = 2 * ballConfig.ballRadius * Vector3.one;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (hasDied) 
            return;
        
        if (!collision.gameObject.CompareTag("Obstacle"))
            return;

        if (collision.gameObject.TryGetComponent<Obstacle>(out var obstacle))
        {
            obstacle.InstantHide();
        } 
            
        FinishGame();
    }

    public void SideJump(float direction)
    {
        _audioSource.PlayOneShot(ballConfig.jump);
        
        var zDelta = direction * ballConfig.sideJumpLength;
        var halfDuration = 0.5f * ballConfig.sideJumpLength / ballConfig.sideJumpSpeed;

        isSideJumping = true;

        DOTween
            .Sequence()
            .Append(
                DOTween
                    .Sequence(view)
                    .Join(
                        view.DOLocalMoveY(ballConfig.sideJumpHeight, halfDuration).SetEase(ballConfig.jumpEase)
                    )
                    .Append(
                        view.DOLocalMoveY(-ballConfig.sideJumpHeight, halfDuration).SetEase(ballConfig.fallEase)
                    )
            )
            .Join(
                _transform
                    .DOMoveZ(zDelta, halfDuration * 2f)
                    .SetEase(Ease.Linear)
            )
            .SetRelative(true)
            .OnComplete(() => isSideJumping = false);
    }

    public void JumpForward()
    {
        _audioSource.PlayOneShot(ballConfig.jump);
        
        var currentStairPos = currentStair.transform.position;
        var nextStairPos = currentStair.nextStair.transform.position;

        var zenithPoint = new Vector3(
            (currentStairPos.x + nextStairPos.x) * 0.5f,
            nextStairPos.y + ballConfig.jumpHeight + ballConfig.ballRadius
        );

        var fallingPoint = new Vector3(
            nextStairPos.x,
            nextStairPos.y + ballConfig.ballRadius
        );

        var fallingDuration = (fallingPoint.x - zenithPoint.x) / ballConfig.ballAverageSpeed;
        var durationToZenith = (zenithPoint.x - _transform.position.x) / ballConfig.ballAverageSpeed;

        _jumpTween.Kill();

        isFalling = false;

        _jumpTween = DOTween
            .Sequence(_transform)
            .Append(
                _transform.DOMoveX(zenithPoint.x, durationToZenith).SetEase(Ease.Linear).OnComplete(AtZenith)
            )
            .Join(
                _transform.DOMoveY(zenithPoint.y, durationToZenith).SetEase(ballConfig.jumpEase)
            )
            // after zenith we go down
            .Append(
                _transform.DOMoveX(fallingPoint.x, fallingDuration).SetEase(Ease.Linear)
            )
            .Join(
                _transform.DOMoveY(fallingPoint.y, durationToZenith).SetEase(ballConfig.fallEase)
            )
            .OnComplete(JumpCompleted);
    }

    private void FinishGame()
    {
        hasDied = true;
        
        _transform.DOKill();
        view.DOKill();
        
        Instantiate(ballConfig.destroyEffect, _transform.position, Quaternion.identity);

        Destroy(gameObject);
        onDeath.Invoke();
    }

    private void JumpCompleted()
    {
        isFalling = false;
        jumpEnded.Invoke();
    }

    private void AtZenith()
    {
        currentStair = currentStair.nextStair;
        isFalling = true;

        ballAtZenith.Invoke();
    }
}