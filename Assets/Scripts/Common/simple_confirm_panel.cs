/* FileName : simple_confirm_panel
 * Author : jaychun
 * Date : 2018-01-17
 * Desc : Common Basic Popup UI
 * 
 */

using UnityEngine;
using UnityEngine.UI;

public class simple_confirm_panel : MonoBehaviour
{
    public Text label_title;
    public Text label_contents;
    public Text label_OkBtn;
    public Text label_CancelBtn;

    public Button button_OK;    

    public GameObject closeBtnObj;    

    public delegate void DelegatorOnOk();
    public DelegatorOnOk OnOk;
    public delegate void DelegatorOnClose();
    public DelegatorOnClose OnClose;

    bool isAddUIPanelDepthSetterAtInit = false;
    public void SetAddUIPanelDepthSetter()
    {
        isAddUIPanelDepthSetterAtInit = true; 
    }

    // Use this for initialization
    void Awake()
    {        
    }    

    public void OnClickOK()
    {
        if (OnOk != null)
        {
            OnOk();
        }
        Close();
    }

    void Close()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        
    }

    public void OnClickClose()
    {
        if(OnClose != null)
        {
            OnClose();
        }
        Close();
    }    
}
