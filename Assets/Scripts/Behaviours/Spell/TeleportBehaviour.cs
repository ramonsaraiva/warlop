using UnityEngine;
using Warlop.Constants;

public class TeleportBehaviour : SpellBehaviour
{
	private float range;

	protected override void Awake()
	{
		base.Awake();

		channeled = false;
		animated = true;

		rate = SpellConstants.TeleportRate;
		range = SpellConstants.TeleportRange;
	}

	protected override void Animate()
	{
		base.Animate();
	}

	protected override void Action()
	{
		base.Action();

		entity.transform.position = entity.transform.position + (entity.LookingDirection * range);
	}
}
