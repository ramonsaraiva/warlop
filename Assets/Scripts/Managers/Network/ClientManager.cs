using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections.Generic;
using System.Linq;

public class ClientInfo
{
    private int networkIdentity;
    private Entity entity;
	private bool[] lastActions;
	private string nickname;
	private Vector3 startPosition;

	public ClientInfo()
	{
		lastActions = new bool[DataManager.DataConstants.Actions.Length];
	}

    public int NetworkIdentity
    {
        get { return networkIdentity; }
        set { networkIdentity = value; }
    }

    public Entity Entity
    {
        get { return entity; }
        set { entity = value; }
    }

	public bool[] LastActions
	{
		get { return lastActions; }
	}

	public string Nickname
	{
		get { return nickname; }
		set { nickname = value; }
	}

	public Vector3 StartPosition
	{
		get { return startPosition; }
		set { startPosition = value; }
	}
}

public class ClientManager
{
    private static NetworkClient client;
    private static int networkIdentity;
	private static string nickname;

    private static Dictionary<int, ClientInfo> clientList;
    private static ClientInfo localClient;

    public static Dictionary<int, ClientInfo> ClientList
    {
        get { return clientList; }
    }

    public static void Connect(string address, int port, string nickname)
    {
        clientList = new Dictionary<int, ClientInfo>();

        client = new NetworkClient();
        RegisterHandlers();
		/*
		if (ServerManager.IsServer)
			client.ConnectWithSimulator(address, port, 120, 0);
		else
		*/
		client.Connect(address, port);
		ClientManager.nickname = nickname;
	}

	public static void InstantiatePlayers()
	{
		for (int i = 0; i < clientList.Count; i++)
		{
			KeyValuePair<int, ClientInfo> client = clientList.ElementAt(i);

			GameObject playerObject = GameObject.Instantiate(DataManager.DataConstants.PlayerPrefab, client.Value.StartPosition, Quaternion.identity) as GameObject;
			client.Value.Entity = playerObject.GetComponent<Entity>();
			client.Value.Entity.NetworkIdentity = client.Key;
			client.Value.Entity.Nickname = client.Value.Nickname;

			if (client.Key == networkIdentity)
			{
				playerObject.name = "Me";
				playerObject.AddComponent<PlayerBehaviour>();
			}
		}
	}

    public static void SendInput(Vector3 direction)
    {
        client.SendByChannel((short) PacketTypes.PlayerInput, new Vector3Packet(direction), Channels.DefaultUnreliable);
    }

	public static void SendPosition(Vector3 position)
	{
		client.SendByChannel((short) PacketTypes.PlayerPosition, new Vector3Packet(position), Channels.DefaultUnreliable);
	}

	public static void SendPlayerRotation(float angle, Vector3 lookingDirection)
	{
		client.SendByChannel((short) PacketTypes.PlayerRotation, new PlayerRotationPacket(angle, lookingDirection), Channels.DefaultUnreliable);
	}

	public static void SendActions(short actions)
	{
		client.SendByChannel((short) PacketTypes.PlayerActions, new ShortPacket(actions), Channels.DefaultReliable);
	}

    private static void RegisterHandlers()
    {
        client.RegisterHandler((short) MsgType.Connect, NewConnection);
        client.RegisterHandler((short) PacketTypes.PlayersHandshake, PlayersHandshake);
        client.RegisterHandler((short) PacketTypes.PlayerConnected, PlayerConnected);
        client.RegisterHandler((short) PacketTypes.StartGame, StartGame);
        client.RegisterHandler((short) PacketTypes.EndGame, EndGame);
        client.RegisterHandler((short) PacketTypes.PlayerInput, PlayerInput);
        client.RegisterHandler((short) PacketTypes.PlayerPosition, PlayerPosition);
        client.RegisterHandler((short) PacketTypes.PlayerRotation, PlayerRotation);
		client.RegisterHandler((short) PacketTypes.PlayerActions, PlayerActions);
		client.RegisterHandler((short) PacketTypes.PlayerDamage, PlayerDamage);
		client.RegisterHandler((short) PacketTypes.PlayerDamageWithForce, PlayerDamageWithForce);
    }

    private static void NewConnection(NetworkMessage netMsg)
    {
        Debug.Log("[CLIENT] Connected to sever");
		client.Send((short)PacketTypes.NewConnection, new StringPacket(nickname));
    }

