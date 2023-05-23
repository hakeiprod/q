using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerManager : MonoBehaviour
{
	public PlayerController playerController;
	public List<PlayerCharacter> players = new List<PlayerCharacter>();
	List<PlayerCharacter> playerInstances = new List<PlayerCharacter>();
	[System.NonSerialized] public int activePlayerIndex = 0;
	void Start()
	{
		InstantiatePlayers();
		ChangeActivePlayer(activePlayerIndex);
	}
	public void ChangeActivePlayer(int index)
	{
		playerInstances.ForEach(p => p.gameObject.SetActive(false));
		playerInstances[index].gameObject.SetActive(true);
	}
	public PlayerCharacter GetActivePlayerInstance()
	{
		return playerInstances[activePlayerIndex];
	}
	void InstantiatePlayers()
	{
		players.ForEach(p => playerInstances.Add(Instantiate(p, this.transform)));
	}
}
