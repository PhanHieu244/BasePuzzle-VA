using UnityEngine;

public class ConfigController : MonoBehaviour
{
	public GameConfig config;

	public static ConfigController instance;

	public static GameConfig Config
	{
		get
		{
			return instance.config;
		}
	}

	private void Awake()
	{
		instance = this;
	}
}
