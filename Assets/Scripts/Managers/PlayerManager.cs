using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerManager : MonoBehaviour
{
	public PlayerController playerController;
	public CinemachineFreeLook cinemachineFreeLook;
	public List<PlayerCharacter> players = new List<PlayerCharacter>();
	List<PlayerCharacter> playerInstances = new List<PlayerCharacter>();
	[System.NonSerialized] int activePlayerIndex = 0;
	void Start()
	{
		InstantiatePlayers();
		ChangeActivePlayer(activePlayerIndex);
	}
	public void ChangeActivePlayer(int index)
	{
		InheritPlayerTransform(GetActivePlayerInstance(), SetActivePlayerInstance(index));
		FollowedCamera();
	}
	public PlayerCharacter GetActivePlayerInstance()
	{
		return playerInstances[activePlayerIndex];
	}
	PlayerCharacter SetActivePlayerInstance(int index)
	{
		activePlayerIndex = index;
		playerInstances.ForEach(p => p.gameObject.SetActive(false));
		playerInstances[index].gameObject.SetActive(true);
		return GetActivePlayerInstance();
	}
	void InheritPlayerTransform(PlayerCharacter prevActivePlayer, PlayerCharacter nextActivePlayer)
	{
		nextActivePlayer.transform.position = prevActivePlayer.transform.position;
		nextActivePlayer.transform.rotation = prevActivePlayer.transform.rotation;
	}
	void InstantiatePlayers()
	{
		players.ForEach(p => playerInstances.Add(Instantiate(p, this.transform)));
	}
	void FollowedCamera()
	{
		cinemachineFreeLook.Follow = GetActivePlayerInstance().transform;
		cinemachineFreeLook.LookAt = GetActivePlayerInstance().transform.Find("Skeleton").Find("Hips").Find("Spine").Find("Chest").Find("UpperChest").Find("Neck");
	}
}
