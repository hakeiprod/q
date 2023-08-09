using System.Collections.Generic;
using UnityEngine;

public abstract partial class AbstractCharacter : MonoBehaviour
{
	public Animator animator;
	public CharacterController characterController;
	[SerializeField] public Status defaultStatus;
	[System.NonSerialized] public Status currentStatus;
	public Dictionary<string, int> state = new()
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

	protected virtual void Awake()
	{
		currentStatus = defaultStatus;
	}
}