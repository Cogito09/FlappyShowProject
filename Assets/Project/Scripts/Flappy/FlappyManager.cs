using System.Collections.Generic;
using Cngine;
using Flappy;
using Sirenix.Utilities;
using UnityEngine;

public class FlappyManager : MonoBehaviour
{
    public static FlappyManager Instance => _instance;
    private static FlappyManager _instance;

    public bool IsPlaying;
    public ScoreData CurrentScoreData => GameMaster.FlappyScoreManager.CurrentScoreData;

    
    private FlappyGameplayConfig _flappyGameplayConfig;
    private FlappyGameplayConfig FlappyGameplayConfig => _flappyGameplayConfig ??= MainConfig.FlappyGameplayConfig;
    private Queue<FlappyObstacleBehaviour> _flappyObstacles = new Queue<FlappyObstacleBehaviour>();
    private Queue<FlappyTiledObjectBehaviour> _flappyBgs = new Queue<FlappyTiledObjectBehaviour>();
    private float? _speed;
    private float Speed => _speed ??= FlappyGameplayConfig.WorldMovementSpeed;
    private double ObstacleRemoveXPosition => FlappyGameplayConfig.ObstacleRemoveXPosition;
    private double DistanceBetweenObstacles => FlappyGameplayConfig.DistanceBetweenObstacles;



    private float _lastSpawnedObstacleTraveledDistance;
    
    private void Awake()
    {
        _instance = this;
        EventManager.OnFlappyRoundStarted += StartRound;
        EventManager.OnFlappyRoundFinished += EndRound;
        EventManager.OnFlappyRoundReseted += OnReset;
    }

    private void OnDestroy()
    { 
        EventManager.OnFlappyRoundStarted -= StartRound;
        EventManager.OnFlappyRoundFinished -= EndRound;
        EventManager.OnFlappyRoundReseted -= OnReset;
    }
    
    private void Update()
    {
        if (IsPlaying == false)
        {
            return;
        }
        
        UpdateObstacles();
        UpdateBackgroundsTiles();
    }

    private void UpdateBackgroundsTiles()
    {
        _flappyBgs.ForEach(bg => bg.transform.Translate(new Vector3(Speed * Time.deltaTime,0,0)));
        var firstBg = _flappyBgs.Peek();
        
        var OutOfCameraViewPosition = firstBg.GetObjectWidth() * -1;
        if (firstBg.transform.position.x > OutOfCameraViewPosition)
        {
            return;
        }

        var lengthOfAllTiles = firstBg.GetObjectWidth() * _flappyBgs.Count;
        var bgPosition = firstBg.transform.position;

        firstBg.transform.position =  new Vector3(
            bgPosition.x + lengthOfAllTiles,
            bgPosition.y,
            bgPosition.z);
        
        _flappyBgs.Enqueue(_flappyBgs.Dequeue());
    }

    private void UpdateObstacles()
    {
        var translationDistance = Speed * Time.deltaTime;
        _lastSpawnedObstacleTraveledDistance += translationDistance;
        if (_lastSpawnedObstacleTraveledDistance > DistanceBetweenObstacles)
        {
            SpawnNewObstacle();
        }
        
        _flappyObstacles.ForEach(obstacle => obstacle.transform.Translate(translationDistance * -1,0,0));
        if (_flappyObstacles.Count <= 0)
        {
            return;
        }
        
        if (_flappyObstacles.Peek().transform.position.x < ObstacleRemoveXPosition)
        {
            _flappyObstacles.Dequeue().Remove(true);
        }
    }

    private void SpawnNewObstacle()
    {
        var obj = GameMaster.PoolManager.SpawnObject(FlappyPrefabType.Obstacle) as FlappyObstacleBehaviour;
        if (obj != null)
        {
            obj.Setup();
            _flappyObstacles.Enqueue(obj);
        }
            
        Log.Error("FlappyObstacleBehaviour failed to spawn");
    }

    private void ResetTiles()
    {
        _flappyBgs.ForEach(tile => tile.Remove());
        _flappyBgs.Clear();
        
        var numberOfTilesToSpawn = FlappyGameplayConfig.TotalNumberOfVisibleTiles;
        for (int i = 0; i < numberOfTilesToSpawn; i++)
        {
            var obstacle = GameMaster.PoolManager.SpawnObject(FlappyPrefabType.BackgroundTile) as FlappyTiledObjectBehaviour;
            if (obstacle == null)
            {
                Log.Error("Spawned Obstacle is null!");
                continue;
            }
            
            obstacle.transform.position = i == 0 ?  Vector3.zero : new Vector3(obstacle.GetObjectWidth() * i,0,0);
            _flappyBgs.Enqueue(obstacle);
        }
    }
    
    private void ResetObstacles()
    {
        _flappyObstacles.ForEach(obstacle => obstacle.Remove());
        _flappyObstacles.Clear();
        
        var firstObstacleSpawnDistance = FlappyGameplayConfig.FirstObstacleSpawnDistance;
        var spawnPosition = new Vector3(firstObstacleSpawnDistance, 0, 0);

        var obstacle = GameMaster.PoolManager.SpawnObject(FlappyPrefabType.Obstacle) as FlappyObstacleBehaviour;
        if (obstacle == null)
        {
            Log.Error("Failed to spawn obstacle. returning");
            return;
        }
        
        obstacle.transform.position = spawnPosition;
        _flappyObstacles.Enqueue(obstacle);
        
        _lastSpawnedObstacleTraveledDistance = 0;
    }

    private void StartRound()
    {       
        Log.Info("Round Started");
        IsPlaying = true;
    }

    private void OnReset()
    {
        Log.Info("Restart");
        ResetTiles();
        ResetObstacles();
        IsPlaying = false;
    }

    public void EndRound()
    {
        Log.Info("Round Ended");
        IsPlaying = false;
    }
}
