using System;
using UnityEngine;

public class ButtonFacebook : MonoBehaviour
{

	public void OnClick()
	{

	}

	private void Start()
	{

	}

	private void OnDisable()
	{

	}

	private void onInitializationSuccess()
	{

	}

	private void OnLoginSuccess()
	{
		GameState.hint.ChangeValue(10);
		SetVisibility(false);
	}

	private void SetVisibility(bool value)
	{
		base.gameObject.SetActive(value);
	}
}
