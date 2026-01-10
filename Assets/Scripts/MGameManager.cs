using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MGameManager : MonoBehaviour
{
    public GridManager grid;
    [FormerlySerializedAs("moveList")] public MMoveList mMoveList;
    public BonusManager bonusManager;

    public void StartNewGame()
    {
        grid.GenerateGrid();
        mMoveList.GenerateMoveList();
        bonusManager.GenerateBonuses();
    }

    private void Start()
    {
        StartNewGame();
    }
}

