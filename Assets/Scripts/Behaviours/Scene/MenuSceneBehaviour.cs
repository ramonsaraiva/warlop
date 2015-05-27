using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MenuSceneBehaviour : MonoBehaviour
{
	[SerializeField]
	private DataConstants dataConstants;
	[SerializeField]
	private InputField nicknameInput;
	[SerializeField]
	private InputField portInput;
	[SerializeField]
	private InputField ipInput;
	[SerializeField]
	private Button createServer;
	[SerializeField]
	private Button joinServer;
	[SerializeField]
	private Button startGame;
	[SerializeField]
	private Text lobbyText;

	private bool createdOrJoined;

	void Awake()
	{
		startGame.interactable = false;
		lobbyText.enabled = false;

		DataManager.DataConstants = dataConstants;
	}

	void Update()
	{
		if (!lobbyText.enabled)
			return;

		string s;
		if (NetworkServer.active)
			s = "SERVER OPENED\nPLAYERS IN LOBBY: ";
		else
			s = "CONNECTED TO SERVER.\nPLAYERS IN LOBBY: ";

		s += ClientManager.ClientList.Count;
		lobbyText.text = s;
	}

	public void CreateServer()
	{
		ServerManager.Open(int.Parse(portInput.text));
		joinServer.interactable = false;
		DisableUI();
		lobbyText.enabled = true;
	}

	public void JoinServer()
	{
		ClientManager.Connect(ipInput.text, int.Parse(portInput.text));
		DisableUI();
		lobbyText.enabled = true;
	}

	public void StartGame()
	{
		ServerManager.StartGame();
	}

	private void DisableUI()
	{
		nicknameInput.interactable = false;
		ipInput.interactable = false;
		portInput.interactable = false;
		createServer.interactable = false;
		joinServer.interactable = false;
		startGame.interactable = NetworkServer.active;
	}
}
