using PixelCrushers.DialogueSystem;
using UnityEngine;

[RequireComponent(typeof(DialogueSystemTrigger))]
public class MotherNonePlayerCharacter : AbstractCharacter
{
	public DialogueSystemTrigger dialogueSystemTrigger;
	// Start is called before the first frame update
	void Start()
	{
		dialogueSystemTrigger.conversationActor = GameManager.Instance.playerManager.GetActivePlayerInstance().transform;
	}

	// Update is called once per frame
	void Update()
	{

	}
}
