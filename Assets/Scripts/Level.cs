using System;

public enum BlockType
{
    Empty,
    Wall,
    Block,
    BlockFixed,
    Goal
}

public readonly partial struct Level
{
    public readonly int playerI;
    public readonly int playerJ;

    private readonly BlockType[,] _cells;

    public Level(BlockType[,] cells, int playerI, int playerJ)
    {
        _cells = cells ?? throw new ArgumentNullException(nameof(cells));
        if (playerI < 0 || playerI >= _cells.GetLength(0))
        {
            throw new ArgumentOutOfRangeException($"playerI: {playerI}");
        }
        if (playerJ < 0 || playerJ >= _cells.GetLength(1))
        {
            throw new ArgumentOutOfRangeException($"playerJ: {playerJ}");
        }
        this.playerI = playerI;
        this.playerJ = playerJ;
    }

    public Level Move(int dr, int dc)
    {
        int new_r = playerI + dr;
        int new_c = playerJ + dc;
        if (!GetBlock(new_r, new_c, out BlockType block))
        {
            return this;
        }

        if (block == BlockType.Empty || block == BlockType.Goal)
        {
            return new Level(_cells, new_r, new_c);
        }

        if (block == BlockType.Wall || block == BlockType.BlockFixed)
        {
            return this;
        }

        if (block == BlockType.Block)
        {
            int block_r = new_r + dr;
            int block_c = new_c + dc;
            if (GetBlock(block_r, block_c, out BlockType cell))
            {
                if (cell == BlockType.Empty || cell == BlockType.Goal)
                {
                    BlockType[,] newCells = (BlockType[,]) _cells.Clone();
                    newCells[new_r, new_c] = BlockType.Empty;
                    newCells[block_r, block_c] = cell == BlockType.Empty ? BlockType.Block : BlockType.BlockFixed;
                    return new Level(newCells, new_r, new_c);
                }
            }
        }

        return this;
    }

    private bool GetBlock(int i, int j, out BlockType block)
    {
        if (i >= 0 && i < rows && j >= 0 && j < cols)
        {
            block = this[i, j];
            return true;
        }

        block = default;
        return false;
    }

    public BlockType this[int i, int j] => _cells[i, j];

    public int rows => _cells.GetLength(0);

    public int cols => _cells.GetLength(1);
}