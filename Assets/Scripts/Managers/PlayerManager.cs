using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
	public List<PlayerCharacter> players = new List<PlayerCharacter>();
	List<GameObject> playerGameObjects = new List<GameObject>();
	void Start()
	{
		InstantiatePlayers();
		ChangeActivePlayer(0);
	}
	void InstantiatePlayers()
	{
		players.ForEach(p => playerGameObjects.Add(Instantiate(p.gameObject, this.transform)));
	}
	void ChangeActivePlayer(int index)
	{
		playerGameObjects.ForEach(p => p.SetActive(false));
		playerGameObjects[index].SetActive(true);
	}
}
