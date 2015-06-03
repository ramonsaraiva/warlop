using UnityEngine;

public class SpellBehaviour : MonoBehaviour
{
	protected Survivor entity;

	protected bool channeled;
	protected bool animated;

	protected float rate;
	protected float damage;
	protected float channeling;

	private float channelingStopTime;
	private bool entityFrozen;

	private bool inUse;
	private float lastUse;

	protected virtual void Awake()
	{
		entity = GetComponent<Survivor>();
		lastUse = Time.time - rate;
	}

	public virtual void Use()
	{
		bool inCooldown = Time.time <= lastUse + rate;

		if (inCooldown)
			return;

		inUse = true;

		lastUse = Time.time;

		if (channeled)
		{
			entity.Channeling = true;
			channelingStopTime = Time.time + channeling;
		}

		if (animated)
		{
			Animate();
		}

		if (!channeled)
		{
			Action();
			Finish();
		}
	}
	
	protected virtual void Update()
	{
		if (!inUse)
			return;

		if (channeled && entity.Channeling && Time.time > channelingStopTime)
		{
			entity.Channeling = false;
			Action();
			Finish();
		}
	}

	protected virtual void FixedUpdate()
	{
		if (!inUse)
			return;

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

	private void Finish()
	{
		inUse = false;
		entityFrozen = false;
	}
}
