using IceMilkTea.Core;
using UnityEngine;
using UniRx;
public class NotePlayerCharacter : AbstractPlayerCharacter<NotePlayerCharacter>
{
	protected override void Awake()
	{
		base.Awake();
		state.Add("attack1", 9);
		state.Add("attack2", 10);
		stateMachine = SetStateMachine(this);
	}
	protected override void Start()
	{
		base.Start();
		observableStateMachineTrigger.OnStateExitAsObservable().Where(x => x.StateInfo.IsName("Attack1") || x.StateInfo.IsName("Attack2")).Subscribe((x) => stateMachine.SendEvent(state["idle"])).AddTo(this);
	}
	protected override ImtStateMachine<NotePlayerCharacter> SetStateMachine(NotePlayerCharacter character)
	{
		var stateMachine = new ImtStateMachine<NotePlayerCharacter>(character);
		stateMachine.SetStartState<NotePlayerIdleState>();
		stateMachine.AddTransition<NotePlayerIdleState, PlayerWalkState>(state["walk"]);
		stateMachine.AddTransition<NotePlayerIdleState, PlayerJumpState>(state["jump"]);
		stateMachine.AddTransition<NotePlayerIdleState, PlayerFallState>(state["fall"]);
		stateMachine.AddTransition<NotePlayerIdleState, NotePlayerAttack1State>(state["attack1"]);

		stateMachine.AddTransition<PlayerWalkState, NotePlayerIdleState>(state["idle"]);
		stateMachine.AddTransition<PlayerWalkState, PlayerJumpState>(state["jump"]);
		stateMachine.AddTransition<PlayerWalkState, PlayerRunState>(state["run"]);
		stateMachine.AddTransition<PlayerWalkState, PlayerFallState>(state["fall"]);

		stateMachine.AddTransition<PlayerRunState, PlayerWalkState>(state["walk"]);
		stateMachine.AddTransition<PlayerRunState, PlayerJumpState>(state["jump"]);
		stateMachine.AddTransition<PlayerRunState, PlayerFallState>(state["fall"]);

		stateMachine.AddTransition<PlayerJumpState, PlayerFallState>(state["fall"]);

		stateMachine.AddTransition<PlayerFallState, PlayerLandState>(state["land"]);

		stateMachine.AddTransition<PlayerLandState, NotePlayerIdleState>(state["idle"]);
		stateMachine.AddTransition<PlayerLandState, PlayerWalkState>(state["walk"]);

		stateMachine.AddTransition<NotePlayerAttack1State, NotePlayerAttack2State>(state["attack2"]);
		stateMachine.AddTransition<NotePlayerAttack1State, NotePlayerIdleState>(state["idle"]);

		stateMachine.AddTransition<NotePlayerAttack2State, NotePlayerIdleState>(state["idle"]);
		return stateMachine;
	}
	public class NotePlayerAttack1State : NotePlayerAttackState
	{
		private bool attackable = false;
		protected override void Update()
		{
			if (!Context.playerInputAction.fire) attackable = true;
			if (Context.playerInputAction.fire && attackable) Context.stateMachine.SendEvent(Context.state["attack2"]);
		}
	}
	public class NotePlayerAttack2State : NotePlayerAttackState
	{
	}
	public class NotePlayerAttackState : ImtStateMachine<NotePlayerCharacter>.State
	{
		protected override void Enter()
		{
			Context.animator.SetTrigger("Attack");
		}

	}
	public class NotePlayerIdleState : PlayerIdleState
	{
		protected override void Update()
		{
			base.Update();
			if (Context.playerInputAction.fire) Context.stateMachine.SendEvent(Context.state["attack1"]);
		}
	}
}