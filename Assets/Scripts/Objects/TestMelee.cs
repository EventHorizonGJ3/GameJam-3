using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMelee : MeleeWeapon, IPickable
{

    [field: SerializeField] public WeaponsSO WeaponsSO { get; set; }

    public Transform Transform => transform;

    private void Awake()
    {
        
    }
    protected override void Start()
    {
        base.Start();
        durability *= 2;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out IBreakable breakable)) return;
        breakable?.Break();
        durability--;
        if(durability <= 0) gameObject.SetActive(false);
    }
}
