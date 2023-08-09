using UniRx;
using System;
using UnityEngine;

public abstract partial class AbstractPlayerCharacter
{
	[Serializable]
	public class Ability
	{
		public enum Type
		{
			Duration,
			Action
		}
		public enum Status
		{
			Usable,
			Using,
			Unusable,
		}
		[SerializeField] public Type type;
		[SerializeField] readonly public Status status;
		[SerializeField] public float coolTime;
		[SerializeField] public float effectTime;
		[SerializeField] public event Action Action;
		readonly bool input;
		public void Use()
		{
			Action.Invoke();
		}
		void Duration(Action OnStart, Action OnEnd)
		{
			OnStart();
			this.ObserveEveryValueChanged(x => x.input)
				.ThrottleFirst(TimeSpan.FromSeconds(effectTime))
				.Subscribe(_ => OnEnd());
		}
	}
}