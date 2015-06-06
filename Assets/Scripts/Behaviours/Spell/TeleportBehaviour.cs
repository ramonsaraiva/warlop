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

        entity.NetworkCorrection.ForciblyDisable();

        Vector3 desiredPosition = entity.MouseWorldPosition;
        if (Vector3.Distance(desiredPosition, entity.transform.position) > range)
            desiredPosition = entity.transform.position + (entity.LookingDirection * range);

		entity.transform.position = desiredPosition;
        entity.NetworkCorrection.ForciblyEnable();
	}
}
