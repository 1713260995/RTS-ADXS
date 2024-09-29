using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game
{
    public enum SceneName
    {
        Startup,
        Lobby,
    }

    public class GameScene
    {
        public static SceneName currentScene { get; private set; }

        public static void LoadAsync(SceneName scene, Func<AsyncOperation, UniTask> completed)
        {
            if (currentScene == scene)
            {
                return;
            }
            AsyncOperation s = SceneManager.LoadSceneAsync(scene.ToString());
            s.allowSceneActivation = false;
            if (completed == null)
            {
                s.allowSceneActivation = true;
            }
            else
            {
                completed(s).Forget();
            }

            currentScene = scene;
        }

    }
}
