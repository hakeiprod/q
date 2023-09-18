using System.Collections.Generic;
using IceMilkTea.Core;
using UniRx;
using UnityEngine;

public abstract partial class AbstractCharacter : MonoBehaviour
{
	public Animator animator;
	public CharacterController characterController;
	public Dictionary<string, int> state { get; set; } = new()
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
	[SerializeField] public Status defaultStatus;
	public ReactiveProperty<Status> currentStatus = new();
	protected virtual void Awake()
	{
		currentStatus.Value = defaultStatus;
	}
	public class IdleState<T> : ImtStateMachine<T>.State where T : AbstractCharacter
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
	public class WalkState<T> : ImtStateMachine<T>.State where T : AbstractCharacter
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
	public class RunState<T> : ImtStateMachine<T>.State where T : AbstractCharacter
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
	public class JumpState<T> : ImtStateMachine<T>.State where T : AbstractCharacter
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
	public class FallState<T> : ImtStateMachine<T>.State where T : AbstractCharacter
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
	public class LandState<T> : ImtStateMachine<T>.State where T : AbstractCharacter
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
}