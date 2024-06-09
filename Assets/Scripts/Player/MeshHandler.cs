using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshHandler : MonoBehaviour
{
	[Header("Parameters")]
	[SerializeField] float rotationSpeed;
	private void FixedUpdate()
	{
		if (InputManager.IsMoving(out Vector3 direction))
		{
			Quaternion rotation = Quaternion.LookRotation(direction);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * rotationSpeed);
		}
	}
	
}
	

	

	


	
