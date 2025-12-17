using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CUtils
{
	private static AndroidJavaClass cls_UnityPlayer;

	private static AndroidJavaObject obj_Activity;

	static CUtils()
	{
		//AndroidJNI.AttachCurrentThread();
		//cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		//obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
	}

	public static void ShowToast(string message)
	{
		obj_Activity.Call("showToast", message);
	}

	public static bool IsAppInstalled(string packageName)
	{
		return obj_Activity.Call<bool>("isAppInstalled", new object[1] { packageName });
	}

	public static void PushNotification(int id)
	{
		obj_Activity.Call("pushNotificaiton", id);
	}

	public static void RateGame()
	{
		if (JobWorker.instance.onLink2Store != null)
		{
			JobWorker.instance.onLink2Store();
		}
		OpenStore();
		SetRateGame();
	}

	public static void OpenStore()
	{
		Application.OpenURL("https://play.google.com/store/apps/details?id=" + ConfigController.Config.androidPackageID);
	}

	public static void OpenStore(string id)
	{
		Application.OpenURL("https://play.google.com/store/apps/details?id=" + id);
	}

	public static void LikeFacebookPage(string faceID)
	{
		Application.OpenURL("fb://page/" + faceID);
		SetLikeFbPage(faceID);
	}

	public static string ReadFileContent(string path)
	{
		TextAsset textAsset = Resources.Load(path) as TextAsset;
		return (!(textAsset == null)) ? textAsset.text : null;
	}

	public static Vector3 CopyVector3(Vector3 ori)
	{
		return new Vector3(ori.x, ori.y, ori.z);
	}

	public static bool EqualVector3(Vector3 v1, Vector3 v2)
	{
		return Vector3.SqrMagnitude(v1 - v2) <= 1E-07f;
	}

	public static float GetSign(Vector3 A, Vector3 B, Vector3 M)
	{
		return Mathf.Sign((B.x - A.x) * (M.y - A.y) - (B.y - A.y) * (M.x - A.x));
	}

	public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
	{
		Vector3 vector = point - pivot;
		vector = Quaternion.Euler(angles) * vector;
		point = vector + pivot;
		return point;
	}

	public static void Shuffle<T>(T[] array)
	{
		for (int i = 0; i < array.Length; i++)
		{
			T val = array[i];
			int num = UnityEngine.Random.Range(0, array.Length);
			array[i] = array[num];
			array[num] = val;
		}
	}

	public static string[] SeparateLines(string lines)
	{
		return lines.Replace("\r\n", "\n").Replace("\r", "\n").Split("\n"[0]);
	}

	public static void ChangeSortingLayerRecursively(Transform root, string sortingLayerName, int offsetOrder = 0)
	{
		SpriteRenderer component = root.GetComponent<SpriteRenderer>();
		if (component != null)
		{
			component.sortingLayerName = sortingLayerName;
			component.sortingOrder += offsetOrder;
		}
		foreach (Transform item in root)
		{
			ChangeSortingLayerRecursively(item, sortingLayerName, offsetOrder);
		}
	}

	public static void ChangeRendererColorRecursively(Transform root, Color color)
	{
		SpriteRenderer component = root.GetComponent<SpriteRenderer>();
		if (component != null)
		{
			component.color = color;
		}
		foreach (Transform item in root)
		{
			ChangeRendererColorRecursively(item, color);
		}
	}

	public static void ChangeImageColorRecursively(Transform root, Color color)
	{
		Image component = root.GetComponent<Image>();
		if (component != null)
		{
			component.color = color;
		}
		foreach (Transform item in root)
		{
			ChangeImageColorRecursively(item, color);
		}
	}

	public static string GetFacePictureURL(string facebookID, int? width = null, int? height = null, string type = null)
	{
		string text = string.Format("/{0}/picture", facebookID);
		string text2 = ((!width.HasValue) ? string.Empty : ("&width=" + width));
		text2 += ((!height.HasValue) ? string.Empty : ("&height=" + height));
		text2 += ((type == null) ? string.Empty : ("&type=" + type));
		text2 += "&redirect=false";
		if (text2 != string.Empty)
		{
			text = text + "?g" + text2;
		}
		return text;
	}

	public static double GetCurrentTime()
	{
		return DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
	}

	public static double GetCurrentTimeInDays()
	{
		return DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalDays;
	}

	public static double GetCurrentTimeInMills()
	{
		return DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
	}

	public static double GetCurrentTimeInMins()
	{
		return DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMinutes;
	}

	public static T GetRandom<T>(params T[] arr)
	{
		return arr[UnityEngine.Random.Range(0, arr.Length)];
	}

	public static bool IsActionAvailable(string action, int time, bool availableFirstTime = true)
	{
		if (!CPlayerPrefs.HasKey(action + "_time"))
		{
			if (!availableFirstTime)
			{
				SetActionTime(action);
			}
			return availableFirstTime;
		}
		int num = (int)(GetCurrentTime() - GetActionTime(action));
		return num >= time;
	}

	public static double GetActionDeltaTime(string action)
	{
		if (GetActionTime(action) == 0.0)
		{
			return 0.0;
		}
		return GetCurrentTime() - GetActionTime(action);
	}

	public static void SetActionTime(string action)
	{
		CPlayerPrefs.SetDouble(action + "_time", GetCurrentTime());
	}

	public static double GetActionTime(string action)
	{
		return CPlayerPrefs.GetDouble(action + "_time");
	}

	public static BaseController GetGameController()
	{
		GameObject gameObject = GameObject.FindWithTag("GameController");
		return gameObject.GetComponent<BaseController>();
	}

	public static void SetLoggedInFb()
	{
		CPlayerPrefs.SetBool("logged_in_fb", true);
	}

	public static bool IsLoggedInFb()
	{
		return CPlayerPrefs.GetBool("logged_in_fb");
	}

	public static void SetBuyItem()
	{
		CPlayerPrefs.SetBool("buy_item", true);
	}

	public static void SetRemoveAds(bool value)
	{
		CPlayerPrefs.SetBool("remove_ads", value);
	}

	public static bool IsAdsRemoved()
	{
		return CPlayerPrefs.GetBool("remove_ads");
	}

	public static bool IsBuyItem()
	{
		return CPlayerPrefs.GetBool("buy_item");
	}

	public static void SetRateGame()
	{
		CPlayerPrefs.SetBool("rate_game", true);
	}

	public static bool IsGameRated()
	{
		return CPlayerPrefs.GetBool("rate_game");
	}

	public static void SetLikeFbPage(string id)
	{
		CPlayerPrefs.SetBool("like_page_" + id, true);
	}

	public static bool IsLikedFbPage(string id)
	{
		return CPlayerPrefs.GetBool("like_page_" + id);
	}

	public static void SetInitGame()
	{
		CPlayerPrefs.SetBool("init_game", true);
	}

	public static bool IsGameInitialzied()
	{
		return CPlayerPrefs.GetBool("init_game");
	}

	public static void SetAndroidVersion(string version)
	{
		CPlayerPrefs.SetString("android_version", version);
	}

	public static void SetIOSVersion(string version)
	{
		CPlayerPrefs.SetString("ios_version", version);
	}

	public static void SetWindowsPhoneVersion(string version)
	{
		CPlayerPrefs.SetString("wp_version", version);
	}

	public static string GetAndroidVersion()
	{
		return CPlayerPrefs.GetString("android_version", "1.0");
	}

	public static string GetIOSVersion()
	{
		return CPlayerPrefs.GetString("ios_version", "1.0");
	}

	public static string GetWindowsPhoneVersion()
	{
		return CPlayerPrefs.GetString("wp_version", "1.0");
	}

	public static void SetVersionCode(int versionCode)
	{
		CPlayerPrefs.SetInt("game_version_code", versionCode);
	}

	public static int GetVersionCode()
	{
		return CPlayerPrefs.GetInt("game_version_code");
	}

	public static string BuildStringFromCollection(ICollection values, char split = '|')
	{
		string text = string.Empty;
		int num = 0;
		foreach (object value in values)
		{
			text += value;
			if (num != values.Count - 1)
			{
				text += split;
			}
			num++;
		}
		return text;
	}

	public static List<T> BuildListFromString<T>(string values, char split = '|')
	{
		List<T> list = new List<T>();
		if (string.IsNullOrEmpty(values))
		{
			return list;
		}
		string[] array = values.Split(split);
		string[] array2 = array;
		foreach (string value in array2)
		{
			if (!string.IsNullOrEmpty(value))
			{
				T item = (T)Convert.ChangeType(value, typeof(T));
				list.Add(item);
			}
		}
		return list;
	}

	public static bool IsOutOfScreen(Vector3 pos, float padding = 0f)
	{
		float num = UICamera.instance.GetWidth() + padding;
		float num2 = UICamera.instance.GetHeight() + padding;
		return pos.x < 0f - num || pos.x > num || pos.y < 0f - num2 || pos.y > num2;
	}

	public static void SetNumofEnterScene(string sceneName, int value)
	{
		CPlayerPrefs.SetInt("numof_enter_scene_" + sceneName, value);
	}

	public static int GetNumofEnterScene(string sceneName)
	{
		return CPlayerPrefs.GetInt("numof_enter_scene_" + sceneName, 0);
	}

	public static int IncreaseNumofEnterScene(string sceneName)
	{
		int numofEnterScene = GetNumofEnterScene(sceneName);
		SetNumofEnterScene(sceneName, ++numofEnterScene);
		return numofEnterScene;
	}

	public static List<T> GetObjectInRange<T>(Vector3 position, float radius, int layerMask = -5) where T : class
	{
		List<T> list = new List<T>();
		Collider2D[] array = Physics2D.OverlapCircleAll(position, radius, layerMask);
		Collider2D[] array2 = array;
		foreach (Collider2D collider2D in array2)
		{
			list.Add(collider2D.gameObject.GetComponent(typeof(T)) as T);
		}
		return list;
	}

	public static Sprite GetSprite(string textureName, string spriteName)
	{
		Sprite[] array = Resources.LoadAll<Sprite>(textureName);
		Sprite[] array2 = array;
		foreach (Sprite sprite in array2)
		{
			if (sprite.name == spriteName)
			{
				return sprite;
			}
		}
		return null;
	}

	public static List<Transform> GetActiveChildren(Transform parent)
	{
		List<Transform> list = new List<Transform>();
		foreach (Transform item in parent)
		{
			if (item.gameObject.activeSelf)
			{
				list.Add(item);
			}
		}
		return list;
	}

	public static List<Transform> GetChildren(Transform parent)
	{
		List<Transform> list = new List<Transform>();
		foreach (Transform item in parent)
		{
			list.Add(item);
		}
		return list;
	}

	public static void LoadScene(int sceneIndex, bool useScreenFader = false)
	{
		if (useScreenFader)
		{
			ScreenFader.instance.GotoScene(sceneIndex);
		}
		else
		{
			SceneManager.LoadScene(sceneIndex);
		}
	}

	public static void ReloadScene(bool useScreenFader = false)
	{
		int buildIndex = SceneManager.GetActiveScene().buildIndex;
		LoadScene(buildIndex, useScreenFader);
	}

	public static void SetLeaderboardScore(int score)
	{
		CPlayerPrefs.SetInt("leaderboard_score", score);
	}

	public static int GetLeaderboardScore()
	{
		return CPlayerPrefs.GetInt("leaderboard_score");
	}

	public static void CheckConnection(MonoBehaviour behaviour, Action<int> connectionListener)
	{
		behaviour.StartCoroutine(ConnectUrl("http://www.google.com", connectionListener));
	}

	private static IEnumerator ConnectUrl(string url, Action<int> connectionListener)
	{
		WWW www = new WWW(url);
		yield return www;
		if (www.error != null)
		{
			connectionListener(1);
		}
		else if (string.IsNullOrEmpty(www.text))
		{
			connectionListener(2);
		}
		else
		{
			connectionListener(0);
		}
	}

	public static void CheckDisconnection(MonoBehaviour behaviour, Action onDisconnected)
	{
		behaviour.StartCoroutine(ConnectUrl("http://www.google.com", onDisconnected));
	}

	private static IEnumerator ConnectUrl(string url, Action onDisconnected)
	{
		WWW www2 = new WWW(url);
		yield return www2;
		if (www2.error != null)
		{
			yield return new WaitForSeconds(2f);
			www2 = new WWW(url);
			yield return www2;
			if (www2.error != null)
			{
				onDisconnected();
			}
		}
	}

	public static void ShowInterstitialAd()
	{
		if (LevelController.levelCompletions > 2 && !IsBuyItem() && !IsAdsRemoved())
		{
			AdNetworksManager.instance.ShowInterstitial(delegate
			{
				Debug.Log("Interstitial Shown");
			}, string.Empty);
		}
	}

	public static void ShowBannerAd()
	{
		if (!IsBuyItem() && !IsAdsRemoved())
		{
			AdNetworksManager.instance.ShowBanner();
		}
	}

	public static void CloseBannerAd()
	{
		AdNetworksManager.instance.HideBanner();
	}

	public static void ShowFixedBannerAd()
	{
		if (!IsBuyItem() && !IsAdsRemoved() && JobWorker.instance.onShowFixedBanner != null)
		{
			JobWorker.instance.onShowFixedBanner();
		}
	}

	public static void SetAutoSigninGPS(int value)
	{
		CPlayerPrefs.SetInt("auto_sign_in_gps", value);
	}

	public static int GetAutoSigninGPS()
	{
		return CPlayerPrefs.GetInt("auto_sign_in_gps");
	}

	public static IEnumerator LoadPicture(string url, Action<Texture2D> callback, int width, int height, bool useCached = true)
	{
		string localPath = GetLocalPath(url);
		bool loaded = false;
		if (useCached)
		{
			loaded = LoadFromLocal(callback, localPath, width, height);
		}
		if (!loaded)
		{
			WWW www = new WWW(url);
			yield return www;
			if (www.isDone && string.IsNullOrEmpty(www.error))
			{
				callback(www.texture);
				File.WriteAllBytes(localPath, www.bytes);
			}
			else
			{
				LoadFromLocal(callback, localPath, width, height);
			}
		}
	}

	private static string GetLocalPath(string url)
	{
		string fileName = Path.GetFileName(new Uri(url).LocalPath);
		return Application.persistentDataPath + "/" + fileName;
	}

	public static IEnumerator CachePicture(string url, Action<bool> result)
	{
		string localPath = GetLocalPath(url);
		WWW www = new WWW(url);
		yield return www;
		if (www.isDone && string.IsNullOrEmpty(www.error))
		{
			File.WriteAllBytes(localPath, www.bytes);
			if (result != null)
			{
				result(true);
			}
		}
		else if (result != null)
		{
			result(false);
		}
	}

	public static bool IsCacheExists(string url)
	{
		return File.Exists(GetLocalPath(url));
	}

	private static bool LoadFromLocal(Action<Texture2D> callback, string localPath, int width, int height)
	{
		if (File.Exists(localPath))
		{
			byte[] data = File.ReadAllBytes(localPath);
			Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGB24, false);
			texture2D.LoadImage(data);
			if (texture2D != null)
			{
				callback(texture2D);
				return true;
			}
		}
		return false;
	}

	public static Sprite CreateSprite(Texture2D texture, int width, int height)
	{
		return Sprite.Create(texture, new Rect(0f, 0f, width, height), new Vector2(0.5f, 0.5f), 100f);
	}

	public static List<List<T>> Split<T>(List<T> source, Predicate<T> split)
	{
		List<List<T>> list = new List<List<T>>();
		bool flag = false;
		for (int i = 0; i < source.Count; i++)
		{
			T val = source[i];
			if (split(val))
			{
				flag = false;
				continue;
			}
			if (!flag)
			{
				flag = true;
				list.Add(new List<T>());
			}
			list[list.Count - 1].Add(val);
		}
		return list;
	}

	public static List<List<T>> GetArrList<T>(List<T> source, Predicate<T> take)
	{
		List<List<T>> list = new List<List<T>>();
		bool flag = false;
		foreach (T item in source)
		{
			if (take(item))
			{
				if (!flag)
				{
					flag = true;
					list.Add(new List<T>());
				}
				list[list.Count - 1].Add(item);
			}
			else
			{
				flag = false;
			}
		}
		return list;
	}

	public static List<T> ToList<T>(T obj)
	{
		List<T> list = new List<T>();
		list.Add(obj);
		return list;
	}

	public static int ChooseRandomWithProbs(float[] probs)
	{
		float num = 0f;
		foreach (float num2 in probs)
		{
			num += num2;
		}
		float num3 = UnityEngine.Random.value * num;
		for (int j = 0; j < probs.Length; j++)
		{
			if (num3 < probs[j])
			{
				return j;
			}
			num3 -= probs[j];
		}
		return probs.Length - 1;
	}

	public static bool IsObjectSeenByCamera(Camera camera, GameObject gameObj, float delta = 0f)
	{
		Vector3 vector = camera.WorldToViewportPoint(gameObj.transform.position);
		return vector.z > 0f && vector.x > 0f - delta && vector.x < 1f + delta && vector.y > 0f - delta && vector.y < 1f + delta;
	}

	public static Vector3 GetMiddlePoint(Vector3 begin, Vector3 end, float delta = 0f)
	{
		Vector3 vector = Vector3.Lerp(begin, end, 0.5f);
		Vector3 vector2 = end - begin;
		Vector3 normalized = new Vector3(0f - vector2.y, vector2.x, 0f).normalized;
		return vector + normalized * delta;
	}

	public static AnimationClip GetAnimationClip(Animator anim, string name)
	{
		RuntimeAnimatorController runtimeAnimatorController = anim.runtimeAnimatorController;
		for (int i = 0; i < runtimeAnimatorController.animationClips.Length; i++)
		{
			if (runtimeAnimatorController.animationClips[i].name == name)
			{
				return runtimeAnimatorController.animationClips[i];
			}
		}
		return null;
	}

	public static void Swap<T>(ref T lhs, ref T rhs)
	{
		T val = lhs;
		lhs = rhs;
		rhs = val;
	}

	public static void Pause()
	{
	}
}
