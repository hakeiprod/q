using System;
using System.Collections.Generic;
using Cinemachine;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(PlayerInputAction))]
public class PlayerManager : MonoBehaviour
{
	public PlayerInputAction playerInputAction;
	public CinemachineFreeLook cinemachineFreeLook;
	[SerializeReference]
	public List<AbstractPlayerCharacter> players = new();
	readonly List<AbstractPlayerCharacter> playerInstances = new();
	[NonSerialized] int activePlayerIndex = 0;
	[NonSerialized] public ReactiveProperty<AbstractPlayerCharacter> activePlayer = new();
	protected void Awake()
	{
		InstantiatePlayers();
		ChangeActivePlayer(activePlayerIndex);
		activePlayer.Pairwise().Subscribe(p => InheritPlayerTransform(p.Previous, p.Current));
	}
	public void ChangeActivePlayer(int index)
	{
		SetActivePlayerInstance(index);
		FollowedCamera();
	}
	void SetActivePlayerInstance(int index)
	{
		playerInstances.ForEach(p => p.gameObject.SetActive(false));
		activePlayer.Value = playerInstances[index];
		activePlayer.Value.gameObject.SetActive(true);
	}
	void InheritPlayerTransform(AbstractPlayerCharacter prevActivePlayer, AbstractPlayerCharacter nextActivePlayer)
	{
		nextActivePlayer.transform.position = prevActivePlayer.transform.position;
		nextActivePlayer.transform.rotation = prevActivePlayer.transform.rotation;
	}
	void InstantiatePlayers()
	{
		players.ForEach(p => playerInstances.Add(p.Instantiate(transform, playerInputAction)));
	}
	void FollowedCamera()
	{
		cinemachineFreeLook.Follow = activePlayer.Value.transform;
		cinemachineFreeLook.LookAt = activePlayer.Value.transform.Find("PlayerCameraRoot");
	}
}
