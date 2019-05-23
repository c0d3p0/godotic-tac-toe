using System;

using Godot;
using Godot.Collections;

public class TicTacToeGUI : Node
{
    private void ResetGameInfoGUI()
    {
        gameLevelLabel.SetText(gameConfig.GetGameLevelString());
        whoStartsLabel.SetText(gameConfig.GetWhoStartsString());
        playerMarkTextureRect.SetTexture(markTextureMap[gameConfig.PlayerMark]);
        computerMarkTextureRect.SetTexture(markTextureMap[gameConfig.ComputerMark]);
        messageLabel.SetText("");
    }

    private void ResetGameGUI()
    {
        for(int i = 0; i < blockButtonArray.Count; i++)
        {
            blockButtonArray[i].SetDisabled(false);
            blockButtonArray[i].SetDisabledTexture(null);
        }
    }

    private void RestartGame()
    {
        gameConfig.RestartGame = false;
        gameData.ResetAttributes();
        computerAI.UpdateAILevel();
        ResetGameInfoGUI();
        ResetGameGUI();

        if(gameConfig.ShouldComputerStart())
            ExecuteComputerTurn(new Block(-1, -1));
    }

    private void SetBlockAsPlayed(int index, sbyte mark)
    {
        if(index > -1 && index < blockButtonArray.Count &&
                markTextureMap.ContainsKey(mark))
        {
            blockButtonArray[index].SetDisabled(true);
            blockButtonArray[index].SetDisabledTexture(markTextureMap[mark]);
        }
    }

    private void OnBlockButtonPressed(int blockIndex, int rowIndex, int columnIndex)
    {
        if(!gameData.GameOver)
        {
            Block pb = new Block(Convert.ToSByte(rowIndex), Convert.ToSByte(columnIndex));
            ExecuteTurn(pb, 0, Convert.ToSByte(blockIndex), gameConfig.PlayerMark);
            ExecuteComputerTurn(pb);
        }
    }

    private void ExecuteComputerTurn(Block playerMoveBlock)
    {
        if(!gameData.GameOver)
        {
            Block computerMoveBlock = computerAI.GetComputerPlay(playerMoveBlock);
            sbyte buttonIndex = computerMoveBlock.ConvertToIndex();
            ExecuteTurn(computerMoveBlock, 1, buttonIndex, gameConfig.ComputerMark);
        }
    }

    private void ExecuteTurn(Block moveBlock, sbyte playerIndex,
            sbyte buttonIndex, sbyte mark)
    {
        if(!gameData.GameOver)
        {
            gameData.SetBlockValue(moveBlock, playerIndex);
            SetBlockAsPlayed(buttonIndex, mark);
        }
    }

    private void UpdateWinnerLine()
    {
        sbyte mark;
        Array<sbyte> winLineArray = gameData.WinLineArray;

        if(winLineArray.Count > 2)
        {
            mark = gameData.GetBlockValue(Block.CreateBlockFromIndex(winLineArray[0]));
            mark = (sbyte) (mark + 10);

            for(int i = 0; i < winLineArray.Count; i++)
                SetBlockAsPlayed(winLineArray[i], mark);
        }
    }

    private void UpdateWinnerMessage()
    {
        if(gameData.Winner == 0)
            messageLabel.SetText("You won!");
        else if(gameData.Winner == 1)
            messageLabel.SetText("Computer won!");
        else
            messageLabel.SetText("Draw!");
    }

    private void OnRestartButtonPressed()
    {
        RestartGame();
    }

    private void InitializeAttributes()
    {
        gameConfig = GetNodeOrNull<GameSettings>("/root/GameSettings").GameConfig;
        gameData = new GameData(gameConfig);
        computerAI = new ComputerAI(gameConfig, gameData);
        gameLevelLabel = GetNodeOrNull<Label>(gameLevelLabelNodePath);
        whoStartsLabel = GetNodeOrNull<Label>(whoStartsLabelNodePath);
        messageLabel = GetNodeOrNull<Label>(messageLabelNodePath);
        playerMarkTextureRect = GetNodeOrNull<TextureRect>(
                playerMarkTextureRectNodePath);
        computerMarkTextureRect = GetNodeOrNull<TextureRect>(
                computerMarkTextureRectNodePath);
    }

    private void InitializeBlockButtonArray()
    {
        blockButtonArray = new Array<TextureButton>();
        Control blockArea = GetNodeOrNull<Control>(blockAreaNodePath);
        Godot.Collections.Array blockChildrenArray = blockArea.GetChildren();

        for(int i = 0; i < blockChildrenArray.Count; i++)
            blockButtonArray.Add(blockChildrenArray[i] as TextureButton);
    }

    private void InitializeMarkTextureMap()
    {
        String x = "res://.import/x_mark.png-e643d637387af06c50cbf605e8bdebfc.stex"; 
        String o = "res://.import/o_mark.png-0c39b521901408f656fe0a7a8dc7e4da.stex";
        String wX = "res://.import/x_mark_winner.png-892386b39b0de5617626ad06886aec86.stex"; 
        String wO = "res://.import/o_mark_winner.png-a7623af8a85da4c568cf87af0b9841b2.stex";
        markTextureMap = new Dictionary<sbyte, Texture>();
        markTextureMap.Add(0, ResourceLoader.Load(x) as StreamTexture);
        markTextureMap.Add(1, ResourceLoader.Load(o) as StreamTexture);
        markTextureMap.Add(10, ResourceLoader.Load(wX) as StreamTexture);
        markTextureMap.Add(11, ResourceLoader.Load(wO) as StreamTexture);
    }

    public override void _Process(float delta)
    {
        if(gameConfig.RestartGame)
            RestartGame();
        
        if(gameData.GameOver)
        {
            UpdateWinnerMessage();
            UpdateWinnerLine();
        }
    }

    public override void _EnterTree()
    {
        InitializeAttributes();
        InitializeBlockButtonArray();
        InitializeMarkTextureMap();
        RestartGame();
    }


    [Export]
    public NodePath blockAreaNodePath;

    [Export]
    public NodePath gameLevelLabelNodePath;

    [Export]
    public NodePath whoStartsLabelNodePath;

    [Export]
    public NodePath playerMarkTextureRectNodePath;

    [Export]
    public NodePath computerMarkTextureRectNodePath;

    [Export]
    public NodePath messageLabelNodePath;


    private GameConfig gameConfig;
    private GameData gameData;
    private ComputerAI computerAI;

    private Label gameLevelLabel;
    private Label whoStartsLabel;
    private TextureRect playerMarkTextureRect;
    private TextureRect computerMarkTextureRect;
    private Label messageLabel;
    private Array<TextureButton> blockButtonArray;
    private Dictionary<sbyte, Texture> markTextureMap;
}
