using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTypeComparer : IEqualityComparer<SoundType>
{
    public bool Equals(SoundType x, SoundType y)
    {
        return x == y;
    }

    public int GetHashCode(SoundType x)
    {
        return (int)x;
    }
}
