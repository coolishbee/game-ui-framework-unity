using UnityEngine;
using System.Collections.Generic;

public class back_button_listener : MonoBehaviour
{
    static public List<back_button_closeable> closeables = new List<back_button_closeable>();

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {        
        if (Input.GetKeyDown(KeyCode.Escape))
        {            
            OnBackButton();
        }
    }    

    void OnBackButton()
    {        
        closeables.Sort(delegate (back_button_closeable x, back_button_closeable y) {
            return y.score.CompareTo(x.score);
        });
        
        back_button_closeable target = null;
        for (int i = 0; i < closeables.Count; i++)
        {
            if (closeables[i].is_window_active)
            {
                target = closeables[i];
                break;
            }
        }
        
        if (target == null)
            return;
        
        target.Close();
    }
}
