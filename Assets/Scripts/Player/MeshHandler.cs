using System;
using UnityEngine;

public class MeshHandler : MonoBehaviour
{
	[Header("Parameters")]
	[SerializeField] float rotationSpeed;
	[SerializeField] float scaleMult = 1.3f;

	Vector3 startScale;

	private void Start()
	{
		startScale = transform.localScale;
	}

	private void FixedUpdate()
	{
		if (InputManager.IsMoving(out Vector3 direction))
		{
			Quaternion rotation = Quaternion.LookRotation(direction);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * rotationSpeed);
		}
	}

	private void OnEnable()
	{
		RageBar.OnBerserkExtraDmg += Embiggened;
	}
	private void OnDisable()
	{
		RageBar.OnBerserkExtraDmg -= Embiggened;
	}

	private void Embiggened(float obj)
	{
		if (obj == 0)
		{
			transform.localScale = startScale;
		}
		else
		{
			transform.localScale *= scaleMult;
		}
	}


}








