using System;

public readonly partial struct Level
{
    public static Level FromPattern(string[] pattern)
    {
        int rows = pattern.Length;
        int cols = pattern[0].Length;

        var cells = new BlockType[rows, cols];
        int playerI = -1, playerJ = -1;
        for (int i = 0; i < rows; ++i)
        {
            for (int j = 0; j < cols; ++j)
            {
                char c = pattern[i][j];
                BlockType block;
                if (c == 'P')
                {
                    block = BlockType.Empty;
                    if (playerI == -1 && playerJ == -1)
                    {
                        playerI = i;
                        playerJ = j;
                    }
                    else
                    {
                        throw new ArgumentException("Player position already defined");
                    }
                }
                else
                {
                    block = GetBlockType(c);
                }

                cells[i, j] = block;
            }
        }

        return new Level(cells, playerI, playerJ);
    }

    private static BlockType GetBlockType(char code)
    {
        switch (code)
        {
            case '.': return BlockType.Empty;
            case 'W': return BlockType.Wall;
            case 'B': return BlockType.Block;
            case 'G': return BlockType.Goal;
        }

        throw new ArgumentException($"Unexpected code: {code}");
    }
}