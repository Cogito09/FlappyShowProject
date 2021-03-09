using System.Collections;
using System.Collections.Generic;
using Cngine;
using UnityEngine;

namespace Flappy
{
    public class InitialLoaderBehaviour : InitialLoaderTemplate
    {
        protected override void AddLoadActions(ref List<IEnumerator> list) { }

        protected override IEnumerator InitializeGameSceneManager()
        {
            GameMaster.ScenesManager.Initialize<SceneType>(() => GameMaster.PopupManager.HideAll());
            yield return null;
        }

        protected override IEnumerator LoadStartingScene()
        {
            GameMaster.ScenesManager.Switch(SceneType.MainMenu);
            yield return null;
        }

        protected override IEnumerator RegisterSceneSwitcherToGameSceneManager(SceneSwitchBehaviour sceneSwitcher)
        {
            GameMaster.ScenesManager.RegisterSwitcher(sceneSwitcher);
            yield return null;
        }
    }
}