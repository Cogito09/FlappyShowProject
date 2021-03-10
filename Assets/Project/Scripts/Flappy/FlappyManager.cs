using System.Collections.Generic;
using Cngine;
using Cngine.PopupSystem;
using Flappy;
using Sirenix.Utilities;
using UnityEngine;

public class FlappyManager : MonoBehaviour
{
    public static FlappyManager Instance => _instance;
    private static FlappyManager _instance;

    public bool IsPlaying;
    private FlappyGameplayConfig _flappyGameplayConfig;
    private FlappyGameplayConfig FlappyGameplayConfig => _flappyGameplayConfig ??= MainConfig.FlappyGameplayConfig;
    private Queue<FlappyObstacleBehaviour> _flappyObstacles = new Queue<FlappyObstacleBehaviour>();
    private Queue<FlappyTiledBackgroundBehaviour> _flappyBgs = new Queue<FlappyTiledBackgroundBehaviour>();
    private float? _speed;
    private float Speed => _speed ??= FlappyGameplayConfig.WorldMovementSpeed;
    private float? _bGYPosition;
    private float  BGYPosition => _bGYPosition ??= FlappyGameplayConfig._bgYPosition;
    private double ObstacleRemoveXPosition => FlappyGameplayConfig.ObstacleRemoveXPosition;
    private double DistanceBetweenObstacles => FlappyGameplayConfig.DistanceBetweenObstacles;
    
    private float _lastSpawnedObstacleTraveledDistance;

    private static int numberOfManagers;
    private void Awake()
    {
        numberOfManagers++;
        Log.Info($"numberOfManagers : {numberOfManagers}");
        _instance = this;
        EventManager.OnFlappyRoundStarted += StartRound;
        EventManager.OnFlappyRoundFinished += FinishRound;
        EventManager.OnFlappyRoundReseted += OnReset;
        EventManager.OnBombUsed += OnBombUsed;
    }

    private void OnDestroy()
    { 
        EventManager.OnFlappyRoundStarted -= StartRound;
        EventManager.OnFlappyRoundFinished -= FinishRound;
        EventManager.OnFlappyRoundReseted -= OnReset;
        EventManager.OnBombUsed -= OnBombUsed;
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

    private void OnBombUsed()
    {
        if (_flappyObstacles == null)
        {
            return;
        }
        
        while (true)
        {
            if (_flappyObstacles.Count <= 0)
            {
                break;
            }

            var obstacle = _flappyObstacles.Peek();
            if (IsInViewRange(obstacle.transform.position))
            {
                _flappyObstacles.Dequeue().Remove();
                continue;
            }
            
            break;
        }
    }

    private bool IsInViewRange(Vector3 transformPosition)
    {
       return transformPosition.x < SightDistance;
    }

    private double? _sightDistance;
    public double SightDistance => _sightDistance ??= FlappyGameplayConfig.SightDistance;

    private void UpdateBackgroundsTiles()
    {
        _flappyBgs.ForEach(bg => bg.transform.Translate(new Vector3(Speed * Time.deltaTime * -1,0,0)));
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
        var translation = Speed * Time.deltaTime * -1;
        _lastSpawnedObstacleTraveledDistance += Mathf.Abs(translation);
        if (_lastSpawnedObstacleTraveledDistance > DistanceBetweenObstacles)
        {
            _lastSpawnedObstacleTraveledDistance = 0;
            SpawnNewObstacle();
        }
        
        _flappyObstacles.ForEach(obstacle => obstacle.transform.Translate(translation,0,0));
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
            return;
        }
            
        Log.Error("FlappyObstacleBehaviour failed to spawn");
    }

    private void ResetTiles()
    {
        _flappyBgs.ForEach(tile => tile.Remove(true));
        _flappyBgs.Clear();
        
        var numberOfTilesToSpawn = FlappyGameplayConfig.TotalNumberOfVisibleTiles;
        for (int i = 0; i < numberOfTilesToSpawn; i++)
        {
            var obstacle = GameMaster.PoolManager.SpawnObject(FlappyPrefabType.BackgroundTile) as FlappyTiledBackgroundBehaviour;
            if (obstacle == null)
            {
                Log.Error("Spawned Obstacle is null!");
                continue;
            }
            
            obstacle.transform.position = i == 0 ?  new Vector3(0,BGYPosition,0) : new Vector3(obstacle.GetObjectWidth() * i,BGYPosition,0);
            _flappyBgs.Enqueue(obstacle);
        }
    }
    
    private void ResetObstacles()
    {
        _flappyObstacles.ForEach(obstacle => obstacle.Remove(true));
        _flappyObstacles.Clear();
        

        var obstacle = GameMaster.PoolManager.SpawnObject(FlappyPrefabType.Obstacle) as FlappyObstacleBehaviour;
        if (obstacle == null)
        {
            Log.Error("Failed to spawn obstacle. returning");
            return;
        }
        
        _flappyObstacles.Enqueue(obstacle);
        obstacle.Setup(true);

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

    private void FinishRound()
    {
        Log.Info("Round Ended");
        IsPlaying = false;
    }

    public void ObstacleHasBeenHit()
    {
        if (IsPlaying == false)
        {
            Log.Info("Tried to finish game but game is not Playing");
            return;
        }

        IsPlaying = false;
        Log.Info("Obstacle hit, launching finished event");
        EventManager.OnFlappyRoundFinished?.Invoke();
    }
}
