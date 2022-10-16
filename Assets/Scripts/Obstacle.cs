using System;
using Configs;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour
{
    public ObstaclesConfig obstacleConfig;
    public StairsConfig stairsConfig;

    public UnityEvent onHide;

    private bool _hidden;

    private Transform _transform;
    private Sequence _jumpTween;
    private Sequence _ttlDeath;

    public void Awake()
    {
        onHide ??= new UnityEvent();

        _transform = transform;
    }

    public void Activate(Stair stair)
    {
        var stairTransform = stair.transform;
        
        _hidden = false;
        gameObject.SetActive(true);

        var pos = new Vector3(0, 0, Random.Range(-stairsConfig.stairWidth, stairsConfig.stairWidth) * 0.5f);
        
        _transform.localScale = Vector3.one;
        _transform.parent = stairTransform;
        _transform.localPosition = pos;

        _ttlDeath = DOTween.Sequence().SetDelay(obstacleConfig.timeToLive).OnComplete(Hide);

        Jump();
    }

    private void Jump()
    {
        var jumpPos = -obstacleConfig.stepsInJump * (Vector3)stairsConfig.stepSize;

        if (_jumpTween.IsActive())
        {
            _jumpTween.Kill();
        }
        
        _jumpTween = _transform
            .DOLocalJump(
                jumpPos,
                obstacleConfig.jumpPower,
                1,
                obstacleConfig.jumpDuration
            )
            .SetRelative(true)
            .OnComplete(Jump);
    }

    public void Hide()
    {
        if (_hidden)
            return;
        
        _hidden = true;
        _jumpTween.Kill();
        _ttlDeath.Kill();

        _transform
            .DOScale(Vector3.zero, obstacleConfig.hidingDuration)
            .SetEase(obstacleConfig.hidingEase)
            .OnComplete(DestroySelf);
    }

    public void InstantHide()
    {
        _hidden = true;
        _jumpTween.Kill();
        _ttlDeath.Kill();
        DestroySelf();
    }

    public void DestroySelf()
    {
        gameObject.SetActive(false);

        onHide.Invoke();
    }
}