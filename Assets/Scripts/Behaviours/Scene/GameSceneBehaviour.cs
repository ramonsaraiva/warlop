using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameSceneBehaviour : MonoBehaviour
{
	[SerializeField]
	private Text infoText;
	[SerializeField]
	private Image strengthBar;

	[SerializeField]
	private GameObject groundPrefab;
	[SerializeField]
	private GameObject lavaPrefab;
	[SerializeField]
	private Transform groundParent;


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
		CreateMap();
	}

	private void CreateMap()
	{
		GameObject ground;

		for (int i = -6; i <= 6; i++)
		{
			for (int j = -6; j <= 6; j++)
			{
				ground = Instantiate(lavaPrefab, new Vector3(i * 3.24f, j * 3.24f, 0), Quaternion.identity) as GameObject;
				ground.transform.parent = groundParent;
			}
		}

		for (int i = -2; i <= 2; i++)
		{
			for (int j = -2; j <= 2; j++)
			{
				ground = Instantiate(groundPrefab, new Vector3(i * 3.3f, j * 3.3f, 0), Quaternion.identity) as GameObject;
				ground.transform.parent = groundParent;
			}
		}
	}
}
