using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	private static float _destroyedTime = -1f;

	public static T instance
	{
		get
		{
			if (_instance == null)
			{
				InitInstance();
			}
			return _instance;
		}
	}

	private void Awake()
	{
		Init();
	}

	protected void Init()
	{
		if (_instance != null)
		{
			if (_instance != this)
			{
				Object.Destroy(base.gameObject);
			}
		}
		else
		{
			_instance = base.gameObject.GetComponent<T>();
			_destroyedTime = -1f;
			Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	private static void InitInstance()
	{
		if (_destroyedTime == Time.time)
		{
			return;
		}
		if (_instance == null)
		{
			_instance = Object.FindObjectOfType<T>();
			if (!(_instance != null))
			{
				T[] array = Resources.FindObjectsOfTypeAll<T>();
				if (array.Length > 0)
				{
					GameObject gameObject = Object.Instantiate(array[0]) as GameObject;
					_instance = gameObject.GetComponent<T>();
				}
				if (!(_instance != null))
				{
					GameObject gameObject2 = new GameObject();
					gameObject2.name = typeof(T).Name;
					Object.DontDestroyOnLoad(gameObject2);
					_instance = gameObject2.AddComponent<T>();
				}
			}
		}
		else
		{
			Object.DontDestroyOnLoad(_instance.gameObject);
		}
	}

	private void OnDestroy()
	{
		Debug.Log("Singleton destroy");
		_instance = (T)null;
		_destroyedTime = Time.time;
	}
}
