using System;
using Cngine;
using Flappy;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class FlappyTiledBackgroundBehaviour : PoolObject
{    
    private FlappyManager FlappyManager => FlappyManager.Instance;
    private FlappyScoreManager _flappyScoreManager;
    private FlappyScoreManager FlappyScoreManager => _flappyScoreManager ??= GameMaster.FlappyScoreManager;
    private FlappyGameplayConfig _flappyGameplayConfig;
    private FlappyGameplayConfig FlappyGameplayConfig => _flappyGameplayConfig ??= MainConfig.FlappyGameplayConfig;

    [SerializeField] private PotaTween _potaTween;
    [SerializeField] private SpriteRenderer _sprite;
    public float GetObjectWidth()
    {
        return _sprite.sprite.bounds.size.x;
    }

    private void OnEnable()
    {
        EventManager.OnScoreChanged += SetupColor;
        SetupColor();
    }
    
    private void OnDisable()
    {
        EventManager.OnScoreChanged -= SetupColor;
    }

    private void SetupColor()
    {
        if (_sprite == null)
        {
            return;
        }
        
        _potaTween.Stop();
        _potaTween.Color.From =_sprite.color;
        _potaTween.Color.To = FlappyScoreManager.GetBackgroundColorOfCurrentScore();
        _potaTween.Play();
    }
    
    protected override void StartRemovalAnimation() { }
    protected override void StartSpawnAnimation() { }
}
