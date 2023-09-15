using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using IceMilkTea.Core;

public abstract partial class AbstractPlayerCharacter : AbstractCharacter, IPlayerCharacter<AbstractPlayerCharacter>
{
	public ImtStateMachine<AbstractPlayerCharacter> stateMachine { get; set; }
	public List<PlayerCharacterAbility> abilities;
	public PlayerInputAction playerInputAction;
	[NonSerialized] public ObservableStateMachineTrigger observableStateMachineTrigger;
	[NonSerialized] float RotationSmoothTime = 0.12f;
	float _rotationVelocity;
	protected override void Awake()
	{
		base.Awake();
		stateMachine = new ImtStateMachine<AbstractPlayerCharacter>(this);
		observableStateMachineTrigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
		SetStateMachine();
	}
	protected virtual void Start()
	{
		SetAbilities();
		stateMachine.Update();
	}
	protected virtual void Update()
	{
		stateMachine.Update();
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
	public void SetStateMachine()
	{
		stateMachine.SetStartState<IPlayerCharacter<AbstractPlayerCharacter>.PlayerIdleState>();
		stateMachine.AddTransition<IPlayerCharacter<AbstractPlayerCharacter>.PlayerIdleState, IPlayerCharacter<AbstractPlayerCharacter>.PlayerWalkState>(state["walk"]);
		stateMachine.AddTransition<IPlayerCharacter<AbstractPlayerCharacter>.PlayerIdleState, IPlayerCharacter<AbstractPlayerCharacter>.PlayerJumpState>(state["jump"]);
		stateMachine.AddTransition<IPlayerCharacter<AbstractPlayerCharacter>.PlayerIdleState, IPlayerCharacter<AbstractPlayerCharacter>.PlayerFallState>(state["fall"]);

		stateMachine.AddTransition<IPlayerCharacter<AbstractPlayerCharacter>.PlayerWalkState, IPlayerCharacter<AbstractPlayerCharacter>.PlayerIdleState>(state["idle"]);
		stateMachine.AddTransition<IPlayerCharacter<AbstractPlayerCharacter>.PlayerWalkState, IPlayerCharacter<AbstractPlayerCharacter>.PlayerJumpState>(state["jump"]);
		stateMachine.AddTransition<IPlayerCharacter<AbstractPlayerCharacter>.PlayerWalkState, IPlayerCharacter<AbstractPlayerCharacter>.PlayerRunState>(state["run"]);
		stateMachine.AddTransition<IPlayerCharacter<AbstractPlayerCharacter>.PlayerWalkState, IPlayerCharacter<AbstractPlayerCharacter>.PlayerFallState>(state["fall"]);

		stateMachine.AddTransition<IPlayerCharacter<AbstractPlayerCharacter>.PlayerRunState, IPlayerCharacter<AbstractPlayerCharacter>.PlayerWalkState>(state["walk"]);
		stateMachine.AddTransition<IPlayerCharacter<AbstractPlayerCharacter>.PlayerRunState, IPlayerCharacter<AbstractPlayerCharacter>.PlayerJumpState>(state["jump"]);
		stateMachine.AddTransition<IPlayerCharacter<AbstractPlayerCharacter>.PlayerRunState, IPlayerCharacter<AbstractPlayerCharacter>.PlayerFallState>(state["fall"]);

		stateMachine.AddTransition<IPlayerCharacter<AbstractPlayerCharacter>.PlayerJumpState, IPlayerCharacter<AbstractPlayerCharacter>.PlayerFallState>(state["fall"]);

		stateMachine.AddTransition<IPlayerCharacter<AbstractPlayerCharacter>.PlayerFallState, IPlayerCharacter<AbstractPlayerCharacter>.PlayerLandState>(state["land"]);

		stateMachine.AddTransition<IPlayerCharacter<AbstractPlayerCharacter>.PlayerLandState, IPlayerCharacter<AbstractPlayerCharacter>.PlayerIdleState>(state["idle"]);
		stateMachine.AddTransition<IPlayerCharacter<AbstractPlayerCharacter>.PlayerLandState, IPlayerCharacter<AbstractPlayerCharacter>.PlayerWalkState>(state["walk"]);
	}
}