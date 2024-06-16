using System;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
	public static Action<PlayerController> OnPlayerReady;

	[Header("Parameters")]
	[SerializeField] float movingSpeed;
	[Header("Refs")]
	[SerializeField] MeshHandler meshHandler;

	Rigidbody rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}


	private void Start()
	{
		OnPlayerReady?.Invoke(this);
		GameManager.enemyTargetPosition = transform;
	}


	private void FixedUpdate()
	{
		if (InputManager.IsMoving(out Vector3 direction))
		{
			rb.velocity = new Vector3(direction.x * movingSpeed, 0, direction.z * movingSpeed);
		}
		else rb.velocity = Vector3.zero;
	}

	// void SetAttackState(InputAction.CallbackContext context)
	// {
	//     GameManager.PlayerIsAttacking = true;
	//     meshHandler.StartCombo();
	//     StartCoroutine(SetAttackStateFalse());
	// }

	// IEnumerator SetAttackStateFalse()
	// {
	//     yield return null;
	//     GameManager.PlayerIsAttacking = false;
	// }

}
