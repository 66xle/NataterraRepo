using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SceneReference = Eflatun.SceneReference.SceneReference;
using PurrNet;
using System.Collections.Generic;

namespace Systems.SceneManagment
{
    public class SceneLoader : NetworkBehaviour
    {
        [SerializeField] SceneGroup[] sceneGroups;
        [SerializeField] List<SceneReference> scenesToIgnore;
        [SerializeField] SceneReference loadingScene;

        public readonly SceneGroupManager manager = new SceneGroupManager();

        private float targetProgress;
        [HideInInspector] public bool IsLoading = false;

        private void Awake()
        {
            manager.OnSceneLoaded += sceneName => Debug.Log("Loaded: " + sceneName);
            manager.OnSceneUnloaded += sceneName => Debug.Log("Unloaded: " + sceneName);
            manager.OnSceneGroupLoaded += () => Debug.Log("Scene group loaded");
        }


        protected async override void OnSpawned()
        {
            base.OnSpawned();

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
                temp.Scenes = new();

                string scene = GameManager.Instance.MapData.sceneName;
                if (!Extensions.SceneBuildContains(scene))
                {
                    Debug.LogError($"SceneLoader: Scene \"{scene}\" does not exist");
                    return;
                }

#if UNITY_EDITOR
                if (!SceneManager.GetSceneByName(scene).isLoaded)
#endif
                {
                    SceneReference sceneRef = Extensions.SceneGetReference(scene);
                    temp.Scenes.Add(new SceneData(sceneRef, SceneType.ActiveScene));
                }


                temp.Scenes.AddRange(group.Scenes);

                await manager.LoadScenes(temp, scenesToIgnore, progress, true);



                return;
            }

            Debug.LogError($"SceneLoader: Group \"{groupName}\" does not exist: ");
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
