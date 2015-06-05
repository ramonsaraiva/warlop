using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Warlop.Constants;

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
	[SerializeField]
	private BoxCollider2D platformCollider;

	private GameObject[,] map;

	private float groundSize;
	private float lavaSize;

	private float platformDestroyRate = 2f;
	private float platformLastDestroy;
	private int platformCurrentBorder;

	private void Awake()
	{
		groundSize = 3.3f;
		lavaSize = 3.24f;
		platformCurrentBorder = EnvironmentConstants.PlatformSize;
	}

	private void Start()
	{
		platformLastDestroy = Time.time;
		map = new GameObject[EnvironmentConstants.PlatformSize / 2, EnvironmentConstants.PlatformSize];

		platformCollider.size = new Vector2(EnvironmentConstants.PlatformSize * groundSize, EnvironmentConstants.PlatformSize * groundSize);
	}

	private void Update()
	{
		if (EventManager.infos.Count > 0)
			ShowInfo(EventManager.infos.Dequeue());

		if (Time.time > platformLastDestroy + platformDestroyRate)
		{
			DestroyBorder();
			platformLastDestroy = Time.time;
			platformCurrentBorder--;
		}
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

		int halfMapSize = EnvironmentConstants.MapSize / 2;
		int halfPlatformSize = EnvironmentConstants.PlatformSize / 2;

		for (int i = 0; i < EnvironmentConstants.MapSize; i++)
		{
			for (int j = 0; j < EnvironmentConstants.MapSize; j++)
			{
				ground = Instantiate(lavaPrefab, new Vector3((i - halfMapSize) * lavaSize, (j - halfMapSize) * lavaSize, 0), Quaternion.identity) as GameObject;
				ground.transform.parent = groundParent;
			}
		}

		for (int i = 0; i < EnvironmentConstants.PlatformSize; i++)
		{
			for (int j = 0; j < EnvironmentConstants.PlatformSize; j++)
			{
				ground = Instantiate(groundPrefab, new Vector3((i - halfPlatformSize) * groundSize, (j - halfPlatformSize) * groundSize, 0), Quaternion.identity) as GameObject;
				ground.transform.parent = groundParent;
			}
		}
	}

	private void DestroyBorder()
	{
		/*
		for (int i = map.Length - platformCurrentBorder; i < platformCurrentBorder; i++)
		{
			Destroy(map[i,map.Length - platformCurrentBorder]);
			Destroy(map[map.Length - platformCurrentBorder,i]);
		}
		*/
	}
}
