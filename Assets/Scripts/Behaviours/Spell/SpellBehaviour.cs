using UnityEngine;

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

	private bool inUse;
	private float lastUse;

	#region Properties
	#endregion Properties

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
			channelingStopTime = Time.time + channelingTime;
		}

		if (animated)
		{
			Animate();
		}

		if (!channeled)
			Action();
	}
	
	protected virtual void Update()
	{
		if (!inUse)
			return;

		if (channeled && entity.Channeling && Time.time > channelingStopTime)
		{
			entity.Channeling = false;
			Action();
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
}
