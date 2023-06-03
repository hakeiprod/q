using IceMilkTea.Core;
using UnityEngine;

[RequireComponent(typeof(PlayerCharacter))]
public class PlayerCharacterController : AbstractCharacterController
{
	public PlayerCharacter playerCharacter;
	public ImtStateMachine<PlayerCharacterController> stateMachine;
	public float RotationSmoothTime = 0.12f;
	private float _rotationVelocity;
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
			Context.Move(Context.playerCharacter.walk.speed, -Context.playerCharacter.fall.speed);
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
			Context.Move(Context.playerCharacter.run.speed, -Context.playerCharacter.fall.speed);
		}
	}
	public class PlayerJumpState : JumpState<PlayerCharacterController>
	{
		protected override void Update()
		{
			if (Context.transform.position.y > Context.playerCharacter.jump.height) Context.stateMachine.SendEvent((int)Context.State["fall"]);
			Context.Move(Context.playerCharacter.walk.speed, Context.playerCharacter.jump.speed);
		}
	}
	public class PlayerFallState : FallState<PlayerCharacterController>
	{
		protected override void Update()
		{
			Context.Move(Context.playerCharacter.walk.speed, -Context.playerCharacter.fall.speed);
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
	float CalcGravity() { return -(playerCharacter.fall.speed); }
	void Move(float speed, float y)
	{
		var targetRotation = 0f;
		if (move != Vector2.zero)
		{
			targetRotation = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
			float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _rotationVelocity, RotationSmoothTime);
			transform.rotation = Quaternion.Euler(0f, rotation, 0f);
		}
		var targetDirection = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
		characterController.Move(targetDirection.normalized * (speed * Time.deltaTime) + new Vector3(0f, y, 0f) * Time.deltaTime);
	}
}