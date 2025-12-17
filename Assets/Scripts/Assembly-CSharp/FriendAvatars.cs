using System.Collections.Generic;
using UnityEngine;

public class FriendAvatars : MonoBehaviour
{
	public GameObject friendAvatarPrefab;

	public Transform levelsTransform;

	private void Start()
	{
		FaceController.ChangeFriendLevels();
		string friendAvatarUrls = FaceController.GetFriendAvatarUrls();
		string friendLevels = FaceController.GetFriendLevels();
		if (friendAvatarUrls == string.Empty || friendLevels == string.Empty)
		{
			return;
		}
		List<string> list = CUtils.BuildListFromString<string>(friendAvatarUrls);
		List<int> list2 = CUtils.BuildListFromString<int>(friendLevels);
		int num = 0;
		int[] array = new int[50];
		foreach (int item in list2)
		{
			array[item - 1]++;
		}
		int[] array2 = new int[50];
		foreach (string item2 in list)
		{
			if (num >= list2.Count)
			{
				break;
			}
			int num2 = list2[num];
			if (num2 <= 50)
			{
				Transform child = levelsTransform.GetChild(num2 - 1);
				GameObject gameObject = Object.Instantiate(friendAvatarPrefab);
				gameObject.transform.SetParent(base.transform);
				gameObject.transform.position = child.position + new Vector3(0.1f * ((float)(-(array[num2 - 1] - 1)) / 2f + (float)array2[num2 - 1]), 0f);
				gameObject.transform.localScale = Vector3.one;
				FriendAvatar component = gameObject.GetComponent<FriendAvatar>();
				component.url = item2;
				component.index = num;
				array2[num2 - 1]++;
			}
			num++;
		}
	}
}
