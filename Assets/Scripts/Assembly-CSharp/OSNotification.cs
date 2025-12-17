public class OSNotification
{
	public enum DisplayType
	{
		Notification = 0,
		InAppAlert = 1,
		None = 2
	}

	public bool isAppInFocus;

	public bool shown;

	public bool silentNotification;

	public int androidNotificationId;

	public DisplayType displayType;

	public OSNotificationPayload payload;
}
