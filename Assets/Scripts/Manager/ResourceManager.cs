using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{

    Dictionary<string, GameObject> uicache = new Dictionary<string, GameObject>();


    private void Awake()
    {
        uicache.Clear();
    }

    /// <summary>
    /// Resources 폴더내에 있는 프리펩을 로드합니다.(동시에 캐싱)
    /// </summary>
    /// <param name="type">디렉토리 타입.</param>
    /// <param name="prefabName">프리펩 이름.</param>
    public GameObject GetUIPrefab(string type, string prefabName)
    {
        GameObject go;
        if (uicache.TryGetValue(prefabName, out go))
            return go;

        go = (GameObject)Resources.Load("Prefabs/" + type + "/" + prefabName);
        if (go != null)
        {
            uicache.Add(prefabName, go);
        }
        else
        {
            Debug.LogError("ui prefab " + prefabName + " doesn't exist");
        }

        return go;
    }
}
