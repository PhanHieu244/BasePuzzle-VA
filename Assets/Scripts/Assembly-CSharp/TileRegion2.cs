using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileRegion2 : MonoBehaviour
{
	private Dictionary<Vector2, Tile> slots = new Dictionary<Vector2, Tile>();

	private Dictionary<Vector2, Tile> bottomSlots = new Dictionary<Vector2, Tile>();

	public List<Piece> pieces = new List<Piece>();

	public static TileRegion2 instance;

	private GameLevel gameLevel;

	private void Awake()
	{
		instance = this;
	}

	public void LoadBoardBackground()
	{
		Transform backgroundTilesTransform = CMonoUtils.instance.backgroundTilesTransform;
		for (int num = backgroundTilesTransform.childCount - 1; num >= 0; num--)
		{
			Object.DestroyImmediate(backgroundTilesTransform.GetChild(num).gameObject);
		}
		slots.Clear();
		gameLevel = MakeLevelController.instance.gameLevel;
		float num2 = float.MaxValue;
		float num3 = float.MaxValue;
		float num4 = float.MinValue;
		float num5 = float.MinValue;
		if (string.IsNullOrEmpty(gameLevel.positions))
		{
			for (int i = 0; i < MakeLevelController.instance.numCol; i++)
			{
				for (int j = 0; j < MakeLevelController.instance.numRow; j++)
				{
					Tile tile = Object.Instantiate(CMonoUtils.instance.tile_background);
					tile.transform.SetParent(CMonoUtils.instance.backgroundTilesTransform);
					tile.transform.localScale = Vector3.one;
					Vector3 localPosition = GetLocalPosition(i, j);
					tile.transform.localPosition = localPosition;
					tile.position = new Vector2(i, j);
					tile.transform.GetChild(0).GetComponent<Text>().text = i + "," + j;
					tile.type = Tile.Type.Background;
					if (localPosition.x < num2)
					{
						num2 = localPosition.x;
					}
					if (localPosition.x > num4)
					{
						num4 = localPosition.x;
					}
					if (localPosition.y < num3)
					{
						num3 = localPosition.y;
					}
					if (localPosition.y > num5)
					{
						num5 = localPosition.y;
					}
				}
			}
		}
		else
		{
			List<string> list = CUtils.BuildListFromString<string>(gameLevel.positions);
			foreach (string item in list)
			{
				string[] array = item.Split(',');
				int num6 = int.Parse(array[0]);
				int num7 = int.Parse(array[1]);
				Tile tile2 = Object.Instantiate((array.Length != 2) ? CMonoUtils.instance.tile_stone : CMonoUtils.instance.tile_background);
				tile2.transform.SetParent(CMonoUtils.instance.backgroundTilesTransform);
				tile2.transform.localScale = Vector3.one;
				Vector3 localPosition2 = GetLocalPosition(num6, num7);
				tile2.transform.localPosition = localPosition2;
				tile2.position = new Vector2(num6, num7);
				if (array.Length == 2)
				{
					tile2.transform.GetChild(0).GetComponent<Text>().text = num6 + "," + num7;
					slots.Add(tile2.position, tile2);
				}
				if (localPosition2.x < num2)
				{
					num2 = localPosition2.x;
				}
				if (localPosition2.x > num4)
				{
					num4 = localPosition2.x;
				}
				if (localPosition2.y < num3)
				{
					num3 = localPosition2.y;
				}
				if (localPosition2.y > num5)
				{
					num5 = localPosition2.y;
				}
			}
		}
		float num8 = num4 - num2 + 84f + (num2 - 42f) * 2f;
		float num9 = num5 - num3 + 72.744f + (num3 - 36.372f) * 2f;
		base.transform.localPosition = new Vector3(0f, 163f) - new Vector3(num8 / 2f, num9 / 2f);
		GetComponent<RectTransform>().sizeDelta = new Vector2(num8, num9);
	}

	public void LoadBottomBackground()
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
				tile.transform.GetComponent<Image>().SetColorAlpha(0.4f);
				tile.type = Tile.Type.Background;
				bottomSlots.Add(tile.position, tile);
			}
		}
	}

	public void ClearPieces()
	{
		pieces.Clear();
		foreach (Transform item in CMonoUtils.instance.pieceRegion)
		{
			Object.Destroy(item.gameObject);
		}
	}

	public void LoadPieces(GameLevel gameLevel)
	{
		ClearPieces();
		List<string> list = CUtils.BuildListFromString<string>(gameLevel.pieces);
		int num = 0;
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
			for (int i = 0; i < list2.Count; i++)
			{
				string[] array = list2[i].Split(',');
				int num2 = int.Parse(array[0]);
				int num3 = int.Parse(array[1]);
				Vector2 vector2 = new Vector2(num2, num3);
				if (i == list2.Count - 1)
				{
					vector = vector2;
				}
				if (i != list2.Count - 1)
				{
					list3.Add(vector2);
				}
			}
			float num4 = 0.53f;
			Transform pieceRegion = CMonoUtils.instance.pieceRegion;
			Piece piece = CreatePiece(list3, pieceRegion);
			piece.boardPositions = GetMatchPositions(piece, vector);
			piece.isExtra = flag;
			pieces.Add(piece);
			piece.id = num++;
			piece.bottomPosition = vector;
			piece.transform.localScale = Vector3.one * num4;
			piece.transform.localPosition = GetLocalPosition(vector) * 0.53f;
		}
	}

	private Piece CreatePiece(List<Vector2> positions, Transform parent)
	{
		Piece2 piece = Object.Instantiate(CMonoUtils.instance.piece2Prefab);
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
			tile.piece2 = piece;
			tile.type = Tile.Type.Normal;
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

	public bool CheckMatch(Piece2 piece)
	{
		float num = float.MaxValue;
		Tile tile = null;
		foreach (Tile value in bottomSlots.Values)
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
		piece.matches = new List<Tile>();
		if (num < 0.2f)
		{
			bool result = true;
			List<Vector2> matchPositions = GetMatchPositions(piece, tile.position);
			foreach (Vector2 item in matchPositions)
			{
				if (!bottomSlots.ContainsKey(item) || bottomSlots[item].hasCover)
				{
					result = false;
					break;
				}
				piece.matches.Add(bottomSlots[item]);
			}
			return result;
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
}
