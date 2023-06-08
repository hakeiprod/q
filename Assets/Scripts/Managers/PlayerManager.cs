using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

// [RequireComponent(typeof(PlayerInputAction))]
public class PlayerManager : MonoBehaviour
{
	public PlayerInputAction playerInputAction;
	public CinemachineFreeLook cinemachineFreeLook;
	public List<AbstractPlayerCharacter> players = new List<AbstractPlayerCharacter>();
	List<AbstractPlayerCharacter> playerInstances = new List<AbstractPlayerCharacter>();
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
	public AbstractPlayerCharacter GetActivePlayerInstance()
	{
		return playerInstances[activePlayerIndex];
	}
	AbstractPlayerCharacter SetActivePlayerInstance(int index)
	{
		activePlayerIndex = index;
		playerInstances.ForEach(p => p.gameObject.SetActive(false));
		playerInstances[index].gameObject.SetActive(true);
		return GetActivePlayerInstance();
	}
	void InheritPlayerTransform(AbstractPlayerCharacter prevActivePlayer, AbstractPlayerCharacter nextActivePlayer)
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
