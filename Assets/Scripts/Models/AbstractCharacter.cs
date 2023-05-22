using UnityEngine;
public abstract class AbstractCharacter : MonoBehaviour
{
	public int health;
	public int stamina;
	[SerializeField] public Walk walk;
	[SerializeField] public Run run;
	[SerializeField] public Jump jump;
	[SerializeField] public Fall fall;
	[System.Serializable] public class Walk : BaseAction { }
	[System.Serializable] public class Run : BaseAction { }
	[System.Serializable] public class Fall : BaseAction { }
	[System.Serializable]
	public class Jump : BaseAction
	{
		public float height;
	}
	public abstract class BaseAction
	{
		public float speed;
	}
}