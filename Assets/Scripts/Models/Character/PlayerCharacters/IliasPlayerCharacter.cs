using UnityEngine;

public class IliasPlayerCharacter : AbstractPlayerCharacter
{
	[SerializeField] public Status firstSkillStatus;
	public void FirstSkill(PlayerCharacterAbility ability)
	{
		ability.Duration(
			() => currentStatus.Value = firstSkillStatus,
			() => currentStatus.Value = defaultStatus
		);
	}
}