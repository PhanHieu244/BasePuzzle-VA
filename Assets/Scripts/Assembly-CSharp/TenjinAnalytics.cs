using UnityEngine;

public class TenjinAnalytics : MonoBehaviour
{
	public static TenjinAnalytics instance;

	public string apiKey;

	private BaseTenjin tenjinInstance
	{
		get
		{
			return Tenjin.getInstance(apiKey);
		}
	}

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		Connect();
	}

	public void OptIn(bool value)
	{
		if (value)
		{
			tenjinInstance.OptIn();
		}
		else
		{
			tenjinInstance.OptOut();
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (!pauseStatus)
		{
			Connect();
		}
	}

	private void Connect()
	{
		tenjinInstance.Connect();
	}
}
