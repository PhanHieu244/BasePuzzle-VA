using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Piece2 : Piece
{
	private void Start()
	{
		foreach (Tile tile in tiles)
		{
			tile.GetComponent<Image>().sprite = CMonoUtils.instance.tileSprites[id % CMonoUtils.instance.tileSprites.Length];
		}
	}

	public new void OnDrag(PointerEventData eventData)
	{
		if (status == Status.Dragging)
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - beginTouchPosition;
			base.transform.position = beginPosition + vector;
			TileRegion2.instance.CheckMatch(this);
		}
	}

	public new void OnPointerDown(PointerEventData eventData)
	{
		beginPosition = base.transform.position;
		beginTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		base.transform.SetParent(CMonoUtils.instance.dragRegion);
		status = Status.Dragging;
		matches.Clear();
	}

	public new void OnEndDrag(PointerEventData eventData)
	{
		if (matches.Count == tiles.Count)
		{
			iTween.MoveTo(base.gameObject, iTween.Hash("position", matches[0].transform.position, "speed", 10f, "oncomplete", "CompleteMoveToBoard"));
		}
		else
		{
			iTween.ScaleTo(base.gameObject, iTween.Hash("scale", Vector3.one * 0.53f, "speed", 4f));
			iTween.MoveTo(base.gameObject, iTween.Hash("position", beginPosition, "speed", 15f, "oncomplete", "CompleteMoveToBottom"));
		}
		status = Status.OnTween;
	}

	private void CompleteMoveToBoard()
	{
		status = Status.OnBottom;
		base.transform.SetParent(CMonoUtils.instance.pieceRegion);
		if (matches.Count != 0)
		{
			boardPositions.Clear();
		}
		foreach (Tile match in matches)
		{
			boardPositions.Add(match.position);
		}
		MakeLevelController.instance.AdjustPieces();
	}

	private void CompleteMoveToBottom()
	{
		status = Status.OnBottom;
		base.transform.SetParent(CMonoUtils.instance.pieceRegion);
	}
}
