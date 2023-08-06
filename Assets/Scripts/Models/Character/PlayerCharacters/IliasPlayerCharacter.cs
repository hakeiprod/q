using UnityEngine;
using UniRx;
public class IliasPlayerCharacter : AbstractPlayerCharacter
{
	[SerializeField] public Status firstSkillStatus;
	protected override void Awake()
	{
		base.Awake();
		ObserveFirstSkill(10);
	}
	void ObserveFirstSkill(int seconds)
	{
		this.ObserveEveryValueChanged(x => x.firstSkill).Where(x => x)
			.ThrottleFirst(System.TimeSpan.FromSeconds(seconds))
			.Subscribe(_ => currentStatus = firstSkillStatus);
		Observable.Timer(System.TimeSpan.FromSeconds(seconds))
			.Subscribe(_ => currentStatus = defaultStatus);
	}
}