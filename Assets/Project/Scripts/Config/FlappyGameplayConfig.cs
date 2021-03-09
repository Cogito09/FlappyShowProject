using UnityEngine;

namespace Flappy
{
    [CreateAssetMenu(fileName = "FlappyGameplayConfig", menuName = "Configs/FlappyGameplayConfig", order = 0)]
    public class FlappyGameplayConfig : ScriptableObject
    {
        public float WorldMovementSpeed;
        public float MapEdgePosition;
        public int TotalNumberOfVisibleTiles;
        public float FirstObstacleSpawnDistance;
        public double DistanceBetweenObstacles;
        public float ObstacleRemoveXPosition;
    }
}