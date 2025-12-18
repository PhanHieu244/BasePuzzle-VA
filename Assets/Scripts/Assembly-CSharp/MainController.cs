using System.Collections;
using System.Collections.Generic;
using PuzzleGames;
using Superpow;
using UnityEngine;
using UnityEngine.UI;

public class MainController : BaseController
{
	public TileRegion tileRegion;

	public GameObject hintPieces;

	public GameLevel gameLevel;

	public int level = 1;

	public int world = 1;

	public LevelPrefs levelPrefs;

	public Text levelText;

	public static MainController instance;

	protected override void Awake()
	{
		base.Awake();
		instance = this;
	}

	protected override void Start()
	{
		base.Start();
		level = GameState.chosenLevel;
		world = GameState.chosenWorld;
		gameLevel = Resources.Load<GameLevel>("Levels/World_" + world + "/Level_" + level);
		string levelData = Utils.GetLevelData(world, level);
		if (string.IsNullOrEmpty(levelData))
		{
			levelPrefs = new LevelPrefs();
			levelPrefs.piecesPrefs = new List<PiecePrefs>();
		}
		else
		{
			levelPrefs = JsonUtility.FromJson<LevelPrefs>(levelData);
		}
		tileRegion.Load(gameLevel);
		GameState.canPlay = true;
		levelText.text = "Level " + (level - 1);
		ProcessLevelGift();
		Utils.IncreaseNumMoves(world, level);
	}

	public void Replay()
	{
		var gold = ResourceType.Gold.Manager();
		if (gold.GetAmount() < 10)
		{
			UIToastManager.Instance.Show("You do not have enough gold.");
			return;
		}
		
		gold.Subtract(10);
		GameState.canPlay = true;
		foreach (Piece piece in tileRegion.pieces)
		{
			piece.MoveToBottom();
		}
		CSound.instance.Play(CSound.Others.Replay);
	}

	public void ShowHint()
	{
		var hint = ResourceType.Powerup_Helidrop.Manager();
		if (hint.GetAmount() <= 0)
		{
			UIToastManager.Instance.Show("You do not have enough hint.");
		}
		else if (tileRegion.ShowHint())
		{
			hint.Subtract(1);
			AddHint(-1);
		}
	}

	public void AddHint(int num)
	{
		GameState.hint.ChangeValue(num);
	}

	public void ProcessLevelGift()
	{
		if (level % 4 == 3 && !Utils.IsGiftReceived(world, level))
		{
			Utils.ReceiveGift(world, level);
			Timer.Schedule(this, 0.5f, delegate
			{
				DialogController.instance.ShowDialog(DialogType.LevelGift);
			});
		}
	}

	public void OnComplete(int numTile)
	{
		LevelController.levelCompletions++;
		Debug.Log("### OnComplete " + world + " " + level);
		SingletonMono<TimeTracker>.instance.StopTimerAndLogLevel(world, level);
		GameState.canPlay = false;
		SavePrefs();
		int unlockLevel = LevelController.GetUnlockLevel(world);
		if (level == unlockLevel)
		{
			LevelController.SetUnlockLevel(world, unlockLevel + 1);
		}
		Timer.Schedule(this, (float)numTile * 0.03f + 0.7f, delegate
		{
			DialogController.instance.ShowDialog(DialogType.Complete);
		});
		CSound.instance.Play(CSound.Others.Complete);
	}

	private void SavePrefs()
	{
		string data = JsonUtility.ToJson(levelPrefs);
		Utils.SetLevelData(world, level, data);
	}

	public void BackToMainMenu()
	{
		LoadSceneManager.Instance.LoadScene("Home");
	}
}
