using IceMilkTea.Core;
using UnityEngine.InputSystem;
using UnityEngine;
public class PlayerCharacterController : AbstractCharacterController<PlayerCharacterController>
{
	protected override void Awake()
	{
		base.Awake();
		stateMachine = new ImtStateMachine<PlayerCharacterController>(this);

		stateMachine.SetStartState<PlayerIdleState>();
		stateMachine.AddTransition<PlayerIdleState, PlayerWalkState>((int)State["walk"]);
		stateMachine.AddTransition<PlayerIdleState, PlayerJumpState>((int)State["jump"]);
		stateMachine.AddTransition<PlayerIdleState, PlayerFallState>((int)State["fall"]);

		stateMachine.AddTransition<PlayerWalkState, PlayerIdleState>((int)State["idle"]);
		stateMachine.AddTransition<PlayerWalkState, PlayerJumpState>((int)State["jump"]);

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
		protected override void Enter()
		{
			Context.animator.SetTrigger("Idle");
		}
		protected override void Update()
		{
			if (!Context.characterController.isGrounded) Context.stateMachine.SendEvent((int)Context.State["fall"]);
			if (!Context.IsIdling()) Context.stateMachine.SendEvent((int)Context.State["walk"]);
			if (Context.IsJampable()) Context.stateMachine.SendEvent((int)Context.State["jump"]);
		}
	}
	public class PlayerWalkState : WalkState
	{
		protected override void Enter()
		{
			Context.animator.SetTrigger("Walk");
		}
		protected override void Update()
		{
			if (Context.IsIdling()) Context.stateMachine.SendEvent((int)Context.State["idle"]);
			if (Context.IsJampable()) Context.stateMachine.SendEvent((int)Context.State["jump"]);
			Context.characterController.Move(new Vector3(Context.move.x * Context.speed, Context.CalcGravity(), Context.move.y * Context.speed));
		}
	}
	public class PlayerJumpState : JumpState
	{
		protected override void Enter()
		{
			Context.animator.SetTrigger("Jump");
		}
		protected override void Update()
		{
			if (Context.transform.position.y > Context.jumpingHeight) Context.stateMachine.SendEvent((int)Context.State["fall"]);
			Context.characterController.Move(new Vector3(0, Context.jumpingSpeed * Time.deltaTime, 0));
		}
	}
	public class PlayerFallState : FallState
	{
		protected override void Enter()
		{
			Context.animator.SetTrigger("Fall");
		}
		protected override void Update()
		{
			if (Context.characterController.isGrounded) Context.stateMachine.SendEvent((int)Context.State["idle"]);
			Context.characterController.Move(new Vector3(0, Context.CalcGravity(), 0));
		}
		protected override void Exit()
		{
			Context.animator.SetTrigger("Idle");
		}
	}
	bool IsIdling() { return move.x == 0 && move.y == 0; }
	float CalcGravity() { return -(gravity * Time.deltaTime); }
	bool IsJampable() { return jump && characterController.isGrounded; }
}