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

		stateMachine.AddTransition<PlayerFallState, PlayerLandState>((int)State["land"]);

		stateMachine.AddTransition<PlayerLandState, PlayerIdleState>((int)State["idle"]);
		stateMachine.AddTransition<PlayerLandState, PlayerWalkState>((int)State["walk"]);
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
			var forwardDirection = Context.characterController.velocity.normalized;
			if (!Context.IsIdling())
			{
				float targetAngle = Mathf.Atan2(forwardDirection.x, forwardDirection.z) * Mathf.Rad2Deg;
				Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
				Context.transform.rotation = Quaternion.Lerp(Context.transform.rotation, targetRotation, 10 * Time.deltaTime);
			}
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
			if (Context.characterController.isGrounded) Context.stateMachine.SendEvent((int)Context.State["land"]);
			Context.characterController.Move(new Vector3(0, Context.CalcGravity(), 0));
		}
	}
	public class PlayerLandState : LandState
	{
		protected override void Enter()
		{
			Context.animator.SetTrigger("Land");
		}
		protected override void Update()
		{
			// TODO: Landアニメーションが終了した場合、Idleに遷移する
			if (!Context.IsIdling()) Context.stateMachine.SendEvent((int)Context.State["walk"]);
		}
	}
	bool IsIdling() { return move == Vector2.zero; }
	bool IsJampable() { return jump && characterController.isGrounded; }
	float CalcGravity() { return -(gravity * Time.deltaTime); }
}