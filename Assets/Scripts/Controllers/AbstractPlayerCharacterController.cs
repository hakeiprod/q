using IceMilkTea.Core;
using UnityEngine;

[RequireComponent(typeof(AbstractPlayerCharacter))]
public abstract class AbstractPlayerCharacterController : AbstractCharacterController
{
	public AbstractPlayerCharacter playerCharacter;
	public ImtStateMachine<AbstractPlayerCharacterController> stateMachine;
	public float RotationSmoothTime = 0.12f;
	private float _rotationVelocity;
	protected override void Awake()
	{
		base.Awake();
		stateMachine = new ImtStateMachine<AbstractPlayerCharacterController>(this);

		stateMachine.SetStartState<PlayerIdleState<AbstractPlayerCharacterController>>();
		stateMachine.AddTransition<PlayerIdleState<AbstractPlayerCharacterController>, PlayerWalkState<AbstractPlayerCharacterController>>((int)State["walk"]);
		stateMachine.AddTransition<PlayerIdleState<AbstractPlayerCharacterController>, PlayerJumpState<AbstractPlayerCharacterController>>((int)State["jump"]);
		stateMachine.AddTransition<PlayerIdleState<AbstractPlayerCharacterController>, PlayerFallState<AbstractPlayerCharacterController>>((int)State["fall"]);

		stateMachine.AddTransition<PlayerWalkState<AbstractPlayerCharacterController>, PlayerIdleState<AbstractPlayerCharacterController>>((int)State["idle"]);
		stateMachine.AddTransition<PlayerWalkState<AbstractPlayerCharacterController>, PlayerJumpState<AbstractPlayerCharacterController>>((int)State["jump"]);
		stateMachine.AddTransition<PlayerWalkState<AbstractPlayerCharacterController>, PlayerRunState<AbstractPlayerCharacterController>>((int)State["run"]);

		stateMachine.AddTransition<PlayerRunState<AbstractPlayerCharacterController>, PlayerWalkState<AbstractPlayerCharacterController>>((int)State["walk"]);
		stateMachine.AddTransition<PlayerRunState<AbstractPlayerCharacterController>, PlayerJumpState<AbstractPlayerCharacterController>>((int)State["jump"]);

		stateMachine.AddTransition<PlayerJumpState<AbstractPlayerCharacterController>, PlayerFallState<AbstractPlayerCharacterController>>((int)State["fall"]);

		stateMachine.AddTransition<PlayerFallState<AbstractPlayerCharacterController>, PlayerLandState<AbstractPlayerCharacterController>>((int)State["land"]);

		stateMachine.AddTransition<PlayerLandState<AbstractPlayerCharacterController>, PlayerIdleState<AbstractPlayerCharacterController>>((int)State["idle"]);
		stateMachine.AddTransition<PlayerLandState<AbstractPlayerCharacterController>, PlayerWalkState<AbstractPlayerCharacterController>>((int)State["walk"]);
	}
	protected override void Start()
	{
		base.Start();
		stateMachine.Update();
	}
	protected virtual void Update()
	{
		stateMachine.Update();
		Debug.Log(stateMachine.CurrentStateName);
	}
	public class PlayerIdleState<T> : IdleState<T> where T : AbstractPlayerCharacterController
	{
		protected override void Update()
		{
			if (!Context.characterController.isGrounded) Context.stateMachine.SendEvent((int)Context.State["fall"]);
			if (!Context.IsIdling()) Context.stateMachine.SendEvent((int)Context.State["walk"]);
			if (Context.IsJampable()) Context.stateMachine.SendEvent((int)Context.State["jump"]);
		}
	}
	public class PlayerWalkState<T> : WalkState<T> where T : AbstractPlayerCharacterController
	{
		protected override void Update()
		{
			Context.Move(Context.playerCharacter.defaultStatus.walk.speed, -Context.playerCharacter.defaultStatus.fall.speed);
			if (Context.GetCurrentAnimatorClip() != "Walk_N") return;
			if (Context.IsJampable()) Context.stateMachine.SendEvent((int)Context.State["jump"]);
			if (Context.IsIdling()) Context.stateMachine.SendEvent((int)Context.State["idle"]);
			if (Context.run) Context.stateMachine.SendEvent((int)Context.State["run"]);
		}
	}
	public class PlayerRunState<T> : RunState<T> where T : AbstractPlayerCharacterController
	{
		protected override void Update()
		{
			if (Context.IsJampable()) Context.stateMachine.SendEvent((int)Context.State["jump"]);
			if (Context.IsIdling()) Context.stateMachine.SendEvent((int)Context.State["idle"]);
			if (!Context.run) Context.stateMachine.SendEvent((int)Context.State["walk"]);
			Context.Move(Context.playerCharacter.defaultStatus.run.speed, -Context.playerCharacter.defaultStatus.fall.speed);
		}
	}
	public class PlayerJumpState<T> : JumpState<T> where T : AbstractPlayerCharacterController
	{
		protected override void Update()
		{
			if (Context.transform.position.y > Context.playerCharacter.defaultStatus.jump.height) Context.stateMachine.SendEvent((int)Context.State["fall"]);
			Context.Move(Context.playerCharacter.defaultStatus.walk.speed, Context.playerCharacter.defaultStatus.jump.speed);
		}
	}
	public class PlayerFallState<T> : FallState<T> where T : AbstractPlayerCharacterController
	{
		protected override void Update()
		{
			Context.Move(Context.playerCharacter.defaultStatus.walk.speed, -Context.playerCharacter.defaultStatus.fall.speed);
			if (Context.GetCurrentAnimatorClip() != "InAir") return;
			if (Context.characterController.isGrounded) Context.stateMachine.SendEvent((int)Context.State["land"]);
		}
	}
	public class PlayerLandState<T> : LandState<T> where T : AbstractPlayerCharacterController
	{
		protected override void Update()
		{
			if (Context.GetCurrentAnimatorClip() != "JumpLand") return;
			if (Context.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1) Context.stateMachine.SendEvent((int)Context.State["idle"]);
		}
	}
	bool IsIdling() { return move == Vector2.zero; }
	bool IsJampable() { return jump && characterController.isGrounded; }
	void Move(float speed, float y)
	{
		var targetRotation = 0f;
		if (!IsIdling())
		{
			targetRotation = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
			float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _rotationVelocity, RotationSmoothTime);
			transform.rotation = Quaternion.Euler(0f, rotation, 0f);
		}
		var targetDirection = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
		characterController.Move(targetDirection.normalized * (speed * Time.deltaTime) + new Vector3(0f, y, 0f) * Time.deltaTime);
	}
}