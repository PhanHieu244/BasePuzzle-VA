using System;
using System.Collections;
using UnityEngine;

public class TimerText : MonoBehaviour
{
	public bool countUp = true;

	public bool runOnStart;

	public int timeValue;

	public bool showHour;

	public bool showMinute = true;

	public bool showSecond = true;

	public Action onCountDownComplete;

	private bool isRunning;

	private void Start()
	{
		UpdateText();
		if (runOnStart)
		{
			Run();
		}
	}

	public void Run()
	{
		if (!isRunning)
		{
			isRunning = true;
			StartCoroutine(UpdateClockText());
		}
	}

	private IEnumerator UpdateClockText()
	{
		while (isRunning)
		{
			UpdateText();
			yield return new WaitForSeconds(1f);
			if (countUp)
			{
				timeValue++;
			}
			else if (timeValue == 0)
			{
				if (onCountDownComplete != null)
				{
					onCountDownComplete();
				}
				Stop();
			}
			else
			{
				timeValue--;
			}
		}
	}

	public void SetTime(int value)
	{
		if (value < 0)
		{
			value = 0;
		}
		timeValue = value;
		UpdateText();
	}

	public void AddTime(int value)
	{
		timeValue += value;
		UpdateText();
	}

	private void UpdateText()
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds(timeValue);
		CExtension.SetText(value: (showHour && showMinute && showSecond) ? string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds) : ((!showHour || !showMinute) ? string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds) : string.Format("{0:D2}:{1:D2}", timeSpan.Hours, timeSpan.Minutes)), obj: base.gameObject);
	}

	public void Stop()
	{
		isRunning = false;
	}
}
