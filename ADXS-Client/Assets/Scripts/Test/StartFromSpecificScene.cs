using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Assets.GameClientLib.Scripts.Config.SO;
using Assets.Scripts.Common.Enum;
using UnityEngine.AddressableAssets;

[InitializeOnLoad]
public class StartFromSpecificScene
{

    static StartFromSpecificScene()
    {
        // 监听播放状态的更改事件
        var s = Addressables.LoadAssetAsync<TestStartupSceneConfigSO>("Assets/GameClientLib/ScriptableObject/Config/TestStartupScene.asset");
        s.Completed += o =>
        {
            if (o.Result.isStartupCurrentScnene == false)
            {
                EditorApplication.playModeStateChanged += OnPlayModeChanged;
            }
        };

    }

    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        // 当即将进入播放模式时
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            string activeSceneName = SceneName.Startup.ToString();

            // 确认并保存当前修改的场景
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                // 切换到指定的启动场景
                string startScenePath = FindScenePathByName(activeSceneName);
                if (!string.IsNullOrEmpty(startScenePath))
                {
                    EditorSceneManager.OpenScene(startScenePath);
                }
                else
                {
                    Debug.LogError($"无法找到指定场景：{activeSceneName}");
                }
            }
        }
    }

    // 根据场景名称查找该场景的完整路径
    private static string FindScenePathByName(string sceneName)
    {
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.path.Contains(sceneName))
            {
                return scene.path;
            }
        }
        return null;
    }
}
