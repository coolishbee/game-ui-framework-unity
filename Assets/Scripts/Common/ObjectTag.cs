using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ObjectType
{
	None,	
	
	Icon,
	Button,
	Dialog,
	ListItem,
	UI,

	UIPopup,
	Fullscreen,    

    Max
}

public class ObjectTag : MonoBehaviour
{
	public ObjectType	type = ObjectType.None;
	public string		tagName = null;
	bool 		added = false;
	[System.NonSerialized] public GameObject myGameObject;

	bool initiated = false;

	void Awake()
	{
		myGameObject = gameObject;

		if (type != ObjectType.None && !string.IsNullOrEmpty(tagName))
		{
			Init ();
		}
	}
	
	public void SetTag(ObjectType _type, string _tagName)
	{
		if (tagName != null || type != ObjectType.None)
		{            
            Debug.Log("Cannot modify type or tagName!");
			return;
		}
		
		type = _type;
		tagName = _tagName;

		Init ();
	}
	
	static string GetKey(ObjectType _type, string _tagName)
	{
		return _tagName;
	}
	
	string GetKey()
	{
		return GetKey( type, tagName );
	}
	
	public override string ToString ()
	{
		return "["+ type.ToString() + ":" + tagName +"]";
	}

	// Use this for initialization
	void Start ()
	{
		Init();
	}
	
	public void Init()
	{
		if (!initiated)
		{
			initiated = true;

			if (!added)
			{
				Add(this);
				added = true;							
			}
		}
	}
	
	void OnDestroy()
	{
		if (added)
		{
			Remove(this);
			added = false;
		}
	}
	
	public void SetDestroy()
	{
		Remove(this);
		added = false;
		
		Destroy(this);
	}
	
	void OnEnable()
	{
		if (added)
		{
			//TagUtil.DoAction(type, tagName);
		}
	}
	
	//
	// static manager
	//
	static Dictionary<string, ObjectTag> []	objects = CreateObjectDictinary();
	
	static Dictionary<string, ObjectTag> [] CreateObjectDictinary()
	{
		Dictionary<string, ObjectTag> [] _objects = new Dictionary<string, ObjectTag> [(int)ObjectType.Max];
		for (int i=0; i<(int)ObjectType.Max; i++)
		{
			_objects[i] = new Dictionary<string, ObjectTag>();
		}
		return _objects;
	}
		
	static public void RemoveTag(GameObject go)
	{
		ObjectTag oldOT = go.GetComponent<ObjectTag>();
		if (oldOT != null)
		{
			oldOT.SetDestroy();
		}
	}
	
	static public ObjectTag Tagging(GameObject go, ObjectType type, string tagName)
	{
		ObjectTag oldOT = go.GetComponent<ObjectTag>();
		if (oldOT != null)
		{
			oldOT.SetDestroy();
		}
			
		ObjectTag ot = go.AddComponent<ObjectTag>();
		ot.SetTag(type, tagName);
		ot.Start();
		
		return ot;
	}

	static public void DestroyObject(ObjectType type, string tagName)
	{
		ObjectTag oldOT = Find(type, tagName);
		if (oldOT != null)
		{
			oldOT.SetDestroy();
		}
	}
	
	static public void DestroyGameObject(ObjectType type, string tagName)
	{
		ObjectTag oldOT = Find(type, tagName);
		if (oldOT != null)
		{
			Destroy(oldOT.gameObject);
			oldOT.SetDestroy();
		}
	}
	
	static private void Add(ObjectTag tag)
	{
		//if (DebugHelper.enableLog) Debug.Log("Add ObjectTag: "+tag);
		
		if (objects[(int)tag.type].ContainsKey(tag.GetKey()))
		{
//			Debug.LogError( "already exist tag! " + tag.GetKey() );
			Remove(tag, true);
		}
		
		objects[(int)tag.type].Add( tag.GetKey(), tag );
		
		//TagUtil.DoAction(tag.type, tag.tagName);
	}
	
	static private void Remove(ObjectTag tag)
	{
		Remove (tag, false);
	}
	
	static private void Remove(ObjectTag tag, bool forceRemove)
	{
		//if (DebugHelper.enableLog) Debug.Log("Remove ObjectTag: "+tag);
			
		if (forceRemove)
		{
			objects[(int)tag.type].Remove(tag.GetKey());
		}
		else
		{
			ObjectTag oldTag;
			if (objects[(int)tag.type].TryGetValue(tag.GetKey(), out oldTag))
			{
				if (oldTag.gameObject == tag.gameObject)
				{
					objects[(int)tag.type].Remove(tag.GetKey());
				}
			}		
		}
	}
	
