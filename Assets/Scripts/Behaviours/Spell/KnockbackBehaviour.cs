using UnityEngine;
using Warlop.Constants;

public class KnockbackBehaviour : SpellBehaviour
{
	private bool use;

	protected override void Awake()
	{
		base.Awake();

		channeled = true;
		animated = true;

		rate = SpellConstants.KnockbackRate;
		damage = SpellConstants.KnockbackDamage;
		channeling = SpellConstants.KnockbackChanneling;
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		if (!use)
			return;

		use = false;

		if (!ServerManager.IsServer())
			return;

		Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, SpellConstants.KnockbackRadius, 1 << LayerMask.NameToLayer("Player"));
		foreach (Collider2D coll in hit)
		{
			if (coll.gameObject == gameObject)
				continue;

			float knockbackDistance = Vector3.Distance(transform.position, coll.transform.position);
			Vector3 knockbackForce = (coll.transform.position - transform.position).normalized * SpellConstants.KnockbackForce * (2.5f - knockbackDistance);
			Debug.Log(knockbackDistance);
			Debug.Log("2.4 - ^: " + (2.5 - knockbackDistance));
			ServerManager.ApplyDamageWithForce(entity, coll.GetComponent<Survivor>(), SpellConstants.KnockbackDamage, knockbackForce);
		}
	}

	protected override void Animate()
	{
		base.Animate();

		entity.Animator.SetTrigger("Knockback");
	}

	protected override void Action()
	{
		base.Action();
		use = true;
	}
}
