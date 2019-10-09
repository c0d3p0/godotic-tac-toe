using System;

using Godot;
using Godot.Collections;

public class SettingsGUI : Node
{
    public void OnGameLevelButtonPressed(int addition)
    {
        gameConfig.GameLevel = (byte) (gameConfig.GameLevel + addition);
        UpdateGUI();
    }

    public void OnMarkCheckBoxPressed(int pressedIndex, int otherIndex,
            int checkedValue, int uncheckedValue)
    {
        CheckBox cbp = markCheckBoxes[pressedIndex];
        markCheckBoxes[otherIndex].Pressed = !cbp.Pressed;
        gameConfig.PlayerMark = cbp.Pressed ? (sbyte) checkedValue :
                (sbyte) uncheckedValue;
    }

    public void OnWhoStartsCheckBoxPressed(int pressedIndex, int otherIndex,
            int checkedValue, int uncheckedValue)
    {
        CheckBox cbp = whoStartsCheckBoxes[pressedIndex];
        whoStartsCheckBoxes[otherIndex].Pressed = !cbp.Pressed;
        gameConfig.WhoStarts = cbp.Pressed ? (sbyte) checkedValue :
                (sbyte) uncheckedValue;
    }

    public void OnApplyButtonPressed()
    {
        gameSettings.SaveSettings(gameConfig);
    }

    public void OnRevertButtonPressed()
    {
        gameConfig.Copy(gameSettings.GameConfig);
        UpdateGUI();
    }

    public void OnDefaultButtonPressed()
    {
        gameConfig = new GameConfig();
        UpdateGUI();
    }

    public void UpdateGUI()
    {
        actualGameLevelLabel.Text = gameConfig.GetGameLevelString();
        bool isPlayerMarkX = gameConfig.PlayerMark == 0;
        bool shouldComputerStart = gameConfig.ShouldComputerStart();
        markCheckBoxes[0].Pressed = isPlayerMarkX;
        markCheckBoxes[1].Pressed = !isPlayerMarkX;
        whoStartsCheckBoxes[0].Pressed = !shouldComputerStart;
        whoStartsCheckBoxes[1].Pressed = shouldComputerStart;
    }

    private void InitializeCheckBoxArray(Array<CheckBox> checkBoxArray,
            NodePath checkBoxAreaNodePath)
    {
        Control checkBoxesArea = GetNodeOrNull<Control>(checkBoxAreaNodePath);
        Godot.Collections.Array children = checkBoxesArea.GetChildren();

        for(int i = 0; i < children.Count; i++)
            checkBoxArray.Add(children[i] as CheckBox);
    }

    private void InitializeAttributes()
    {
        actualGameLevelLabel = GetNodeOrNull<Label>(actualGameLevelLabelNodePath);
        gameSettings = GetNodeOrNull<GameSettings>("/root/GameSettings");
        gameConfig = new GameConfig();
        gameConfig.Copy(gameSettings.GameConfig);
        markCheckBoxes = new Array<CheckBox>();
        whoStartsCheckBoxes = new Array<CheckBox>();
        InitializeCheckBoxArray(markCheckBoxes, markCheckBoxesAreaNodePath);
        InitializeCheckBoxArray(whoStartsCheckBoxes, whoStartsCheckBoxesAreaNodePath);
        UpdateGUI();
    }

    public override void _EnterTree()
    {
        InitializeAttributes();
    }


    [Export]
    public NodePath actualGameLevelLabelNodePath;

    [Export]
    public NodePath markCheckBoxesAreaNodePath;

    [Export]
    public NodePath whoStartsCheckBoxesAreaNodePath;


    private Array<CheckBox> markCheckBoxes;
    private Array<CheckBox> whoStartsCheckBoxes;
    private Label actualGameLevelLabel;


    private GameSettings gameSettings;
    private GameConfig gameConfig;
}
