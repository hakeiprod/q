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
	public float RotationSmoothTime = 0.12f;
	private float _rotationVelocity;
	[System.NonSerialized] public Vector2 move;
	[System.NonSerialized] public bool jump;
	[System.NonSerialized] public bool run;
	[System.NonSerialized] public bool firstSkill;
	[System.NonSerialized] public bool secondSkill;
	[System.NonSerialized] public bool thirdSkill;

	protected virtual void Awake()
	{
		currentStatus = defaultStatus;
	}
	public bool IsIdling() { return move == Vector2.zero; }
	public bool IsJampable() { return jump && characterController.isGrounded; }
	public void Move(float speed, float y)
	{
		var targetRotation = 0f;
		if (!IsIdling())
		{
			targetRotation = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
			float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _rotationVelocity, RotationSmoothTime);
			transform.rotation = Quaternion.Euler(0f, rotation, 0f);
		}
		var targetDirection = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
		characterController.Move(targetDirection.normalized * (speed * Time.deltaTime) + new Vector3(0f, y, 0f) * Time.deltaTime);
	}
}