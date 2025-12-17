using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Superpow;
using UnityEngine;
using UnityEngine.UI;

public class TileRegion : MonoBehaviour
{
	public class TileComparer : IComparer<Tile>
	{
		public int Compare(Tile x, Tile y)
		{
			double num = F(x);
			double num2 = F(y);
			if (num < num2)
			{
				return -1;
			}
			if (num == num2)
			{
				return 0;
			}
			return 1;
		}

		private double F(Tile a)
		{
			return a.boardPosition.x + a.boardPosition.y;
		}
	}

	public Vector2 size;

	private Dictionary<Vector2, Tile> slots = new Dictionary<Vector2, Tile>();

	public List<Piece> pieces = new List<Piece>();

	private List<Piece> hintPieces = new List<Piece>();

	public static TileRegion instance;

	private LevelPrefs levelPrefs;

	private void Awake()
	{
		instance = this;
	}

	private void LoadBoardBackground()
	{
		for (int i = 0; (float)i < size.y; i++)
		{
			for (int j = 0; (float)j < size.x; j++)
			{
				Tile tile = Object.Instantiate(CMonoUtils.instance.tile_background);
				tile.transform.SetParent(base.transform);
				tile.transform.localScale = Vector3.one;
				Vector3 localPosition = ((i % 2 != 0) ? new Vector3((float)i * 1.5f * 42f, (float)j * 72.744f + 36.372f) : new Vector3((float)i * 1.5f * 42f, (float)j * 72.744f));
				tile.transform.localPosition = localPosition;
				tile.position = new Vector2(i, j);
				tile.transform.GetChild(0).GetComponent<Text>().text = i + "," + j;
			}
		}
	}

	private void LoadBottomBackground()
	{
		for (int i = 0; i < 17; i++)
		{
			for (int j = 0; j < 7; j++)
			{
				Tile tile = Object.Instantiate(CMonoUtils.instance.tile_background);
				tile.transform.SetParent(CMonoUtils.instance.bottomRegion);
				tile.transform.localScale = Vector3.one * 0.53f;
				Vector3 localPosition = GetLocalPosition(i, j);
				tile.transform.localPosition = localPosition * 0.53f;
				tile.position = new Vector2(i, j);
				tile.transform.GetChild(0).GetComponent<Text>().text = i + "," + j;
				tile.transform.GetChild(0).GetComponent<Text>().fontSize = 25;
			}
		}
	}

	public void Load(GameLevel gameLevel)
	{
		levelPrefs = MainController.instance.levelPrefs;
		List<string> list = CUtils.BuildListFromString<string>(gameLevel.positions);
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		float num3 = float.MinValue;
		float num4 = float.MinValue;
		foreach (string item in list)
		{
			string[] array = item.Split(',');
			int num5 = int.Parse(array[0]);
			int num6 = int.Parse(array[1]);
			Tile tile = Object.Instantiate(CMonoUtils.instance.tile_background2);
			tile.transform.SetParent(CMonoUtils.instance.backgroundTilesTransform);
			tile.transform.localScale = Vector3.one;
			Vector3 localPosition = GetLocalPosition(num5, num6);
			tile.transform.localPosition = localPosition;
			tile.position = new Vector2(num5, num6);
		}
		foreach (string item2 in list)
		{
			string[] array2 = item2.Split(',');
			int num7 = int.Parse(array2[0]);
			int num8 = int.Parse(array2[1]);
			Tile tile2 = Object.Instantiate((array2.Length != 2) ? CMonoUtils.instance.tile_stone : CMonoUtils.instance.tile_background);
			tile2.transform.SetParent(CMonoUtils.instance.backgroundTilesTransform);
			tile2.transform.localScale = Vector3.one;
			Vector3 localPosition2 = GetLocalPosition(num7, num8);
			tile2.transform.localPosition = localPosition2;
			tile2.position = new Vector2(num7, num8);
			if (array2.Length == 2)
			{
				slots.Add(tile2.position, tile2);
			}
			if (localPosition2.x < num)
			{
				num = localPosition2.x;
			}
			if (localPosition2.x > num3)
			{
				num3 = localPosition2.x;
			}
			if (localPosition2.y < num2)
			{
				num2 = localPosition2.y;
			}
			if (localPosition2.y > num4)
			{
				num4 = localPosition2.y;
			}
		}
		float num9 = num3 - num + 84f + (num - 42f) * 2f;
		float num10 = num4 - num2 + 72.744f + (num2 - 36.372f) * 2f;
		base.transform.localPosition = GetComponent<RectTransform>().localPosition - new Vector3(num9 / 2f, num10 / 2f);
		GetComponent<RectTransform>().sizeDelta = new Vector2(num9, num10);
		LoadPieces(gameLevel);
	}

