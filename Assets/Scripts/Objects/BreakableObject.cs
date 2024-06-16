using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour, IBreakable
{
    [SerializeField] int hp = 2;
    bool isBroken;

    public bool IsBroken { get => isBroken; set => isBroken = value; }

    public void Break()
    {
        if (IsBroken) return;
        hp--;
        if (hp <= 0)
        {
            isBroken = true;
            gameObject.SetActive(false);
        }
    }
}
