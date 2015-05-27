using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
	public enum InputActions
	{
		Shoot = 0,
		Sprint = 1,
	}

	private int networkIdentity;
	private int identity;

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

	protected virtual void Awake()
	{
	}
}
