using UnityEngine;
using UnityEngine.Networking;

public class NetworkTrigger : MonoBehaviour
{
	protected Collider2D other;

	public void OnTriggerEnter2D(Collider2D other)
	{
		this.other = other;

		bool keep = ClientAction();

		if (keep && NetworkServer.active)
			ServerAction();
	}

	protected virtual bool ClientAction() { return false; }
	protected virtual void ServerAction() { }
}
