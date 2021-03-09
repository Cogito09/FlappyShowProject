using Cngine;
using UnityEngine;

namespace Flappy
{
    public class ScenesManager : CngineSceneManager<SceneType>
    {
    }

    public enum SceneType
    {
        Unknown = 0,
        MainMenu = 1,
        Game = 2
    }
}