using IceMilkTea.Core;
public class NotePlayerCharacter : AbstractPlayerCharacter<NotePlayerCharacter>
{
	protected override void Awake()
	{
		base.Awake();
		stateMachine = SetStateMachine(this);
	}
}