	static public ObjectTag Find(ObjectType type, string tagName)
	{
		string key = ObjectTag.GetKey(type, tagName);
		ObjectTag objectTag;
		if (objects[(int)type].TryGetValue(key, out objectTag))
		{			
			return objectTag;
		}
		return null;
	}

	static public bool IsActive(ObjectType type)
	{
		return objects[(int)type].Count > 0;
	}
	
	static public bool IsActive(ObjectType type, string tagName)
	{
		string key = ObjectTag.GetKey(type, tagName);
		ObjectTag objectTag;
		if (objects[(int)type].TryGetValue(key, out objectTag))
		{			
			return objectTag.gameObject.activeInHierarchy;
		}
		return false;
	}
	
	static public bool HasTagType(ObjectType type, string tagTypeName)
	{
		string tagTypeKey = GetKey(type, tagTypeName);
        using (var e = objects[(int)type].GetEnumerator())
        {
            while (e.MoveNext())
            {
                KeyValuePair<string, ObjectTag> kv = e.Current;

    			if (kv.Key.StartsWith(tagTypeKey))
    			{
    				return true;
			    }
            }
		}
		return false;
	}
	
	static public List<ObjectTag> GetTagTypeList(ObjectType type, string tagTypeName)
	{
		List<ObjectTag>	tagList = new List<ObjectTag>();
		
		string tagTypeKey = GetKey(type, tagTypeName);
        using (var e = objects[(int)type].GetEnumerator())
        {
            while (e.MoveNext())
            {
                KeyValuePair<string, ObjectTag> kv = e.Current;
    		
    			if (kv.Key.StartsWith(tagTypeKey))
    			{
    				tagList.Add (kv.Value);
    			}
            }
		}
		return tagList;
	}
	
	static public Dictionary<string, ObjectTag> GetTagTypeList(ObjectType type)
	{
		 return objects[(int)type];
	}
	
	static public void SendMessage(ObjectType type, string functionName)
	{
        using (var e = GetTagTypeList(type).GetEnumerator())
        {
            while (e.MoveNext())
            {
                KeyValuePair<string, ObjectTag> kv = e.Current;
    			kv.Value.gameObject.SendMessage(functionName, SendMessageOptions.DontRequireReceiver);
            }
		}
	}
	
	static public void SendMessageValue(ObjectType type, string functionName, object obj)
	{
        using (var e = GetTagTypeList(type).GetEnumerator())
        {
            while (e.MoveNext())
            {
                KeyValuePair<string, ObjectTag> kv = e.Current;

                kv.Value.gameObject.SendMessage(functionName, obj, SendMessageOptions.DontRequireReceiver);
            }
		}
	}
	
	static public void SendMessage(ObjectType type, string tagName, string functionName)
	{
		ObjectTag ot = ObjectTag.Find(type, tagName);
		if (ot != null)
		{
			ot.gameObject.SendMessage(functionName, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	static public void SendMessage(ObjectType type, string tagName, string functionName, object obj)
	{
		ObjectTag ot = ObjectTag.Find(type, tagName);
		if (ot != null)
		{
			ot.gameObject.SendMessage(functionName, obj, SendMessageOptions.DontRequireReceiver);
		}
	}	
	
	static public void BroadcastMessage(ObjectType type, string tagName, string functionName)
	{
		ObjectTag ot = ObjectTag.Find(type, tagName);
		if (ot != null)
		{
			ot.gameObject.BroadcastMessage(functionName, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	static public void BroadcastMessage(ObjectType type, string tagName, string functionName, object obj)
	{
		ObjectTag ot = ObjectTag.Find(type, tagName);
		if (ot != null)
		{
			ot.gameObject.BroadcastMessage(functionName, obj, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	static public void SetActive(ObjectType type, string tagName, bool state)
	{
		ObjectTag ot = ObjectTag.Find(type, tagName);
		if (ot != null)
		{
			ot.gameObject.SetActive(state);
		}
	}	
	
	static public void Destroy(ObjectType type)
	{
		//string startKey = type.ToString()+"@";
		
        using (var e = objects[(int)type].GetEnumerator())
        {
            while (e.MoveNext())
            {
                KeyValuePair<string, ObjectTag> kv = e.Current;

                //if (kv.Key.StartsWith(startKey))
                {
                    ObjectTag ot = kv.Value;
                    if (ot.gameObject != null)
                    {
                        Destroy(ot.gameObject);
                    }
                }
            }
        }
	}
}