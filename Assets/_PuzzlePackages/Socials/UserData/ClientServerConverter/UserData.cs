using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[Serializable]
public class GameInfo
{
    public bool               hackSuspicion;
    public UserProfile        userInfo;
    public int                changeName;
    public LevelDataServer    level;
    public UserResourceServer userResource;
    public PowerupDataServer  powerup;

    public GameInfo()
    {
    }
}

[Serializable]
public class LevelDataServer
{
    public int level;
    public int numPlay;
    public int firstTryWins;
    public int totalLost;
    public int numberLostAfterSupportPack;

    public LevelDataServer()
    {
    }

    public LevelDataServer(ALevelData aLevelData)
    {
        Update(aLevelData);
    }

    public void Update(ALevelData aLevelData)
    {
        level                         = aLevelData.level;
        numPlay                       = aLevelData.numPlay;
        firstTryWins                  = aLevelData.firstTryWins;
        totalLost                     = aLevelData.totalLost;
        numberLostAfterSupportPack = aLevelData.numberLostAfterSupportPack;
    }
}

[Serializable]
public class PowerupDataServer
{
    public int[] powerups;
    public int[] powerupFrees;

    public void Update(PowerupData powerupData)
    {
        powerups = new int[powerupData.powerups.Length];
        for (int i = 0; i < powerups.Length; i++)
        {
            powerups[i] = powerupData.powerups[i];
        }

        powerupFrees = new int[powerupData.powerupFrees.Length];
        for (int i = 0; i < powerups.Length; i++)
        {
            powerupFrees[i] = powerupData.powerupFrees[i];
        }
    }
}


[Serializable]
public class UserResourceServer
{
    public int heart;
    public int gold;
    public int star;
    public bool removeAds;

    public long timeInfiHeart;
    public long timeX2Reward;

    public UserResourceServer()
    {
    }
}