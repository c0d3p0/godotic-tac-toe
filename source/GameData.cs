using System;

using Godot;
using Godot.Collections;

public class GameData
{
	private void CheckGame(Block block, sbyte playerIndex)
	{
        if(HasWon(new Block(block.row, 0), new Block(0, 1), playerIndex) ||
            HasWon(new Block(0, block.column), new Block(1, 0), playerIndex) ||
            HasWonInDiagonal(block, playerIndex))
        {
            winner = playerIndex;
            gameOver = true;
        }
        else if(moveAmount >= 9 && gameOver == false)
        {
            winner = 2;
            gameOver = true;
        }
	}

    private bool HasWonInDiagonal(Block block, sbyte playerIndex)
    {
        bool won = false;

        if(block.IsInDiagonalTopLeft())
            won |= HasWon(new Block(0, 0), new Block(1, 1), playerIndex);   

        if(block.IsInDiagonalTopRight())
            won |= HasWon(new Block(0, 2), new Block(1, -1), playerIndex);

        return won;
    }

    private bool HasWon(Block startBlock, Block incrementBlock, sbyte playerIndex)
    {
        winLineArray.Clear();
        sbyte playerMark = playerMarkArray[playerIndex];
        sbyte rowIndex;
        sbyte columnIndex;

        for(byte i = 0; i < 3; i++)
        {
            rowIndex = (sbyte) (startBlock.row + (i * incrementBlock.row));
            columnIndex = (sbyte) (startBlock.column + (i * incrementBlock.column));

            if(ticTacToeMatrix[rowIndex][columnIndex] == playerMark)
                winLineArray.Add(Block.ConvertToIndex(rowIndex, columnIndex));
        }

        if(winLineArray.Count > 2)
            return true;

        return false;
    }

	public void SetBlockValue(Block block, sbyte playerIndex)
	{
        if(!gameOver && playerIndex < 2 && IsBlockEmpty(block))
        {
            ticTacToeMatrix[block.row][block.column] = playerMarkArray[playerIndex];
            moveAmount++;
            CheckGame(block, playerIndex);
        }
	}

    public bool IsBlockEmpty(Block block)
    {
        if(block.IsValidBlock())
            return ticTacToeMatrix[block.row][block.column] < 0;
        
        return false;
    }

    public bool IsBlockValue(Block block, sbyte mark)
    {
        if(block.IsValidBlock())
            return ticTacToeMatrix[block.row][block.column] == mark;
        
        return false;
    }

	public void ResetAttributes()
	{
        winner = -1;
        gameOver = false;
        moveAmount = 0;
        ResetTicTacToeMatrix();
        ResetPlayerMarkArray();
        
        if(winLineArray == null)
            winLineArray = new Array<sbyte>();
        else
            winLineArray.Clear();
	}

    private void ResetTicTacToeMatrix()
    {
        if(ticTacToeMatrix == null)
            ticTacToeMatrix = new Array<Array<sbyte>>();

        for(byte i = 0; i < 3; i++)
        {
            if(ticTacToeMatrix.Count <= i)
                ticTacToeMatrix.Add(new Array<sbyte>());
            else if(ticTacToeMatrix[i] == null)
                ticTacToeMatrix[i] = new Array<sbyte>();
            else
                ticTacToeMatrix[i].Clear();

            for(byte j = 0; j < 3; j++)
                ticTacToeMatrix[i].Add(sbyte.MinValue);
        }
    }


    private void ResetPlayerMarkArray()
    {
        if(playerMarkArray == null)
            playerMarkArray = new Array<sbyte>();
        else
            playerMarkArray.Clear();

        playerMarkArray.Add(gameConfig.PlayerMark);
        playerMarkArray.Add(gameConfig.ComputerMark);
    }
    
    public GameData(GameConfig gameConfig)
    {
        this.gameConfig = gameConfig;
        ResetAttributes();
    }

    public sbyte GetBlockValue(Block block)
    {
        if(block.IsValidBlock())
            return ticTacToeMatrix[block.row][block.column];

        return sbyte.MinValue;
    }

    // TODO: Remove it
    public void PrintTicTacToe()
    {
        Array<sbyte> rowArray;
        String line;
        for(int i = 0; i < ticTacToeMatrix.Count; i++)
        {
            rowArray = ticTacToeMatrix[i];
            line = "";

            for(int j = 0; j < rowArray.Count; j++)
                line+= rowArray[j] + " * ";
            
            GD.Print(line);
        }
    }

    public GameConfig GameConfig
    {
        set
        {
            gameConfig = value;
        }
    }

    public byte MoveAmount
	{
        get
        {
		    return moveAmount;
        }
	}

	public Array<sbyte> WinLineArray
	{
        get
        {
		    return winLineArray;
        }
	}

	public sbyte Winner
	{
        get
        {
		    return winner;
        }
	}

    public bool GameOver
	{
        get
        {
		    return gameOver;
        }
	}


    private GameConfig gameConfig;
    private Array<sbyte> winLineArray;

    private Array<Array<sbyte>> ticTacToeMatrix;
	private Array<sbyte> playerMarkArray;
	private byte moveAmount;
	private sbyte winner;
	private bool gameOver;
}