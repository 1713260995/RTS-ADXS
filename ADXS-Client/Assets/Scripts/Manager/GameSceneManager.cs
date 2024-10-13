using Assets.GameClientLib.Scripts.Utils;
using Assets.GameClientLib.Utils.Singleton;
using Assets.Scripts.Common.Enum;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game
{
    public class GameSceneManager : Singleton<GameSceneManager>
    {
        public SceneName currentScene { get; private set; }

        public GameSceneManager()
        {
            string currentName = SceneManager.GetActiveScene().name;
            currentScene = currentName.StrParseEnum<SceneName>();
        }


        public void LoadAsync(SceneName scene, Func<AsyncOperation, UniTask> loadCompleted, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            if (currentScene == scene)
            {
                return;
            }
            AsyncOperation s = SceneManager.LoadSceneAsync(scene.ToString(), loadMode);
            s.allowSceneActivation = false;
            if (loadCompleted == null)
            {
                s.allowSceneActivation = true;
            }
            else
            {
                loadCompleted(s).Forget();
            }

            currentScene = scene;
        }

        public void Load(SceneName scene, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            if (currentScene == scene) throw new InvalidOperationException("CurrentScene == " + scene);
            currentScene = scene;
            SceneManager.LoadScene(scene.ToString(), loadMode);
            Debug.Log("加载场景完成：" + currentScene);
        }

    }
}
