using System;

using Godot;
using Godot.Collections;


public class GameSettings : Node
{
    private void SaveConfigurationFile()
    {
        Dictionary jsonMap = new Dictionary();
        jsonMap.Add("gameLevel", gameConfig.GameLevel);
        jsonMap.Add("playerMark", gameConfig.PlayerMark);
        jsonMap.Add("WhoStarts", gameConfig.WhoStarts);
        JSONUtil.WriteToJSONFile<Dictionary>(jsonMap, CONFIG_FILE_PATH);
    }

    private void LoadConfigurationFile()
    {
        Dictionary jsonMap = JSONUtil.ParseFileAs<Dictionary>(CONFIG_FILE_PATH);

        if(jsonMap != null)
        {
            try
            {
                if(jsonMap.Contains("gameLevel"))
                    gameConfig.GameLevel = Convert.ToByte(jsonMap["gameLevel"]);
            
                if(jsonMap.Contains("playerMark"))
                    gameConfig.PlayerMark = Convert.ToSByte(jsonMap["playerMark"]);

                if(jsonMap.Contains("WhoStarts"))
                    gameConfig.WhoStarts = Convert.ToSByte(jsonMap["WhoStarts"]);

                return;
            }
            catch
            {
                gameConfig = new GameConfig();
                GD.Print("Problems loading configuration file");
            }
        }
        
        
        CreateDefaultConfigurationFile();
    }

    private void CreateDefaultConfigurationFile()
    {
        FileUtil.CreateFile(CONFIG_FILE_PATH);
        SaveConfigurationFile();
    }

    public void SaveSettings(GameConfig gameConfig)
    {
        this.gameConfig.Copy(gameConfig);
        this.gameConfig.RestartGame = true;
        SaveConfigurationFile();
    }

    private void LoadSettings()
    {
        if(FileUtil.Exists(CONFIG_FILE_PATH))
            LoadConfigurationFile();
        else
            CreateDefaultConfigurationFile();
    }

    public void InitializeGameData()
    {
        if(gameConfig == null)
            gameConfig = new GameConfig();
    }

    public override void _EnterTree()
    {
        InitializeGameData();
        LoadSettings();
    }

    public GameConfig GameConfig
    {
        get
        {
            return gameConfig;
        }
    }
    

    private GameConfig gameConfig;
    private const String CONFIG_FILE_PATH = "res://config.cfg";
}
