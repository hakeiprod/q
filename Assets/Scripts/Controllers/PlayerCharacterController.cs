using IceMilkTea.Core;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerCharacterController : AbstractCharacterController
{
	public ImtStateMachine<PlayerCharacterController> stateMachine;
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
		stateMachine.AddTransition<PlayerWalkState, PlayerRunState>((int)State["run"]);

		stateMachine.AddTransition<PlayerRunState, PlayerWalkState>((int)State["walk"]);

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
	public void OnRun(InputAction.CallbackContext context)
	{
		run = context.performed;
	}
	public class PlayerIdleState : IdleState<PlayerCharacterController>
	{
		protected override void Update()
		{
			if (!Context.characterController.isGrounded) Context.stateMachine.SendEvent((int)Context.State["fall"]);
			if (!Context.IsIdling()) Context.stateMachine.SendEvent((int)Context.State["walk"]);
			if (Context.IsJampable()) Context.stateMachine.SendEvent((int)Context.State["jump"]);
		}
	}
	public class PlayerWalkState : WalkState<PlayerCharacterController>
	{
		protected override void Update()
		{
			if (Context.GetCurrentAnimatorClip() != "Walk_N") return;
			if (Context.IsJampable()) Context.stateMachine.SendEvent((int)Context.State["jump"]);
			if (Context.IsIdling()) Context.stateMachine.SendEvent((int)Context.State["idle"]);
			if (Context.run) Context.stateMachine.SendEvent((int)Context.State["run"]);
			Context.transform.rotation = Context.CalcRotation();
			Context.characterController.Move(new Vector3(Context.move.x * Context.speed, Context.CalcGravity(), Context.move.y * Context.speed));
		}
	}
	public class PlayerRunState : RunState<PlayerCharacterController>
	{
		protected override void Update()
		{
			if (Context.IsJampable()) Context.stateMachine.SendEvent((int)Context.State["jump"]);
			if (Context.IsIdling()) Context.stateMachine.SendEvent((int)Context.State["idle"]);
			if (!Context.run) Context.stateMachine.SendEvent((int)Context.State["walk"]);
			Context.transform.rotation = Context.CalcRotation();
			Context.characterController.Move(new Vector3(Context.move.x * Context.speed, Context.CalcGravity(), Context.move.y * Context.speed));
		}
	}
	public class PlayerJumpState : JumpState<PlayerCharacterController>
	{
		protected override void Update()
		{
			if (Context.transform.position.y > Context.jumpingHeight) Context.stateMachine.SendEvent((int)Context.State["fall"]);
			Context.characterController.Move(new Vector3(0, Context.jumpingSpeed * Time.deltaTime, 0));
		}
	}
	public class PlayerFallState : FallState<PlayerCharacterController>
	{
		protected override void Update()
		{
			if (Context.GetCurrentAnimatorClip() != "InAir") return;
			if (Context.characterController.isGrounded) Context.stateMachine.SendEvent((int)Context.State["land"]);
			Context.characterController.Move(new Vector3(0, Context.CalcGravity(), 0));
		}
	}
	public class PlayerLandState : LandState<PlayerCharacterController>
	{
		protected override void Update()
		{
			if (Context.GetCurrentAnimatorClip() != "JumpLand") return;
			if (Context.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1) Context.stateMachine.SendEvent((int)Context.State["idle"]);
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