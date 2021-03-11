using UnityEngine;

namespace Flappy
{
    [CreateAssetMenu(fileName = "FacebookConfig", menuName = "Configs/FacebookConfig", order = 0)]
    public class FacebookConfig : ScriptableObject
    {
        public string ShareText;
        public string ShareURL;
        public string SharePhotoURL;

        public string GetShowScoreText(int currentScore)
        {
            return $"Scored : {currentScore}. Can yo ubeat it?";
        }
    }
}