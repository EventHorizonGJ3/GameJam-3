using UnityEngine;
using UnityEngine.InputSystem;
public class ShittiWeaponPicker : MonoBehaviour
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

	}

	void PickUpWeapon(InputAction.CallbackContext context)
	{
		InputManager.IsMoving(out Vector3 direction);
		interactPos = transform.position + direction;
		Collider[] colliderInRange = new Collider[allWeaponsColliders.Length];
		if (colliderInRange.Length < 0)
			colliderInRange = new Collider[3];
		int numberOfWeapons = Physics.OverlapSphereNonAlloc(interactPos, interactionRadius, colliderInRange, weaponLayer);
		canInteract = false;

		for (int i = 0; i < numberOfWeapons; i++)
		{
			Collider weaponsCol = colliderInRange[i];
			if (Vector3.Distance(transform.position, weaponsCol.transform.position) <= interactionOffest)
			{
				pickableWeapon = weaponsCol.GetComponent<IPickable>();
				pickableWeapon.Transform.position = handTransfrom.position;
				pickableWeapon.Transform.parent = handTransfrom;
				playerComboM.UpdateCurrentWeapon(pickableWeapon.WeaponSo);
				break;
			}
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
