using UnityEngine;
using System.Collections;

public class top_notification_ui : MonoBehaviour {
	public RectTransform notiAnchor;

	GameObject prefabText;

	top_notification_ui_item_text_only[] unusedItems;
	top_notification_ui_item_text_only[] visibleItems;
	float[] timeStamp;
	int visibleItemOffset = 0;

	// Use this for initialization
	void Start () {
        prefabText = ResourceManager.Instance.GetUIPrefab("ui", "top_notification_ui_text");	
		unusedItems = new top_notification_ui_item_text_only[5];
		visibleItems = new top_notification_ui_item_text_only[5];
		timeStamp = new float[5];
		for( int i = 0; i < 5; i++ ) {
			visibleItems[i] = null;
			timeStamp[i] = -1f;

			GameObject go = Util.AddChild( notiAnchor.gameObject, prefabText );
			unusedItems[ i ] = go.GetComponent<top_notification_ui_item_text_only>();
            Util.SetActive(unusedItems[i].gameObject, false);            
		}
	}
	
	// Update is called once per frame
	void Update () {
		float expireTime = 3f;
		for( int i = 0; i < visibleItems.Length; i++ ) {
			if( visibleItems[i] == null ) 
				continue;

			float elapsed = Time.realtimeSinceStartup - timeStamp[i];
			if( elapsed > expireTime ) {
				visibleItems[i].Disappear();

				for( int j = 0; j < unusedItems.Length; j++ ) {
					if( unusedItems[j] == null ) {
						unusedItems[j] = visibleItems[i];
						visibleItems[i] = null;
						break;
					}
				}

				if( visibleItems[i] != null ) {
					Debug.LogError( "cannot find empty slot");
				}
			}
		}	
	}

	public void AddNoti( string text ) {
		top_notification_ui_item_text_only item = AllocItem();

		if( item == null ) 
			return;

        Util.SetActive(item.gameObject, true);

		item.transform.localPosition = Vector3.zero;
		item.transform.localScale = Vector3.one;
		item.SetText( text );
		timeStamp[visibleItemOffset] = Time.realtimeSinceStartup;
		visibleItems[ visibleItemOffset++ ] = item;

		if( visibleItemOffset >= visibleItems.Length ) visibleItemOffset -= visibleItems.Length;

		ArrangePosition();
	}

	void ArrangePosition() {
		float gap_h = 45f;
		int offset = visibleItemOffset-1;
		for( int i = 0; i < visibleItems.Length; i++ ) {
			int curOffset = offset - i;
			if( curOffset < 0 ) 
				curOffset += visibleItems.Length;

			if( visibleItems[curOffset] == null ) 
				continue;

			Transform trans = visibleItems[curOffset].transform;

			//iTween.MoveTo(trans.gameObject, iTween.Hash("y", gap_h*i, "easeType", "easeOutQuad", "loopType", "none", "islocal", true,"time",0.1f, "delay", 0 ) ) ;
		}
	}

	top_notification_ui_item_text_only AllocItem() {
		// pick item up among the unused
		for( int i = 0; i < unusedItems.Length; i++ ) {
			if( unusedItems[i] != null ) {
				top_notification_ui_item_text_only v = unusedItems[i];
				unusedItems[i] = null;
				return v;
			}
		}

		// pick up the oldest one
		int lastIdx = -1;
		for( int i = 0; i < visibleItems.Length-1; i++ ) {
			int curIdx = visibleItemOffset + i;
			if( curIdx >= visibleItems.Length ) 
				curIdx -= visibleItems.Length;

			if( visibleItems[ curIdx ] != null ) {
				lastIdx = curIdx;
				break;
			}
		}

		if( lastIdx == -1 ) {
			Debug.LogError( "something wrong.... ");
			return null;
		}

		top_notification_ui_item_text_only vv = visibleItems[ lastIdx ];
		visibleItems[ lastIdx ] = null;

		return vv;
	}
}
