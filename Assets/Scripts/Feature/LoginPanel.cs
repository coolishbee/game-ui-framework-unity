using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanel : MonoBehaviour
{

    private void Awake()
    {
        UIManager.Instance.Init();
#if UNITY_ANDROID || UNITY_EDITOR
        gameObject.AddComponent<back_button_listener>();
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickLoginBtn()
    {
        Util.ShowSimpleConfirmPanel("notice", "아이디를 다시 입력해주세요.",
            delegate () {
                Debug.Log("yes");
            }, delegate () {
                Debug.Log("no");
            });
    }

    public void OnClickOneBtn()
    {
        Util.ShowSimpleConfirmPanel("알림", "One Button", delegate () { Debug.Log("yes"); });
    }

    public void OnClickToastBtn()
    {
        Util.AddTopNotification("111Percent Toast Message!!!");
    }
}
