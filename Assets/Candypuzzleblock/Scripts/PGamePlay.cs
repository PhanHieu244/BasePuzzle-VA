using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Xml.Linq;


using DG.Tweening;
using PuzzleGames;


// This script has main logic to run entire gameplay.
public class PGamePlay : PSingleton<PGamePlay>,IPointerDownHandler,IPointerUpHandler, IBeginDragHandler,IDragHandler
{
	[HideInInspector]
	public List<PBlock> blockGrid;

	PShapeInfo _currentPShape = null;
	Transform hittingBlock = null;

	List<PBlock> highlightingBlocks;

	public AudioClip blockPlaceSound;
	public AudioClip blockSelectSound;

	// Line break sounds.
	[SerializeField] private AudioClip lineClear1;
	[SerializeField] private AudioClip lineClear2;
	[SerializeField] private AudioClip lineClear3;
	[SerializeField] private AudioClip lineClear4;

	public AudioClip blockNotPlacedSound;

	[Tooltip("Max no. of times rescue can be used in 1 game. -1 is infinite")]
	[SerializeField] private int MaxAllowedRescuePerGame = 0;

	[Tooltip("Max no. of times rescue can be used in 1 game using watch video. -1 is infinite")]
	[SerializeField] private int MaxAllowedVideoWatchRescue = 0;

	[HideInInspector]
	public int TotalFreeRescueDone = 0;

	[HideInInspector]
	public int TotalRescueDone = 0;

	[HideInInspector]
	public int MoveCount = 0;

	public Sprite BombSprite;
	public PTimer timeSlider;

	public  bool isHelpOnScreen = false;

    public int currentID;

	void Start ()
	{
        Application.targetFrameRate = 60;
		//Generate board from GameBoardGenerator Script Component.
		GetComponent<PGameBoardGenerator> ().GenerateBoard ();
		highlightingBlocks = new List<PBlock> ();

		#region time mode
		// Timer will start with TIME and CHALLENGE mode.
		if(PGameController.gameMode == GameMode.TIMED || PGameController.gameMode == GameMode.CHALLENGE)
		{
			timeSlider.gameObject.SetActive(true);
		}
		#endregion

		#region check for help
		Invoke("CheckForHelp",0.5F);
		#endregion
	}

	#region IBeginDragHandler implementation

