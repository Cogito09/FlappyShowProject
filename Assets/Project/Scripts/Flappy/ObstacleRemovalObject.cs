using UnityEngine;

namespace Flappy
{
    public class ObstacleRemovalObject : FlappyPoolRemovalObject
    {
        [SerializeField] private SpriteRenderer _spriteRenderer1;
        [SerializeField] private SpriteRenderer _spriteRenderer2;

        private void OnEnable()
        {
            EventManager.OnFlappyRoundReseted += OnRoundReseted;
        }
        
        private void OnDisable()
        {
            EventManager.OnFlappyRoundReseted -= OnRoundReseted;
        }
        
        private void OnRoundReseted()
        {
            Remove(true);
        }
        
        public void Setup(Color color,Sprite sprite)
        {
            _spriteRenderer1.color = color;
            _spriteRenderer2.color = color;
            _spriteRenderer1.sprite = sprite;
            _spriteRenderer2.sprite = sprite;
        }
    }
}