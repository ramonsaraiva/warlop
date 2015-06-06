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

        if (ServerManager.IsServer())
        {
            Vector3 desiredPosition = entity.MouseWorldPosition;
            if (Vector3.Distance(desiredPosition, entity.transform.position) > range)
                desiredPosition = entity.transform.position + (entity.LookingDirection * range);

            ServerManager.Teleport(entity, desiredPosition);
            /*
             * okay, so now to fix everything inside the teleport spell
             * i need to correct (self position) if this is wrong related to the server
             * so when receiving the server position, even if its me, if im not synced
             * i NEED to fix the position...
             */
        }
	}
}
