using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Game;
using PuzzleGames;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using View.Game;

namespace View.Control
{
    public class ScrollView : MonoBehaviour
    {
       public GameObject PuzzleGamePrefab;
       public GameObject winPanel;
       public GameObject losePanel;
       public TextMeshProUGUI timeToPlay;

       private bool _scrollEnabled;

       private int _selectedLevel;
       private GameObject[] _levels;
       private Tuple<float, float>[] _levelBounds;

       private float _listBottom;
       private float _listTop;
       
       private const float CameraZoomTime = 1f;

       private NavigationScript _navigation;
       private MoveDisplay _moveDisplay;
       private GameAudio _gameAudio;
       private BoardAction _boardAction;

       private void Awake()
       {
            LeanTween.init(30000);

          _navigation = GameObject.FindGameObjectWithTag("Navigation").GetComponent<NavigationScript>();
          _moveDisplay = GameObject.FindGameObjectWithTag("MoveDisplay").GetComponent<MoveDisplay>();
          _gameAudio = GameObject.FindGameObjectWithTag("GameAudio").GetComponent<GameAudio>();
          
          var language = Levels.CurrentLanguage;
          if (language.Equals("default")) {
             return;
          }
          
          try {
             var map = new Dictionary<string, string> {
                ["MusicToggleText"] = "music",
                ["SfxToggleText"] = "sfx",
                ["CreditText"] = "credit",
                ["VersionText"] = "version"
             };
                
             foreach(var entry in map) {
                var textDisplay = GameObject.FindGameObjectWithTag(entry.Key).GetComponent<Text>();
                textDisplay.text = Levels.Localization[language][entry.Value];
             }
          } catch (Exception e) {
             Debug.LogWarning("Failed to apply localization for " + language + "\n" + e);
          }
       }

       private void Start()
       {
          _selectedLevel = Levels.CurrentLevelNum + 1;
          
          GenerateLevelsList();

          if (_levels[_selectedLevel] == null) {
             Debug.LogError($"Failed to start at level ({_selectedLevel})");
             return;
          }
          
          var puzzleState = _levels[_selectedLevel].GetComponent<PuzzleState>();
          var puzzleScale = _levels[_selectedLevel].GetComponent<PuzzleScale>();
          
          puzzleState.BoardEnabled = true;
          puzzleScale.PuzzleInit += OnPuzzleInit;
          _boardAction = _levels[_selectedLevel].GetComponent<BoardAction>();
          _levels[_selectedLevel].GetComponent<BoardAction>().PuzzleWin += OnPuzzleWin;
          _levels[_selectedLevel].GetComponent<BoardAction>().PuzzleLose += OnPuzzleLose;
          puzzleState.LevelStateChanged += OnLevelStateChanged;
          
          var bounds = _levelBounds[_selectedLevel];
          var mid = (bounds.Item1 + bounds.Item2) / 2f;

          transform.position = Vector3.up * mid;
          
          puzzleState.Init(_selectedLevel);
       }

       IEnumerator CoGoHome()
       {
          yield return new WaitForSeconds(2f);
          EnableScroll();
       }

       private void FixedUpdate()
       {
          timeToPlay.text = _boardAction.getTimeToPlay().ToString();
       }

       private void Update()
       {
       }

       private void InterpolateCameraZoom(int currentLevel)
       {
       }

       private void Magnetize(int currentLevel)
       {
       }

       private void RevealLevels(int currentLevel)
       {
       }

       public void EnableScroll()
       {
          LoadSceneManager.Instance.LoadScene("Home");
       }

       private void DisableScroll(float delay = 0f)
       {
       }

       public void RestartLevel()
       {
          if (_scrollEnabled) {
             return;
          }
          
          _levels[_selectedLevel].GetComponent<PuzzleState>().RestartLevel();
       }

       private void GenerateLevelsList()
       {
          _levelBounds = new Tuple<float, float>[Levels.LevelCount];
          _levels = new GameObject[Levels.LevelCount];
          
          const float margin = 1.5f;

          _listBottom = margin;
          
          var prevOffset = 0f;

          GenerateLevel(_selectedLevel, margin, ref prevOffset);
          
          _listTop = prevOffset - margin;
       }

