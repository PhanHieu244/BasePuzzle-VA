using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogOverlay : MonoBehaviour
{
	private Image overlay;

	private void Awake()
	{
		overlay = GetComponent<Image>();
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
		
	}

	private void OnDialogOpened()
	{
		overlay.enabled = true;
	}

	private void OnDialogClosed()
	{
		overlay.enabled = false;
	}

	private void OnDestroy()
	{
		DialogController instance = DialogController.instance;
		instance.onDialogsOpened = (Action)Delegate.Remove(instance.onDialogsOpened, new Action(OnDialogOpened));
		DialogController instance2 = DialogController.instance;
		instance2.onDialogsClosed = (Action)Delegate.Remove(instance2.onDialogsClosed, new Action(OnDialogClosed));
	}
}
