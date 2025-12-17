using System;
using UnityEngine;

public abstract class MaxSdkBase
{
	public enum ConsentDialogState
	{
		Unknown = 0,
		Applies = 1,
		DoesNotApply = 2
	}

	public enum BannerPosition
	{
		TopLeft = 0,
		TopCenter = 1,
		TopRight = 2,
		Centered = 3,
		BottomLeft = 4,
		BottomCenter = 5,
		BottomRight = 6
	}

	public class SdkConfiguration
	{
		public ConsentDialogState ConsentDialogState;
	}

	public struct Reward
	{
		public string Label;

		public int Amount;

		public override string ToString()
		{
			return "Reward: " + Amount + " " + Label;
		}

		public bool IsValid()
		{
			return !string.IsNullOrEmpty(Label) && Amount > 0;
		}
	}

	protected static void ValidateAdUnitIdentifier(string adUnitIdentifier, string debugPurpose)
	{
		if (string.IsNullOrEmpty(adUnitIdentifier))
		{
			Debug.LogError("No MAX Ads Ad Unit ID specified for: " + debugPurpose);
		}
	}

	protected static void InitCallbacks()
	{
		Type typeFromHandle = typeof(MaxSdkCallbacks);
		MaxSdkCallbacks component = new GameObject("MaxSdkCallbacks", typeFromHandle).GetComponent<MaxSdkCallbacks>();
		if (MaxSdkCallbacks.Instance != component)
		{
			Debug.LogWarning("It looks like you have the " + typeFromHandle.Name + " on a GameObject in your scene. Please remove the script from your scene.");
		}
	}
}
