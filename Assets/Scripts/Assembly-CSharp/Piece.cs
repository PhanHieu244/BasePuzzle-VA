using System.Collections.Generic;
using Superpow;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Piece : MonoBehaviour
{
	public enum Status
	{
		Dragging = 0,
		OnBoard = 1,
		OnBottom = 2,
		OnTween = 3
	}

	public int id;

	public Vector2 center;

	public List<Vector2> defaultPositions = new List<Vector2>();

	public List<Vector2> boardPositions = new List<Vector2>();

	public List<Vector2> tilePositions = new List<Vector2>();

	public List<Tile> matches = new List<Tile>();

	public List<Tile> tiles = new List<Tile>();

	public Vector2 bottomPosition;

	public Tile bottomBackground;

	public Status status = Status.OnBottom;

	protected Vector3 beginPosition;

	protected Vector3 beginTouchPosition;

	public bool isExtra;

	public GameObject shadows;

	private Vector3 upDelta;

	public Tile tileCenter
	{
		get
		{
			return tiles[0];
		}
	}

	private void Start()
	{
		foreach (Tile tile in tiles)
		{
			tile.GetComponent<Image>().sprite = CMonoUtils.instance.tileSprites[id];
		}
		UpdatePiece();
	}

	private Vector3 GetUpDelta()
	{
		float num = float.MaxValue;
		foreach (Tile tile in tiles)
		{
			num = Mathf.Min(num, tile.transform.position.y);
		}
		return Vector3.up * (beginTouchPosition.y + 1.3f - num);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (status == Status.Dragging)
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - beginTouchPosition;
			base.transform.position = beginPosition + upDelta + vector;
			TileRegion.instance.CheckMatch(this);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if ((status == Status.OnBoard || status == Status.OnBottom) && GameState.canPlay)
		{
			beginPosition = base.transform.position;
			beginTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			upDelta = GetUpDelta();
			iTween.ScaleTo(base.gameObject, Vector3.one, 0.06f);
			iTween.MoveTo(base.gameObject, beginPosition + upDelta, 0.06f);
			base.transform.SetParent(CMonoUtils.instance.dragRegion);
			if (status == Status.OnBoard)
			{
				TileRegion.instance.ClearCovers(this);
			}
			status = Status.Dragging;
			matches.Clear();
			boardPositions.Clear();
			UpdatePiece();
			CSound.instance.Play(CSound.Others.Select);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (status == Status.Dragging)
		{
			if (matches.Count == tiles.Count)
			{
				CSound.instance.Play(CSound.Others.OnBoard);
				iTween.MoveTo(base.gameObject, iTween.Hash("position", matches[0].transform.position, "speed", 10f, "oncomplete", "CompleteMoveToBoard"));
			}
			else
			{
				iTween.ScaleTo(base.gameObject, iTween.Hash("scale", Vector3.one * 0.53f, "speed", 4f));
				iTween.MoveTo(base.gameObject, iTween.Hash("position", bottomBackground.transform.position, "speed", 15f, "oncomplete", "CompleteMoveToBottom_EndDrag"));
			}
			status = Status.OnTween;
			ResetMatchColor();
		}
	}

	public void MoveToBottom()
	{
		if (status == Status.OnBoard)
		{
			base.transform.SetParent(CMonoUtils.instance.dragRegion);
			TileRegion.instance.ClearCovers(this);
			iTween.ScaleTo(base.gameObject, iTween.Hash("scale", Vector3.one * 0.53f, "speed", 4f));
			iTween.MoveTo(base.gameObject, iTween.Hash("position", bottomBackground.transform.position, "speed", 20f, "oncomplete", "CompleteMoveToBottom"));
			status = Status.OnTween;
		}
	}

	public void FadeIn()
	{
		foreach (Tile tile in tiles)
		{
			tile.GetComponent<Image>().canvasRenderer.SetAlpha(1f);
		}
	}

	public void ResetMatchColor()
	{
		foreach (Transform item in CMonoUtils.instance.highlightsTransform)
		{
			if (item.gameObject.activeSelf)
			{
				MainController.instance.GetComponent<Pooler>().Push(item.gameObject);
			}
		}
	}

	public void HighlightMatchColor()
	{
		foreach (Tile match in matches)
		{
			GameObject pooledObject = MainController.instance.GetComponent<Pooler>().GetPooledObject();
			pooledObject.transform.SetParent(CMonoUtils.instance.highlightsTransform);
			pooledObject.transform.localScale = Vector3.one;
			pooledObject.transform.position = match.transform.position;
		}
	}

	private void CompleteMoveToBoard()
	{
		status = Status.OnBoard;
		base.transform.SetParent(CMonoUtils.instance.piecesTransform);
		foreach (Tile match in matches)
		{
			match.hasCover = true;
			boardPositions.Add(match.position);
		}
		UpdateTileBoardPosition();
		TileRegion.instance.CheckGameComplete();
		UpdatePiece();
		Utils.IncreaseNumMoves(GameState.chosenWorld, GameState.chosenLevel);
	}

	public void UpdateTileBoardPosition()
	{
		for (int i = 0; i < tiles.Count; i++)
		{
			tiles[i].boardPosition = boardPositions[i];
		}
	}

	private void CompleteMoveToBottom()
	{
		status = Status.OnBottom;
		base.transform.SetParent(CMonoUtils.instance.piecesBottomTransform);
		UpdatePiece();
		FadeIn();
	}

	private void CompleteMoveToBottom_EndDrag()
	{
		CompleteMoveToBottom();
		CSound.instance.Play(CSound.Others.OnBottom);
	}

	public void UpdatePiece()
	{
		shadows.SetActive(status == Status.OnBottom);
	}
}
