using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class ViewManager : MonoBehaviour
{
	public GameManager gameManager;
	public UIDocument uiDocument;

	protected virtual void Start()
	{
		var root = uiDocument.rootVisualElement;
		var activePlayer = gameManager.playerManager.activePlayer;
		var ability = root.Query<Toggle>("Ability").First();
		var health = root.Query<ProgressBar>("Health").First();
		activePlayer
			.Subscribe(x =>
			{
				// ability
				x.abilities[0].state.Subscribe(x =>
				{
					switch (x)
					{
						case Ability.State.Usable:
							ability.visible = true;
							ability.value = false;
							break;
						case Ability.State.Using:
							ability.value = true;
							break;
						case Ability.State.Unusable:
							ability.value = false;
							ability.visible = false;
							break;
					}
				});

				// health bar
				// x.currentStatus.Subscribe(x =>
				// {
				// 	health.highValue = x.maxHealth;
				// 	x.health.Subscribe(x => health.lowValue = x);
				// });
			});

	}

}
