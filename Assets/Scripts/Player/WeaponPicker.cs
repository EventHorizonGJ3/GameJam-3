
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponPicker : MonoBehaviour
{
	[SerializeField] Transform rightHand;
	[SerializeField] Transform leftHand;
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
			interactPos = transform.position + direction / 2;
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
				UIManager.OnHint?.Invoke(true);
			}
		}
		else UIManager.OnHint?.Invoke(false);

    }

	void PickUpWeapon(InputAction.CallbackContext context)
	{
		if (canInteract)
		{
			if (pickableWeapon.IsEnemyWeapon)
				return;

			if (currentWeapon != null && pickableWeapon.Transform.gameObject != currentWeapon)
			{
				currentWeapon.SetActive(false);
				currentWeapon.transform.parent = null;
			}
			AudioManager.instance.PlaySFX(AudioManager.instance.AudioData.SFX_WeaponPickUp, transform);
			currentWeapon = pickableWeapon.Transform.gameObject;

			pickableWeapon.Transform.position = rightHand.position; // mette l'arma in mano
			pickableWeapon.MyWeapon.Grabbed?.Invoke(leftHand);
			pickableWeapon.Transform.parent = rightHand;
			pickableWeapon.Transform.rotation = rightHand.rotation;

			weaponSpawner?.StartRespawn(); // triggera il respawn dell'arma
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
