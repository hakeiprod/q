using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	// Start is called before the first frame update
	public PlayerManager playerManager;
	public enum State
	{
		Default,
		Battle,
		Timeline,
	}
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
