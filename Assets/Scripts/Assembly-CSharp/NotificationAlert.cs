using UnityEngine;

public class NotificationAlert : CMonoBehaviour
{
	public enum Type
	{
		taskComplete = 0
	}

	public GameObject notificationObj;

	public Animator anim;

	public Type alertType;

	public float showAnimationTime = 5f;

	protected virtual void Start()
	{
		if (anim == null)
		{
			anim = GetComponent<Animator>();
		}
		bool active = IsNotificationAlertVisible();
		notificationObj.SetActive(active);
	}

	protected void OnAlertNotification()
	{
		notificationObj.SetActive(true);
		anim.SetBool("show", true);
		Invoke("OnAlertComplete", showAnimationTime);
	}

	protected void OnAlertComplete()
	{
		if (anim.isActiveAndEnabled)
		{
			anim.SetBool("show", false);
		}
	}

	public void OnHideNotification()
	{
		notificationObj.SetActive(false);
	}

	public virtual bool IsNotificationAlertVisible()
	{
		return false;
	}
}
