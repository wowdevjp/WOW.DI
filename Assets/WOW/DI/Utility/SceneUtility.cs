using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WOW.DI.Utility
{
    internal static class SceneUtilityDI
    {
        public static IEnumerable<Scene> GetAllScenes()
        {
            var list = new List<Scene>();
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                list.Add(scene);
            }
            return list.ToArray();
        }

        public static GameObject[] GetRootGameObjects(GameObject dontdestroyGameObject = null)
        {
            var rootObjects = GetAllScenes().SelectMany(scene => scene.GetRootGameObjects()).ToList();
            if(dontdestroyGameObject != null)
            {
                rootObjects.AddRange(dontdestroyGameObject.scene.GetRootGameObjects());
            }
            return rootObjects.Where(obj => obj != null).ToArray();
        }

        public static T[] FindAllSceneObjectsByType<T>() where T : MonoBehaviour
        {
            var dontdestroyGameObject = new GameObject("Temp");
            GameObject.DontDestroyOnLoad(dontdestroyGameObject);
            var objects = GetRootGameObjects(dontdestroyGameObject).SelectMany(obj => obj.GetComponentsInChildren<T>(true)).ToList();
            GameObject.DestroyImmediate(dontdestroyGameObject);
            return objects.ToArray();
        }
    }
}