using System;
using UnityEngine;

[Serializable]
public abstract class InterstitialAdUnit : AdUnit
{
	protected Action _onAdClosed;

	public Action onAdClosed;

	[HideInInspector]
	public bool isShowing;

	public new abstract bool IsLoaded();

	public abstract void Show(Action onAdClosed);

	public abstract void Dispose();
}
