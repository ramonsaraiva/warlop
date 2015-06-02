using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
	public enum InputActions
	{
		Fireball = 0,
		Explosion = 1,
	}

	private int networkIdentity;
	private int identity;
	private string nickname;

	public int NetworkIdentity
	{
		get { return networkIdentity; }
		set { networkIdentity = value; }
	}

	public int Identity
	{
		get { return identity; }
		set { identity = value; }
	}

	public string Nickname
	{
		get { return nickname; }
		set { nickname = value; }
	}

	protected virtual void Awake()
	{
	}
}
