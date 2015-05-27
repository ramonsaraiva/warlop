using UnityEngine;
using System.Collections;

public class ArrowTrigger : NetworkTrigger
{
	public Survivor entity { get; set; }
	private Survivor otherEntity;

	protected override bool ClientAction()
	{
		if (other.attachedRigidbody == null)
		{
			Destroy(gameObject);
			return false;
		}

		otherEntity = other.GetComponent<Survivor>();
		
		if (otherEntity == entity)
			return false;

		Destroy(gameObject);
		return true;
	}

	protected override void ServerAction()
	{
		ServerManager.PlayerShot(entity, otherEntity);
	}
}
