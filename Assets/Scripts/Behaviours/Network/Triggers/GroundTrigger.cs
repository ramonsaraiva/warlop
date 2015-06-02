using UnityEngine;

public class GroundTrigger : NetworkTrigger
{
	private Survivor entity;

	protected override bool ClientAction()
	{
		entity = other.GetComponent<Survivor>();

		if (!entity)
			return false;
		return true;
	}

	protected override void ServerAction()
	{
		entity.EnteredSafeArea();
	}
}
