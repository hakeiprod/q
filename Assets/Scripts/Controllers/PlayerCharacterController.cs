using IceMilkTea.Core;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerCharacter))]
public class PlayerCharacterController : AbstractCharacterController
{
	public PlayerCharacter playerCharacter;
	public Dictionary<string, int> walk = new Dictionary<string, int>()
	{
		{ "speed", 0 },
	};
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
		stateMachine.AddTransition<PlayerRunState, PlayerJumpState>((int)State["jump"]);

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
		Debug.Log(stateMachine.CurrentStateName);
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
			Context.Move(Context.playerCharacter.walk.speed, Context.CalcGravity());
			if (Context.GetCurrentAnimatorClip() != "Walk_N") return;
			if (Context.IsJampable()) Context.stateMachine.SendEvent((int)Context.State["jump"]);
			if (Context.IsIdling()) Context.stateMachine.SendEvent((int)Context.State["idle"]);
			if (Context.run) Context.stateMachine.SendEvent((int)Context.State["run"]);
		}
	}
	public class PlayerRunState : RunState<PlayerCharacterController>
	{
		protected override void Update()
		{
			if (Context.IsJampable()) Context.stateMachine.SendEvent((int)Context.State["jump"]);
			if (Context.IsIdling()) Context.stateMachine.SendEvent((int)Context.State["idle"]);
			if (!Context.run) Context.stateMachine.SendEvent((int)Context.State["walk"]);
			Context.Move(Context.playerCharacter.run.speed, Context.CalcGravity());
		}
	}
	public class PlayerJumpState : JumpState<PlayerCharacterController>
	{
		protected override void Update()
		{
			if (Context.transform.position.y > Context.playerCharacter.jump.height) Context.stateMachine.SendEvent((int)Context.State["fall"]);
			Context.Move(Context.speed, Context.jumpingSpeed * Time.deltaTime);
		}
	}
	public class PlayerFallState : FallState<PlayerCharacterController>
	{
		protected override void Update()
		{
			Context.Move(Context.playerCharacter.jump.speed, Context.CalcGravity());
			if (Context.GetCurrentAnimatorClip() != "InAir") return;
			if (Context.characterController.isGrounded) Context.stateMachine.SendEvent((int)Context.State["land"]);
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
	void Move(float moveSpeed, float y)
	{
		transform.rotation = CalcRotation();
		characterController.Move(new Vector3(move.x * moveSpeed, y, move.y * moveSpeed));
	}
}