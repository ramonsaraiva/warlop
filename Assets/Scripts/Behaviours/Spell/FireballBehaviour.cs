using UnityEngine;
using Warlop.Constants;

public class FireballBehaviour : SpellBehaviour
{
	private bool use;

	protected override void Awake()
	{
		base.Awake();

		channeled = false;
		animated = false;

		rate = SpellConstants.FireballRate;
		damage = SpellConstants.FireballDamage;
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		if (!use)
			return;

		use = false;

		GameObject fireball = Instantiate(entity.fireballPrefab, transform.position, transform.rotation) as GameObject;
		Rigidbody2D fireballRigidbody = fireball.GetComponent<Rigidbody2D>();
		fireballRigidbody.velocity = entity.LookingDirection * SpellConstants.FireballForce;
		fireballRigidbody.AddTorque(SpellConstants.FireballTorque);
		fireball.GetComponent<FireballTrigger>().entity = entity;
	}

	protected override void Animate()
	{
		base.Animate();
	}

	protected override void Action()
	{
		base.Action();
		use = true;
		Debug.Log("Fireball skill action");
	}
}
