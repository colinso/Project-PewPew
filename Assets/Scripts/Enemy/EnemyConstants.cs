using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConstants
{
    public enum EnemyTypes { Baddie, DistanceBaddie,
        Boss
    }

    static public int TypeToDistance(EnemyTypes type)
    {
        switch (type)
        {
            case EnemyTypes.Baddie : return 1;
            case EnemyTypes.DistanceBaddie: return 5;
            default: return 1;
        }
    }
}
