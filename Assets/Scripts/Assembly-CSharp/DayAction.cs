using UnityEngine;
using UnityEngine.Events;

public class DayAction : MonoBehaviour
{
	public int dayInterval = 1;

	public string actionName = string.Empty;

	public UnityEvent onActionReached;

	private void Start()
	{
		int num = (int)CUtils.GetCurrentTimeInDays();
		int @int = CPlayerPrefs.GetInt("day_action_" + actionName, -1);
		if (num - @int >= dayInterval)
		{
			CPlayerPrefs.SetInt("day_action_" + actionName, num);
			if (onActionReached != null)
			{
				onActionReached.Invoke();
			}
		}
	}
}
