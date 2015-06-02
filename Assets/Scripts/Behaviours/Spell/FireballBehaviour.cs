using UnityEngine;
using Warlop.Constants;

public class FireballBehaviour : SpellBehaviour
{
	protected override void Awake()
	{
		base.Awake();

		channeled = false;
		animated = false;

		rate = SpellConstants.FireballRate;
		damage = SpellConstants.FireballDamage;
	}

	protected override void Animate()
	{
		base.Animate();
	}

	protected override void Action()
	{
		base.Action();


	}
}
