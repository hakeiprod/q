using System.Collections.Generic;
using IceMilkTea.Core;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public abstract class AbstractCharacterController<T> : MonoBehaviour
{
	public CharacterController characterController;
	public Animator animator;
	public ImtStateMachine<T> stateMachine;
	public Dictionary<string, int> State = new Dictionary<string, int>()
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
	public float speed;
	public float jumpingHeight;
	public float jumpingSpeed;
	public float gravity;
	[System.NonSerialized] public Vector2 move;
	[System.NonSerialized] public bool jump;
	protected virtual void Awake() { }
	protected virtual void Start() { }
	public class IdleState : ImtStateMachine<T>.State
	{
		protected override void Enter() { }
		protected override void Update() { }
	}
	public class WalkState : ImtStateMachine<T>.State
	{
		protected override void Enter() { }
		protected override void Update() { }
	}
	public class RunState : ImtStateMachine<T>.State
	{
		protected override void Enter() { }
		protected override void Update() { }
	}
	public class JumpState : ImtStateMachine<T>.State
	{
		protected override void Update() { }
	}
	public class FallState : ImtStateMachine<T>.State
	{
		protected override void Update() { }
	}
	public class LandState : ImtStateMachine<T>.State
	{
		protected override void Update() { }
	}
}