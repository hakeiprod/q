using System;
using IceMilkTea.Core;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public abstract partial class AbstractPlayerCharacter : AbstractCharacter
{
	ImtStateMachine<AbstractPlayerCharacter> stateMachine;
	[NonSerialized] public ObservableStateMachineTrigger observableStateMachineTrigger;
	protected override void Awake()
	{
		base.Awake();
		SetStates();
		observableStateMachineTrigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
	}
	void Start()
	{
		stateMachine.Update();
	}
	void Update()
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
}