using System;
using System.Collections.Generic;
using UnityEngine;

public class FaceController
{
	public const string INVITABLE_OFFSET = "invitable_offset";

	public static int GetInvitableOffset()
	{
		return CPlayerPrefs.GetInt("invitable_offset", 0);
	}

	public static void SetInvitableOffet(int value)
	{
		CPlayerPrefs.SetInt("invitable_offset", value);
	}

	public static void IncreaseInvitableOffset()
	{
		int invitableOffset = GetInvitableOffset();
		SetInvitableOffet(invitableOffset + 1);
	}

	public static void SetPermissions(string permissions)
	{
		CPlayerPrefs.SetString("facebook_permissions", permissions);
	}

	public static string GetPermissions()
	{
		return CPlayerPrefs.GetString("facebook_permissions", string.Empty);
	}

	public static bool HasPublishActions()
	{
		string permissions = GetPermissions();
		return permissions.Contains("publish_actions");
	}

	public static void SetFriendAvatarUrls(string avatarUrls)
	{
		CPlayerPrefs.SetString("facebook_friends_avatar_urls", avatarUrls);
	}

	public static string GetFriendAvatarUrls()
	{
		return CPlayerPrefs.GetString("facebook_friends_avatar_urls", string.Empty);
	}

	public static void SetMyAvatarUrl(string url)
	{
		CPlayerPrefs.SetString("facebook_my_avatar_url", url);
	}

	public static string GetMyAvatarUrl()
	{
		return CPlayerPrefs.GetString("facebook_my_avatar_url", string.Empty);
	}

	public static string GetFriendAvatarUrl(int index)
	{
		string friendAvatarUrls = GetFriendAvatarUrls();
		List<string> list = CUtils.BuildListFromString<string>(friendAvatarUrls);
		if (index < list.Count)
		{
			return list[index];
		}
		return string.Empty;
	}

	public static void SetFriendLevels(string friendLevels)
	{
		CPlayerPrefs.SetString("facebook_friends_levels", friendLevels);
	}

	public static string GetFriendLevels()
	{
		return CPlayerPrefs.GetString("facebook_friends_levels", string.Empty);
	}

	public static void SetFriendScores(int index, string scores)
	{
		CPlayerPrefs.SetString("facebook_friends_scores_" + index, scores);
	}

	public static string GetFriendScores(int index)
	{
		return CPlayerPrefs.GetString("facebook_friends_scores_" + index, string.Empty);
	}

	public static void SetMyScores(string scores)
	{
		CPlayerPrefs.SetString("facebook_my_scores", scores);
	}

	public static string GetMyScores()
	{
		return CPlayerPrefs.GetString("facebook_my_scores", string.Empty);
	}

	public static int GetMyScore(int level)
	{
		string myScores = GetMyScores();
		List<int> list = CUtils.BuildListFromString<int>(myScores);
		if (level <= list.Count)
		{
			return list[level - 1];
		}
		return 0;
	}

	public static void SetMyScore(int level, int score)
	{
		string myScores = GetMyScores();
		List<int> list = CUtils.BuildListFromString<int>(myScores);
		if (level <= list.Count)
		{
			list[level - 1] = score;
		}
		else if (level == list.Count + 1)
		{
			list.Add(score);
		}
		string myScores2 = CUtils.BuildStringFromCollection(list);
		SetMyScores(myScores2);
	}

	public static int GetFriendScore(int index, int level)
	{
		string friendScores = GetFriendScores(index);
		List<int> list = CUtils.BuildListFromString<int>(friendScores);
		if (level <= list.Count)
		{
			return list[level - 1];
		}
		return 0;
	}

	public static List<int> GetFriendIndexesPassed(int level)
	{
		List<int> list = new List<int>();
		string friendLevels = GetFriendLevels();
		List<int> list2 = CUtils.BuildListFromString<int>(friendLevels);
		for (int i = 0; i < list2.Count; i++)
		{
			if (list2[i] > level)
			{
				list.Add(i);
			}
		}
		return list;
	}

	public static void ChangeFriendLevels()
	{
		string friendLevels = GetFriendLevels();
		if (friendLevels == string.Empty)
		{
			return;
		}
		int @int = CPlayerPrefs.GetInt("facebook_daychange_level", -1);
		if (DateTime.Now.DayOfYear == @int)
		{
			return;
		}
		CPlayerPrefs.SetInt("facebook_daychange_level", DateTime.Now.DayOfYear);
		List<int> list = CUtils.BuildListFromString<int>(friendLevels);
		for (int i = 0; i < list.Count; i++)
		{
			int num = UnityEngine.Random.Range(0, 3);
			list[i] += num;
			if (list[i] >= 50)
			{
				list[i] = 50;
			}
		}
		friendLevels = CUtils.BuildStringFromCollection(list);
		SetFriendLevels(friendLevels);
	}
}
