using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SnapScrollRect : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IEventSystemHandler
{
	public int screens;

	private float[] points;

	public int speed = 10;

	private float stepSize;

	private ScrollRect scroll;

	private bool lerp;

	private float target;

	[HideInInspector]
	public int index;

	public GameObject[] indicators;

	public Text tabName;

	public string tabNamePrefix;

	public ScaleItem scaleItem;

	public Action<int> onPageChanged;

	private float beginDragTimer;

	private int direction;

	private void Awake()
	{
		scroll = base.gameObject.GetComponent<ScrollRect>();
		if (screens != 0)
		{
			InitPoints(screens);
		}
	}

	public void InitPoints(int _screens)
	{
		screens = _screens;
		points = new float[screens];
		if (screens > 1)
		{
			stepSize = 1f / (float)(screens - 1);
			for (int i = 0; i < screens; i++)
			{
				points[i] = (float)i * stepSize;
			}
		}
		else
		{
			points[0] = 0f;
		}
	}

	private void Update()
	{
		if (!lerp)
		{
			return;
		}
		if (scroll.horizontal)
		{
			scroll.horizontalNormalizedPosition = Mathf.Lerp(scroll.horizontalNormalizedPosition, target, (float)speed * scroll.elasticity * Time.deltaTime);
			if (Mathf.Approximately(scroll.horizontalNormalizedPosition, target))
			{
				lerp = false;
			}
		}
		else
		{
			scroll.verticalNormalizedPosition = Mathf.Lerp(scroll.verticalNormalizedPosition, target, (float)speed * scroll.elasticity * Time.deltaTime);
			if (Mathf.Approximately(scroll.verticalNormalizedPosition, target))
			{
				lerp = false;
			}
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		beginDragTimer = Time.time;
	}

	public void OnEndDrag(PointerEventData data)
	{
		float num = Time.time - beginDragTimer;
		if (num > 0.15f)
		{
			index = (scroll.horizontal ? FindNearest_SlowSwipe(scroll.horizontalNormalizedPosition, points) : ((!scroll.vertical) ? index : FindNearest_SlowSwipe(scroll.verticalNormalizedPosition, points)));
			MoveToPage(index);
			return;
		}
		direction = ((!(scroll.velocity.x > 0f)) ? 1 : (-1));
		Timer.Schedule(this, 0.1f, delegate
		{
			index = (scroll.horizontal ? FindNearest_FastSwipe(scroll.horizontalNormalizedPosition, points) : ((!scroll.vertical) ? index : FindNearest_FastSwipe(scroll.verticalNormalizedPosition, points)));
			MoveToPage(index);
		});
	}

	public void NextPage()
	{
		MoveToPage(index + 1);
		CSound.instance.PlayButton();
	}

	public void PreviousPage()
	{
		MoveToPage(index - 1);
		CSound.instance.PlayButton();
	}

	public void MoveToPage(int pageIndex)
	{
		index = Mathf.Clamp(pageIndex, 0, screens - 1);
		target = points[index];
		lerp = true;
		UpdateIndicator();
		if (onPageChanged != null)
		{
			onPageChanged(index);
		}
		if (scaleItem.enabled)
		{
			ScaleEffect(index);
		}
	}

	public void SetPage(int pageIndex)
	{
		index = Mathf.Clamp(pageIndex, 0, screens - 1);
		target = points[index];
		if (scroll.horizontal)
		{
			scroll.horizontalNormalizedPosition = target;
		}
		else
		{
			scroll.verticalNormalizedPosition = target;
		}
		UpdateIndicator();
		if (onPageChanged != null)
		{
			onPageChanged(index);
		}
		if (scaleItem.enabled)
		{
			ScaleEffect(index, true);
		}
	}

	public void UpdateIndicator()
	{
		for (int i = 0; i < indicators.Length; i++)
		{
			indicators[i].SetActive(i == index);
		}
		if (tabName != null)
		{
			tabName.text = tabNamePrefix + (index + 1);
		}
	}

	public void OnDrag(PointerEventData data)
	{
		lerp = false;
	}

	private int FindNearest_SlowSwipe(float f, float[] array)
	{
		float num = float.PositiveInfinity;
		int result = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (Mathf.Abs(array[i] - f) < num)
			{
				num = Mathf.Abs(array[i] - f);
				result = i;
			}
		}
		return result;
	}

	private int FindNearest_FastSwipe(float f, float[] array)
	{
		float num = array[1] - array[0];
		int result = ((direction == 1) ? (array.Length - 1) : 0);
		for (int i = index; i >= 0 && i < array.Length; i += direction)
		{
			if (Mathf.Abs(i - index) <= 1 && Mathf.Abs(array[i] - f) < 0.2f * num)
			{
				result = i;
				break;
			}
			if ((direction == 1 && array[i] - f > 0f) || (direction == -1 && array[i] - f < 0f))
			{
				result = i;
				break;
			}
		}
		return result;
	}

	private void ScaleEffect(int index, bool immediate = false)
	{
		int num = 0;
		foreach (Transform item in scroll.content.transform)
		{
			if (immediate)
			{
				item.localScale = Vector3.one * ((index != num) ? scaleItem.scaleNormal : scaleItem.scaleTo);
			}
			else
			{
				iTween.ScaleTo(item.gameObject, Vector3.one * ((index != num) ? scaleItem.scaleNormal : scaleItem.scaleTo), scaleItem.time);
			}
			num++;
		}
	}
}
