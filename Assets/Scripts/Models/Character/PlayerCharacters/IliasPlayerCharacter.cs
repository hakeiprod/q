using UnityEngine;
using UniRx;
using System;

public class IliasPlayerCharacter : AbstractPlayerCharacter
{
	[SerializeField] public Status firstSkillStatus;
	protected override void Start()
	{
		ObserveFirstSkill(10);
	}
	void ObserveFirstSkill(int seconds)
	{
		this.ObserveEveryValueChanged(x => x.playerInputAction.firstSkill).Where(x => x)
			.ThrottleFirst(TimeSpan.FromSeconds(seconds))
			.Subscribe(_ =>
		SetDurationSeconds(seconds, () => currentStatus = firstSkillStatus, () => currentStatus = defaultStatus)
			);
	}
	void SetDurationSeconds(double seconds, Action OnStart, Action OnEnd)
	{
		OnStart();
		Observable.Timer(TimeSpan.FromSeconds(seconds))
			.Subscribe(_ => OnEnd());
	}
}