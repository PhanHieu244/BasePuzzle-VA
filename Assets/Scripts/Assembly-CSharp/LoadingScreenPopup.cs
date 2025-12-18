using System;
using System.Collections;
using UnityEngine;

public class LoadingScreenPopup : MonoBehaviour
{
	private static LoadingScreenPopup _instance;

	[SerializeField]
	private GameObject _panel;

	[SerializeField]
	private float _timeToShow = 4f;

	private bool _isShowing;

	public static LoadingScreenPopup instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = UnityEngine.Object.FindObjectOfType<LoadingScreenPopup>();
			}
			if (_instance == null)
			{
				_instance = new GameObject("LoadingScreen").AddComponent<LoadingScreenPopup>();
				UnityEngine.Object.DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	public void Show()
	{
		if (!_isShowing)
		{
			Debug.Log("### ADS POPUP - SUB");
			StartCoroutine(Showing());
		}
	}

	private void OnAdClosed()
	{
		Debug.Log("### ADS POPUP - Closed/UNSUB");
		_isShowing = false;
		Debug.Log("### ADS POPUP - HIDE2");
		_panel.SetActive(false);
	}

	private IEnumerator Showing()
	{
		_panel.SetActive(true);
		_isShowing = true;
		float timer = 0f;
		while (_isShowing && timer < _timeToShow)
		{
			yield return null;
			timer += Time.deltaTime;
		}
		_isShowing = false;
		if (timer >= _timeToShow)
		{
			Debug.Log("### ADS POPUP - UNSUB");
		}
		Debug.Log("### ADS POPUP - HIDE1");
		_panel.SetActive(false);
	}
}
