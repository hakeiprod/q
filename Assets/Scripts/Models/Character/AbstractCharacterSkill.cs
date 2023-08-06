using IceMilkTea.Core;

public abstract partial class AbstractCharacter
{
	public class Skill
	{
		enum Type
		{
			Duration,
			Action
		}
		public ImtStateMachine<Skill> stateMachine;
		public Skill()
		{
			stateMachine = new ImtStateMachine<Skill>(this);
		}
	}
}