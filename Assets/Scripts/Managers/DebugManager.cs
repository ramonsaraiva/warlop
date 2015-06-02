using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
	[SerializeField]
	private Text playerPosition;
	[SerializeField]
	private Text playerDirection;
	[SerializeField]
	private Text playerAngle;
	[SerializeField]
	private Text playerSpeed;
	[SerializeField]
	private Text playerHealth;
	[SerializeField]
	private Text flying;
	[SerializeField]
	private Text cooldown;

	private GameObject me;
	private PlayerBehaviour playerBehaviour;
	private Survivor entity;

	private void Start()
	{
		me = GameObject.Find("Me");
		playerBehaviour = me.GetComponent<PlayerBehaviour>();
		entity = me.GetComponent<Survivor>();
	}

	void Update()
	{
		playerPosition.text = "Position: " + me.transform.position;
		playerDirection.text = "Direction: " + entity.MovementDirection;
		playerAngle.text = "Angle: " + entity.Angle;
		playerSpeed.text = "Speed: " + entity.Speed;
		playerHealth.text = "Health: " + entity.Hp;
		flying.text = "Flying: " + entity.Flying;
		cooldown.text = "Cooldown: " + entity.Cooldown;
	}
}
