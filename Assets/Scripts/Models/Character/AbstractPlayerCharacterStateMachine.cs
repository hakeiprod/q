using IceMilkTea.Core;

public abstract partial class AbstractPlayerCharacter
{
	void SetStateMachine()
	{
		stateMachine = new ImtStateMachine<AbstractPlayerCharacter>(this);
		stateMachine.SetStartState<PlayerIdleState<AbstractPlayerCharacter>>();
		stateMachine.AddTransition<PlayerIdleState<AbstractPlayerCharacter>, PlayerWalkState<AbstractPlayerCharacter>>((int)state["walk"]);
		stateMachine.AddTransition<PlayerIdleState<AbstractPlayerCharacter>, PlayerJumpState<AbstractPlayerCharacter>>((int)state["jump"]);
		stateMachine.AddTransition<PlayerIdleState<AbstractPlayerCharacter>, PlayerFallState<AbstractPlayerCharacter>>((int)state["fall"]);

		stateMachine.AddTransition<PlayerWalkState<AbstractPlayerCharacter>, PlayerIdleState<AbstractPlayerCharacter>>((int)state["idle"]);
		stateMachine.AddTransition<PlayerWalkState<AbstractPlayerCharacter>, PlayerJumpState<AbstractPlayerCharacter>>((int)state["jump"]);
		stateMachine.AddTransition<PlayerWalkState<AbstractPlayerCharacter>, PlayerRunState<AbstractPlayerCharacter>>((int)state["run"]);
		stateMachine.AddTransition<PlayerWalkState<AbstractPlayerCharacter>, PlayerFallState<AbstractPlayerCharacter>>((int)state["fall"]);

		stateMachine.AddTransition<PlayerRunState<AbstractPlayerCharacter>, PlayerWalkState<AbstractPlayerCharacter>>((int)state["walk"]);
		stateMachine.AddTransition<PlayerRunState<AbstractPlayerCharacter>, PlayerJumpState<AbstractPlayerCharacter>>((int)state["jump"]);
		stateMachine.AddTransition<PlayerRunState<AbstractPlayerCharacter>, PlayerFallState<AbstractPlayerCharacter>>((int)state["fall"]);

		stateMachine.AddTransition<PlayerJumpState<AbstractPlayerCharacter>, PlayerFallState<AbstractPlayerCharacter>>((int)state["fall"]);

		stateMachine.AddTransition<PlayerFallState<AbstractPlayerCharacter>, PlayerLandState<AbstractPlayerCharacter>>((int)state["land"]);

		stateMachine.AddTransition<PlayerLandState<AbstractPlayerCharacter>, PlayerIdleState<AbstractPlayerCharacter>>((int)state["idle"]);
		stateMachine.AddTransition<PlayerLandState<AbstractPlayerCharacter>, PlayerWalkState<AbstractPlayerCharacter>>((int)state["walk"]);
	}
	public class PlayerIdleState<T> : IdleState<T> where T : AbstractPlayerCharacter
	{
		protected override void Update()
		{
			if (!Context.characterController.isGrounded) Context.stateMachine.SendEvent(Context.state["fall"]);
			if (!Context.IsIdling()) Context.stateMachine.SendEvent(Context.state["walk"]);
			if (Context.IsJampable()) Context.stateMachine.SendEvent(Context.state["jump"]);
		}
	}
	public class PlayerWalkState<T> : WalkState<T> where T : AbstractPlayerCharacter
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
	public class PlayerRunState<T> : RunState<T> where T : AbstractPlayerCharacter
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
	public class PlayerJumpState<T> : JumpState<T> where T : AbstractPlayerCharacter
	{
		protected override void Update()
		{
			if (Context.transform.position.y > Context.currentStatus.Value.jump.height) Context.stateMachine.SendEvent(Context.state["fall"]);
			Context.Move(Context.currentStatus.Value.walk.speed, Context.currentStatus.Value.jump.speed);
		}
	}
	public class PlayerFallState<T> : FallState<T> where T : AbstractPlayerCharacter
	{
		protected override void Update()
		{
			Context.Move(Context.currentStatus.Value.walk.speed, -Context.currentStatus.Value.fall.speed);
			if (Context.GetCurrentAnimatorClip() != "InAir") return;
			if (Context.characterController.isGrounded) Context.stateMachine.SendEvent(Context.state["land"]);
		}
	}
	public class PlayerLandState<T> : LandState<T> where T : AbstractPlayerCharacter
	{
		protected override void Update()
		{
			if (Context.GetCurrentAnimatorClip() != "JumpLand") return;
			if (Context.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1) Context.stateMachine.SendEvent(Context.state["idle"]);
		}
	}
}