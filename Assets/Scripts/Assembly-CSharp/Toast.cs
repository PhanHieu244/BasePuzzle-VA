using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{
	private class AToast
	{
		public string msg;

		public float time;

		public AToast(string msg, float time)
		{
			this.msg = msg;
			this.time = time;
		}
	}

	public RectTransform backgroundTransform;

	public RectTransform messageTransform;

	public static Toast instance;

	[HideInInspector]
	public bool isShowing;

	private Queue<AToast> queue = new Queue<AToast>();

	private void Awake()
	{
		instance = this;
		SetEnabled(false);
	}

	public void SetMessage(string msg)
	{
		messageTransform.GetComponent<Text>().text = msg;
		Timer.Schedule(this, 0f, delegate
		{
			backgroundTransform.sizeDelta = new Vector2(messageTransform.GetComponent<Text>().preferredWidth + 30f, backgroundTransform.sizeDelta.y);
		});
	}

	private void Show(AToast aToast)
	{
		SetMessage(aToast.msg);
		SetEnabled(true);
		GetComponent<Animator>().SetBool("show", true);
		Invoke("Hide", aToast.time);
		isShowing = true;
	}

	public void ShowMessage(string msg, float time = 1.5f)
	{
		AToast item = new AToast(msg, time);
		queue.Enqueue(item);
		ShowOldestToast();
	}

	private void Hide()
	{
		GetComponent<Animator>().SetBool("show", false);
		Invoke("CompleteHiding", 1f);
	}

	private void CompleteHiding()
	{
		SetEnabled(false);
		isShowing = false;
		ShowOldestToast();
	}

	private void ShowOldestToast()
	{
		if (queue.Count != 0 && !isShowing)
		{
			AToast aToast = queue.Dequeue();
			Show(aToast);
		}
	}

	private void SetEnabled(bool enabled)
	{
		foreach (Transform item in base.transform)
		{
			item.gameObject.SetActive(enabled);
		}
	}
}
