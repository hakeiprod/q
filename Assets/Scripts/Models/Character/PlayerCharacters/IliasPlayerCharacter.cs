using UnityEngine;

public class IliasPlayerCharacter : AbstractPlayerCharacter
{
	[SerializeField] public Status firstSkillStatus;
	public void FirstSkill(Ability ability)
	{
		ability.Duration(
			() =>
			{
				currentStatus = firstSkillStatus;
			},
			() =>
			{
				currentStatus = defaultStatus;
				ability.state = Ability.State.Unusable;
			});
	}
}