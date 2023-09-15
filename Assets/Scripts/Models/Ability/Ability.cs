using System;
using UniRx;

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
	public ReactiveProperty<State> state { get; set; } = new(State.Usable);
}