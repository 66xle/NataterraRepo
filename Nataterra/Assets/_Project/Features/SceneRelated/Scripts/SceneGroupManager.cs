using Eflatun.SceneReference;
using PurrNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Systems.SceneManagment
{
    public class SceneGroupManager
    {
        public event Action<string> OnSceneLoaded = delegate { };
        public event Action<string> OnSceneUnloaded = delegate { };
        public event Action OnSceneGroupLoaded = delegate { };

        SceneGroup ActiveSceneGroup;

        public async Task<List<string>> LoadScenes(PlayerID id, SceneGroup group, bool reloadDupScenes = false)
        {
            ActiveSceneGroup = group;
            var loadedScenes = new List<string>();

            int sceneCount = SceneManager.sceneCount;

            for (var i = 0; i < sceneCount; i++)
            {
                loadedScenes.Add(SceneManager.GetSceneAt(i).name);
            }

            var totalScenesToLoad = ActiveSceneGroup.Scenes.Count;

            var operationGroup = new AsyncOperationGroup(totalScenesToLoad);

            List<string> scenes = new(); 

            for (var i = 0; i < totalScenesToLoad; i++)
            {
                var sceneData = group.Scenes[i];
                if (reloadDupScenes == false && loadedScenes.Contains(sceneData.Name)) continue;

                NetworkManager network = InstanceHandler.NetworkManager;
                Scene scene = SceneManager.GetSceneByName(sceneData.Name);

                if (!id.isServer)
                {
                    if (!scene.IsValid() || !scene.isLoaded)
                    {
                        Debug.LogError($"Scene {sceneData.Name} isn't loaded on the server.");
                        continue;
                    }

                    if (!network.sceneModule.TryGetSceneID(scene, out SceneID sceneId))
                    {
                        Debug.LogError($"Couldn't get SceneID for {sceneData.Name}");
                        continue;
                    }

                    network.scenePlayersModule.AddPlayerToScene(id, sceneId);

                    scenes.Add(sceneData.Name);
                }
                else
                {
                    var operation = network.sceneModule.LoadSceneAsync(sceneData.Reference.Name, LoadSceneMode.Additive);
                    operationGroup.Operations.Add(operation);
                }

                OnSceneLoaded.Invoke(sceneData.Name);
            }

            while (!operationGroup.IsDone)
            {
                await Task.Yield();
            }


            Scene activeScene = SceneManager.GetSceneByName(ActiveSceneGroup.FindSceneNameByType(SceneType.ActiveScene));

            if (activeScene.IsValid())
            {
                SceneManager.SetActiveScene(activeScene);
            }

            OnSceneGroupLoaded.Invoke();

            return scenes;
        }

        public async Task<List<string>> UnloadScenes(PlayerID id, SceneGroup group, List<SceneReference> scenesToIgnore = null)
        {
            var scenes = new List<string>();
            var activeScene = SceneManager.GetActiveScene().name;

            int sceneCount = SceneManager.sceneCount;

            for (var i = sceneCount - 1; i >= 0; i--)
            {
                var sceneAt = SceneManager.GetSceneAt(i);
                if (!sceneAt.isLoaded) continue;

                var sceneName = sceneAt.name;

                bool isMatch = false;

                foreach (SceneReference sceneRef in scenesToIgnore) 
                {
                    if (sceneRef.Name == sceneName)
                    {
                        isMatch = true;
                        break;
                    }
                }

                if (isMatch) continue;
                scenes.Add(sceneName);
            }

            var operationGroup = new AsyncOperationGroup(scenes.Count);

            List<string> listOfScenes = new();

            foreach (string sceneName in scenes)
            {
                NetworkManager network = InstanceHandler.NetworkManager;
                Scene scene = SceneManager.GetSceneByName(sceneName);

                if (!scene.isLoaded)
                    continue;

                bool registered = network.sceneModule.TryGetSceneID(scene, out SceneID sceneId);

                if (!registered)
                {
                    var operation = SceneManager.UnloadSceneAsync(scene);
                    if (operation == null) continue;

                    operationGroup.Operations.Add(operation);
                }
                else
                {
                    if (!id.isServer)
                    {
                        network.scenePlayersModule.RemovePlayerFromScene(id, sceneId);
                        listOfScenes.Add(sceneName);
                    }
                    else
                    {
                        var operation = network.sceneModule.UnloadSceneAsync(scene);
                        if (operation == null) continue;

                        operationGroup.Operations.Add(operation);
                    }
                }

                OnSceneUnloaded.Invoke(sceneName);
            }

            while (!operationGroup.IsDone)
            {
                await Task.Delay(100);
            }

            return listOfScenes;

            //await Resources.UnloadUnusedAssets();
        }

        
    }

    public readonly struct AsyncOperationGroup
    {
        public readonly List<AsyncOperation> Operations;

        public float Progress => Operations.Count == 0 ? 0 : Operations.Average(o => o.progress);
        public bool IsDone => Operations.All(o => o.isDone);

        public AsyncOperationGroup(int initialCapacity)
        {
            Operations = new List<AsyncOperation>(initialCapacity);
        }
    }


}
