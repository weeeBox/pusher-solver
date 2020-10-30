using System;

public readonly partial struct Level
{
    public static bool operator ==(Level l1, Level l2)
    {
        return l1.Equals(l2);
    }

    public static bool operator !=(Level l1, Level l2)
    {
        return !l1.Equals(l2);
    }
}