	private void LoadPieces(GameLevel gameLevel)
	{
		List<string> list = CUtils.BuildListFromString<string>(gameLevel.pieces);
		int id = 0;
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		float num3 = float.MinValue;
		float num4 = float.MinValue;
		foreach (string item in list)
		{
			List<string> list2 = CUtils.BuildListFromString<string>(item, '-');
			Vector2 vector = Vector2.zero;
			List<Vector2> list3 = new List<Vector2>();
			bool flag = list2[list2.Count - 1] == "r";
			if (flag)
			{
				list2.RemoveAt(list2.Count - 1);
			}
			int num5 = 0;
			for (num5 = 0; num5 < list2.Count; num5++)
			{
				string[] array = list2[num5].Split(',');
				int num6 = int.Parse(array[0]);
				int num7 = int.Parse(array[1]);
				Vector2 vector2 = new Vector2(num6, num7);
				if (num5 == list2.Count - 1)
				{
					vector = vector2;
				}
				if (num5 != list2.Count - 1)
				{
					list3.Add(vector2);
				}
			}
			PiecePrefs piecePrefs = levelPrefs.piecesPrefs.Find((PiecePrefs x) => x.id == id);
			bool flag2 = piecePrefs != null;
			float num8 = ((!flag2) ? 0.53f : 1f);
			Transform parent = ((!flag2) ? CMonoUtils.instance.piecesBottomTransform : CMonoUtils.instance.piecesTransform);
			Piece piece = CreatePiece(list3, parent);
			piece.isExtra = flag;
			pieces.Add(piece);
			piece.id = id++;
			piece.bottomPosition = vector;
			piece.transform.localScale = Vector3.one * num8;
			if (flag2)
			{
				piece.status = Piece.Status.OnBoard;
				Vector2 vector3 = Utils.ConvertToVector2(piecePrefs.boardPosition);
				piece.transform.localPosition = GetLocalPosition(vector3);
				piece.boardPositions = GetMatchPositions(piece, vector3);
				piece.UpdateTileBoardPosition();
				foreach (Vector2 boardPosition in piece.boardPositions)
				{
					if (slots.ContainsKey(boardPosition))
					{
						slots[boardPosition].hasCover = true;
						continue;
					}
					levelPrefs.piecesPrefs = new List<PiecePrefs>();
					Transform piecesTransform = CMonoUtils.instance.piecesTransform;
					for (int num9 = piecesTransform.childCount - 1; num9 >= 0; num9--)
					{
						Object.DestroyImmediate(piecesTransform.GetChild(num9).gameObject);
					}
					piecesTransform = CMonoUtils.instance.piecesBottomTransform;
					for (int num10 = piecesTransform.childCount - 1; num10 >= 0; num10--)
					{
						Object.DestroyImmediate(piecesTransform.GetChild(num10).gameObject);
					}
					foreach (Vector2 key in slots.Keys)
					{
						slots[key].hasCover = false;
					}
					pieces.Clear();
					LoadPieces(gameLevel);
					return;
				}
			}
			else
			{
				piece.transform.localPosition = GetLocalPosition(vector) * 0.53f;
			}
			num5 = 0;
			foreach (Vector2 matchPosition in GetMatchPositions(piece, piece.bottomPosition))
			{
				Tile tile = Object.Instantiate(CMonoUtils.instance.tile_background_bottom);
				tile.transform.SetParent(CMonoUtils.instance.bottomRegionBGTransform);
				tile.transform.localScale = Vector3.one * 0.53f;
				tile.transform.localPosition = GetLocalPosition(matchPosition) * 0.53f;
				tile.position = matchPosition;
				Vector3 localPosition = tile.transform.localPosition;
				if (localPosition.x < num)
				{
					num = localPosition.x;
				}
				if (localPosition.x > num3)
				{
					num3 = localPosition.x;
				}
				if (localPosition.y < num2)
				{
					num2 = localPosition.y;
				}
				if (localPosition.y > num4)
				{
					num4 = localPosition.y;
				}
				if (num5 == 0)
				{
					piece.bottomBackground = tile;
				}
				num5++;
			}
		}
		float num11 = num3 - num + 84f + (num - 42f) * 2f;
		float num12 = num4 - num2 + 72.744f + (num2 - 36.372f) * 2f;
		RectTransform component = CMonoUtils.instance.bottomRegionBGTransform.GetComponent<RectTransform>();
		component.localPosition = new Vector3(0f, 13.7f) - new Vector3(num11 / 2f, num12 / 2f);
		component.sizeDelta = new Vector2(num11, num12);
		RectTransform component2 = CMonoUtils.instance.piecesBottomTransform.GetComponent<RectTransform>();
		component2.localPosition = component.localPosition;
		component2.sizeDelta = component.sizeDelta;
	}

