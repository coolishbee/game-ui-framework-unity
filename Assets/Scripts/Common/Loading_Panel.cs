using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading_Panel : MonoBehaviour {

    public Text Label_Text;

    public Image Sprite_Black;
    public RawImage BG_Texture;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowBlackBG(bool show)
    {
        Sprite_Black.color = new Color(1, 1, 1, (show ? 1f : 0.01f));
    }

    public void ShowText(string text)
    {
        Label_Text.text = text;

        var col = Label_Text.color;
        col.a = 1f;
        TweenAlpha ta = TweenAlpha.Begin(Label_Text.gameObject, 1f, 0.5f);
        ta.method = EaseType.linear;
        ta.style = TweenAlpha.Style.PingPong;
        ta.delay = 2f;

        ShowBlackBG(true);
    }

    public void ShowBackground(Texture tex)
    {
        BG_Texture.texture = tex;
    }
}
