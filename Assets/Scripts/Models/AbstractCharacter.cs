using UnityEngine;

public abstract class AbstractCharacter : MonoBehaviour
{
	[SerializeField] public Status defaultStatus;
	[System.NonSerialized] public Status currentStatus;

	void Awake()
	{
		currentStatus = defaultStatus;
	}
}
[System.Serializable]
public class Status
{
	public int health;
	public int stamina;
	[SerializeField] public Action walk;
	[SerializeField] public Action run;
	[SerializeField] public Jump jump;
	[SerializeField] public Action fall;
	[System.Serializable]
	public class Jump : Action
	{
		public float height;
	}
	[System.Serializable]
	public class Action
	{
		public float speed;
	}
}