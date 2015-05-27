using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameSceneBehaviour : MonoBehaviour
{
	public void OnLevelWasLoaded(int level)
	{
		ClientManager.InstantiatePlayers();
	}
}
