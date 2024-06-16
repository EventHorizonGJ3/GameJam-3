using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager : MonoBehaviour
{
    public static bool PlayerIsAttacking;
    public static bool IsHoldingMelee;
    public static bool IsHoldingRanged;
    public static Transform enemyTargetPosition;

    private void Awake()
    {
        Application.targetFrameRate = 100;
    }


}
