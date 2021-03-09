using System;
using Cngine;
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
            EventManager.OnFlappyRoundFinished += Setup;
            EventManager.OnFlappyRoundStarted += Setup;
            EventManager.OnFlappyRoundReseted += Setup;
            Setup();
        }

        private void OnDestroy()
        {
            EventManager.OnScoreChanged -= UpdateScore;
            EventManager.OnBombsQuantityChanged -= OnBombQuantityChanged;
            EventManager.OnFlappyRoundFinished -= Setup;
            EventManager.OnFlappyRoundStarted -= Setup;
            EventManager.OnFlappyRoundReseted -= Setup;
        }
        
        private void Setup()
        {
            var showPanel = FlappyManager.Instance != null && FlappyManager.Instance.IsPlaying == false;
            gameObject.ChangeActive(showPanel);
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