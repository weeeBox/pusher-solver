using System;
using System.Linq;

public readonly partial struct Level
{
    public bool Equals(Level other)
    {
        return playerI == other.playerI && playerJ == other.playerJ && _cells.Cast<BlockType>().SequenceEqual(other._cells.Cast<BlockType>());
    }

    public override bool Equals(object obj)
    {
        return obj is Level other && Equals(other);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = playerI;
            hashCode = (hashCode * 397) ^ playerJ;
            hashCode = (hashCode * 397) ^ (_cells != null ? _cells.GetHashCode() : 0);
            return hashCode;
        }
    }
}