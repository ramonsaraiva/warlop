using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameSceneBehaviour : MonoBehaviour
{
	[SerializeField]
	private Text infoText;
	[SerializeField]
	private Image strengthBar;

	private void Update()
	{
		if (EventManager.infos.Count > 0)
			ShowInfo(EventManager.infos.Dequeue());
	}
	
	public void ShowInfo(string info)
	{
		infoText.text = info;
		infoText.enabled = true;

		Invoke("HideInfo", 3f);
	}

	private void HideInfo()
	{
		infoText.enabled = false;
	}

	public void OnLevelWasLoaded(int level)
	{
		ClientManager.InstantiatePlayers();
	}

	public void SetStrength(float strength)
	{
		strengthBar.fillAmount = strength / 10f;
	}
}
