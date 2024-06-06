using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMelee : MeleeWeapon
{

    Collider meleeCollider;

    private void Awake()
    {
        meleeCollider = GetComponent<Collider>();
    }
    protected override void Start()
    {
        base.Start();
        durability *= 2;
        meleeCollider.isTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IBreakable breakable)) return;
        breakable?.Break();
        durability--;
        if(durability <= 0) gameObject.SetActive(false);
    }
}
