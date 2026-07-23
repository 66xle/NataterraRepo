using PurrNet;
using PurrNet.Modules;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SceneReference = Eflatun.SceneReference.SceneReference;

namespace Systems.SceneManagment
{
    public class SceneLoader : NetworkBehaviour
    {
        [SerializeField] SceneGroup[] sceneGroups;
        [SerializeField] List<SceneReference> scenesToIgnore;
        [SerializeField] SceneReference loadingScene;

        public readonly SceneGroupManager manager = new SceneGroupManager();

        [HideInInspector] public bool IsLoading = false;

        public Action<Scene> OnSceneInitialized;

        Dictionary<string, bool> _dictOfSceneInit;

        SceneGroup _tempSceneGroup; 

        private void Awake()
        {
            manager.OnSceneLoaded += sceneName => Debug.Log("Loaded: " + sceneName);
            manager.OnSceneUnloaded += sceneName => Debug.Log("Unloaded: " + sceneName);
            manager.OnSceneGroupLoaded += () => Debug.Log("Scene group loaded");

            _dictOfSceneInit = new Dictionary<string, bool>();

            InstanceHandler.RegisterInstance(this);
        }


        protected override void OnSpawned(bool asServer)
        {
            base.OnSpawned();

            if (isHost) return;

            Init(asServer);
        }

        private async void Init(bool asServer)
        {
            await LoadSceneGroup(sceneGroups[0].GroupName, asServer);

            Debug.Log("Run Setup");
            SceneInitialize.Instance.Invoke();
        }

        public async void CloseLoadingScreen()
        {
            await SceneManager.UnloadSceneAsync(loadingScene.Path);
        }

        public void SetSceneInitialized(string scene)
        {
            if (!_dictOfSceneInit.ContainsKey(scene))
                return;

            _dictOfSceneInit[scene] = true;
        }


        public async Task LoadSceneGroup(string groupName, bool asServer)
        {
            _tempSceneGroup = null;

            foreach (SceneGroup group in sceneGroups)
            {
                if (group.GroupName != groupName) continue;

                SceneGroup temp = new SceneGroup();
                temp.GroupName = group.GroupName;
                temp.Scenes = new();

                string scene = GameManager.Instance.MapData.sceneName;
                SceneReference sceneRef = Extensions.SceneGetReference(scene);

                if (!Extensions.SceneBuildContains(scene))
                {
                    Debug.LogError($"SceneLoader: Scene \"{scene}\" does not exist");
                    return;
                }

                
                
#if UNITY_EDITOR
                if (!SceneManager.GetSceneByName(scene).isLoaded) 
#endif
                {
                    // Add environment scene
                    temp.Scenes.Add(new SceneData(sceneRef, SceneType.ActiveScene));
                }

                temp.Scenes.AddRange(group.Scenes);

                _tempSceneGroup = temp;

                _dictOfSceneInit.Clear();
                foreach (SceneData data in group.Scenes)
                {
                    _dictOfSceneInit.Add(data.Name, false);
                }


                if (asServer)
                {
                    await ServerUnloadScene();
                    await ServerLoadScene();
                }
                else
                {
                    List<string> scenes = await ServerUnloadScene();
                    await WaitUntilPlayerHasUnloadedScenes(scenes);
                    Debug.Log("Finshed Unloading Scenes"); 

                    await ServerLoadScene(); 
                    await WaitUntilPlayerHasScenesInitialized(scenes);
                    Debug.Log("Finshed Loading Scenes");
                }

                return;
            }

            Debug.LogError($"SceneLoader: Group \"{groupName}\" does not exist: ");
        }

        [ServerRpc]
        async Task<List<string>> ServerLoadScene(RPCInfo info = default)
        {
            return await manager.LoadScenes(info.sender, _tempSceneGroup, true);
        }

        [ServerRpc]
        async Task<List<string>> ServerUnloadScene(RPCInfo info = default)
        {
            return await manager.UnloadScenes(info.sender, _tempSceneGroup, scenesToIgnore);
        }

        async Task WaitUntilPlayerHasScenesInitialized(List<string> scenes)
        {
            while (true)
            {
                bool complete = true;

                foreach (string scene in _dictOfSceneInit.Keys)
                {
                    if (!_dictOfSceneInit[scene])
                    {
                        complete = false;
                        break;
                    }
                }

                if (!complete)
                {
                    await Task.Yield();
                    continue;
                }

                return;
            }
        }

        async Task WaitUntilPlayerHasUnloadedScenes(List<string> scenes)
        {
            while (true)
            {
                bool complete = true;

                foreach (var sceneName in scenes)
                {
                    Scene scene = SceneManager.GetSceneByName(sceneName);

                    if (scene.isLoaded)
                    {
                        complete = false;
                        break;
                    }
                }

                if (complete)
                    return;

                await Task.Yield();
            }
        }
    }
}
