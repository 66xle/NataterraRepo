using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : Singleton<Bootstrapper>
{
    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static async void Init()
    {
        Debug.Log("Bootstrapper...");
        await SceneManager.LoadSceneAsync("Bootstrapper", LoadSceneMode.Single);
    }
}
