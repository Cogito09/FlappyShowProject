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
            EventManager.OnFlappyRoundFinished += Hide;
            EventManager.OnFlappyRoundStarted += Show;
            EventManager.OnFlappyRoundReseted += Hide;
            Hide();
        }

        private void OnDestroy()
        {
            EventManager.OnScoreChanged -= UpdateScore;
            EventManager.OnBombsQuantityChanged -= OnBombQuantityChanged;
            EventManager.OnFlappyRoundFinished -= Hide;
            EventManager.OnFlappyRoundStarted -= Show;
            EventManager.OnFlappyRoundReseted -= Hide;
        }

        private void Show()
        {
            gameObject.ChangeActive(true);
            Setup();
        }

        private void Hide()
        {
            gameObject.ChangeActive(false);
        }
        
        private void Setup()
        {
            UpdateScore();
            OnBombQuantityChanged();
        }
        
        private void UpdateScore()
        {
            _score.text = GameMaster.FlappyScoreManager?.CurrentFlappyScore?.Score.ToString() ?? string.Empty;
        }
        
        public void OnBombQuantityChanged()
        {
            _bombs.text = GameMaster.FlappyScoreManager?.CurrentFlappyScore?.NumberOfBombs.ToString() ?? string.Empty;
        }
    }
}