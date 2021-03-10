using Cngine;
using Flappy;
using UnityEngine;

public class FlappyTiledObjectBehaviour : PoolObject
{    
    private FlappyManager FlappyManager => FlappyManager.Instance;
    private FlappyGameplayConfig _flappyGameplayConfig;
    private FlappyGameplayConfig FlappyGameplayConfig => _flappyGameplayConfig ??= MainConfig.FlappyGameplayConfig;

    [SerializeField] private SpriteRenderer _sprite;
    public float GetObjectWidth()
    {
        return _sprite.sprite.bounds.size.x;
    }
    
    protected override void StartRemovalAnimation() { }
    protected override void StartSpawnAnimation() { }
}
