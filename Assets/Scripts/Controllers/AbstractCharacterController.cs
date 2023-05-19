using System.Collections.Generic;
using IceMilkTea.Core;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public abstract class AbstractCharacterController : MonoBehaviour
{
	public CharacterController characterController;
	public Animator animator;
	public Dictionary<string, int> State = new Dictionary<string, int>()
	{
		{ "idle", 0 },
		{ "walk", 1 },
		{ "run", 2 },
		{ "crouch", 3 },
		{ "jump", 4 },
		{ "fall", 5 },
		{ "injure", 6 },
		{ "die", 7 },
	};
	public float speed;
	public float jumpingHeight;
	public float jumpingSpeed;
	public float gravity;
	[System.NonSerialized] public Vector2 move;
	[System.NonSerialized] public bool jump;
	protected virtual void Awake() { }
	protected virtual void Start() { }
	public class IdleState<T> : ImtStateMachine<T>.State
	{
		protected override void Update()
		{

		}
	}
	public class WalkState<T> : ImtStateMachine<T>.State
	{
		protected override void Update() { }
	}
	public class JumpState<T> : ImtStateMachine<T>.State
	{
		protected override void Update() { }
	}
	public class FallState<T> : ImtStateMachine<T>.State
	{
		protected override void Update() { }
	}
}