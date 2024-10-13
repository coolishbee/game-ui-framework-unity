using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [System.NonSerialized]
    public top_notification_ui topNotiUI;

    private void Awake()
    {
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void Init()
    {
        GameObject topNotiObj = AddUI("top_notification_ui");
        if (topNotiObj != null)
        {
            topNotiUI = topNotiObj.GetComponent<top_notification_ui>();
        }
    }

    public GameObject AddUI(string prefabName)
    {
        return AddUI(GameObject.Find("Root"), prefabName);
    }

    public GameObject AddUI(GameObject parent, string prefabName)
    {
        GameObject prefab = ResourceManager.Instance.GetUIPrefab("ui", prefabName);
        if (prefab == null)
        {
            Debug.Log("cannot find " + prefabName);
            return null;
        }
        GameObject obj = Util.AddChild(parent, prefab);
        obj.layer = prefab.layer;
        return obj;
    }
    
}
