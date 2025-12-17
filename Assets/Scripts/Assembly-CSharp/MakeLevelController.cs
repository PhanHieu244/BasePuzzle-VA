using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeLevelController : BaseController
{
	public TileRegion2 tileRegion;

	public GameLevel gameLevel;

	public InputField worldInput;

	public InputField levelInput;

	public InputField numColInput;

	public InputField numRowInput;

	public Text loadLevelText;

	public Button mapButton;

	public Button piecesButton;

	public Button applyButton;

	public int world;

	public int level;

	public int numRow;

	public int numCol;

	public List<Tile> listSlots = new List<Tile>();

	public List<Tile> listExtraTile = new List<Tile>();

	public bool doneGeneratePieces;

	private string piecesResult = string.Empty;

	private List<List<Tile>> pieces = new List<List<Tile>>();

	public int totalTiles;

	public static MakeLevelController instance;

	protected override void Awake()
	{
		base.Awake();
		instance = this;
	}

	protected override void Start()
	{
		base.Start();
		tileRegion.LoadBottomBackground();
		worldInput.text = PlayerPrefs.GetInt("level_editor_world", 1).ToString();
		levelInput.text = PlayerPrefs.GetInt("level_editor_level", 1).ToString();
		numRowInput.text = "5";
		numColInput.text = "6";
		UpdateUI();
	}

	public void OnInputValueChanged()
	{
		if (worldInput.text == "-")
		{
			worldInput.text = string.Empty;
		}
		if (levelInput.text == "-")
		{
			levelInput.text = string.Empty;
		}
		if (numRowInput.text == "-")
		{
			numRowInput.text = string.Empty;
		}
		if (numColInput.text == "-")
		{
			numColInput.text = string.Empty;
		}
		if (!string.IsNullOrEmpty(worldInput.text))
		{
			int.TryParse(worldInput.text, out world);
		}
		if (!string.IsNullOrEmpty(levelInput.text))
		{
			int.TryParse(levelInput.text, out level);
		}
		if (!string.IsNullOrEmpty(numRowInput.text))
		{
			int.TryParse(numRowInput.text, out numRow);
			if (numRow < 1)
			{
				numRowInput.text = "1";
			}
			else if (numRow > 7)
			{
				numRowInput.text = "7";
			}
		}
		if (!string.IsNullOrEmpty(numColInput.text))
		{
			int.TryParse(numColInput.text, out numCol);
			if (numCol < 1)
			{
				numColInput.text = "1";
			}
			else if (numCol > 7)
			{
				numColInput.text = "9";
			}
		}
		UpdateLoadLevelText();
	}

	public void UpdateLoadLevelText()
	{
		GameLevel gameLevel = Resources.Load<GameLevel>("Levels/World_" + world + "/Level_" + level);
		loadLevelText.text = ((!(gameLevel == null)) ? "Load" : "Add");
	}

	public void OnLoadClick()
	{
		gameLevel = Resources.Load<GameLevel>("Levels/World_" + world + "/Level_" + level);
		if (gameLevel == null)
		{
		}
		piecesResult = string.Empty;
		totalTiles = 0;
		pieces.Clear();
		listExtraTile.Clear();
		doneGeneratePieces = false;
		tileRegion.LoadBoardBackground();
		if (!string.IsNullOrEmpty(gameLevel.pieces))
		{
			tileRegion.LoadPieces(gameLevel);
			foreach (Piece piece in tileRegion.pieces)
			{
				pieces.Add(piece.tiles);
			}
			piecesResult = gameLevel.pieces;
			doneGeneratePieces = true;
		}
		else
		{
			tileRegion.ClearPieces();
		}
		LoadListSlots();
		UpdateUI();
		PlayerPrefs.SetInt("level_editor_world", world);
		PlayerPrefs.SetInt("level_editor_level", level);
	}

	private void UpdateUI()
	{
		if (gameLevel == null)
		{
			mapButton.interactable = false;
			piecesButton.interactable = false;
			applyButton.interactable = false;
		}
		else
		{
			mapButton.interactable = true;
			piecesButton.interactable = !string.IsNullOrEmpty(gameLevel.positions);
			applyButton.interactable = doneGeneratePieces;
		}
	}

	private void LoadListSlots()
	{
		listSlots.Clear();
		foreach (Transform item in CMonoUtils.instance.backgroundTilesTransform)
		{
			Tile component = item.GetComponent<Tile>();
			if (component.isActive && component.type == Tile.Type.Background)
			{
				listSlots.Add(component);
			}
		}
	}

	public void GeneratePositions()
	{
		string text = string.Empty;
		LoadListSlots();
		foreach (Tile listSlot in listSlots)
		{
			string text2 = text;
			text = text2 + listSlot.position.x + "," + listSlot.position.y + "|";
		}
		gameLevel.positions = text;
		MonoBehaviour.print(text);
		CreateOrReplaceAsset(gameLevel, GetLevelPath(world, level));
		UpdateLoadLevelText();
		UpdateUI();
	}

	public void GeneratePieces()
	{
		List<Tile> list = new List<Tile>();
		if (doneGeneratePieces)
		{
			foreach (Tile listSlot in listSlots)
			{
				if (!listSlot.isActive && !listExtraTile.Contains(listSlot))
				{
					list.Add(listSlot);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			listExtraTile.AddRange(list);
			string text = string.Empty;
			foreach (Tile item in list)
			{
				string text2 = text;
				text = text2 + item.position.x + "," + item.position.y + "-";
			}
			text += "0,0-r|";
			MonoBehaviour.print(text);
			piecesResult += text;
			gameLevel.pieces = piecesResult;
			tileRegion.LoadPieces(gameLevel);
			CreateOrReplaceAsset(gameLevel, GetLevelPath(world, level));
			return;
		}
		foreach (Tile listSlot2 in listSlots)
		{
			if (!listSlot2.isActive && !HasElement(listSlot2))
			{
				list.Add(listSlot2);
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		totalTiles += list.Count;
		pieces.Add(list);
		piecesResult = CreatePiecesResult();
		MonoBehaviour.print(piecesResult);
		if (totalTiles != listSlots.Count)
		{
			return;
		}
		doneGeneratePieces = true;
		gameLevel.pieces = piecesResult;
		tileRegion.LoadPieces(gameLevel);
		foreach (Tile listSlot3 in listSlots)
		{
			listSlot3.SetActive(true);
		}
		UpdateUI();
		CreateOrReplaceAsset(gameLevel, GetLevelPath(world, level));
	}

	private string CreatePiecesResult()
	{
		string text = string.Empty;
		foreach (List<Tile> piece in pieces)
		{
			foreach (Tile item in piece)
			{
				string text2 = text;
				text = text2 + item.position.x + "," + item.position.y + "-";
			}
			text += "0,0|";
		}
		return text;
	}

	public void AdjustPieces()
	{
		string text = string.Empty;
		foreach (Piece piece in tileRegion.pieces)
		{
			string text2;
			foreach (Vector2 defaultPosition in piece.defaultPositions)
			{
				text2 = text;
				text = text2 + defaultPosition.x + "," + defaultPosition.y + "-";
			}
			text2 = text;
			text = text2 + piece.boardPositions[0].x + "," + piece.boardPositions[0].y;
			if (piece.isExtra)
			{
				text += "-r";
			}
			text += "|";
		}
		gameLevel.pieces = text;
		CreateOrReplaceAsset(gameLevel, GetLevelPath(world, level));
		piecesResult = gameLevel.pieces;
	}

	public void ApplyLevel()
	{
	}

	private T CreateOrReplaceAsset<T>(T asset, string path) where T : ScriptableObject
	{
		return (T)null;
	}

	private bool HasElement(Tile element)
	{
		foreach (List<Tile> piece in pieces)
		{
			foreach (Tile item in piece)
			{
				if (element == item)
				{
					return true;
				}
			}
		}
		return false;
	}

	private string GetLevelPath(int world, int level)
	{
		return "Assets/Hexa_Puzzle/Resources/Levels/World_" + world + "/Level_" + level + ".asset";
	}
}
