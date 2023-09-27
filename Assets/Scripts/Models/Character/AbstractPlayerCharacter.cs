using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using IceMilkTea.Core;

public abstract class AbstractPlayerCharacter : AbstractCharacter
{
	public List<PlayerCharacterAbility> abilities;
	public PlayerInputAction playerInputAction;
	[NonSerialized] public ObservableStateMachineTrigger observableStateMachineTrigger;
	[NonSerialized] float RotationSmoothTime = 0.12f;
	float _rotationVelocity;
	protected virtual void Start()
	{
		SetAbilities();
		observableStateMachineTrigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
	}
	public string GetCurrentAnimatorClip()
	{
		return animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
	}
	public bool IsIdling() { return playerInputAction.move == Vector2.zero; }
	public bool IsJampable() { return playerInputAction.jump && characterController.isGrounded; }
	public void Move(float speed, float y)
	{
		var targetRotation = 0f;
		var move = playerInputAction.move;
		if (!IsIdling())
		{
			targetRotation = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
			float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _rotationVelocity, RotationSmoothTime);
			transform.rotation = Quaternion.Euler(0f, rotation, 0f);
		}
		var targetDirection = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
		characterController.Move(targetDirection.normalized * (speed * Time.deltaTime) + new Vector3(0f, y, 0f) * Time.deltaTime);
	}
	public AbstractPlayerCharacter Instantiate(Transform transform, PlayerInputAction playerInputAction)
	{
		this.playerInputAction = playerInputAction;
		return Instantiate(this, transform);
	}
	void SetAbilities()
	{
		abilities[0].Setup(playerInputAction, (x) => x.ability0);
		// abilities[1].Setup(playerInputAction, (x) => x.ability1);
		// abilities[2].Setup(playerInputAction, (x) => x.ability2);
	}
}
public abstract class AbstractPlayerCharacter<T> : AbstractPlayerCharacter where T : AbstractPlayerCharacter<T>
{
	public ImtStateMachine<T> stateMachine;
	protected override void Awake()
	{
		base.Awake();
	}
	protected override void Start()
	{
		base.Start();
		stateMachine.Update();
	}
	protected virtual void Update()
	{
		Debug.Log(stateMachine.CurrentStateName);
		stateMachine.Update();
	}
	protected virtual ImtStateMachine<T> SetStateMachine(T character)
	{
		var stateMachine = new ImtStateMachine<T>(character);
		stateMachine.SetStartState<PlayerIdleState>();
		stateMachine.AddTransition<PlayerIdleState, PlayerWalkState>(state["walk"]);
		stateMachine.AddTransition<PlayerIdleState, PlayerJumpState>(state["jump"]);
		stateMachine.AddTransition<PlayerIdleState, PlayerFallState>(state["fall"]);

		stateMachine.AddTransition<PlayerWalkState, PlayerIdleState>(state["idle"]);
		stateMachine.AddTransition<PlayerWalkState, PlayerJumpState>(state["jump"]);
		stateMachine.AddTransition<PlayerWalkState, PlayerRunState>(state["run"]);
		stateMachine.AddTransition<PlayerWalkState, PlayerFallState>(state["fall"]);

		stateMachine.AddTransition<PlayerRunState, PlayerWalkState>(state["walk"]);
		stateMachine.AddTransition<PlayerRunState, PlayerJumpState>(state["jump"]);
		stateMachine.AddTransition<PlayerRunState, PlayerFallState>(state["fall"]);

		stateMachine.AddTransition<PlayerJumpState, PlayerFallState>(state["fall"]);

		stateMachine.AddTransition<PlayerFallState, PlayerLandState>(state["land"]);

		stateMachine.AddTransition<PlayerLandState, PlayerIdleState>(state["idle"]);
		stateMachine.AddTransition<PlayerLandState, PlayerWalkState>(state["walk"]);
		return stateMachine;
	}
	public class PlayerIdleState : IdleState<T>
	{
		protected override void Update()
		{
			if (!Context.characterController.isGrounded) Context.stateMachine.SendEvent(Context.state["fall"]);
			if (!Context.IsIdling()) Context.stateMachine.SendEvent(Context.state["walk"]);
			if (Context.IsJampable()) Context.stateMachine.SendEvent(Context.state["jump"]);
		}
	}
	public class PlayerWalkState : WalkState<T>
	{
		protected override void Update()
		{
			Context.Move(Context.currentStatus.Value.walk.speed, -Context.currentStatus.Value.fall.speed);
			if (!Context.characterController.isGrounded) Context.stateMachine.SendEvent(Context.state["fall"]);
			if (Context.GetCurrentAnimatorClip() != "Walk_N") return;
			if (Context.IsJampable()) Context.stateMachine.SendEvent(Context.state["jump"]);
			if (Context.IsIdling()) Context.stateMachine.SendEvent(Context.state["idle"]);
			if (Context.playerInputAction.run) Context.stateMachine.SendEvent(Context.state["run"]);
		}
	}
	public class PlayerRunState : RunState<T>
	{
		protected override void Update()
		{
			if (!Context.characterController.isGrounded) Context.stateMachine.SendEvent(Context.state["fall"]);
			if (Context.IsJampable()) Context.stateMachine.SendEvent(Context.state["jump"]);
			if (Context.IsIdling()) Context.stateMachine.SendEvent(Context.state["idle"]);
			if (!Context.playerInputAction.run) Context.stateMachine.SendEvent(Context.state["walk"]);
			Context.Move(Context.currentStatus.Value.run.speed, -Context.currentStatus.Value.fall.speed);
		}
	}
	public class PlayerJumpState : JumpState<T>
	{
		protected override void Update()
		{
			if (Context.transform.position.y > Context.currentStatus.Value.jump.height) Context.stateMachine.SendEvent(Context.state["fall"]);
			Context.Move(Context.currentStatus.Value.walk.speed, Context.currentStatus.Value.jump.speed);
		}
	}
	public class PlayerFallState : FallState<T>
	{
		protected override void Update()
		{
			Context.Move(Context.currentStatus.Value.walk.speed, -Context.currentStatus.Value.fall.speed);
			if (Context.GetCurrentAnimatorClip() != "InAir") return;
			if (Context.characterController.isGrounded) Context.stateMachine.SendEvent(Context.state["land"]);
		}
	}
	public class PlayerLandState : LandState<T>
	{
		protected override void Update()
		{
			if (Context.GetCurrentAnimatorClip() != "JumpLand") return;
			if (Context.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1) Context.stateMachine.SendEvent(Context.state["idle"]);
		}
	}
}