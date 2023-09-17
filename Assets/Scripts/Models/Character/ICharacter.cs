using IceMilkTea.Core;
using UnityEngine;
using System.Collections.Generic;
public interface ICharacter<T> where T : AbstractCharacter
{
	public Dictionary<string, int> state { get; set; }
	public class IdleState : ImtStateMachine<T>.State
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
	public class WalkState : ImtStateMachine<T>.State
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
	public class RunState : ImtStateMachine<T>.State
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
	public class JumpState : ImtStateMachine<T>.State
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
	public class FallState : ImtStateMachine<T>.State
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
	public class LandState : ImtStateMachine<T>.State
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