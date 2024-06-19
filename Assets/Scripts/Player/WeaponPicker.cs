
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponPicker : MonoBehaviour
{
    [SerializeField] Transform handTransfrom;
    [SerializeField] LayerMask weaponLayer;
    [SerializeField] PlayerComboM playerComboM;

    Collider[] allWeaponsColliders;
    IPickable pickableWeapon;
    IWeaponSpawner weaponSpawner;
    GameObject currentWeapon;

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
        InputManager.ActionMap.Player.Interact.started -= PickUpWeapon;
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
        if (InputManager.IsMoving(out Vector3 direction))
        {
            interactPos = transform.position + direction/2;
        }
        Collider[] colliderInRange = new Collider[allWeaponsColliders.Length];
        int numberOfWeapons = Physics.OverlapSphereNonAlloc(interactPos, interactionRadius, colliderInRange, weaponLayer);
        canInteract = false;



        
        if (numberOfWeapons > 0)
        {
            if (Vector3.Distance(transform.position, colliderInRange[0].transform.position) <= interactionOffest)
            {
                canInteract = true;
                weaponSpawner = colliderInRange[0].GetComponentInParent<IWeaponSpawner>();
                pickableWeapon = colliderInRange[0].GetComponent<IPickable>();
                
            }
        }




    }

    void PickUpWeapon(InputAction.CallbackContext context)
    {
        if (canInteract)
        {
            pickableWeapon.Transform.position = handTransfrom.position; // mette l'arma in mano
            pickableWeapon.Transform.parent = handTransfrom;

            if (currentWeapon != null) currentWeapon.SetActive(false); currentWeapon.transform.parent = null;
            currentWeapon = pickableWeapon.Transform.gameObject;

            weaponSpawner.StartRespawn(); // triggera il respawn dell'arma
            playerComboM.UpdateCurrentWeapon(pickableWeapon.MyWeapon);
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
        Gizmos.DrawWireSphere(interactPos, interactionRadius);
    }


}
