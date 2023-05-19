using IceMilkTea.Core;
using UnityEngine.InputSystem;
using UnityEngine;
public class PlayerCharacterController : AbstractCharacterController
{
	public ImtStateMachine<AbstractCharacterController> stateMachine;

	protected override void Awake()
	{
		base.Awake();
		stateMachine = new ImtStateMachine<AbstractCharacterController>(this);

		stateMachine.SetStartState<PlayerIdleState>();
		stateMachine.AddTransition<PlayerIdleState, PlayerWalkState>((int)State["walk"]);
		stateMachine.AddTransition<PlayerIdleState, PlayerJumpState>((int)State["jump"]);
		stateMachine.AddTransition<PlayerIdleState, PlayerFallState>((int)State["fall"]);

		stateMachine.AddTransition<PlayerWalkState, PlayerIdleState>((int)State["idle"]);

		stateMachine.AddTransition<PlayerJumpState, PlayerFallState>((int)State["fall"]);

		stateMachine.AddTransition<PlayerFallState, PlayerIdleState>((int)State["idle"]);
	}
	protected override void Start()
	{
		stateMachine.Update();
	}
	void Update()
	{
		Debug.Log(stateMachine.CurrentStateName);
		stateMachine.Update();
	}
	public void OnMove(InputAction.CallbackContext context)
	{
		move = context.ReadValue<Vector2>();
	}
	public void OnJump(InputAction.CallbackContext context)
	{
		jump = context.performed;
	}
	public class PlayerIdleState : IdleState
	{
		protected override void Update()
		{
			var playerContext = (PlayerCharacterController)Context;
			if (!Context.characterController.isGrounded)
				playerContext.stateMachine.SendEvent((int)Context.State["fall"]);
			if (!playerContext.CheckIdle())
				playerContext.stateMachine.SendEvent((int)Context.State["walk"]);
			if (Context.jump && Context.characterController.isGrounded)
				playerContext.stateMachine.SendEvent((int)Context.State["jump"]);
		}
	}
	public class PlayerWalkState : WalkState
	{
		protected override void Update()
		{
			var playerContext = (PlayerCharacterController)Context;
			if (playerContext.CheckIdle())
				playerContext.stateMachine.SendEvent((int)Context.State["idle"]);
			Context.characterController.Move(new Vector3(Context.move.x * Context.speed, 0, Context.move.y * Context.speed));
		}
	}
	public class PlayerJumpState : JumpState
	{
		protected override void Update()
		{
			Context.characterController.Move(new Vector3(0, 1 * Time.deltaTime, 0));
			if (Context.transform.position.y > Context.jumpingHeight)
			{
				var playerContext = (PlayerCharacterController)Context;
				playerContext.stateMachine.SendEvent((int)Context.State["fall"]);
			}
		}
	}
	public class PlayerFallState : FallState
	{
		protected override void Update()
		{
			Context.characterController.Move(new Vector3(0, -1 * Time.deltaTime, 0));
			if (Context.characterController.isGrounded)
			{
				var playerContext = (PlayerCharacterController)Context;
				playerContext.stateMachine.SendEvent((int)Context.State["idle"]);
			}
		}
	}
	bool CheckIdle() { return move.x == 0 && move.y == 0; }
}