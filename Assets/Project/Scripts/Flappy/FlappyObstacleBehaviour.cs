using Cngine;
using Flappy;
using UnityEngine;

public class FlappyObstacleBehaviour : PoolObject
{
    [SerializeField] private SpriteRenderer _upperSprite;
    [SerializeField] private SpriteRenderer _downSprite;
    
    private FlappyGameplayConfig _flappyGameplayConfig;
    private FlappyGameplayConfig FlappyGameplayConfig => _flappyGameplayConfig ??= MainConfig.FlappyGameplayConfig;
    private FlappyObstaclesConfig.ObstacleConfig _obstacleConfig;
    private FlappyObstaclesConfig.ObstacleConfig ObstacleConfig => _obstacleConfig ??= FlappyGameplayConfig.GetObstacleTypeByScore(GameMaster.FlappyScoreManager.CurrentScoreData);

    public void Setup()
    {
        SetupSprites();
        SetupPosition();
    }

    private void SetupPosition()
    {
        var yPositionRange = FlappyGameplayConfig.RandomizeYPositionForObstacle();
        var xPosition = FlappyGameplayConfig.ObstacleSpawnDistanceFromCenter;
        transform.position = new Vector3(xPosition, yPositionRange, transform.position.z);
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
