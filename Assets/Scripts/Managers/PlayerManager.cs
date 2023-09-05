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
	public List<AbstractPlayerCharacter> players = new List<AbstractPlayerCharacter>();
	readonly List<AbstractPlayerCharacter> playerInstances = new List<AbstractPlayerCharacter>();
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
	AbstractPlayerCharacter SetActivePlayerInstance(int index)
	{
		playerInstances.ForEach(p => p.gameObject.SetActive(false));
		activePlayer.Value = playerInstances[index];
		activePlayer.Value.gameObject.SetActive(true);
		return activePlayer.Value;
	}
	void InheritPlayerTransform(AbstractPlayerCharacter prevActivePlayer, AbstractPlayerCharacter nextActivePlayer)
	{
		nextActivePlayer.transform.position = prevActivePlayer.transform.position;
		nextActivePlayer.transform.rotation = prevActivePlayer.transform.rotation;
	}
	void InstantiatePlayers()
	{
		players.ForEach(p =>
		{
			var instance = Instantiate(p, transform);
			instance.playerInputAction = playerInputAction;
			playerInstances.Add(instance);
		});
	}
	void FollowedCamera()
	{
		cinemachineFreeLook.Follow = activePlayer.Value.transform;
		cinemachineFreeLook.LookAt = activePlayer.Value.transform.Find("PlayerCameraRoot");
	}
}
