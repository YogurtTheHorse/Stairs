using System;
using UnityEngine;
using UnityEngine.Events;

public class Stair : MonoBehaviour
{
    public Stair nextStair;

    public UnityEvent onHide;

    private GameObject _gameObject;

    private void Awake()
    {
        _gameObject = gameObject;
        onHide ??= new UnityEvent();
        
    }

    public void Hide()
    {
        _gameObject.SetActive(false);
        onHide.Invoke();
        
        foreach (var obstacle in GetComponentsInChildren<Obstacle>())
        {
            obstacle.InstantHide();
        }
    }

    public void Activate()
    {
        _gameObject.SetActive(true);
    }
}