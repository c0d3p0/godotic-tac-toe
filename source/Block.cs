using System;

public struct Block
{
    public Block(sbyte row, sbyte column)
    {
        this.row = row;
        this.column = column;
    }

    public void Set(sbyte row, sbyte column)
    {
        this.row = row;
        this.column = column;
    }

    public bool IsInDiagonalTopLeft()
    {
        return row == column;
    }

    public bool IsInDiagonalTopRight()
    {
        return Convert.ToByte(Math.Abs(row - column)) == 2 || 
                (row == 1 && column == 1);
    }

    public bool IsInCenter()
    {
        return row == 1 && column == 1;
    }

    public bool IsValidBlock()
    {
        return row > -1 && row < 3 && column > -1 && column < 3;
    }
    
    public static bool IsValidBlock(sbyte row, sbyte column)
    {
        return row > -1 && row < 3 && column > -1 && column < 3;
    }

    public sbyte ConvertToIndex()
	{
		return ConvertToIndex(row, column);
	}

    public static sbyte ConvertToIndex(sbyte row, sbyte column)
    {
        if(IsValidBlock(row, column))
            return (sbyte) ((3 * row) + column);

        return sbyte.MinValue;
    }

    public static Block CreateBlockFromIndex(sbyte index)
    {
        sbyte row = (sbyte) (index / 3);
        sbyte column = (sbyte) (index % 3);
        return new Block(row, column);
    }

    public sbyte row;
    public sbyte column;
}
