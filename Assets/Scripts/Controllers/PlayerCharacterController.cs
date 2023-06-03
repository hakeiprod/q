using IceMilkTea.Core;
using UnityEngine;

[RequireComponent(typeof(PlayerCharacter))]
public class PlayerCharacterController : AbstractCharacterController
{
	public PlayerCharacter playerCharacter;
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
			Context.Move(Context.playerCharacter.walk.speed, Context.playerCharacter.jump.speed * Time.deltaTime);
		}
	}
	public class PlayerFallState : FallState<PlayerCharacterController>
	{
		protected override void Update()
		{
			Context.Move(Context.playerCharacter.walk.speed, Context.CalcGravity());
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
	float CalcGravity() { return -(playerCharacter.fall.speed * Time.deltaTime); }
	void Move(float moveSpeed, float y)
	{
		var inputDirection = new Vector3(move.x, 0.0f, move.y).normalized;
		var targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
		var targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
		var targetDirection = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
		transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
		characterController.Move(targetDirection.normalized * moveSpeed + new Vector3(0.0f, y, 0.0f));
	}
}