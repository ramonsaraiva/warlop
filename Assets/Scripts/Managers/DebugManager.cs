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
	private Text arrowStrength;

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
		arrowStrength.text = "Arrow Strength: " + entity.ArrowStrength;
	}
}
