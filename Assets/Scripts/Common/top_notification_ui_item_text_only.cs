using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class top_notification_ui_item_text_only : MonoBehaviour {
	public Text label;	

	void Awake() {
		//label = GetComponent<Text>();		
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetText( string t ) {
		StopCoroutine( "CO_Disappear" );
        label.color = new Color(label.color.r, label.color.g, label.color.b, 1f);				

		label.text = t;		
	}	

	public void Disappear() {
		StopCoroutine( "CO_Disappear" );
		StartCoroutine( "CO_Disappear" );
	}

	IEnumerator CO_Disappear() {
		float time = 0.5f;
		float elapsed = 0f;

		while( elapsed < time ) {
            label.color = new Color(label.color.r, label.color.g, label.color.b, 1f - (elapsed / time));
            //label.alpha = 1f - (elapsed / time);
			elapsed += Time.deltaTime;
			yield return null;
		}

        label.color = new Color(label.color.r, label.color.g, label.color.b, 0f);
        //label.alpha = 0f;
        Util.SetActive(gameObject, false);
	}
}