       private void GenerateLevel(int level, float margin, ref float prevOffset)
       {
          var puzzleGame = Instantiate(PuzzleGamePrefab);
          puzzleGame.name = $"PuzzleGame ({level})";
          puzzleGame.transform.SetParent(transform);

          var puzzleScale = puzzleGame.GetComponent<PuzzleScale>();

          var board = Levels.BuildLevel(level);

          if (board == null) {
             Debug.LogWarning($"Failed to create level ({level})");
             return;
          }
          
          var boardSize = (Vector2) board.Size * puzzleScale.Scaling / 2f;;
          
          _levels[level] = puzzleGame;

          puzzleGame.GetComponent<PuzzleState>().BoardEnabled = false;
          
          puzzleGame.GetComponent<BoardInput>().enabled = false;

          var boardHeight = boardSize.y * puzzleScale.Scaling / 2f;

          var boardStartBounds = prevOffset;
          prevOffset += boardHeight + margin;

          const float animationSpeed = 0.4f;
          const float delayScale = 0f;
          
          puzzleGame.transform.localPosition += Vector3.down * prevOffset;
          puzzleGame.GetComponent<PuzzleState>().Save(level, animationSpeed, delayScale);
          
          prevOffset += boardHeight + margin;
          var boardEndBounds = prevOffset;
          
          _levelBounds[level] = new Tuple<float, float>(boardStartBounds, boardEndBounds);
       }

       private void OnPuzzleInit()
       {
          if (_scrollEnabled) {
             return;
          }
          
          var puzzleScale = _levels[_selectedLevel].GetComponent<PuzzleScale>();
          CameraScript.FitToDimensions(puzzleScale.Dimensions, puzzleScale.Margin);
       }

       private void OnPuzzleWin(int level)
       {
          var puzzleState = _levels[_selectedLevel].GetComponent<PuzzleState>();
          
          // --- Dọn dẹp ---
          puzzleState.BoardEnabled = false;
          _levels[_selectedLevel].GetComponent<GameBoardAudio>().enabled = true;
          _levels[_selectedLevel].GetComponent<PuzzleScale>().PuzzleInit -= OnPuzzleInit;
          _levels[_selectedLevel].GetComponent<BoardAction>().PuzzleWin -= OnPuzzleWin;
          puzzleState.LevelStateChanged -= OnLevelStateChanged;
          
          // --- Analytics ---
          Analytics.CustomEvent("nodulus.level.complete", new Dictionary<string, object> {
             { "level.name", puzzleState.Metadata.Name },
             { "level.moves", puzzleState.NumMoves },
             { "level.movesBestScore", puzzleState.Metadata.MovesBestScore },
             { "level.timeElapsed", puzzleState.TimeElapsed },
             { "level.winCount", puzzleState.Metadata.WinCount }
          });

          // 2. Mở Bảng Hoàn thành (Completed Panel)
          // TODO: điều chỉnh độ trễ nếu cần
          const float openPanelDelay = 1.0f; // Đợi 1 giây trước khi hiển thị
          LeanTween.delayedCall(openPanelDelay, () => {
             if (winPanel != null)
             {
                winPanel.SetActive(true);
             }
             else
             {
                Debug.LogWarning("completedPanel chưa được gán trong Inspector!");
             }
          });

          StartCoroutine(CoGoHome());

          // --- LOGIC TẢI CẤP ĐỘ TIẾP THEO ĐÃ BỊ XÓA ---
       }

       private void OnPuzzleLose()
       {
          var heart = ResourceType.Heart.Manager();
          heart.Subtract(1);
          const float openPanelDelay = 1.0f; // Đợi 1 giây trước khi hiển thị
          LeanTween.delayedCall(openPanelDelay, () => {
             if (winPanel != null)
             {
                losePanel.SetActive(true);
             }
             else
             {
                Debug.LogWarning("completedPanel chưa được gán trong Inspector!");
             }
          });
          
          StartCoroutine(CoGoHome());

       }

       public void AddTime()
       {
          _boardAction.AddTime(5);
       }

       private void OnPan(TKPanRecognizer recognizer)
       {
       }

       private void OnPanComplete(TKPanRecognizer recognizer)
       {
       }

       private static void OnLevelStateChanged(Level level, bool win)
       {
          Levels.SaveLevel(level, win);
       }

       private int FindLevel(float yPos)
       {
          var bounds = _levelBounds[_selectedLevel];
          if (yPos < bounds.Item1) {
             for (var level = _selectedLevel - 1; level >= 0; level--) {
                bounds = _levelBounds[level];
                if (yPos <= bounds.Item2) {
                   return level;
                }
             }
          } else if (yPos > bounds.Item2) {
             for (var level = _selectedLevel + 1; level < Levels.LevelCount; level++) {
                bounds = _levelBounds[level];
                if (yPos >= bounds.Item1) {
                   return level;
                }
             }
             
          }

          return _selectedLevel;
       }

       private void OnTap(TKTapRecognizer recognizer)
       {
       }
       
       public void ToggleSettings()
       {
          _navigation.ToggleSettings();
       }

       public void ToggleMusic()
       {
          _gameAudio.MusicEnabled = !_gameAudio.MusicEnabled;
       }

       public void ToggleSfx()
       {
          _gameAudio.SfxEnabled = !_gameAudio.SfxEnabled;
       }

       public void ToggleFreeze()
       {
       }
    }
}