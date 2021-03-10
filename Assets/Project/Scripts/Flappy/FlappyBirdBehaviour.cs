using Cngine;
using Flappy;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FlappyBirdBehaviour : MonoBehaviour
{
    private FlappyGameplayConfig _flappyGameplayConfig;
    private FlappyGameplayConfig FlappyGameplayConfig => _flappyGameplayConfig ??= MainConfig.FlappyGameplayConfig;
    [SerializeField] private string StopAnimatonTrigger;
    [SerializeField] private string StartAnimatonTrigger;
    [SerializeField] private float _gravityForce;
    [SerializeField] private float _jumpForceMultiplier; 
    [SerializeField] private float _jumpCurveDuration;
    [SerializeField] private AnimationCurve _jumpCurve;
    [SerializeField] private Animator _animator;
    private Vector3 BirdStartPosition => FlappyGameplayConfig.BridStartPosition;
    private float GravityForce => _gravityForce * Time.deltaTime * -1;
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
        var timePassedSinceTap = Time.time - _lastBumpTimestamp;;

        var bumpTime = timePassedSinceTap / _jumpCurveDuration;
        var upForce =  _jumpCurve.Evaluate(bumpTime) * _jumpForceMultiplier;

        transform.Translate(0, upForce, 0);
    }

    private void SimulateGravity()
    {
        transform.Translate(0,GravityForce,0);
    }
    
    private void Bump()
    {
        _lastBumpTimestamp = Time.time;
    }

    public void Reset()
    {
        _animator.StopPlayback();
        transform.position = BirdStartPosition;
        _lastPosition = Vector3.zero;
    }
    
    private void StopAnimation()
    {
        _animator.SetTrigger(StopAnimatonTrigger);
    }
    
    private void StartAnimation()
    {
        _animator.SetTrigger(StartAnimatonTrigger);
    }
}
