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
	protected override void Awake()
	{
		base.Awake();
		observableStateMachineTrigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
	}
	protected virtual void Start()
	{
		SetAbilities();
	}
	public string GetCurrentAnimatorClip()
	{
		return animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
	}
	public void OnStateExitAsObservableByName(string stateInfoName, Action<ObservableStateMachineTrigger.OnStateInfo> Subscribe)
	{
		observableStateMachineTrigger.OnStateExitAsObservable().Where(x => x.StateInfo.IsName(stateInfoName)).Subscribe(Subscribe).AddTo(this);
	}
	public void OnStateEnterAsObservableByName(string stateInfoName, Action<ObservableStateMachineTrigger.OnStateInfo> Subscribe)
	{
		observableStateMachineTrigger.OnStateEnterAsObservable().Where(x => x.StateInfo.IsName(stateInfoName)).Subscribe(Subscribe).AddTo(this);
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
public abstract class AbstractPlayerCharacter<T> : AbstractPlayerCharacter, IPlayerCharacter<T> where T : AbstractPlayerCharacter<T>
{
	public ImtStateMachine<T> stateMachine { get; set; }
	protected override void Start()
	{
		stateMachine.Update();
	}
	protected virtual void Update()
	{
		stateMachine.Update();
	}
	protected virtual ImtStateMachine<T> SetStateMachine(T character)
	{
		var stateMachine = new ImtStateMachine<T>(character);
		stateMachine.SetStartState<IPlayerCharacter<T>.PlayerIdleState>();
		stateMachine.AddTransition<IPlayerCharacter<T>.PlayerIdleState, IPlayerCharacter<T>.PlayerWalkState>(state["walk"]);
		stateMachine.AddTransition<IPlayerCharacter<T>.PlayerIdleState, IPlayerCharacter<T>.PlayerJumpState>(state["jump"]);
		stateMachine.AddTransition<IPlayerCharacter<T>.PlayerIdleState, IPlayerCharacter<T>.PlayerFallState>(state["fall"]);

		stateMachine.AddTransition<IPlayerCharacter<T>.PlayerWalkState, IPlayerCharacter<T>.PlayerIdleState>(state["idle"]);
		stateMachine.AddTransition<IPlayerCharacter<T>.PlayerWalkState, IPlayerCharacter<T>.PlayerJumpState>(state["jump"]);
		stateMachine.AddTransition<IPlayerCharacter<T>.PlayerWalkState, IPlayerCharacter<T>.PlayerRunState>(state["run"]);
		stateMachine.AddTransition<IPlayerCharacter<T>.PlayerWalkState, IPlayerCharacter<T>.PlayerFallState>(state["fall"]);

		stateMachine.AddTransition<IPlayerCharacter<T>.PlayerRunState, IPlayerCharacter<T>.PlayerWalkState>(state["walk"]);
		stateMachine.AddTransition<IPlayerCharacter<T>.PlayerRunState, IPlayerCharacter<T>.PlayerJumpState>(state["jump"]);
		stateMachine.AddTransition<IPlayerCharacter<T>.PlayerRunState, IPlayerCharacter<T>.PlayerFallState>(state["fall"]);

		stateMachine.AddTransition<IPlayerCharacter<T>.PlayerJumpState, IPlayerCharacter<T>.PlayerFallState>(state["fall"]);

		stateMachine.AddTransition<IPlayerCharacter<T>.PlayerFallState, IPlayerCharacter<T>.PlayerLandState>(state["land"]);

		stateMachine.AddTransition<IPlayerCharacter<T>.PlayerLandState, IPlayerCharacter<T>.PlayerIdleState>(state["idle"]);
		stateMachine.AddTransition<IPlayerCharacter<T>.PlayerLandState, IPlayerCharacter<T>.PlayerWalkState>(state["walk"]);
		return stateMachine;
	}
}