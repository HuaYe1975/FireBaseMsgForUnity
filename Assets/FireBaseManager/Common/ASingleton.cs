using UnityEngine;

public abstract class ASingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	public abstract string TypeName { get; }

	public bool IsMarkedForDestroy { get; private set; }

	protected virtual void Awake()
	{
		if (instance != null && instance != this) {
			Debug.LogWarning("Destroying concurrent instance of type : " +  typeof(T).ToString() + " on gameobject : " + gameObject.name);
			DestroyInstance();
			return;
		}
		
		Debug.Log("REGISTER " + TypeName + " of type : " + typeof(T).ToString() + " on gameobject : " + gameObject.name);
		instance = this as T;
	}

	protected virtual void DestroyInstance()
	{
		Destroy(this);
		IsMarkedForDestroy = true;
	}

	protected virtual void OnDestroy()
	{
		Debug.Log("DESTROY " + TypeName + " instance of type : " + typeof(T).ToString() + " on gameobject : " + gameObject.name);
		if (instance == this) {
			Debug.Log("RESET " + TypeName + " instance value of " +  typeof(T).ToString() + " from gameobject : " + gameObject.name);
			instance = null;
		}
	}

	public static T TryFindInstance()
	{
		if( instance == null )
		{
			instance = GameObject.FindObjectOfType<T>();
		}
		
		return instance;
	}

	public static bool HasInstance()
	{
		return (TryFindInstance() != null);
	}

	protected static T instance = null;
	public static T GetInstance
	{
		get
		{
			return TryFindInstance();
		}
	}
}