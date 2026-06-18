using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SceneReference = Eflatun.SceneReference.SceneReference;

namespace Systems.SceneManagment
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] SceneGroup[] sceneGroups;
        [SerializeField] SceneReference loadingScene;

        public readonly SceneGroupManager manager = new SceneGroupManager();

        private float targetProgress;
        private bool Init = false;
        [HideInInspector] public bool IsLoading = false;

        private void Awake()
        {
            manager.OnSceneLoaded += sceneName => Debug.Log("Loaded: " + sceneName);
            manager.OnSceneUnloaded += sceneName => Debug.Log("Unloaded: " + sceneName);
            manager.OnSceneGroupLoaded += () => Debug.Log("Scene group loaded");
        }


        async void Start()
        {
            Init = true;

            await LoadSceneGroup(sceneGroups[0].GroupName);
        }

        public async void CloseLoadingScreen()
        {
            await SceneManager.UnloadSceneAsync(loadingScene.Path);
        }

        public async Task LoadSceneGroup(string groupName)
        {
            targetProgress = 1f;

            LoadingProgress progress = new LoadingProgress();
            progress.Progressed += target => targetProgress = Mathf.Max(target, targetProgress);


            foreach (SceneGroup group in sceneGroups)
            {
                if (group.GroupName != groupName) continue;

                SceneGroup temp = new SceneGroup();
                temp.GroupName = group.GroupName;
                temp.Scenes = new(group.Scenes);

                //if (Init)
                //{
                //    await SceneManager.LoadSceneAsync(loadingScene.Path, LoadSceneMode.Additive);
                //    await manager.UnloadScenes();
                //}

                await manager.LoadScenes(temp, progress);

                return;
            }

            Debug.LogError($"Group Name does not exist: {groupName}");
        }
    }

    public class LoadingProgress : IProgress<float>
    {
        public event Action<float> Progressed;

        const float ratio = 1f;

        public void Report(float value)
        {
            Progressed?.Invoke(value / ratio);
        }
    }
}
