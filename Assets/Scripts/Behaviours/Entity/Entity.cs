using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
	public enum InputActions
	{
		PrimarySkill = 0,
		SecondarySkill = 1,
		QSkill = 2,
		ESkill = 3,
		RSkill = 4,
		TSkill = 5
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
