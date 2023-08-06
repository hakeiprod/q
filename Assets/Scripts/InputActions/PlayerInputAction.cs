using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerManager))]
public class PlayerInputAction : MonoBehaviour
{
	public PlayerManager playerManager;

	public void OnMove(InputAction.CallbackContext context)
	{
		playerManager.GetActivePlayerInstance().move = context.ReadValue<Vector2>();
	}
	public void OnJump(InputAction.CallbackContext context)
	{
		playerManager.GetActivePlayerInstance().jump = context.performed;
	}
	public void OnRun(InputAction.CallbackContext context)
	{
		playerManager.GetActivePlayerInstance().run = context.performed;
	}
	public void OnSelectSlot(InputAction.CallbackContext context)
	{
		var index = (int)context.ReadValue<float>();
		if (index > 0) playerManager.ChangeActivePlayer(index - 1);
	}
	public void OnFirstSkill(InputAction.CallbackContext context)
	{
		playerManager.GetActivePlayerInstance().firstSkill = context.performed;
	}
	public void OnSecondSkill(InputAction.CallbackContext context)
	{
		playerManager.GetActivePlayerInstance().secondSkill = context.performed;
	}
	public void OnThirdSkill(InputAction.CallbackContext context)
	{
		playerManager.GetActivePlayerInstance().thirdSkill = context.performed;
	}

}
