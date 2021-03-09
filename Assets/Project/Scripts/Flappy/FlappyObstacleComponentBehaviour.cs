using Flappy;
using UnityEngine;

namespace CosmosQuest
{
    public class FlappyObstacleComponentBehaviour : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Bird"))
            {
                EventManager.OnFlappyObstacleHit?.Invoke();
            }
        }
    }
}