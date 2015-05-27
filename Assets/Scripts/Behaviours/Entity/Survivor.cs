using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Survivor : Living
{
	#region UnityFields
	[SerializeField]
	private GameObject arrowPrefab;
	[SerializeField]
	private Canvas canvas;
	[SerializeField]
	private Image healthBar;
	[SerializeField]
	private Text scoreText;
	#endregion UnityFields

	private Vector3 lookingDirection;

	private float hp;
	private int score;

	#region Properties
	public Vector3 LookingDirection
	{
		get
		{
			return lookingDirection;
		}

		set
		{
			lookingDirection = value;
			lookingDirection.z = 0;
			lookingDirection.Normalize();
		}
	}

	public float Hp
	{
		get { return hp; }
		set { hp = value; }
	}

	public int Score
	{
		get { return score; }
		set { score = value; }
	}
	#endregion Properties

	#region UnityMethods
	protected override void Awake()
	{
		base.Awake();
		hp = 100f;
	}

	public void Update()
	{
		canvas.transform.rotation = Quaternion.identity;
		healthBar.fillAmount = hp / 100f;
		scoreText.text = score.ToString();
	}
	#endregion UnityMethods

	public override void SetAction(InputActions action, bool value)
	{
		switch (action)
		{
			case InputActions.Shoot:
				if (value)
					Shoot();
				break;
			case InputActions.Sprint:
				//sprint
				break;
		}
	}

	private void Shoot()
	{
		GameObject arrow = Instantiate(arrowPrefab, transform.position, transform.rotation) as GameObject;
		arrow.GetComponent<Rigidbody2D>().velocity = LookingDirection * 10f;
		arrow.GetComponent<ArrowTrigger>().entity = this;
	}
}
