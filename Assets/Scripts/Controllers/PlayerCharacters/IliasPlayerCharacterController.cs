using IceMilkTea.Core;
using UnityEngine;
using UniRx;

public class IliasPlayerCharacterController : AbstractPlayerCharacterController
{
	public ImtStateMachine<IliasPlayerCharacterController> iliasStateMachine;
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
			.Subscribe(_ => playerCharacter.defaultStatus.run.speed = 20);
		Observable.Timer(System.TimeSpan.FromSeconds(seconds))
			.Subscribe(_ => playerCharacter.defaultStatus.run.speed = 8);
	}
}