	private Piece CreatePiece(List<Vector2> positions, Transform parent)
	{
		Piece piece = Object.Instantiate(CMonoUtils.instance.piecePrefab);
		piece.center = positions[0];
		piece.transform.SetParent(parent);
		piece.defaultPositions.AddRange(positions);
		Vector3 localPosition = GetLocalPosition(piece.center);
		foreach (Vector2 defaultPosition in piece.defaultPositions)
		{
			int num = (int)defaultPosition.x;
			int num2 = (int)defaultPosition.y;
			Tile tile = Object.Instantiate(CMonoUtils.instance.tile_normal);
			tile.transform.SetParent(piece.transform);
			tile.transform.localScale = Vector3.one;
			Vector3 localPosition2 = GetLocalPosition(num, num2);
			tile.transform.localPosition = localPosition2 - localPosition;
			tile.position = new Vector2(num, num2);
			tile.piece = piece;
			Tile tile2 = Object.Instantiate(CMonoUtils.instance.tile_shadow);
			tile2.transform.SetParent(piece.shadows.transform);
			tile2.transform.localScale = Vector3.one;
			tile2.transform.localPosition = tile.transform.localPosition + new Vector3(5f, -5f);
			piece.tiles.Add(tile);
		}
		foreach (Vector2 defaultPosition2 in piece.defaultPositions)
		{
			piece.tilePositions.Add(defaultPosition2 - positions[0]);
		}
		return piece;
	}

	private Vector3 GetLocalPosition(int col, int row)
	{
		return (col % 2 != 0) ? new Vector3(((float)col * 1.5f + 1f) * 42f, (float)(row + 1) * 72.744f) : new Vector3(((float)col * 1.5f + 1f) * 42f, ((float)row + 0.5f) * 72.744f);
	}

	private Vector3 GetLocalPosition(Vector2 position)
	{
		return GetLocalPosition((int)position.x, (int)position.y);
	}

	public bool CheckMatch(Piece piece)
	{
		float num = float.MaxValue;
		Tile tile = null;
		foreach (Tile value in slots.Values)
		{
			if (!value.hasCover)
			{
				float num2 = Vector3.Distance(piece.tileCenter.transform.position, value.transform.position);
				if (num2 < num)
				{
					num = num2;
					tile = value;
				}
			}
		}
		piece.ResetMatchColor();
		piece.matches = new List<Tile>();
		if (num < 0.4f)
		{
			bool flag = true;
			List<Vector2> matchPositions = GetMatchPositions(piece, tile.position);
			foreach (Vector2 item in matchPositions)
			{
				if (!slots.ContainsKey(item) || slots[item].hasCover)
				{
					flag = false;
					break;
				}
				piece.matches.Add(slots[item]);
			}
			if (flag)
			{
				piece.HighlightMatchColor();
			}
			return flag;
		}
		return false;
	}

	public void ClearCovers(Piece piece)
	{
		foreach (Vector2 boardPosition in piece.boardPositions)
		{
			slots[boardPosition].hasCover = false;
		}
	}

