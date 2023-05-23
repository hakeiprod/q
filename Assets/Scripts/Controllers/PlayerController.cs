using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerManager))]
public class PlayerController : MonoBehaviour
{
	public PlayerManager playerManager;

	public void OnMove(InputAction.CallbackContext context)
	{
		playerManager.GetActivePlayerInstance().playerCharacterController.move = context.ReadValue<Vector2>();
	}
	public void OnJump(InputAction.CallbackContext context)
	{
		playerManager.GetActivePlayerInstance().playerCharacterController.jump = context.performed;
	}
	public void OnRun(InputAction.CallbackContext context)
	{
		playerManager.GetActivePlayerInstance().playerCharacterController.run = context.performed;
	}
	public void OnSelectSlot(InputAction.CallbackContext context)
	{
		var index = (int)context.ReadValue<float>();
		if (index > 0)
			playerManager.ChangeActivePlayer((int)context.ReadValue<float>() - 1);
	}

}