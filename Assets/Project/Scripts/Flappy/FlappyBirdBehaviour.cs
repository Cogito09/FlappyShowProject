﻿using Cngine;
using Flappy;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FlappyBirdBehaviour : MonoBehaviour
{
    [SerializeField] private float _gravitySpeed;
    [SerializeField] private float _forceMultiplier; 
    [SerializeField] private AnimationCurve _bumpCurve;
    [SerializeField] private Animator _animator;
    private float _lastBumpTimestamp;
    private Vector3 _lastPosition;

    private void Awake()
    {
        EventManager.OnFlappyRoundReseted += Reset;
        EventManager.OnFlappyRoundStarted -= StartAnimation;
        EventManager.OnFlappyRoundFinished -= StopAnimation;
    }
    
    private void OnDestroy()
    {
        EventManager.OnFlappyRoundReseted -= Reset;
        EventManager.OnFlappyRoundStarted -= StartAnimation;
        EventManager.OnFlappyRoundFinished -= StopAnimation;
    }
    
    private void Update()
    {
        if (FlappyManager.Instance == null || FlappyManager.Instance.IsPlaying == false)
        {
            return;
        }
        
        ReadInput();
        PreformMovement();
        SimulateGravity();
    }

    private void ReadInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Bump();
        }
    }

    private void PreformMovement()
    {
        var bumpTime = Time.time - _lastBumpTimestamp;
        var force =  _bumpCurve.Evaluate(bumpTime) * _forceMultiplier;

        transform.position = new Vector3(transform.position.x, transform.position.y + force, transform.position.z);
    }

    private void SimulateGravity()
    {
        transform.position = new Vector3(transform.localPosition.x, transform.position.y - _gravitySpeed, transform.position.z); 
    }
    
    private void Bump()
    {
        _lastBumpTimestamp = Time.time;
    }

    public void Reset()
    {
        transform.position = Vector3.zero;
        _lastPosition = Vector3.zero;
    }
    
    private void StopAnimation()
    {
        _animator.StartPlayback();
    }
    private void StartAnimation()
    {
        _animator.StopPlayback();
    }
}
