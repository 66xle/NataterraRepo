using PurrNet;
using Systems.SceneManagment;
using UnityEngine;

public class SceneNotify : NetworkBehaviour
{
    protected override void OnSpawned()
    {
        base.OnSpawned();

        if (InstanceHandler.TryGetInstance<SceneLoader>(out SceneLoader loader))
        {
            loader.SetSceneInitialized(gameObject.scene.name);
            Debug.Log($"Nofity {gameObject.scene.name}");
        }
        else
            Debug.LogError($"SceneNotify: Failed to get SceneLoader instance for Scene {gameObject.scene.name}");
    }
}
