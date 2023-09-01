using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class ViewManager : MonoBehaviour
{
	public GameManager gameManager;
	public UIDocument uiDocument;

	void Start()
	{
		var root = uiDocument.rootVisualElement;
		var activePlayer = gameManager.playerManager.GetActivePlayerInstance();

		// ability
		var ability = root.Query<Toggle>("Ability").First();
		activePlayer.ObserveEveryValueChanged(x => x.abilities[0].state)
		.Subscribe(x =>
		{
			switch (x)
			{
				case AbstractPlayerCharacter.Ability.State.Usable:
					ability.visible = true;
					ability.value = false;
					break;
				case AbstractPlayerCharacter.Ability.State.Using:
					ability.value = true;
					break;
				case AbstractPlayerCharacter.Ability.State.Unusable:
					ability.value = false;
					ability.visible = false;
					break;
			}
		});

		// health bar
		var health = root.Query<ProgressBar>("Health").First();
		activePlayer.ObserveEveryValueChanged(x => x.currentStatus).Subscribe(
			(x) =>
			{
				Debug.Log("aaaaa");
				health.highValue = x.maxHealth;
				health.lowValue = x.health;
			}
		);
	}

}