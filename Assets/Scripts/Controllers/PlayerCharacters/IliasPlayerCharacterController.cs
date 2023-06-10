using IceMilkTea.Core;
using UniRx;

public class IliasPlayerCharacterController : AbstractPlayerCharacterController
{
	public IliasPlayerCharacter iliasPlayerCharacter;
	protected override void Awake()
	{
		iliasPlayerCharacter = (IliasPlayerCharacter)playerCharacter;
		base.Awake();
		this.ObserveFirstSkill(10);
	}
	protected override void Start()
	{
		base.Start();
		this.ObserveFirstSkill(10);
	}
	protected override void Update()
	{
		base.Update();
	}
	void ObserveFirstSkill(int seconds)
	{
		this.ObserveEveryValueChanged(x => x.firstSkill).Where(x => x)
			.ThrottleFirst(System.TimeSpan.FromSeconds(seconds))
			.Subscribe(_ => playerCharacter.currentStatus.run.speed = iliasPlayerCharacter.firstSkillStatus.run.speed);
		Observable.Timer(System.TimeSpan.FromSeconds(seconds))
			.Subscribe(_ => playerCharacter.currentStatus.run.speed = playerCharacter.defaultStatus.run.speed);
	}
}