using System.Collections;
using UnityEngine;

public class BaseController : MonoBehaviour
{
	public GameMaster gameMaster;

	public string sceneName;

	public Music.Type music;

	protected int numofEnterScene;

	protected virtual void Awake()
	{
		if (GameMaster.instance == null && gameMaster != null)
		{
			Object.Instantiate(gameMaster);
		}
		//iTween.dimensionMode = iTween.DimensionMode.mode2D;
		CPlayerPrefs.useRijndael(true);
		numofEnterScene = CUtils.IncreaseNumofEnterScene(sceneName);
	}

	protected virtual void Start()
	{
		CPlayerPrefs.Save();
		if (JobWorker.instance.onEnterScene != null)
		{
			JobWorker.instance.onEnterScene(sceneName);
		}
		Music.instance.Play(music);
	}

	public virtual void OnApplicationPause(bool pause)
	{
		Debug.Log("On Application Pause");
		CPlayerPrefs.Save();
	}

	private IEnumerator SavePrefs()
	{
		while (true)
		{
			yield return new WaitForSeconds(5f);
			CPlayerPrefs.Save();
		}
	}
}
