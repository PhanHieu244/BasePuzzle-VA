using System;

[Serializable]
public abstract class BannerAdUnit : AdUnit
{
	public enum Position
	{
		Top = 0,
		Bottom = 1
	}

	public Position adPosition = Position.Bottom;

	protected bool hasToShowBannerOnLoadCompleted;

	public abstract void Show();

	public abstract void Hide();

	public abstract void Dispose();
}
