using System;
using IceMilkTea.Core;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public abstract partial class AbstractPlayerCharacter : AbstractCharacter
{
	[NonSerialized] public ObservableStateMachineTrigger observableStateMachineTrigger;
	[NonSerialized] public PlayerInputAction playerInputAction;
	[NonSerialized] public float RotationSmoothTime = 0.12f;
	[SerializeField] public Ability[] abilities;
	float _rotationVelocity;
	ImtStateMachine<AbstractPlayerCharacter> stateMachine;
	protected override void Awake()
	{
		base.Awake();
		SetStateMachine();
		observableStateMachineTrigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
	}
	protected virtual void Start()
	{
		stateMachine.Update();
	}
	protected virtual void Update()
	{
		stateMachine.Update();
		Debug.Log(stateMachine.CurrentStateName);
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
}