	private List<Vector2> GetMatchPositions(Piece piece, Vector2 centerPosition)
	{
		List<Vector2> list = new List<Vector2>();
		int num = (int)(centerPosition.x - piece.tileCenter.position.x);
		if (num % 2 == 0)
		{
			list.AddRange(piece.tilePositions);
		}
		else
		{
			int num2 = ((centerPosition.x % 2f != 0f) ? 1 : (-1));
			foreach (Vector2 tilePosition in piece.tilePositions)
			{
				float num3 = tilePosition.y;
				if ((int)tilePosition.x % 2 == 1)
				{
					num3 += (float)num2;
				}
				list.Add(new Vector2(tilePosition.x, num3));
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			list[i] += centerPosition;
		}
		return list;
	}

	public bool ShowHint()
	{
		foreach (Piece piece2 in pieces)
		{
			if ((bool)hintPieces.Find((Piece x) => x.id == piece2.id) || piece2.isExtra || piece2.status != Piece.Status.OnBoard)
			{
				continue;
			}
			List<Piece> list = FindSamePieces(piece2);
			bool flag = false;
			foreach (Piece item in list)
			{
				if (piece2.boardPositions[0] == item.center)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				continue;
			}
			ShowHint(piece2);
			return true;
		}
		foreach (Piece piece in pieces)
		{
			if ((bool)hintPieces.Find((Piece x) => x.id == piece.id) || piece.isExtra || piece.status != Piece.Status.OnBottom || !ShowHint(piece))
			{
				continue;
			}
			return true;
		}
		return false;
	}

	private bool ShowHint(Piece piece)
	{
		List<Piece> list = FindSamePieces(piece);
		foreach (Piece samePiece in list)
		{
			if (!samePiece.isExtra)
			{
				Piece piece2 = list.Find((Piece x) => x.status == Piece.Status.OnBoard && x.boardPositions[0] == samePiece.center);
				if (piece2 == null)
				{
					Piece piece3 = CreatePiece(samePiece.defaultPositions, CMonoUtils.instance.hintPiecesTransform);
					piece3.transform.localScale = Vector3.one * 7f;
					piece3.transform.localPosition = GetLocalPosition(samePiece.center);
					piece3.id = piece.id;
					iTween.ScaleTo(piece3.gameObject, Vector3.one, 0.3f);
					hintPieces.Add(piece3);
					return true;
				}
			}
		}
		return false;
	}

	private List<Piece> FindSamePieces(Piece sample)
	{
		List<Piece> list = new List<Piece>();
		foreach (Piece piece in pieces)
		{
			if (!(piece == sample))
			{
				List<Vector2> matchPositions = GetMatchPositions(piece, sample.center);
				if (Compare2List(matchPositions, sample.defaultPositions))
				{
					list.Add(piece);
				}
			}
		}
		list.Insert(0, sample);
		return list;
	}

	private bool Compare2List(List<Vector2> list1, List<Vector2> list2)
	{
		if (list1.Count != list2.Count)
		{
			return false;
		}
		for (int i = 0; i < list1.Count; i++)
		{
			if (list1[i] != list2[i])
			{
				return false;
			}
		}
		return true;
	}

	public void CheckGameComplete()
	{
		bool flag = true;
		foreach (Tile value in slots.Values)
		{
			if (!value.hasCover)
			{
				flag = false;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		SavePrefs();
		List<Tile> tileOnBoard = GetTileOnBoard();
		tileOnBoard = tileOnBoard.OrderByDescending((Tile x) => x, new TileComparer()).ToList();
		StartCoroutine(FadeTiles(tileOnBoard));
		hintPieces.Clear();
		foreach (Transform item in CMonoUtils.instance.hintPiecesTransform)
		{
			Object.Destroy(item.gameObject);
		}
		MainController.instance.OnComplete(tileOnBoard.Count);
	}

	private List<Tile> GetTileOnBoard()
	{
		List<Tile> list = new List<Tile>();
		foreach (Piece piece in pieces)
		{
			if (piece.status == Piece.Status.OnBoard)
			{
				list.AddRange(piece.tiles);
			}
		}
		return list;
	}

	private IEnumerator FadeTiles(List<Tile> fadedTiles)
	{
		foreach (Tile tile in fadedTiles)
		{
			tile.GetComponent<Image>().CrossFadeAlpha(0f, 0.5f, true);
			yield return new WaitForSeconds(0.03f);
		}
	}

	private void SavePrefs()
	{
		levelPrefs.piecesPrefs = new List<PiecePrefs>();
		foreach (Piece piece in pieces)
		{
			if (piece.status == Piece.Status.OnBoard)
			{
				PiecePrefs piecePrefs = new PiecePrefs();
				piecePrefs.id = piece.id;
				piecePrefs.boardPosition = Utils.ConvertToString(piece.boardPositions[0]);
				levelPrefs.piecesPrefs.Add(piecePrefs);
			}
		}
	}
}
