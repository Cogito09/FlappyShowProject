using Cngine;
using Flappy;
using UnityEngine;

public class FlappyObstacleBehaviour : PoolObject
{
    [SerializeField] private SpriteRenderer _sprite;
    public float GetObjectWidth()
    {
        return _sprite.size.x;
    }
    
    private FlappyGameplayConfig _flappyGameplayConfig;
    private FlappyGameplayConfig FlappyGameplayConfig => _flappyGameplayConfig ??= MainConfig.FlappyGameplayConfig;
    private FlappyManager _flappyManager;
    private FlappyManager FlappyManager => _flappyManager ??= FlappyManager.Instance;
    
    private float? _moveSpeed ;
    private float MoveSpeed => _moveSpeed ??= FlappyGameplayConfig.WorldMovementSpeed;

    private void FixedUpdate()
    {
        if (FlappyManager.Instance.IsPlaying == false)
        {
            return;
        }
        
        transform.position = new Vector2(transform.localPosition.x - MoveSpeed, transform.position.y);
    }


    protected override void StartRemovalAnimation()
    {
        var obj = GameMaster.PoolManager.SpawnObject(FlappyPrefabType.ObstacleRemovalAnimation);
        obj.transform.position = transform.position;
    }

    protected override void StartSpawnAnimation()
    {
    }

    public void Setup()
    {
        .CurrentScoreData;
    }
}
