using System;
using UnityEngine;
using UnityEngine.UI;

public class HintText : MonoBehaviour
{
	private void Start()
	{
		StoredValue<int> hint = GameState.hint;
		hint.onValueChanged = (Action)Delegate.Combine(hint.onValueChanged, new Action(OnValueChanged));
		OnValueChanged();
	}

	private void OnValueChanged()
	{
		GetComponent<Text>().text = GameState.hint.GetValue().ToString();
	}

	private void OnDestroy()
	{
		StoredValue<int> hint = GameState.hint;
		hint.onValueChanged = (Action)Delegate.Remove(hint.onValueChanged, new Action(OnValueChanged));
	}
}
