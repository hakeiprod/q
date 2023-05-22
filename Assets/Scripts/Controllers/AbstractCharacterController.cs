using System.Collections.Generic;
using IceMilkTea.Core;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
using System;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public abstract class AbstractCharacterController<T> : MonoBehaviour
{
	public CharacterController characterController;
	public Animator animator;
	[System.NonSerialized] public ObservableStateMachineTrigger observableStateMachineTrigger;
	public ImtStateMachine<T> stateMachine;
	public Dictionary<string, int> State = new Dictionary<string, int>()
	{
		{ "idle", 0 },
		{ "walk", 1 },
		{ "run", 2 },
		{ "crouch", 3 },
		{ "jump", 4 },
		{ "fall", 5 },
		{ "land", 6 },
		{ "injure", 7 },
		{ "die", 8 },
	};
	public float speed;
	public float jumpingHeight;
	public float jumpingSpeed;
	public float gravity;
	[System.NonSerialized] public Vector2 move;
	[System.NonSerialized] public bool jump;
	protected virtual void Awake()
	{
		observableStateMachineTrigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
	}
	protected virtual void Start()
	{
	}
	public class IdleState : ImtStateMachine<T>.State
	{
		protected override void Enter() { }
		protected override void Update() { }
	}
	public class WalkState : ImtStateMachine<T>.State
	{
		protected override void Enter() { }
		protected override void Update() { }
	}
	public class RunState : ImtStateMachine<T>.State
	{
		protected override void Enter() { }
		protected override void Update() { }
	}
	public class JumpState : ImtStateMachine<T>.State
	{
		protected override void Update() { }
	}
	public class FallState : ImtStateMachine<T>.State
	{
		protected override void Update() { }
	}
	public class LandState : ImtStateMachine<T>.State
	{
		protected override void Update() { }
	}
	public void OnStateExitAsObservableByName(string stateInfoName, Action<UniRx.Triggers.ObservableStateMachineTrigger.OnStateInfo> Subscribe)
	{
		observableStateMachineTrigger.OnStateExitAsObservable().Where(x => x.StateInfo.IsName(stateInfoName)).Subscribe(Subscribe).AddTo(this);
	}
	public void OnStateEnterAsObservableByName(string stateInfoName, Action<UniRx.Triggers.ObservableStateMachineTrigger.OnStateInfo> Subscribe)
	{
		observableStateMachineTrigger.OnStateEnterAsObservable().Where(x => x.StateInfo.IsName(stateInfoName)).Subscribe(Subscribe).AddTo(this);
	}
}