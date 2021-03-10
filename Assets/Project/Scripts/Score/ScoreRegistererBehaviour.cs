using System;
using System.Collections;
using System.Collections.Generic;
using Cngine;
using JetBrains.Annotations;
using UnityEngine;

namespace Flappy
{
    public class ScoreRegistererBehaviour : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Bird") == false)
            {
                return;
            }

            GameMaster.FlappyScoreManager.IncrementScore();
            gameObject.Deactivate();
        }

        public void OnSpawned()
        {
            gameObject.Activate();
        }
    }
}

