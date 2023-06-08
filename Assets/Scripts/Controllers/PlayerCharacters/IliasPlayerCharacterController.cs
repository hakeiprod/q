using IceMilkTea.Core;

public class IliasPlayerCharacterController : AbstractPlayerCharacterController
{
	public ImtStateMachine<IliasPlayerCharacterController> iliasStateMachine;
	public class IliasPlayerFirstSkillState : PlayerFirstSkillState<IliasPlayerCharacterController>
	{
		protected override void Update()
		{ }
	}
	public class IliasPlayerSecondSkillState : PlayerSecondSkillState<IliasPlayerCharacterController>
	{
		protected override void Update()
		{ }
	}
	public class IliasPlayerThirdSkillState : PlayerThirdSkillState<IliasPlayerCharacterController>
	{
		protected override void Update()
		{ }
	}
}