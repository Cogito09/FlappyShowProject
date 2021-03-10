using Cngine;
using UnityEngine;

namespace Flappy
{
    public class ObstacleRemovalObject : FlappyPoolRemovalObject
    {
        [SerializeField] private SpriteRenderer _upperSpriteRenderer;
        [SerializeField] private SpriteRenderer _downSpriteRenderer;
        [SerializeField] private PotaTween _tweenUpper;
        [SerializeField] private PotaTween _tweenDown;
        
        private FlappyGameplayConfig _gameplayConfig;
        private FlappyGameplayConfig GameplayConfig => _gameplayConfig ??= MainConfig.FlappyGameplayConfig;
        private float WorldSpeed => _worldSpeed ??= GameplayConfig.WorldMovementSpeed;
        private float? _worldSpeed;
        
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

        private void Update()
        {
            var moveVector = WorldSpeed * Time.deltaTime * -1;
            transform.Translate(moveVector,0,0);
        }
        
        public void Setup(Color color,Sprite sprite,float upperObstacleHeight,float downObstacleHeight)
        {
            _upperSpriteRenderer.color = color;
            _downSpriteRenderer.color = color;
            
            _upperSpriteRenderer.sprite = sprite;
            _downSpriteRenderer.sprite = sprite;

            _tweenUpper.Position.IsLocal = true;
            _tweenUpper.Position.IgnoreAxis.X = true;
            _tweenUpper.Position.IgnoreAxis.Z = true;
            _tweenUpper.Position.From = new Vector3(0, upperObstacleHeight, 0);
            _tweenUpper.Position.To = new Vector3(0, 10, 0);

            _tweenDown.Position.IsLocal = true;
            _tweenDown.Position.IgnoreAxis.X = true;
            _tweenDown.Position.IgnoreAxis.Z = true;
            _tweenDown.Position.From = new Vector3(0, downObstacleHeight, 0);
            _tweenDown.Position.To = new Vector3(0, -10, 0);

            _tweenUpper.Play();
            _tweenDown.Play();
        }
    }
}