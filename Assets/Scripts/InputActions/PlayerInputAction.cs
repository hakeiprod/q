using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerManager))]
public class PlayerInputAction : MonoBehaviour
{
	public PlayerManager playerManager;
	[System.NonSerialized] public Vector2 move;
	[System.NonSerialized] public bool jump;
	[System.NonSerialized] public bool run;
	[System.NonSerialized] public bool firstSkill;
	[System.NonSerialized] public bool secondSkill;
	[System.NonSerialized] public bool thirdSkill;

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
		firstSkill = context.performed;
	}
	public void OnSecondSkill(InputAction.CallbackContext context)
	{
		secondSkill = context.performed;
	}
	public void OnThirdSkill(InputAction.CallbackContext context)
	{
		thirdSkill = context.performed;
	}

}
