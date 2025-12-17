using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollViewContent : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public SnapScrollRect snapScroll;

	public int numItems = 1;

	public void OnPointerClick(PointerEventData ped)
	{
		Vector2 localPoint;
		if (RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), ped.position, ped.pressEventCamera, out localPoint))
		{
			int pageIndex = (int)(localPoint.x / (GetComponent<RectTransform>().sizeDelta.x / (float)numItems));
			snapScroll.MoveToPage(pageIndex);
		}
	}
}
