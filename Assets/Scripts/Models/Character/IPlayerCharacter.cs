using UnityEngine;
public interface IPlayerCharacter<T> : ICharacter<T> where T : AbstractPlayerCharacter<T>
{
	public class PlayerIdleState : IdleState
	{
		protected override void Update()
		{
			if (!Context.characterController.isGrounded) Context.stateMachine.SendEvent(Context.state["fall"]);
			if (!Context.IsIdling()) Context.stateMachine.SendEvent(Context.state["walk"]);
			if (Context.IsJampable()) Context.stateMachine.SendEvent(Context.state["jump"]);
		}
	}
	public class PlayerWalkState : WalkState
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
	public class PlayerRunState : RunState
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
	public class PlayerJumpState : JumpState
	{
		protected override void Update()
		{
			if (Context.transform.position.y > Context.currentStatus.Value.jump.height) Context.stateMachine.SendEvent(Context.state["fall"]);
			Context.Move(Context.currentStatus.Value.walk.speed, Context.currentStatus.Value.jump.speed);
		}
	}
	public class PlayerFallState : FallState
	{
		protected override void Update()
		{
			Context.Move(Context.currentStatus.Value.walk.speed, -Context.currentStatus.Value.fall.speed);
			if (Context.GetCurrentAnimatorClip() != "InAir") return;
			if (Context.characterController.isGrounded) Context.stateMachine.SendEvent(Context.state["land"]);
		}
	}
	public class PlayerLandState : LandState
	{
		protected override void Update()
		{
			if (Context.GetCurrentAnimatorClip() != "JumpLand") return;
			if (Context.animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1) Context.stateMachine.SendEvent(Context.state["idle"]);
		}
	}
}