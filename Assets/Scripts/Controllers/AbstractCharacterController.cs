using System.Collections.Generic;
using IceMilkTea.Core;
using UniRx.Triggers;
using UnityEngine;
using UniRx;
using System;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public abstract class AbstractCharacterController : MonoBehaviour
{
	public CharacterController characterController;
	public Animator animator;
	[System.NonSerialized] public ObservableStateMachineTrigger observableStateMachineTrigger;
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
	[System.NonSerialized] public bool run;
	protected virtual void Awake()
	{
		observableStateMachineTrigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
	}
	protected virtual void Start()
	{
	}
	public class IdleState<T> : ImtStateMachine<T>.State where T : AbstractCharacterController
	{
		protected override void Enter()
		{
			Context.animator.SetBool("Idle", true);
		}
		protected override void Exit()
		{
			Context.animator.SetBool("Idle", false);
		}
	}
	public class WalkState<T> : ImtStateMachine<T>.State where T : AbstractCharacterController
	{
		protected override void Enter()
		{

			Context.animator.SetBool("Walk", true);
		}
		protected override void Exit()
		{
			Context.animator.SetBool("Walk", false);
		}
	}
	public class RunState<T> : ImtStateMachine<T>.State where T : AbstractCharacterController
	{
		protected override void Enter()
		{
			Context.animator.SetBool("Run", true);
		}
		protected override void Exit()
		{
			Context.animator.SetBool("Run", false);
		}
	}
	public class JumpState<T> : ImtStateMachine<T>.State where T : AbstractCharacterController
	{
		protected override void Enter()
		{
			Context.animator.SetBool("Jump", true);
		}
		protected override void Exit()
		{
			Context.animator.SetBool("Jump", false);
		}
	}
	public class FallState<T> : ImtStateMachine<T>.State where T : AbstractCharacterController
	{
		protected override void Enter()
		{
			Context.animator.SetBool("Fall", true);
		}
		protected override void Exit()
		{
			Context.animator.SetBool("Fall", false);
		}
	}
	public class LandState<T> : ImtStateMachine<T>.State where T : AbstractCharacterController
	{
		protected override void Enter()
		{
			Context.animator.SetBool("Land", true);
		}
		protected override void Exit()
		{
			Context.animator.SetBool("Land", false);
		}
	}
	public string GetCurrentAnimatorClip()
	{
		return animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
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