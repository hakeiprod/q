using System.Collections.Generic;
using IceMilkTea.Core;
using UniRx;
using UnityEngine;

public abstract partial class AbstractCharacter : MonoBehaviour, ICharacter<AbstractCharacter>
{
	public Animator animator;
	public CharacterController characterController;
	public Dictionary<string, int> state { get; set; } = new()
	{
		{ "idle", 0 },
		{ "walk", 1 },
		{ "run", 2 },
		{ "crouch", 3 },
		{ "jump", 4 },
		{ "fall", 5 },
		{ "land", 6 },
		{ "injure", 7 },
		{ "die", 8 },
	};
	[SerializeField] public Status defaultStatus;
	public ReactiveProperty<Status> currentStatus = new();
	protected virtual void Awake()
	{
		currentStatus.Value = defaultStatus;
	}
}