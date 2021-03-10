using TMPro;
using UnityEngine;

namespace Flappy
{
    public class UIScoreSlotBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _index;
        [SerializeField] private TextMeshProUGUI _score;
        
        public void Setup(int index,int score)
        {
            _index.text = $"{index.ToString()}.";
            _score.text = score.ToString();
        }
    }
}