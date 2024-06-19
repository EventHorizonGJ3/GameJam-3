using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
	public static ActionMap ActionMap;

	static InputManager()
	{
		ActionMap = new ActionMap();
		ActionMap.Enable();
	}

	public static Vector3 MovementInput => ActionMap.Player.Movement.ReadValue<Vector3>();

	public static bool IsMoving(out Vector3 direction)
	{
		direction = MovementInput;
		return direction != Vector3.zero;
	}

	public static void SwitchToUIInputs()
	{
		ActionMap.Player.Disable();
		ActionMap.UserInterface.Enable();
	}
	public static void SwitchPlayerInputs()
	{
		ActionMap.Player.Enable();
		ActionMap.UserInterface.Disable();
    }



}