    private static void PlayersHandshake(NetworkMessage netMsg)
    {
        HandshakePacket p = netMsg.ReadMessage<HandshakePacket>();
        networkIdentity = p.networkIdentity;

        for (int i = 0; i < p.players; i++)
        {
            ClientInfo clientInfo = new ClientInfo();
            clientInfo.NetworkIdentity = p.playersIdentities[i];
			clientInfo.Nickname = p.playersNicknames[i];
            clientList.Add(clientInfo.NetworkIdentity, clientInfo);
        }
    }

    private static void PlayerConnected(NetworkMessage netMsg)
    {
        ClientInfo clientInfo = new ClientInfo();
		IdentifiedStringPacket p = netMsg.ReadMessage<IdentifiedStringPacket>();
		clientInfo.NetworkIdentity = p.networkIdentity;
		clientInfo.Nickname = p.value;

		if (clientInfo.NetworkIdentity == networkIdentity)
			localClient = clientInfo;

        clientList.Add(clientInfo.NetworkIdentity, clientInfo);
    }

    private static void StartGame(NetworkMessage netMsg)
    {
        InitPacket p = netMsg.ReadMessage<InitPacket>();

		Application.LoadLevel(1);

        for (int i = 0; i < clientList.Count; i++)
			clientList[p.identities[i]].StartPosition = p.positions[i];
    }

	private static void EndGame(NetworkMessage netMsg)
	{
		for (int i = 0; i < clientList.Count; i++)
			GameObject.Destroy(clientList.ElementAt(i).Value.Entity.gameObject);
	}

    private static void PlayerInput(NetworkMessage netMsg)
    {
        IdentifiedVector3Packet p = netMsg.ReadMessage<IdentifiedVector3Packet>();
        if (clientList.ContainsKey(p.networkIdentity))
            ((Living) clientList[p.networkIdentity].Entity).UpdateDirection(p.value);
    }

    private static void PlayerPosition(NetworkMessage netMsg)
    {
        IdentifiedVector3Packet p = netMsg.ReadMessage<IdentifiedVector3Packet>();
		Living entity = (Living) clientList[p.networkIdentity].Entity;

		bool desync = p.networkIdentity == networkIdentity && Vector3.Distance(p.value, entity.transform.position) > 1.0f;

		if ((clientList.ContainsKey(p.networkIdentity) && p.networkIdentity != networkIdentity))// || desync)
			entity.NetworkCorrection.NewServerPosition(p.value);
    }

    private static void PlayerRotation(NetworkMessage netMsg)
    {
		IdentifiedPlayerRotationPacket p = netMsg.ReadMessage<IdentifiedPlayerRotationPacket>();
		Survivor entity = (Survivor) clientList[p.networkIdentity].Entity;

		if ((clientList.ContainsKey(p.networkIdentity)))
		{
			entity.UpdateRotation(p.angle, 90);
			entity.LookingDirection = p.lookingDirection;
		}
    }

	private static void PlayerActions(NetworkMessage netMsg)
	{
		IdentifiedShortPacket p = netMsg.ReadMessage<IdentifiedShortPacket>();

		for (int i = 0; i < DataManager.DataConstants.Actions.Length; i++)
		{
			bool action = (p.value & (1 << i)) > 0 ? true : false;

			if (action != clientList[p.networkIdentity].LastActions[i])
			{
				clientList[p.networkIdentity].LastActions[i] = action;
				((Living) clientList[p.networkIdentity].Entity).SetAction((Entity.InputActions) i, action);
			}
		}
	}

	private static void PlayerDamage(NetworkMessage netMsg)
	{
		DamagePacket p = netMsg.ReadMessage<DamagePacket>();
		Survivor from = (Survivor) clientList[p.fromNetworkIdentity].Entity;
		Survivor to = (Survivor) clientList[p.toNetworkIdentity].Entity;

		if (to.ReceivedDamage(p.damage))
		{
			from.Score += 1;
			EventManager.AddInfoEvent(from.Nickname + " killed " + to.Nickname);
			to.LastDamager = to;
			return;
		}

		to.LastDamager = from;
	}

	private static void PlayerDamageWithForce(NetworkMessage netMsg)
	{
		DamageWithForcePacket p = netMsg.ReadMessage<DamageWithForcePacket>();
		Survivor from = (Survivor) clientList[p.fromNetworkIdentity].Entity;
		Survivor to = (Survivor) clientList[p.toNetworkIdentity].Entity;

		if (to.ReceivedDamageWithForce(p.damage, p.force))
		{
			from.Score += 1;
			EventManager.AddInfoEvent(from.Nickname + " killed " + to.Nickname);
			to.LastDamager = to;
			return;
		}

		to.LastDamager = from;
	}
}