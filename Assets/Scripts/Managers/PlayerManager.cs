using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public PlayerCharacter[] playerCharacters;
	void Start()
	{
		foreach (var playerCharacter in playerCharacters)
			Instantiate(playerCharacter);
	}
}
