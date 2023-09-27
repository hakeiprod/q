using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerManager))]
public class PlayerInputAction : MonoBehaviour
{
	public PlayerManager playerManager;
	[NonSerialized] public Vector2 move = new();
	[NonSerialized] public bool jump = false;
	[NonSerialized] public bool run = false;
	[NonSerialized] public bool fire = false;
	[NonSerialized] public bool ability0 = false;
	[NonSerialized] public bool ability1 = false;
	[NonSerialized] public bool ability2 = false;

	public void OnFire(InputAction.CallbackContext context)
	{
		fire = context.performed;
	}
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
