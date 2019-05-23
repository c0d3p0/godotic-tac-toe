using System;

using Godot;
using Godot.Collections;


public class ComputerAI
{	
	public Block GetComputerPlay(Block opponentMoveBlock)
	{
		desiredMoveArray = null;
		FillPossibleMoveArray();

		if(MustDoPlannedMove())
		{
			if(gameData.MoveAmount > 1)
            	StoreAdvancedMoves(opponentMoveBlock);
			else
				StoreBasicMoves(opponentMoveBlock);
		}

		if(desiredMoveArray == null || desiredMoveArray.Count < 1)
		{
			if(possibleMoveArray.Count < 1)
				possibleMoveArray.Add(4);

			desiredMoveArray = possibleMoveArray;
		}

		int rng = GetRNG(0, desiredMoveArray.Count - 1);
		lastMove = gameBlocks[desiredMoveArray[rng]];
		return lastMove;
	}

    private void PlanAdvancedMove(Block moveBlock, sbyte startBlockIndex,
            sbyte incrementBlockIndex, sbyte mark)
    {
		sbyte ri;
        sbyte ci;
		sbyte index0;
		auxBlockArray.Clear();

        for(sbyte i = 0; i < 3; i++)
        {
            ri = gameBlocks[startBlockIndex].row;
			ri += (sbyte) (i * incrementBlocks[incrementBlockIndex].row);
            ci = gameBlocks[startBlockIndex].column;
			ci += (sbyte) (i * incrementBlocks[incrementBlockIndex].column);
			index0 = Block.ConvertToIndex(ri, ci);

			if(!gameBlocks[index0].Equals(moveBlock))
				auxBlockArray.Add(gameBlocks[index0].ConvertToIndex());
		}

		index0 = auxBlockArray[0];
		sbyte index1 = auxBlockArray[1];

		if(gameData.IsBlockValue(gameBlocks[index0], mark) &&
				gameData.IsBlockEmpty(gameBlocks[index1]))
		{
			goodMoveArray.Add(index1);
		}
		else if(gameData.IsBlockValue(gameBlocks[index1], mark) &&
				gameData.IsBlockEmpty(gameBlocks[index0]))
		{
			goodMoveArray.Add(auxBlockArray[0]);
		}
		else
		{
			if(gameData.IsBlockEmpty(gameBlocks[index0]) &&
					!normalMoveArray.Contains(index0))
			{
				normalMoveArray.Add(index0);
			}
			
			if(gameData.IsBlockEmpty(gameBlocks[index1]) &&
					!normalMoveArray.Contains(index1))
			{
				normalMoveArray.Add(index1);
			}
		}
    }

	private void PlanAllAdvancedMove(Block move, sbyte mark)
	{
		PlanAdvancedMove(move, Block.ConvertToIndex(move.row, 0), 0, mark); // Row
		PlanAdvancedMove(move, Block.ConvertToIndex(0, move.column), 1, mark); // Column

		if(move.IsInDiagonalTopLeft())
			PlanAdvancedMove(move, 0, 2, mark);
		
		if(move.IsInDiagonalTopRight())
			PlanAdvancedMove(move, 2, 3, mark);
	}

    private void StoreAdvancedMoves(Block moveBlock)
    {	
		goodMoveArray.Clear();
        normalMoveArray.Clear();
		PlanAllAdvancedMove(lastMove, gameConfig.ComputerMark);  // It plans all offensive moves.

        if(goodMoveArray.Count < 1) // It plans all defensive moves.
			PlanAllAdvancedMove(moveBlock, gameConfig.PlayerMark);

        if(goodMoveArray.Count > 0)
            desiredMoveArray = goodMoveArray; // It turns good moves in desired moves.
		else
        	desiredMoveArray = normalMoveArray; // It turns normal moves in desired moves.
    }

	private void StoreBasicMoves(Block opponentMoveBlock)
	{
		normalMoveArray.Clear();

		if(!opponentMoveBlock.IsInCenter()) // Add move in center block several times
		{
			sbyte index = Block.ConvertToIndex(1, 1);

			for(int i = 0; i < aiLevel; i++)
				normalMoveArray.Add(index);
		}

		normalMoveArray.Add(Block.ConvertToIndex(0, 0)); // Add all diagonal moves
		normalMoveArray.Add(Block.ConvertToIndex(0, 2));
		normalMoveArray.Add(Block.ConvertToIndex(2, 0));
		normalMoveArray.Add(Block.ConvertToIndex(2, 2));
		desiredMoveArray = normalMoveArray;
	}

    private void FillPossibleMoveArray()
    {
		sbyte index;
        possibleMoveArray.Clear();

        for(sbyte i = 0; i < 3; i++)
        {
            for(sbyte j = 0; j < 3; j++)
            {
				index = Block.ConvertToIndex(i, j);

                if(gameData.IsBlockEmpty(gameBlocks[index]))
                    possibleMoveArray.Add(index);
            }
        }
    }

	private int GetRNG(int min, int max)
	{
		random.Randomize();
		return random.RandiRange(min, max);
	}

    private bool MustDoPlannedMove()
    {
        int max = possibleMoveArray.Count * aiLevel;
        return GetRNG(1, max) >= possibleMoveArray.Count;
    }

	private Block GetBlock(sbyte row, sbyte column)
	{
		return gameBlocks[Block.ConvertToIndex(row, column)];
	}

    public void UpdateAILevel()
    {
        aiLevel = Convert.ToByte(Math.Pow(3, gameConfig.GameLevel));
    }
    
    private void InitializeAttributes(GameConfig gameConfig, GameData gameData)
    {
        goodMoveArray = new Array<sbyte>();
		normalMoveArray = new Array<sbyte>();
		possibleMoveArray = new Array<sbyte>();
		auxBlockArray = new Array<sbyte>();
        random = new RandomNumberGenerator();
        this.gameData = gameData;
        this.gameConfig = gameConfig;
        lastMove = new Block(-1, -1);
		incrementBlocks = new Block[]{new Block(0, 1), new Block(1, 0),
				new Block(1, 1), new Block(1, -1)};
		InitializeAllBlocks();
        UpdateAILevel();
    }

	private void InitializeAllBlocks()
	{
		gameBlocks = new Block[9];
		byte index = 0;

		for(sbyte i = 0; i < 3; i++)
		{
			for(sbyte j = 0; j < 3; j++)
				gameBlocks[index++] = new Block(i, j);
		}
	}
	

    public ComputerAI(GameConfig gameConfig, GameData gameData)
    {
        InitializeAttributes(gameConfig, gameData);
    }


    private GameData gameData;
    private GameConfig gameConfig;
    private byte aiLevel;

	private Block lastMove;
	private Array<sbyte> goodMoveArray;
	private Array<sbyte> normalMoveArray;
	private Array<sbyte> possibleMoveArray;
    private Array<sbyte> desiredMoveArray;
	private Array<sbyte> auxBlockArray;
	private Block[] gameBlocks;
	private Block[] incrementBlocks;
	private RandomNumberGenerator random;
}