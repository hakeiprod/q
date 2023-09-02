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
	[System.NonSerialized] int activePlayerIndex = 0;
	public ReactiveProperty<AbstractPlayerCharacter> activePlayer = new();
	protected void Awake()
	{
		InstantiatePlayers();
		activePlayer.Value = playerInstances[activePlayerIndex];
		ChangeActivePlayer(activePlayerIndex);
	}
	public void ChangeActivePlayer(int index)
	{
		InheritPlayerTransform(activePlayer.Value, SetActivePlayerInstance(index));
		FollowedCamera();
	}
	AbstractPlayerCharacter SetActivePlayerInstance(int index)
	{
		activePlayerIndex = index;
		activePlayer.Value = playerInstances[activePlayerIndex];
		playerInstances.ForEach(p => p.gameObject.SetActive(false));
		playerInstances[index].gameObject.SetActive(true);
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
