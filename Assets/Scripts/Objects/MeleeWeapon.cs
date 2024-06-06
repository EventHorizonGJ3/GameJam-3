using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    protected int durability;
    protected int damage;

    protected virtual void Start()
    {
        durability = 1;
        damage = 1;
    }

}
