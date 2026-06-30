using Eflatun.SceneReference;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public static class Extensions
{
    public static SceneReference SceneGetReference(string sceneName)
    {
        int count = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < count; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = Path.GetFileNameWithoutExtension(path);

            if (name == sceneName)
                return SceneReference.FromScenePath(path);
        }

        return null;
    }

    public static int SceneGetInt(string sceneName)
    {
        int count = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < count; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = Path.GetFileNameWithoutExtension(path);

            if (name == sceneName)
                return i + 1;
        }

        return -1;
    }

    public static bool SceneBuildContains(string sceneName)
    {
        int count = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < count; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = Path.GetFileNameWithoutExtension(path);

            if (name == sceneName)
                return true;
        }

        return false;
    }

    public static List<string> SceneGetList()
    {
        List<string> scenes = new();

        int sceneCount = SceneManager.sceneCountInBuildSettings;

        for (int i = 0; i < sceneCount; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            scenes.Add(Path.GetFileNameWithoutExtension(scenePath));
        }

        return scenes;
    }


    public static bool Contains<T, TKey>(List<T> list, TKey value, Func<T, TKey> selector)
    {
        var comparer = EqualityComparer<TKey>.Default;

        foreach (T item in list)
        {
            if (!comparer.Equals(selector(item), value))
                continue;

            return true;
        }

        return false;
    }
    public static bool Contains<T, TKey>(List<T> list, TKey value, Func<T, TKey> selector, out T found)
    {
        var comparer = EqualityComparer<TKey>.Default;

        foreach (T item in list)
        {
            if (!comparer.Equals(selector(item), value)) 
                continue;

            found = item;
            return true;
        }

        found = default;
        return false;
    }
}
