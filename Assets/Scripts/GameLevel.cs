using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameLevelPrefabs
{
    [SerializeField] public Transform player;

    [SerializeField] public Transform block;

    [SerializeField] public Transform wall;

    [SerializeField] public Transform goal;
}

public class GameLevel : MonoBehaviour
{
    [SerializeField] private GameLevelPrefabs _prefabs;

    private Level _initialLevel;
    private Level _level;
    private Transform _player;

    private List<Transform> _walls;
    private List<Transform> _blocks;
    private List<Transform> _goals;

    private void Awake()
    {
        var pattern = new[]
        {
            "..WWW......",
            ".B...BBB.W.",
            ".GWWW...W..",
            "W....W.W.W.",
            "...G..B..G.",
            "W.W..W.W..W",
            "....W.G.W..",
            "P.W......W.",
        };
        _walls = new List<Transform>();
        _blocks = new List<Transform>();
        _goals = new List<Transform>();
        _player = Instantiate(_prefabs.player);

        _initialLevel = Level.FromPattern(pattern);
        ResetLevel();
    }

    public bool MoveUp()
    {
        return Move(-1, 0);
    }

    public bool MoveDown()
    {
        return Move(1, 0);
    }

    public bool MoveLeft()
    {
        return Move(0, -1);
    }

    public bool MoveRight()
    {
        return Move(0, 1);
    }

    public void ResetLevel()
    {
        level = _initialLevel;
    }

    private bool Move(int di, int dj)
    {
        var oldLevel = level;
        level = level.Move(di, dj);
        return oldLevel != level;
    }

    public BlockType this[int i, int j] => _level[i, j];

    public int playerI => _level.playerI;
    public int playerJ => _level.playerJ;

    public int rows => _level.rows;
    public int cols => _level.cols;

    private Level level
    {
        get => _level;
        set
        {
            Transform NextBlock(IList<Transform> list, int index, Transform prefab)
            {
                if (list.Count == index)
                {
                    list.Add(Instantiate(prefab));
                }

                return list[index];
            }

            _level = value;
            _player.localPosition = new Vector3(value.playerJ, 0, -value.playerI);

            int wallIndex = 0;
            int blockIndex = 0;
            int goalIndex = 0;
            for (int i = 0; i < _level.rows; ++i)
            {
                for (int j = 0; j < _level.cols; ++j)
                {
                    Transform block = null;
                    BlockType blockType = _level[i, j];
                    switch (blockType)
                    {
                        case BlockType.Empty:
                            break;
                        case BlockType.Wall:
                            block = NextBlock(_walls, wallIndex++, _prefabs.wall);
                            break;
                        case BlockType.Block:
                        case BlockType.BlockFixed:
                            block = NextBlock(_blocks, blockIndex++, _prefabs.block);
                            break;
                        case BlockType.Goal:
                            block = NextBlock(_goals, goalIndex++, _prefabs.goal);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (block != null)
                    {
                        block.localPosition = new Vector3(j, 0, -i);
                    }
                }
            }
        }
    }
}