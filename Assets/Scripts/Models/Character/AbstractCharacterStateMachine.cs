using IceMilkTea.Core;

public abstract partial class AbstractCharacter
{
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