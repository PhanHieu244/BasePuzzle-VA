using Superpow;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
	public enum Type
	{
		Background = 0,
		Normal = 1,
		Stone = 2
	}

	public const float EDGE_SIZE = 42f;

	public const float WIDTH = 84f;

	public const float HEIGHT = 72.744f;

	public Type type = Type.Normal;

	public bool isActive = true;

	public bool hasCover;

	public Vector2 position;

	public Vector2 boardPosition;

	public Piece piece;

	public Piece2 piece2;

	private void Start()
	{
		if (Utils.IsMakingLevel())
		{
			CanvasGroup component = GetComponent<CanvasGroup>();
			if (component != null)
			{
				component.interactable = true;
				component.blocksRaycasts = true;
			}
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (Utils.IsMakingLevel() && type == Type.Background)
		{
			isActive = !isActive;
			SetActive(isActive);
		}
	}

	public void SetActive(bool isActive)
	{
		this.isActive = isActive;
		GetComponent<Image>().SetColorAlpha(isActive ? 1 : 0);
		base.transform.GetChild(0).GetComponent<Text>().SetColorAlpha(isActive ? 1 : 0);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (piece != null)
		{
			piece.OnDrag(eventData);
		}
		if (piece2 != null)
		{
			piece2.OnDrag(eventData);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (piece != null)
		{
			piece.OnPointerDown(eventData);
		}
		if (piece2 != null)
		{
			piece2.OnPointerDown(eventData);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (piece != null)
		{
			piece.OnEndDrag(eventData);
		}
		if (piece2 != null)
		{
			piece2.OnEndDrag(eventData);
		}
	}
}
