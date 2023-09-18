using UnityEngine;
using IceMilkTea.Core;

public class IliasPlayerCharacter : AbstractPlayerCharacter<IliasPlayerCharacter>
{
	[SerializeField] public Status firstSkillStatus;
	protected override void Awake()
	{
		base.Awake();
		stateMachine = SetStateMachine(this);
	}
	void OnDisable()
	{
		if (stateMachine.Running) stateMachine.SendEvent(state["idle"]);
	}
	public void FirstSkill(PlayerCharacterAbility ability)
	{
		ability.Duration(
			() => currentStatus.Value = firstSkillStatus,
			() => currentStatus.Value = defaultStatus
		);
	}
}