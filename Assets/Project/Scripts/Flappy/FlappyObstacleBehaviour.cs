using Cngine;
using Flappy;
using UnityEngine;

public class FlappyObstacleBehaviour : PoolObject
{
    [SerializeField] private SpriteRenderer _upperSprite;
    [SerializeField] private SpriteRenderer _downSprite;
    
    private FlappyGameplayConfig _flappyGameplayConfig;
    private FlappyGameplayConfig FlappyGameplayConfig => _flappyGameplayConfig ??= MainConfig.FlappyGameplayConfig;
    private FlappyObstaclesConfig _flappyObstaclesConfig;
    private FlappyObstaclesConfig FlappyObstaclesConfig => _flappyObstaclesConfig ??= MainConfig.FlappyObstaclesConfig;
    
    private FlappyObstaclesConfig.ObstacleConfig _obstacleConfig;
    private FlappyObstaclesConfig.ObstacleConfig ObstacleConfig => _obstacleConfig ??= FlappyGameplayConfig.GetObstacleTypeByScore(GameMaster.FlappyScoreManager.CurrentScoreData);
    private FlappyManager _flappyManager;
    private FlappyManager FlappyManager => _flappyManager ??= FlappyManager.Instance;
    
    private float? _moveSpeed ;
    private float MoveSpeed => _moveSpeed ??= FlappyGameplayConfig.WorldMovementSpeed;

    private void Update()
    {
        if (FlappyManager.Instance.IsPlaying == false)
        {
            return;
        }
        
        transform.position = new Vector2(transform.localPosition.x - MoveSpeed, transform.position.y);
    }

    public void Setup()
    {
        SetupSprites();
        SetupPosition();
    }

    private void SetupPosition()
    {
        var YpositionRange = FlappyGameplayConfig.RandomizeYPositionForObstacle();
        transform.position = new Vector3(transform.position.x, YpositionRange, transform.position.z);
    }

    private void SetupSprites()
    {
        var sprite = ObstacleConfig.Sprite;
        var color = ObstacleConfig.RandomizeColor();
        
        _upperSprite.sprite = sprite;
        _upperSprite.color = color;
        
        _downSprite.sprite = sprite;
        _downSprite.color = color;
    }
    protected override void StartRemovalAnimation()
    {
        var obj = GameMaster.PoolManager.SpawnObject(FlappyPrefabType.ObstacleRemovalAnimation);
        obj.transform.position = transform.position;
    }

    protected override void StartSpawnAnimation()
    {
    }

    public override void OnSpawned()
    {
        _obstacleConfig = null;
        base.OnSpawned();

    }

    public override void Remove(bool instant = false)
    {
        _obstacleConfig = null;
        base.Remove(instant);
    }


}
