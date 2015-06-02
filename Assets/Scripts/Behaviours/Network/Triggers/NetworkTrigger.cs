using UnityEngine;
using UnityEngine.Networking;

public class NetworkTrigger : MonoBehaviour
{
	public enum TriggerTypes
	{
		Enter = 0,
		Exit,
		Stay,
	}

	[SerializeField]
	private TriggerTypes type;

	protected Collider2D other;

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (type != TriggerTypes.Enter)
			return;

		Action(other);
	}

	public void OnTriggerExit2D(Collider2D other)
	{
		if (type != TriggerTypes.Exit)
			return;

		Action(other);
	}

	private void Action(Collider2D other)
	{
		this.other = other;

		bool keep = ClientAction();

		if (keep && NetworkServer.active)
			ServerAction();
	}

	protected virtual bool ClientAction() { return false; }
	protected virtual void ServerAction() { }
}
