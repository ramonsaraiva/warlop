using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections.Generic;
using System.Linq;

#region Packets
public enum PacketTypes : short
{
	NewConnection = MsgType.Highest + 1,
    LostConnection,
    PlayersHandshake,
    PlayerConnected,
	PlayerDisconnected,
    StartGame,
	EndGame,
    PlayerInput,
	PlayerPosition,
	PlayerRotation,
	PlayerActions,
	PlayerDamage,
	PlayerDamageWithForce,
	PlayerEnteredSafeArea,
	PlayerLeftSafeArea,
}

public class HandshakePacket : MessageBase
{
    public int networkIdentity;
    public int players;
    public int[] playersIdentities;
	public string[] playersNicknames;

    public override void Deserialize(NetworkReader reader)
    {
        networkIdentity = reader.ReadInt32();
        players = reader.ReadInt32();
        playersIdentities = new int[players];
		playersNicknames = new string[players];

		for (int i = 0; i < players; i++)
		{
			playersIdentities[i] = reader.ReadInt32();
			playersNicknames[i] = reader.ReadString();
		}
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(networkIdentity);
        writer.Write(players);

		for (int i = 0; i < players; i++)
		{
			writer.Write(playersIdentities[i]);
			writer.Write(playersNicknames[i]);
		}
    }
}

public class InitPacket : MessageBase
{
    public int[] identities;
    public Vector3[] positions;

    public override void Deserialize(NetworkReader reader)
    {
        identities = new int[ClientManager.ClientList.Count];
        positions = new Vector3[ClientManager.ClientList.Count];

        for (int i = 0; i < ClientManager.ClientList.Count; i++)
        {
            identities[i] = reader.ReadInt32();
            positions[i] = reader.ReadVector3();
        }
    }

    public override void Serialize(NetworkWriter writer)
    {
        for (int i = 0; i < ClientManager.ClientList.Count; i++)
        {
            writer.Write(identities[i]);
            writer.Write(positions[i]);
        }
    }
}

public class IdentifiedPacket : MessageBase
{
    public int networkIdentity;

    public override void Deserialize(NetworkReader reader)
    {
        networkIdentity = reader.ReadInt32();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(networkIdentity);
    }
    
    public IdentifiedPacket(int networkIdentity)
    {
        this.networkIdentity = networkIdentity;
    }

    public IdentifiedPacket() { }
}

public class RelationPacket : MessageBase
{
	public int fromNetworkIdentity;
	public int toNetworkIdentity;

	public override void Deserialize(NetworkReader reader)
	{
		fromNetworkIdentity = reader.ReadInt32();
		toNetworkIdentity = reader.ReadInt32();
	}

	public override void Serialize(NetworkWriter writer)
	{
		writer.Write(fromNetworkIdentity);
		writer.Write(toNetworkIdentity);
	}

	public RelationPacket(int fromNetworkIdentity, int toNetworkIdentity)
	{
		this.fromNetworkIdentity = fromNetworkIdentity;
		this.toNetworkIdentity = toNetworkIdentity;
	}

	public RelationPacket() { }
}

public class StringPacket : MessageBase
{
	public string value;

	public override void Deserialize(NetworkReader reader)
	{
		value = reader.ReadString();
	}

	public override void Serialize(NetworkWriter writer)
	{
		writer.Write(value);
	}

	public StringPacket(string value)
	{
		this.value = value;
	}

	public StringPacket() { }
}

public class IdentifiedStringPacket : IdentifiedPacket
{
	public string value;

	public override void Deserialize(NetworkReader reader)
	{
		base.Deserialize(reader);
		value = reader.ReadString();
	}

	public override void Serialize(NetworkWriter writer)
	{
		base.Serialize(writer);
		writer.Write(value);
	}

	public IdentifiedStringPacket(int networkIdentity, string value) : base(networkIdentity)
	{
		this.value = value;
	}

	public IdentifiedStringPacket() { }
}

public class Vector3Packet : MessageBase
{
    public Vector3 value;

    public override void Deserialize(NetworkReader reader)
    {
        value = reader.ReadVector3();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(value);
    }

    public Vector3Packet(Vector3 value)
    {
        this.value = value;
    }

    public Vector3Packet() { }
}

public class IdentifiedVector3Packet : IdentifiedPacket
{
    public Vector3 value;

    public override void Deserialize(NetworkReader reader)
    {
        base.Deserialize(reader);
        value = reader.ReadVector3();
    }

