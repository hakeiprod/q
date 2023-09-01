using UniRx;
using System;
using UnityEngine;
using UnityEngine.Events;

public abstract partial class AbstractPlayerCharacter
{
	void SetAbilities()
	{
		abilities[0].Setup(playerInputAction, (x) => x.ability0);
		// abilities[1].Setup(playerInputAction, (x) => x.ability1);
		// abilities[2].Setup(playerInputAction, (x) => x.ability2);
	}
	[Serializable]
	public class Ability
	{
		public enum Type
		{
			Duration,
			Action
		}
		public enum State
		{
			Usable,
			Using,
			Unusable,
		}
		[SerializeField] public Type type;
		[SerializeField] public float coolTime;
		[SerializeField] public float effectTime;
		[SerializeField] public UnityEvent<Ability> Event;
		[NonSerialized] public State state = State.Usable;
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
			state = State.Using;
			Observable.Timer(TimeSpan.FromSeconds(effectTime))
				.Subscribe(_ =>
				{
					state = State.Unusable;
					OnEnd();
					Observable.Timer(TimeSpan.FromSeconds(effectTime))
					.Subscribe(_ => state = State.Usable);
				});
		}
	}
}