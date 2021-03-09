using System;
using TMPro;
using UnityEngine;

namespace Flappy
{
    public class UIGamplayScoreBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _score;
        [SerializeField] private TextMeshProUGUI _bombs;

        private void Awake()
        {
            EventManager.OnScoreChanged += UpdateScore;
            EventManager.OnBombsQuantityChanged += OnBombQuantityChanged;
        }

        private void OnDestroy()
        {
            EventManager.OnScoreChanged -= UpdateScore;
            EventManager.OnBombsQuantityChanged -= OnBombQuantityChanged;
        }

        private void UpdateScore()
        {
            _score.text = GameMaster.FlappyScoreManager.CurrentScoreData.Score.ToString();
        }
        
        public void OnBombQuantityChanged()
        {
            _bombs.text = GameMaster.FlappyScoreManager.CurrentScoreData.NumberOfBombs.ToString();
        }

    }
}