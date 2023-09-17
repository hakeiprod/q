using IceMilkTea.Core;
public class DefaultPlayerCharacter : AbstractPlayerCharacter<DefaultPlayerCharacter>
{
	protected override void Awake()
	{
		base.Awake();
		stateMachine = SetStateMachine(this);
	}
}