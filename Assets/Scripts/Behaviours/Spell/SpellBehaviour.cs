using UnityEngine;
using System.Collections;

public class SpellBehaviour : MonoBehaviour
{
	protected Survivor entity;

	protected float rate;
	protected float damage;

	protected bool channeled;
	protected bool animated;

	private float channelingTime;
	private float channelingStopTime;
	private bool entityFrozen;

	protected virtual void Awake()
	{
	}

	protected virtual void Use()
	{
		if (channeled)
		{
			entity.Channeling = true;
			channelingStopTime = Time.time + channelingTime;
		}

		if (animated)
		{
			Animate();
		}
	}
	
	protected virtual void Update()
	{
		if (channeled && entity.Channeling && Time.time > channelingStopTime)
		{
			entity.Channeling = false;
			Action();
		}
	}

	protected virtual void FixedUpdate()
	{
		if (channeled && !entityFrozen)
		{
			entity.Rigidbody.velocity = Vector3.zero;
			entityFrozen = true;
		}
	}

	protected virtual void Animate()
	{
	}

	protected virtual void Action()
	{
	}
}
