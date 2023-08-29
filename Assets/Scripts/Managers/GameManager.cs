using UnityEngine;

public class GameManager : MonoBehaviour
{
	public PlayerManager playerManager;
	public ViewManager viewManager;
	public enum State
	{
		Watch,
		Play
	}
}