	/// <summary>
	/// Raises the begin drag event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnBeginDrag (PointerEventData eventData)
	{
		if (_currentPShape != null) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (eventData.position);
			pos.z = _currentPShape.transform.localPosition.z;
			_currentPShape.transform.localPosition = pos;
		}
	}

	#endregion

	#region IPointerDownHandler implementation
	/// <summary>
	/// Raises the pointer down event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerDown (PointerEventData eventData)
	{
		if (eventData.pointerCurrentRaycast.gameObject != null) {
			Transform clickedObject = eventData.pointerCurrentRaycast.gameObject.transform;

			if (clickedObject.GetComponent<PShapeInfo>() != null) 
			{
				if (clickedObject.transform.childCount > 0) {
					_currentPShape = clickedObject.GetComponent<PShapeInfo>();
					Vector3 pos = Camera.main.ScreenToWorldPoint (eventData.position);
					_currentPShape.transform.localScale = Vector3.one;
					_currentPShape.transform.localPosition = new Vector3 (pos.x, pos.y, 0);
					PAudioManager.Instance.PlaySound (blockSelectSound);

					if (isHelpOnScreen) {
						GetComponent<PInGameHelp> ().StopHelp ();
					}
				}
			}
		}
	}
	#endregion

	#region IPointerUpHandler implementation
	/// <summary>
	/// Raises the pointer up event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerUp (PointerEventData eventData)
	{
		if (_currentPShape != null) {

			if (highlightingBlocks.Count > 0) 
			{
				SetImageToPlacingBlocks ();
				Destroy (_currentPShape.gameObject);
				_currentPShape = null;
				MoveCount += 1;
				Invoke ("CheckBoardStatus", 0.1F);
			} else {
		
				_currentPShape.transform.DOLocalMove (Vector3.zero, 0.5F);
				_currentPShape.transform.DOScale (Vector3.one * 0.6F, 0.5F);
		
				_currentPShape = null;
				PAudioManager.Instance.PlaySound (blockNotPlacedSound);
			}
		}
	}
	#endregion

	#region IDragHandler implementation
	/// <summary>
	/// Raises the drag event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnDrag (PointerEventData eventData)
	{
		if (_currentPShape != null) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (eventData.position);
			pos = new Vector3(pos.x, (pos.y + 1F),0F);

			_currentPShape.transform.position = pos;

			RaycastHit2D hit = Physics2D.Raycast (_currentPShape.GetComponent<PShapeInfo>().firstBlock.block.position, Vector2.zero, 1);

			if (hit.collider != null) 
			{
				if (hittingBlock == null || hit.collider.transform != hittingBlock) {
					hittingBlock = hit.collider.transform;
					CanPlaceShape (hit.collider.transform);
				}
			} else {
				StopHighlighting ();
			}
		}
	}
	#endregion

	/// <summary>
	/// Determines whether this instance can place shape the specified currentHittingBlock.
	/// </summary>
	public bool CanPlaceShape (Transform currentHittingBlock)
	{
		PBlock currentCell = currentHittingBlock.GetComponent<PBlock> ();

		int currentRowID = currentCell.rowID;
		int currentColumnID = currentCell.columnID;

		StopHighlighting ();

		bool canPlaceShape = true;
		foreach (ShapeBlock c in _currentPShape.ShapeBlocks) 
		{
			PBlock checkingCell = blockGrid.Find (o => o.rowID == currentRowID + (c.rowID + _currentPShape.startOffsetX) && o.columnID == currentColumnID + (c.columnID - _currentPShape.startOffsetY));

			if ((checkingCell == null) || (checkingCell != null && checkingCell.isFilled)) {
				canPlaceShape = false;
				highlightingBlocks.Clear ();
				break;
			} else {
				if (!highlightingBlocks.Contains (checkingCell)) {
					highlightingBlocks.Add (checkingCell);
				}
			}
		}

		if (canPlaceShape) {
			SetHighLightImage ();
		}

		return canPlaceShape;
	}

	/// <summary>
	/// Sets the high light image.
	/// </summary>
	void SetHighLightImage ()
	{
		foreach (PBlock c in highlightingBlocks) {
			c.SetHighlightImage (_currentPShape.blockImage);
		}
	}

	/// <summary>
	/// Stops the highlighting.
	/// </summary>
	void StopHighlighting ()
	{
		if (highlightingBlocks != null && highlightingBlocks.Count > 0) {
			foreach (PBlock c in highlightingBlocks) {
				c.StopHighlighting ();
			}
		}
		hittingBlock = null;
		highlightingBlocks.Clear ();
	}

	/// <summary>
	/// Sets the image to placing blocks.
	/// </summary>
	void SetImageToPlacingBlocks ()
	{
		if (highlightingBlocks != null && highlightingBlocks.Count > 0) 
		{
			foreach (PBlock c in highlightingBlocks) {
				c.SetBlockImage (_currentPShape.blockImage,_currentPShape.ShapeID);
			}
		}
        currentID = int.Parse(_currentPShape.blockImage.name) - 1;
		PAudioManager.Instance.PlaySound (blockPlaceSound);
	}

	/// <summary>
	/// Checks the board status.
	/// </summary>
	void CheckBoardStatus()
	{
		int placingShapeBlockCount = highlightingBlocks.Count;
		List<int> updatedRows = new List<int> ();
		List<int> updatedColumns = new List<int> ();

		List<List<PBlock>> breakingRows = new List<List<PBlock>> ();
		List<List<PBlock>> breakingColumns = new List<List<PBlock>> ();

		foreach (PBlock b in highlightingBlocks) {
			if (!updatedRows.Contains (b.rowID)) {
				updatedRows.Add(b.rowID);
			}
			if (!updatedColumns.Contains (b.columnID)) {
				updatedColumns.Add(b.columnID);
			}
		}
		highlightingBlocks.Clear ();

		foreach(int rowID in updatedRows) {
			List<PBlock> currentRow = GetEntireRow (rowID);
			if (currentRow != null) {
				breakingRows.Add (currentRow);
			}
		}

		foreach (int columnID in updatedColumns) {
			List<PBlock> currentColumn = GetEntireColumn (columnID);
			if (currentColumn != null) {
				breakingColumns.Add (currentColumn);
			}
		}

		if (breakingRows.Count > 0 || breakingColumns.Count > 0) {
			StartCoroutine(BreakAllCompletedLines(breakingRows, breakingColumns,placingShapeBlockCount));
		} else {
			PBlockShapeSpawner.Instance.FillShapeContainer ();
			PScoreManager.Instance.AddScore (10 * placingShapeBlockCount);
		}

		if (PGameController.gameMode == GameMode.BLAST || PGameController.gameMode == GameMode.CHALLENGE) {
			Invoke ("UpdateBlockCount", 0.5F);
		}
	}

	public void ResetShape()
	{
		PBlockShapeSpawner.Instance.ResetShapeContainer();
	}

	public void ClearMap()
	{
		foreach (PBlock b in blockGrid) {
			b.ClearBlock(currentID);
		}
	}

	/// <summary>
	/// Gets the entire row.
	/// </summary>
	/// <returns>The entire row.</returns>
	/// <param name="rowID">Row I.</param>
	List<PBlock> GetEntireRow(int rowID)
	{
		List<PBlock> thisRow = new List<PBlock> ();
		for (int columnIndex = 0; columnIndex < PGameBoardGenerator.Instance.TotalColumns; columnIndex++) {
			PBlock pBlock = blockGrid.Find (o => o.rowID == rowID && o.columnID == columnIndex); 

			if (pBlock.isFilled) {
				thisRow.Add (pBlock);
			} else {
				return null;
			}
		}
		return thisRow;	
	}

	/// <summary>
	/// Gets the entire row for rescue.
	/// </summary>
	/// <returns>The entire row for rescue.</returns>
	/// <param name="rowID">Row I.</param>
	List<PBlock> GetEntireRowForRescue(int rowID)
	{
		List<PBlock> thisRow = new List<PBlock> ();
		for (int columnIndex = 0; columnIndex < PGameBoardGenerator.Instance.TotalColumns; columnIndex++) {
			PBlock pBlock = blockGrid.Find (o => o.rowID == rowID && o.columnID == columnIndex); 
			thisRow.Add (pBlock);
		}
		return thisRow;	
	}

	/// <summary>
	/// Gets the entire column.
	/// </summary>
	/// <returns>The entire column.</returns>
	/// <param name="columnID">Column I.</param>
	List<PBlock> GetEntireColumn(int columnID)
	{
		List<PBlock> thisColumn = new List<PBlock> ();
		for (int rowIndex = 0; rowIndex < PGameBoardGenerator.Instance.TotalRows; rowIndex++) {
			PBlock pBlock = blockGrid.Find (o => o.rowID == rowIndex && o.columnID == columnID); 
			if (pBlock.isFilled) {
				thisColumn.Add (pBlock);
			} else {
				return null;
			}
		}
		return thisColumn;	
	}

	/// <summary>
	/// Gets the entire column for rescue.
	/// </summary>
	/// <returns>The entire column for rescue.</returns>
	/// <param name="columnID">Column I.</param>
	List<PBlock> GetEntireColumnForRescue(int columnID)
	{
		List<PBlock> thisColumn = new List<PBlock> ();
		for (int rowIndex = 0; rowIndex < PGameBoardGenerator.Instance.TotalRows; rowIndex++) {
			PBlock pBlock = blockGrid.Find (o => o.rowID == rowIndex && o.columnID == columnID); 
			thisColumn.Add (pBlock);
		}
		return thisColumn;	
	}

	/// <summary>
	/// Breaks all completed lines.
	/// </summary>
	/// <returns>The all completed lines.</returns>
	/// <param name="breakingRows">Breaking rows.</param>
	/// <param name="breakingColumns">Breaking columns.</param>
	/// <param name="placingShapeBlockCount">Placing shape block count.</param>
	IEnumerator BreakAllCompletedLines(List<List<PBlock>> breakingRows, List<List<PBlock>> breakingColumns, int placingShapeBlockCount)
	{
		int TotalBreakingLines = breakingRows.Count + breakingColumns.Count;
	
		if (TotalBreakingLines == 1) {
			PAudioManager.Instance.PlaySound (lineClear1);
			PScoreManager.Instance.AddScore (100 + (placingShapeBlockCount * 10));
		} else if (TotalBreakingLines == 2) {
			PAudioManager.Instance.PlaySound (lineClear2);
			PScoreManager.Instance.AddScore (300+ (placingShapeBlockCount * 10));
		} else if (TotalBreakingLines == 3) {
			PAudioManager.Instance.PlaySound (lineClear3);
			PScoreManager.Instance.AddScore (600+ (placingShapeBlockCount * 10));
		} else if (TotalBreakingLines >= 4) {
			PAudioManager.Instance.PlaySound (lineClear4);
			if (TotalBreakingLines == 4) {
				PScoreManager.Instance.AddScore (1000+ (placingShapeBlockCount * 10));
			} else {
				PScoreManager.Instance.AddScore ((300 * TotalBreakingLines)+ (placingShapeBlockCount * 10));
			}
		}

		yield return 0;
		if (breakingRows.Count > 0) {
			foreach(List<PBlock> thisLine in breakingRows) {
				StartCoroutine(BreakThisLine (thisLine));
			}	
		}
		if (breakingRows.Count > 0) {
			yield return new WaitForSeconds (0.1F);
		}

		if (breakingColumns.Count > 0) {
			foreach(List<PBlock> thisLine in breakingColumns){
				StartCoroutine(BreakThisLine (thisLine));
			}	
		}


		PBlockShapeSpawner.Instance.FillShapeContainer ();

		#region time mode
		if(PGameController.gameMode == GameMode.TIMED || PGameController.gameMode == GameMode.CHALLENGE){
			timeSlider.AddSeconds(TotalBreakingLines * 5);
		}
		#endregion
	}

	/// <summary>
	/// Breaks the this line.
	/// </summary>
	/// <returns>The this line.</returns>
	/// <param name="breakingLine">Breaking line.</param>
	IEnumerator BreakThisLine(List<PBlock> breakingLine)
	{
		foreach (PBlock b in breakingLine) {
            //b.ClearBlock (int.Parse(breakingLine[breakingLine.Count-1].blockImage.GetComponent<Image>().sprite.name) -1);
            b.ClearBlock(currentID);
			yield return new WaitForSeconds (0.02F);
		}
		yield return 0;
	}

	/// <summary>
	/// Determines whether this instance can existing blocks placed the specified OnBoardBlockShapes.
	/// </summary>
	/// <returns><c>true</c> if this instance can existing blocks placed the specified OnBoardBlockShapes; otherwise, <c>false</c>.</returns>
	/// <param name="OnBoardBlockShapes">On board block shapes.</param>
	public bool CanExistingBlocksPlaced(List<PShapeInfo> OnBoardBlockShapes)
	{
		foreach (PBlock block in blockGrid) {
			if (!block.isFilled) {
				foreach (PShapeInfo info in OnBoardBlockShapes) {
					bool canPlace = CheckShapeCanPlace (block, info);
					if (canPlace) {
						return true;
					}
				}
			}
		}
		return false;
	}

	/// <summary>
	/// Checks the shape can place.
	/// </summary>
	/// <returns><c>true</c>, if shape can place was checked, <c>false</c> otherwise.</returns>
	/// <param name="placingPBlock">Placing block.</param>
	/// <param name="placingBlockPShape">Placing block shape.</param>
	bool CheckShapeCanPlace(PBlock placingPBlock, PShapeInfo placingBlockPShape)
	{
		int currentRowID = placingPBlock.rowID;
		int currentColumnID = placingPBlock.columnID;

		if (placingBlockPShape != null && placingBlockPShape.ShapeBlocks != null) {
			foreach (ShapeBlock c in placingBlockPShape.ShapeBlocks) {
				PBlock checkingCell = blockGrid.Find (o => o.rowID == currentRowID + (c.rowID + placingBlockPShape.startOffsetX) && o.columnID == currentColumnID + (c.columnID - placingBlockPShape.startOffsetY));

				if ((checkingCell == null) || (checkingCell != null && checkingCell.isFilled)) {
					return false;
				}
			}
		}
		return true;
	}

	/// <summary>
	/// Raises the unable to place shape event.
	/// </summary>
	public void OnUnableToPlaceShape()
	{
		if ((TotalRescueDone < MaxAllowedRescuePerGame) || MaxAllowedRescuePerGame < 0) {
			PGamePlayUI.Instance.ShowRescue (GameOverReason.OUT_OF_MOVES);
		} else {
			OnGameOver ();
		}
	}

	/// <summary>
	/// Raises the bomb counter over event.
	/// </summary>
	public void OnBombCounterOver(){
		if ((TotalRescueDone < MaxAllowedRescuePerGame) || MaxAllowedRescuePerGame < 0) {
			PGamePlayUI.Instance.ShowRescue (GameOverReason.BOMB_COUNTER_ZERO);
		} else {
			OnGameOver ();
		}
	}

	/// <summary>
	/// Executes the rescue.
	/// </summary>
	void ExecuteRescue()
	{
		if (PGamePlayUI.Instance.currentGameOverReson == GameOverReason.OUT_OF_MOVES) {
			int totalBreakingColumns = 3;
			int totalBreakingRows = 3;

			int totalColumns = PGameBoardGenerator.Instance.TotalColumns;
			int totalRows = PGameBoardGenerator.Instance.TotalRows;

            int startingColumn = (int)((totalColumns / 2F) - (totalBreakingColumns / 2F));
            //int startingColumn = Random.Range(0, totalColumns - TotalBreakingColumns);
            int startingRow = (int)((totalRows / 2F) - (totalBreakingRows / 2F));

			List<List<PBlock>> breakingColums = new List<List<PBlock>> ();

			for (int columnIndex = startingColumn; columnIndex <= (startingColumn + (totalBreakingColumns - 1)); columnIndex++) {
				breakingColums.Add (GetEntireColumnForRescue (columnIndex));
			}

			List<List<PBlock>> breakingRows = new List<List<PBlock>> ();

			for (int rowIndex = startingRow; rowIndex <= (startingRow + (totalBreakingRows - 1)); rowIndex++) {
				breakingRows.Add (GetEntireRowForRescue (rowIndex));
			}
			StartCoroutine (BreakAllCompletedLines (breakingRows, breakingColums, 0));
		}

		#region bomb mode
		if ((PGameController.gameMode == GameMode.BLAST || PGameController.gameMode == GameMode.CHALLENGE) && PGamePlayUI.Instance.currentGameOverReson == GameOverReason.BOMB_COUNTER_ZERO) {
			List<PBlock> bombBlocks = blockGrid.FindAll (o => o.isBomb);
			foreach (PBlock block in bombBlocks) {
				if (block.bombCounter <= 1) {
					block.SetCounter (block.bombCounter + 4);
				}
				block.DecreaseCounter ();
			}
		}
		#endregion

		#region time mode
		if (PGameController.gameMode == GameMode.TIMED || PGameController.gameMode == GameMode.CHALLENGE) 
		{
			if(PGamePlayUI.Instance.currentGameOverReson == GameOverReason.TIME_OVER)
			{
				timeSlider.AddSeconds(30);
			}
			timeSlider.ResumeTimer();
		}
		#endregion
	}

    public void InstantRescue()
    {
        int totalBreakingColumns = 3;
        int totalBreakingRows = 3;

        int totalColumns = PGameBoardGenerator.Instance.TotalColumns;
        int totalRows = PGameBoardGenerator.Instance.TotalRows;

        //int startingColumn = (int)((totalColumns / 2F) - (TotalBreakingColumns / 2F));
        int startingColumn = Random.Range(0, totalColumns - totalBreakingColumns);
        //int startingRow = (int)((totalRows / 2F) - (TotalBreakingRows / 2F));
        int startingRow = Random.Range(0, totalRows - totalBreakingRows);

        List<List<PBlock>> breakingColums = new List<List<PBlock>>();

        for (int columnIndex = startingColumn; columnIndex <= (startingColumn + (totalBreakingColumns - 1)); columnIndex++)
        {
            breakingColums.Add(GetEntireColumnForRescue(columnIndex));
        }

        List<List<PBlock>> breakingRows = new List<List<PBlock>>();

        for (int rowIndex = startingRow; rowIndex <= (startingRow + (totalBreakingRows - 1)); rowIndex++)
        {
            breakingRows.Add(GetEntireRowForRescue(rowIndex));
        }
        StartCoroutine(BreakAllCompletedLines(breakingRows, breakingColums, 0));
    }

	/// <summary>
	/// Ises the free rescue available.
	/// </summary>
	/// <returns><c>true</c>, if free rescue available was ised, <c>false</c> otherwise.</returns>
	public bool IsFreeRescueAvailable()
	{
		if((TotalFreeRescueDone < MaxAllowedVideoWatchRescue) || (MaxAllowedVideoWatchRescue < 0))
		{
			return true;
		}
		return false;
	}

	/// <summary>
	/// Raises the rescue done event.
	/// </summary>
	/// <param name="isFreeRescue">If set to <c>true</c> is free rescue.</param>
	public void OnRescueDone(bool isFreeRescue) 
	{
		if (isFreeRescue) {
			TotalFreeRescueDone += 1;
		} else {
			TotalRescueDone += 1;
		}
		CloseRescuePopup ();
		Invoke ("ExecuteRescue", 0.5F);
	}

	/// <summary>
	/// Raises the rescue discarded event.
	/// </summary>
	public void OnRescueDiscarded()
	{
        CloseRescuePopup();
		OnGameOver ();
	}

	/// <summary>
	/// Closes the rescue popup.
	/// </summary>
	void CloseRescuePopup()
	{
		GameObject currentWindow = PStackManager.Instance.WindowStack.Peek ();
		if (currentWindow != null) {
			if (currentWindow.OnWindowRemove () == false) {
				Destroy (currentWindow);
			}
			PStackManager.Instance.WindowStack.Pop ();
		}
	}

	/// <summary>
	/// Raises the game over event.
	/// </summary>
	public void OnGameOver()
	{
        
		GameObject gameOverScreen = PStackManager.Instance.SpawnUIScreen ("GameOver");
        /*
        int coinReward = 0;
        if (GameController.gameMode == GameMode.CLASSIC)
            coinReward = 15;
        else if (GameController.gameMode == GameMode.TIMED)
            coinReward = 10;
        else if (GameController.gameMode == GameMode.CHALLENGE)
            coinReward = 15;
        else if (GameController.gameMode == GameMode.BLAST)
            coinReward = 5;
        else if (GameController.gameMode == GameMode.ADVANCE)
            coinReward = 25;
            */
        gameOverScreen.GetComponent<PGameOver> ().SetLevelScore (FindObjectOfType<PScoreManager>().GetScore(), 10);
        //FireBaseManager.instance.LogScreen("High Score : " + FindObjectOfType<ScoreManager>().GetScore().ToString() 
        //               + "  " + GameController.gameMode.ToString());
      //  StartCoroutine(ShowAds());
      StartCoroutine(CoGoHome());
	}

	IEnumerator CoGoHome()
	{
		yield return new WaitForSeconds (2F);
		LoadSceneManager.Instance.LoadScene("Home");
	}
    IEnumerator ShowAds()
    {
        yield return new WaitForSeconds(0.5f);
       // AdsControl.Instance.showAds();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Clearing");
            InstantRescue();
        }
    }

    #region Bomb Mode Specific
    /// <summary>
    /// Updates the block count.
    /// </summary>
    void UpdateBlockCount()
	{
		List<PBlock> bombBlocks = blockGrid.FindAll (o => o.isBomb);
		foreach (PBlock block in bombBlocks) {
			block.DecreaseCounter ();
		}

		bool doPlaceBomb = false;
		if (MoveCount <= 10) {
			if (MoveCount % 5 == 0) {
				doPlaceBomb = true;
			}
		} else if (MoveCount <= 30) {
			if (((MoveCount-10) % 4 == 0)) {
				doPlaceBomb = true;
			}

		} else if (MoveCount <= 48) {
			if (((MoveCount-30) % 3 == 0)) {
				doPlaceBomb = true;
			}
		} else {
			if (((MoveCount-48) % 2 == 0)) {
				doPlaceBomb = true;
			}
		}

		if (doPlaceBomb) {
			PlaceAtRandomPlace ();
		}
	}

	/// <summary>
	/// Places at random place.
	/// </summary>
	void PlaceAtRandomPlace()
	{
		List<int> BombPlacingRows = new List<int> ();

		for (int rowIndex = 0; rowIndex < PGameBoardGenerator.Instance.TotalRows; rowIndex++) {
			bool canPlace = CheckRowForBombPlace (rowIndex);
			if (canPlace) {
				BombPlacingRows.Add (rowIndex);
			}
		}

		int RandomRowForPlacingBomb = 0;
		if (BombPlacingRows.Count > 0) {
			RandomRowForPlacingBomb = BombPlacingRows [UnityEngine.Random.Range (0, BombPlacingRows.Count)];
		} else {
			RandomRowForPlacingBomb = UnityEngine.Random.Range (0, PGameBoardGenerator.Instance.TotalRows);
		}

		PlaceBombAtRow (RandomRowForPlacingBomb);
	}

	/// <summary>
	/// Places the bomb at row.
	/// </summary>
	/// <param name="rowIndex">Row index.</param>
	void PlaceBombAtRow(int rowIndex)
	{
		List<PBlock> emptyBlockForThisRow = blockGrid.FindAll (o => o.rowID == rowIndex && o.isFilled == false); 
		PBlock randomPBlock = emptyBlockForThisRow [UnityEngine.Random.Range (0, emptyBlockForThisRow.Count)];

		if (randomPBlock != null) {
			randomPBlock.ConvertToBomb ();
		}
	}

	/// <summary>
	/// Checks the row for bomb place.
	/// </summary>
	/// <returns><c>true</c>, if row for bomb place was checked, <c>false</c> otherwise.</returns>
	/// <param name="rowIndex">Row index.</param>
	bool CheckRowForBombPlace(int rowIndex)
	{
		PBlock pBlock = blockGrid.Find (o => o.rowID == rowIndex && o.isFilled == true);
		int totalEmptyBlocks = blockGrid.FindAll (o => o.rowID == rowIndex && o.isFilled == false).Count;

		if (pBlock != null && totalEmptyBlocks >= 2) {
			return true;
		} else {
			return false;
		}
	}
	#endregion


	#region show help if mode opened first time
	/// <summary>
	/// Checks for help.
	/// </summary>
	public void CheckForHelp()
	{
		bool isHelpShown = false;
		if (PGameController.gameMode == GameMode.BLAST || PGameController.gameMode == GameMode.ADVANCE || PGameController.gameMode == GameMode.TIMED) {
			isHelpShown = PlayerPrefs.GetInt ("isHelpShown_" + PGameController.gameMode.ToString (), 0) == 0 ? false : true;
			if (!isHelpShown) {
				PInGameHelp pInGameHelp = gameObject.AddComponent<PInGameHelp> ();
				pInGameHelp.StartHelp ();
			} else {
				ShowBasicHelp ();
			}
		} else {
			ShowBasicHelp ();
		}
	}

	/// <summary>
	/// Raises the help popup closed event.
	/// </summary>
	public void OnHelpPopupClosed()
	{
		if (PGameController.gameMode == GameMode.TIMED) {
			timeSlider.ResumeTimer ();
		} 
		ShowBasicHelp ();
	}

	/// <summary>
	/// Shows the basic help.
	/// </summary>
	void ShowBasicHelp()
	{
		bool isBasicHelpShown = PlayerPrefs.GetInt ("isBasicHelpShown", 0) == 0 ? false : true;
		if (!isBasicHelpShown) {
			PInGameHelp pInGameHelp = gameObject.GetComponent<PInGameHelp> ();
			if (pInGameHelp == null) {
				pInGameHelp = gameObject.AddComponent<PInGameHelp> ();
			}
			pInGameHelp.ShowBasicHelp ();
		}
	}
	#endregion
}