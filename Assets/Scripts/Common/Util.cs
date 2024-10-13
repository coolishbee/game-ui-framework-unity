using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    /// <summary>
    /// 로드된 프리펩(GameObject)를 부모(parent GameObject)에 생성합니다.
    /// </summary>
    /// <param name="parent">저장할 키값.</param>
    /// <param name="prefab">파일디렉토리 경로.</param>
    static public GameObject AddChild(GameObject parent, GameObject prefab)
    {
        GameObject go = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
#if UNITY_EDITOR
        UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Create Object");
#endif
        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.SetParent(parent.transform);

            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;

            go.GetComponent<RectTransform>().sizeDelta = prefab.GetComponent<RectTransform>().sizeDelta;
        }
        go.GetComponent<RectTransform>().anchoredPosition = prefab.GetComponent<RectTransform>().anchoredPosition;
        return go;
    }

    // ex) Util.ShowSimpleConfirmPanel("알림", "Two Button", delegate (){ Debug.Log("yes"); },delegate(){ Debug.Log("no"); });
    // ex) Util.ShowSimpleConfirmPanel("알림", "One Button", delegate (){ Debug.Log("yes"); });
    //
    static public simple_confirm_panel ShowSimpleConfirmPanel(string title, string contents,
        simple_confirm_panel.DelegatorOnOk onOK = null, simple_confirm_panel.DelegatorOnClose onClose = null)
    {
        GameObject prefab = ResourceManager.Instance.GetUIPrefab("ui", "simple_confirm_panel");
        GameObject go = AddChild(GameObject.Find("Root"), prefab);

        simple_confirm_panel confirmPanel = null;

        if (go != null)
        {
            confirmPanel = go.GetComponent<simple_confirm_panel>();
            confirmPanel.label_title.text = title;
            confirmPanel.label_contents.text = contents;
            confirmPanel.OnOk = onOK;
            confirmPanel.OnClose = onClose;

            Util.SetActive(confirmPanel.closeBtnObj, (onClose != null) ? true : false);


            back_button_closeable bbc = go.AddComponent<back_button_closeable>();
            bbc.closeDelegator = confirmPanel.OnClickClose;
        }

        return confirmPanel;
    }

    static public void SetActive(GameObject go, bool active)
    {
        if (go != null)
        {
            go.SetActive(active);
        }
    }

    static public void AddTopNotification(string text)
    {

        if (UIManager.Instance && UIManager.Instance.topNotiUI != null)
        {
            UIManager.Instance.topNotiUI.AddNoti(text);
        }
    }
}
