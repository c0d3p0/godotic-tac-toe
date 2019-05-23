using System;

using Godot;
using Godot.Collections;

public class GameConfig
{
    public byte GameLevel
    {
        get
        {
            return gameLevel;
        }
        set
        {
            if(value < 1)
                gameLevel = 1;
            else if(value > 4)
                gameLevel = 4;
            else
                gameLevel = value;
        }
    }
    public sbyte PlayerMark
    {
        get
        {
            return playerMark;
        }
        set
        {
            if(value != 0 && value != 1)
                playerMark = 0;
            else
                playerMark = value;
        }
    }

    public sbyte ComputerMark
    {
        get
        {
            if(playerMark == 0)
                return 1;
            else
                return 0;
        }
    }

    public bool RestartGame
    {
        get
        {
            return restartGame;
        }
        set
        {
            restartGame = value;
        }
    }

    public sbyte WhoStarts
    {
        get
        {
            return whoStarts;
        }
        set
        {
            if(whoStarts > 1)
                whoStarts = 1;
            else if(whoStarts < 0)
                whoStarts = 0;
            else
                whoStarts = value;
        }
    }

    public String GetGameLevelString()
    {
        return levelNameMap[gameLevel];
    }

    public String GetWhoStartsString()
    {
        if(whoStarts == 0)
            return "Player";
        else
            return "Computer";
    }

    public bool ShouldComputerStart()
    {
        return whoStarts == 1;
    }

    public void Copy(GameConfig gameConfig)
    {
        this.playerMark = gameConfig.PlayerMark;
        this.gameLevel = gameConfig.GameLevel;
        this.whoStarts = gameConfig.whoStarts;
    }

    private void CreateLevelNameMap()
    {
        levelNameMap = new Dictionary<byte, String>();
        levelNameMap.Add(1, "Easy");
        levelNameMap.Add(2, "Normal");
        levelNameMap.Add(3, "Hard");
        levelNameMap.Add(4, "Very Hard");
    }

    public GameConfig()
    {
        gameLevel = 2;
        playerMark = 0;
        restartGame = false;
        whoStarts = 0;
        CreateLevelNameMap();
    }


    private byte gameLevel;
    private sbyte playerMark;
    private sbyte whoStarts;

    private bool restartGame;
    private Dictionary<byte, String> levelNameMap;
}

