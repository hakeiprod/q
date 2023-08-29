using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerManager))]
public class PlayerInputAction : MonoBehaviour
{
	public PlayerManager playerManager;
	[NonSerialized] public Vector2 move;
	[NonSerialized] public bool jump;
	[NonSerialized] public bool run;
	[NonSerialized] public bool ability0 = false;
	[NonSerialized] public bool ability1 = false;
	[NonSerialized] public bool ability2 = false;

	public void OnMove(InputAction.CallbackContext context)
	{
		move = context.ReadValue<Vector2>();
	}
	public void OnJump(InputAction.CallbackContext context)
	{
		jump = context.performed;
	}
	public void OnRun(InputAction.CallbackContext context)
	{
		run = context.performed;
	}
	public void OnSelectSlot(InputAction.CallbackContext context)
	{
		var index = (int)context.ReadValue<float>();
		if (index > 0) playerManager.ChangeActivePlayer(index - 1);
	}
	public void OnFirstSkill(InputAction.CallbackContext context)
	{
		ability0 = context.performed;
	}
	public void OnSecondSkill(InputAction.CallbackContext context)
	{
		ability1 = context.performed;
	}
	public void OnThirdSkill(InputAction.CallbackContext context)
	{
		ability2 = context.performed;
	}

}
