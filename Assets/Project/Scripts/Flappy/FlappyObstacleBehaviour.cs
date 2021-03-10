using Cngine;
using Flappy;
using UnityEngine;

public class FlappyObstacleBehaviour : PoolObject
{
    [SerializeField] private ScoreRegistererBehaviour _scoreRegistererBehaviour;
    [SerializeField] private SpriteRenderer _upperSprite;
    [SerializeField] private SpriteRenderer _downSprite;
    
    private FlappyGameplayConfig _flappyGameplayConfig;
    private FlappyGameplayConfig FlappyGameplayConfig => _flappyGameplayConfig ??= MainConfig.FlappyGameplayConfig;
    private FlappyObstaclesConfig.ObstacleConfig _obstacleConfig;
    private FlappyObstaclesConfig.ObstacleConfig ObstacleConfig => _obstacleConfig ??= FlappyGameplayConfig.GetObstacleTypeByScore(GameMaster.FlappyScoreManager.CurrentScoreData);
    
    public Sprite Sprite;
    public Color Color;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
    
    public void Setup(bool isInitialObstacle = false)
    {
        SetupSprites();
        SetupPosition(isInitialObstacle);
        _scoreRegistererBehaviour.OnSpawned();
    }

    private void SetupPosition(bool isInitialObstacle)
    {
        var yPositionRange = FlappyGameplayConfig.RandomizeYPositionForObstacle();
        var xPosition = isInitialObstacle == false ? FlappyGameplayConfig.ObstacleSpawnDistanceFromCenter : FlappyGameplayConfig.FirstObstacleSpawnPositionX;
        transform.position = new Vector3(xPosition, yPositionRange, transform.position.z);
    }
    
    private void SetupSprites()
    {
        Sprite = ObstacleConfig.Sprite;
        Color = ObstacleConfig.RandomizeColor();
        
        _upperSprite.sprite = Sprite;
        _upperSprite.color = Color;
        
        _downSprite.sprite = Sprite;
        _downSprite.color = Color;
    }
    
    protected override void StartRemovalAnimation()
    {
        var obj = GameMaster.PoolManager.SpawnObject(FlappyPrefabType.ObstacleRemovalAnimation) as ObstacleRemovalObject;
        obj.Setup(Color,Sprite);
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
