using UnityEngine;

public abstract partial class AbstractCharacter
{
	[System.Serializable]
	public class Status
	{
		public int health;
		public int maxHealth;
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
}