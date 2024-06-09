using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponPicker : MonoBehaviour
{
    [SerializeField] Transform handTransfrom;
    [SerializeField] LayerMask weaponLayer;
    [SerializeField] PlayerComboM playerComboM;
    
    Collider[] allWeaponsColliders;
    IPickable pickableWeapon;

    Vector3 interactPos = Vector3.zero;
    [SerializeField] float interactionRadius = 2;
    [SerializeField] float interactionOffest;

    bool canInteract;

    private void OnEnable()
    {
        InputManager.ActionMap.Player.Interact.started += PickUpWeapon;
    }
    private void OnDisable()
    {
        InputManager.ActionMap.Player.Interact.started += PickUpWeapon;
    }


    private void Start()
    {
        interactPos = transform.position;
        var weaponList = WeaponsPooler.SharedInstance.GetWeaponList();
        allWeaponsColliders = new Collider[weaponList.Count];
        for (int i = 0; i < weaponList.Count; i++)
        {
            allWeaponsColliders[i] = weaponList[i].Transform.GetComponent<Collider>();
        }
    }
        
            
        

    private void Update()
    {
        if(InputManager.IsMoving(out Vector3 direction))
        {
            interactPos = transform.position + direction;
        }
        Collider[] colliderInRange = new Collider[allWeaponsColliders.Length];
        int numberOfWeapons = Physics.OverlapSphereNonAlloc(interactPos, interactionRadius, colliderInRange, weaponLayer);
        canInteract = false;
        

        for (int i = 0;i < numberOfWeapons;i++)
        {
            Collider weaponsCol = colliderInRange[i];
            if(Vector3.Distance(transform.position, weaponsCol.transform.position) <= interactionOffest)
            {
                canInteract = true;
                pickableWeapon = weaponsCol.GetComponent<IPickable>();
                break;
            }
        }
    }

    void PickUpWeapon(InputAction.CallbackContext context)
    {
        if (canInteract)
        {
            pickableWeapon.Transform.position = handTransfrom.position;
            pickableWeapon.Transform.parent = handTransfrom;
            playerComboM.UpdateCurrentWeapon(pickableWeapon.WeaponsSO);
        }
    }


    private void OnDrawGizmos()
    {
        if (canInteract)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        // Disegna la sfera di interazione
        Gizmos.DrawWireSphere(interactPos, interactionRadius );
    }


}
