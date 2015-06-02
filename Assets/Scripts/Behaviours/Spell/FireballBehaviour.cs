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

		GameObject spell = Instantiate(entity.fireballPrefab, transform.position, transform.rotation) as GameObject;
		Rigidbody2D spellRigidbody = spell.GetComponent<Rigidbody2D>();
		spellRigidbody.velocity = entity.LookingDirection * SpellConstants.FireballForce;
		spellRigidbody.AddTorque(SpellConstants.FireballTorque);
		spell.GetComponent<FireballTrigger>().entity = entity;
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
