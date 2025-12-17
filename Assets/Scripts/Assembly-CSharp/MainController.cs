using System.Collections;
using System.Collections.Generic;
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
		levelText.text = "Level " + level;
		ProcessLevelGift();
		Utils.IncreaseNumMoves(world, level);
		CUtils.ShowBannerAd();
	}

	public void Replay()
	{
		GameState.canPlay = true;
		foreach (Piece piece in tileRegion.pieces)
		{
			piece.MoveToBottom();
		}
		CUtils.ShowInterstitialAd();
		CSound.instance.Play(CSound.Others.Replay);
	}

	public void ShowHint()
	{
		if (GameState.hint.GetValue() <= 0)
		{
			if (Purchaser.instance.isEnable)
			{
				DialogController.instance.ShowDialog(DialogType.Shop);
			}
		}
		else if (tileRegion.ShowHint())
		{
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
			StartCoroutine(ReportCompletition());
		}
		Timer.Schedule(this, (float)numTile * 0.03f + 0.7f, delegate
		{
			DialogController.instance.ShowDialog(DialogType.Complete);
		});
		CSound.instance.Play(CSound.Others.Complete);
	}

	private IEnumerator ReportCompletition()
	{
		int levelsCount2 = 0;
		for (int i = 1; i <= 4; i++)
		{
			levelsCount2 += LevelController.GetUnlockLevel(i);
		}
		levelsCount2 -= 4;
		string url = "https://1-dot-luee-wally-v2-cpc.appspot.com/game_progress?game_id=" + Application.identifier + "&gaid=" + AdNetworksManager.instance.advertisingId + "&score=" + levelsCount2 + "&is_new_record=true";
		yield return new WWW(url);
	}

	private void SavePrefs()
	{
		string data = JsonUtility.ToJson(levelPrefs);
		Utils.SetLevelData(world, level, data);
	}
}
