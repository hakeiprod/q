using UnityEngine;

public class IliasPlayerCharacter : AbstractPlayerCharacter
{
	[SerializeField] public Status firstSkillStatus;
}
public class BuffStatus : Status
{
	public int duration;
}