    public override void Serialize(NetworkWriter writer)
    {
        base.Serialize(writer);
        writer.Write(value);
    }

    public IdentifiedVector3Packet(int networkIdentity, Vector3 value) : base(networkIdentity)
    {
        this.value = value;
    }

    public IdentifiedVector3Packet() { }
}

public class ShortPacket : MessageBase
{
    public short value;

    public override void Deserialize(NetworkReader reader)
    {
        value = reader.ReadInt16();
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(value);
    }

    public ShortPacket(short value)
    {
        this.value = value;
    }

    public ShortPacket() { }
}

public class IdentifiedShortPacket : IdentifiedPacket
{
	public short value;

    public override void Deserialize(NetworkReader reader)
    {
        base.Deserialize(reader);
        value = reader.ReadInt16();
    }

    public override void Serialize(NetworkWriter writer)
    {
        base.Serialize(writer);
        writer.Write(value);
    }

    public IdentifiedShortPacket(int networkIdentity, short value) : base(networkIdentity)
    {
        this.value = value;
    }

    public IdentifiedShortPacket() { }
}

public class PlayerRotationPacket : MessageBase
{
	public float angle;
	public Vector3 lookingDirection;

	public override void Deserialize(NetworkReader reader)
	{
		angle = reader.ReadSingle();
		lookingDirection = reader.ReadVector3();
	}

	public override void Serialize(NetworkWriter writer)
	{
		writer.Write(angle);
		writer.Write(lookingDirection);
	}

	public PlayerRotationPacket(float angle, Vector3 lookingDirection)
	{
		this.angle = angle;
		this.lookingDirection = lookingDirection;
	}

	public PlayerRotationPacket() { }
}

public class IdentifiedPlayerRotationPacket : PlayerRotationPacket
{
	public int networkIdentity;

	public override void Deserialize(NetworkReader reader)
	{
		networkIdentity = reader.ReadInt32();
		base.Deserialize(reader);
	}

	public override void Serialize(NetworkWriter writer)
	{
		writer.Write(networkIdentity);
		base.Serialize(writer);
	}

	public IdentifiedPlayerRotationPacket(int networkIdentity, float angle, Vector3 lookingDirection) : base(angle, lookingDirection)
	{
		this.networkIdentity = networkIdentity;
	}

	public IdentifiedPlayerRotationPacket() { }
}

public class DamagePacket : RelationPacket
{
	public float damage;

	public override void Deserialize(NetworkReader reader)
	{
		base.Deserialize(reader);
		damage = reader.ReadSingle();
	}

	public override void Serialize(NetworkWriter writer)
	{
		base.Serialize(writer);
		writer.Write(damage);
	}

	public DamagePacket(int fromNetworkIdentity, int toNetworkIdentity, float damage) : base(fromNetworkIdentity, toNetworkIdentity)
	{
		this.damage = damage;
	}

	public DamagePacket() { }
}

public class DamageWithForcePacket : DamagePacket
{
	public Vector3 force;

	public override void Deserialize(NetworkReader reader)
	{
		base.Deserialize(reader);
		force = reader.ReadVector3();
	}

	public override void Serialize(NetworkWriter writer)
	{
		base.Serialize(writer);
		writer.Write(force);
	}

	public DamageWithForcePacket(int fromNetworkIdentity, int toNetworkIdentity, float damage, Vector3 force) : base(fromNetworkIdentity, toNetworkIdentity, damage)
	{
		this.force = force;
	}

	public DamageWithForcePacket() { }
}
#endregion Packets

public class ServerManager
{
	/* TODO: move the nickname to somewhere else */
    public static void Open (int port, string nickname)
    {
        NetworkServer.Listen(port);
        RegisterHandlers();

        Debug.Log("[SERVER] Opened on port " + port);

        ClientManager.Connect("127.0.0.1", port, nickname);
    }

    public static void StartGame()
    {
		if (!NetworkServer.active)
			return;

        InitPacket p = new InitPacket();
        p.identities = new int[ClientManager.ClientList.Count];
        p.positions = new Vector3[ClientManager.ClientList.Count];

        for (int i = 0; i < ClientManager.ClientList.Count; i++)
        {
            p.identities[i] = ClientManager.ClientList.ElementAt(i).Value.NetworkIdentity;
            p.positions[i] = new Vector3(Random.Range(-4f, 4f), 1f, 0);
        }

        NetworkServer.SendByChannelToAll((short)PacketTypes.StartGame, p, Channels.DefaultReliable);
    }

