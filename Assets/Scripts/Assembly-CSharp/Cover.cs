using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cover : MonoBehaviour
{
	public RectTransform left;

	public RectTransform right;

	public RectTransform above;

	public RectTransform below;

	private void Start()
	{
		UpdateCover();
		UICamera instance = UICamera.instance;
		instance.onScreenSizeChanged = (Action)Delegate.Combine(instance.onScreenSizeChanged, new Action(UpdateCover));
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		UpdateCover();
		UICamera instance = UICamera.instance;
		instance.onScreenSizeChanged = (Action)Delegate.Combine(instance.onScreenSizeChanged, new Action(UpdateCover));
	}

	private void UpdateCover()
	{
		float width = UICamera.instance.GetWidth();
		float height = UICamera.instance.GetHeight();
		float virtualWidth = UICamera.instance.virtualWidth;
		float virtualHeight = UICamera.instance.virtualHeight;
		float num = (virtualWidth - width * 200f) / 2f;
		float num2 = (virtualHeight - height * 200f) / 2f;
		left.gameObject.SetActive(num > 0.0001f);
		right.gameObject.SetActive(num > 0.0001f);
		above.gameObject.SetActive(num2 > 0.0001f);
		below.gameObject.SetActive(num2 > 0.0001f);
		float x = ((!UICamera.instance.landscape) ? 800 : 400);
		float y = ((!UICamera.instance.landscape) ? 400 : 800);
		if (left.sizeDelta.x < num)
		{
			left.sizeDelta = new Vector2(num, left.sizeDelta.y);
			right.sizeDelta = new Vector2(num, right.sizeDelta.y);
		}
		else
		{
			left.sizeDelta = new Vector2(x, left.sizeDelta.y);
			right.sizeDelta = new Vector2(x, right.sizeDelta.y);
		}
		if (above.sizeDelta.y < num2)
		{
			above.sizeDelta = new Vector2(above.sizeDelta.x, num2);
			below.sizeDelta = new Vector2(below.sizeDelta.x, num2);
		}
		else
		{
			above.sizeDelta = new Vector2(above.sizeDelta.x, y);
			below.sizeDelta = new Vector2(below.sizeDelta.x, y);
		}
	}
}
