using UniRx;
using System;
using UnityEngine;
using UnityEngine.Events;


[Serializable]
public class PlayerCharacterAbility : Ability
{
	[SerializeField] public Type type;
	[SerializeField] public float coolTime;
	[SerializeField] public float effectTime;
	[SerializeField] public UnityEvent<Ability> Event;
	public void Setup(PlayerInputAction playerInputAction, Func<PlayerInputAction, bool> func)
	{
		playerInputAction
			.ObserveEveryValueChanged(func)
			.Skip(1)
			.ThrottleFirst(TimeSpan.FromSeconds(effectTime))
			.Subscribe(x => { if (x) Use(); });
	}
	private void Use()
	{
		Event.Invoke(this);
	}
	public void Duration(Action OnStart, Action OnEnd)
	{
		OnStart();
		state.Value = State.Using;
		Observable.Timer(TimeSpan.FromSeconds(effectTime))
			.Subscribe(_ =>
			{
				state.Value = State.Unusable;
				OnEnd();
				Observable.Timer(TimeSpan.FromSeconds(effectTime))
				.Subscribe(_ => state.Value = State.Usable);
			});
	}
}