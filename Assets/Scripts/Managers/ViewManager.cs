using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class ViewManager : MonoBehaviour
{
	public GameManager gameManager;
	public UIDocument uiDocument;
	public Toggle ability;

	void Start()
	{
		var root = uiDocument.rootVisualElement;
		ability = root.Query<Toggle>("Ability").First();
		gameManager
			.playerManager
			.GetActivePlayerInstance()
			.ObserveEveryValueChanged(x => x.abilities[0].state)
			.Subscribe(x =>
			{
				switch (x)
				{
					case AbstractPlayerCharacter.Ability.State.Usable:
						ability.value = false;
						break;
					case AbstractPlayerCharacter.Ability.State.Using:
						ability.value = true;
						break;
					case AbstractPlayerCharacter.Ability.State.Unusable:
						ability.value = false;
						break;
				}
			});
	}

}