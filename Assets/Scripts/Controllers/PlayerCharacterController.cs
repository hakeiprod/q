using IceMilkTea.Core;
using UnityEngine.InputSystem;
using UnityEngine;
using UniRx;

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
		base.Start();
		stateMachine.Update();
	}
	void Update()
	{
		stateMachine.Update();
		Debug.Log(stateMachine.CurrentStateName + " : " + animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
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
			Context.animator.SetBool("Idle", true);
		}
		protected override void Update()
		{
			if (!Context.characterController.isGrounded) Context.stateMachine.SendEvent((int)Context.State["fall"]);
			if (!Context.IsIdling()) Context.stateMachine.SendEvent((int)Context.State["walk"]);
			if (Context.IsJampable()) Context.stateMachine.SendEvent((int)Context.State["jump"]);
		}
		protected override void Exit()
		{
			Context.animator.SetBool("Idle", false);
		}
	}
	public class PlayerWalkState : WalkState
	{
		protected override void Enter()
		{
			Context.animator.SetBool("Walk", true);
		}
		protected override void Update()
		{
			if (Context.IsJampable()) Context.stateMachine.SendEvent((int)Context.State["jump"]);
			if (Context.IsIdling()) Context.stateMachine.SendEvent((int)Context.State["idle"]);
			else Context.transform.rotation = Context.CalcRotation();
			Context.characterController.Move(new Vector3(Context.move.x * Context.speed, Context.CalcGravity(), Context.move.y * Context.speed));
		}
		protected override void Exit()
		{
			Context.animator.SetBool("Walk", false);
		}
	}
	public class PlayerJumpState : JumpState
	{
		protected override void Enter()
		{
			Context.animator.SetBool("Jump", true);
		}
		protected override void Update()
		{
			if (Context.transform.position.y > Context.jumpingHeight) Context.stateMachine.SendEvent((int)Context.State["fall"]);
			Context.characterController.Move(new Vector3(0, Context.jumpingSpeed * Time.deltaTime, 0));
		}
		protected override void Exit()
		{
			Context.animator.SetBool("Jump", false);
		}
	}
	public class PlayerFallState : FallState
	{
		protected override void Enter()
		{
			Context.animator.SetBool("Fall", true);
		}
		protected override void Update()
		{
			if (Context.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "InAir") return;
			if (Context.characterController.isGrounded) Context.stateMachine.SendEvent((int)Context.State["land"]);
			Context.characterController.Move(new Vector3(0, Context.CalcGravity(), 0));
		}
		protected override void Exit()
		{
			Context.animator.SetBool("Fall", false);
		}
	}
	public class PlayerLandState : LandState
	{
		protected override void Enter()
		{
			Context.animator.SetBool("Land", true);
		}
		protected override void Update()
		{
			if (Context.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "JumpLand") return;
			if (Context.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1) Context.stateMachine.SendEvent((int)Context.State["idle"]);
		}
		protected override void Exit()
		{
			Context.animator.SetBool("Land", false);
		}
	}
	bool IsIdling() { return move == Vector2.zero; }
	bool IsJampable() { return jump && characterController.isGrounded; }
	float CalcGravity() { return -(gravity * Time.deltaTime); }
	Quaternion CalcRotation()
	{
		var inputDirection = new Vector3(move.x, 0.0f, move.y).normalized;
		float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
		Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
		return Quaternion.Lerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
	}
}