using UnityEngine;

public class CupManager : MonoBehaviour
{
    public Collector[] cups; // Array of CupController scripts attached to the cups

    private void Start()
    {
        foreach (Collector cup in cups)
        {
            cup.OnBallsCollected += CheckCupsStatus; // Subscribe to the event when balls are collected in a cup
        }
    }

    private void CheckCupsStatus()
    {
        bool allCupsReachedMinimum = true;

        foreach (Collector cup in cups)
        {
            if (cup.collectCount < cup.needToCollect)
            {
                allCupsReachedMinimum = false;
                break;
            }
        }

        if (allCupsReachedMinimum)
        {
            BGameManager.instance.GameWin();
        }
    }

    private void GameWin()
    {
        Debug.Log("Congratulations! You won the game!");
        // Add any additional game win logic here
    }
}
