using UnityEngine;

public class back_button_closeable : MonoBehaviour
{
    [System.NonSerialized] public GameObject targetObj;
    [System.NonSerialized] public int score;
    [System.NonSerialized] public bool is_window_active = true;
    

    public delegate bool IsCloseable();
    public IsCloseable isCloseable;

    public delegate void CloseDelegator();
    public CloseDelegator closeDelegator;    

    void Awake()
    {        
    }

    void OnEnable()
    {

        back_button_listener.closeables.Add(this);
        
    }

    void OnDisable()
    {
        if (back_button_listener.closeables.Contains(this))
        {
            back_button_listener.closeables.Remove(this);
        }
    }

    public void Close()
    {
        Debug.Log("test 2");
        if (closeDelegator != null)
        {
            closeDelegator();
            return;
        }

        GameObject obj = targetObj;
        if (obj == null)
        {
            obj = gameObject;
        }

        Debug.Log("test 3");
        Destroy(obj);
    }
}
