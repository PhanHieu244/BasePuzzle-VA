using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class InviteFriendsDialog : Dialog
{
	public RowInviteFriends Row;

	public RectTransform contentTransform;

	public Action<string> onInviteProcessed;

	public Scrollbar scrollBar;

	private List<InvitableFriend> data;

	public void Build(List<InvitableFriend> data)
	{
		this.data = data;
		for (int i = 0; i < data.Count; i++)
		{
			string text = data[i].name;
			string avatarUrl = data[i].avatarUrl;
			RowInviteFriends rowInviteFriends = UnityEngine.Object.Instantiate(Row);
			rowInviteFriends.Build(text, avatarUrl);
			rowInviteFriends.transform.SetParent(contentTransform);
			rowInviteFriends.rectTransform.localScale = Vector3.one;
			rowInviteFriends.rectTransform.anchoredPosition = new Vector3(0f, (25 - i) * 110);
		}
		contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, 110 * data.Count);
	}

	protected override void Start()
	{
		base.Start();
		scrollBar.value = 1f;
	}

	public void Invite()
	{
		CSound.instance.PlayButton();
		if (CheckRequirement())
		{
			string recipients = GetRecipients();
			onInviteProcessed(recipients);
			Close();
		}
	}

	private bool CheckRequirement()
	{
		int num = 0;
		for (int i = 0; i < data.Count; i++)
		{
			RowInviteFriends component = contentTransform.GetChild(i).GetComponent<RowInviteFriends>();
			data[i].shouldInvite = component.toggleCheck.isOn;
			if (component.toggleCheck.isOn)
			{
				num++;
			}
		}
		if (num >= 40)
		{
			return true;
		}
		return false;
	}

	private string GetRecipients()
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < data.Count; i++)
		{
			if (data[i].shouldInvite)
			{
				string id = data[i].id;
				if (i != data.Count - 1)
				{
					stringBuilder.Append(id).Append(",");
				}
				else
				{
					stringBuilder.Append(id);
				}
			}
		}
		return stringBuilder.ToString();
	}
}
