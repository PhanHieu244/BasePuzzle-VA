using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListView
{
	public enum Type
	{
		Verticle = 0,
		Horizontal = 1
	}

	public Type listType;

	private RectTransform root;

	private List<RectTransform> items;

	private float itemSize;

	private MonoBehaviour behaviour;

	private bool isSnapScroll;

	public ListView(MonoBehaviour behaviour)
	{
		this.behaviour = behaviour;
		items = new List<RectTransform>();
	}

	public ListView SetType(Type listType)
	{
		this.listType = listType;
		return this;
	}

	public ListView SetSnapScroll(bool isSnap)
	{
		isSnapScroll = true;
		return this;
	}

	public ListView SetRoot(RectTransform root)
	{
		this.root = root;
		foreach (RectTransform item in root)
		{
			items.Add(item);
		}
		return this;
	}

	public ListView SetItemSize(float itemSize)
	{
		this.itemSize = itemSize;
		return this;
	}

	public void Build()
	{
		UpdateList();
	}

	private void UpdateList()
	{
		List<RectTransform> activeItems = GetActiveItems();
		float num = ((!isSnapScroll) ? 0f : itemSize);
		if (listType == Type.Horizontal)
		{
			root.sizeDelta = new Vector2((float)activeItems.Count * itemSize + num, root.sizeDelta.y);
			for (int i = 0; i < activeItems.Count; i++)
			{
				RectTransform rectTransform = activeItems[i];
				rectTransform.SetLocalX(itemSize * (float)i + rectTransform.sizeDelta.x / 2f + num / 2f);
			}
		}
		else
		{
			root.sizeDelta = new Vector2(root.sizeDelta.x, (float)activeItems.Count * itemSize + num);
			for (int j = 0; j < activeItems.Count; j++)
			{
				RectTransform rectTransform2 = activeItems[j];
				rectTransform2.SetLocalY((0f - itemSize) * (float)j - rectTransform2.sizeDelta.y / 2f - num / 2f);
			}
		}
	}

	public void DisappearItems(params int[] indexes)
	{
		foreach (int index in indexes)
		{
			items[index].gameObject.SetActive(false);
		}
		UpdateList();
	}

	public void DisappearItemsAfterTime(float delay, params int[] indexes)
	{
		behaviour.StartCoroutine(IEDisappearItems(indexes, delay));
	}

	private IEnumerator IEDisappearItems(int[] indexes, float delay)
	{
		yield return new WaitForSeconds(delay);
		DisappearItems(indexes);
	}

	private List<RectTransform> GetActiveItems()
	{
		return items.FindAll((RectTransform x) => x.gameObject.activeSelf);
	}

	public int GetActiveItemsCount()
	{
		return GetActiveItems().Count;
	}
}