	public static void EndGame()
	{
		NetworkServer.SendByChannelToAll((short)PacketTypes.EndGame, new EmptyMessage(), Channels.DefaultReliable);
	}
	
	public static bool IsServer()
	{
		return NetworkServer.active;
	}

	public static void ApplyDamage(Survivor from, Survivor to, float damage)
	{
		NetworkServer.SendByChannelToAll((short)PacketTypes.PlayerDamage, new DamagePacket(from.NetworkIdentity, to.NetworkIdentity, damage), Channels.DefaultReliable);
	}

	public static void ApplyDamageWithForce(Survivor from, Survivor to, float damage, Vector3 force)
	{
		NetworkServer.SendByChannelToAll((short)PacketTypes.PlayerDamageWithForce, new DamageWithForcePacket(from.NetworkIdentity, to.NetworkIdentity, damage, force), Channels.DefaultReliable);
	}

    private static void RegisterHandlers()
    {
        NetworkServer.RegisterHandler((short) PacketTypes.NewConnection, NewConnection);
        NetworkServer.RegisterHandler((short) PacketTypes.LostConnection, LostConnection);
        NetworkServer.RegisterHandler((short) PacketTypes.PlayerInput, PlayerInput);
        NetworkServer.RegisterHandler((short) PacketTypes.PlayerPosition, PlayerPosition);
		NetworkServer.RegisterHandler((short) PacketTypes.PlayerRotation, PlayerRotation);
		NetworkServer.RegisterHandler((short) PacketTypes.PlayerActions, PlayerActions);
    }

    private static void NewConnection(NetworkMessage netMsg)
    {
		string nickname = netMsg.ReadMessage<StringMessage>().value;

        HandshakePacket p = new HandshakePacket();
        p.networkIdentity = netMsg.conn.connectionId;
        p.players = ClientManager.ClientList.Count;
        p.playersIdentities = new int[ClientManager.ClientList.Count];
		p.playersNicknames = new string[ClientManager.ClientList.Count];

		for (int i = 0; i < ClientManager.ClientList.Count; i++)
		{
			p.playersIdentities[i] = ClientManager.ClientList.ElementAt(i).Value.NetworkIdentity;
			p.playersNicknames[i] = ClientManager.ClientList.ElementAt(i).Value.Nickname;
		}

        NetworkServer.SendToClient(netMsg.conn.connectionId, (short) PacketTypes.PlayersHandshake, p);
        NetworkServer.SendByChannelToAll((short) PacketTypes.PlayerConnected, new IdentifiedStringPacket(netMsg.conn.connectionId, nickname), Channels.DefaultReliable);
    }

    private static void LostConnection(NetworkMessage netMsg)
    {
		//NetworkServer.SendByChannelToAll((short)PacketTypes.PlayerDisconnected, new IntegerMessage(netMsg.conn.connectionId));
    }

    private static void PlayerInput(NetworkMessage netMsg)
    {
        Vector3 value = netMsg.ReadMessage<Vector3Packet>().value;
        NetworkServer.SendByChannelToAll((short) PacketTypes.PlayerInput, new IdentifiedVector3Packet(netMsg.conn.connectionId, value), Channels.DefaultUnreliable);
    }

    private static void PlayerPosition(NetworkMessage netMsg)
    {
        Vector3 value = netMsg.ReadMessage<Vector3Packet>().value;

		/*
		Vector3 serverPosition = ClientManager.ClientList[netMsg.conn.connectionId].Entity.transform.position;

		float desyncDistance = Vector3.Distance(value, serverPosition);
		if (desyncDistance > 1.0f)
			value = serverPosition;
		*/

        NetworkServer.SendByChannelToAll((short) PacketTypes.PlayerPosition, new IdentifiedVector3Packet(netMsg.conn.connectionId, value), Channels.DefaultUnreliable);
    }

	private static void PlayerRotation(NetworkMessage netMsg)
	{
		PlayerRotationPacket p = netMsg.ReadMessage<PlayerRotationPacket>();
        NetworkServer.SendByChannelToAll((short) PacketTypes.PlayerRotation, new IdentifiedPlayerRotationPacket(netMsg.conn.connectionId, p.angle, p.lookingDirection), Channels.DefaultUnreliable);
	}

	private static void PlayerActions(NetworkMessage netMsg)
	{
		short value = netMsg.ReadMessage<ShortPacket>().value;
		NetworkServer.SendByChannelToAll((short)PacketTypes.PlayerActions, new IdentifiedShortPacket(netMsg.conn.connectionId, value), Channels.DefaultReliable);
	}

}