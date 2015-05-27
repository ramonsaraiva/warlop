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
	private Text nicknameText;
	[SerializeField]
	private Text scoreText;
	[SerializeField]
	private AudioSource hurtSource;
	#endregion UnityFields

	private Vector3 lookingDirection;

	private float hp;
	private int score;

	private float lastShot;
	private float shotRate = 3f;

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
		lastShot = Time.time - shotRate;
	}

	public void Update()
	{
		canvas.transform.rotation = Quaternion.identity;
		healthBar.fillAmount = hp / 100f;
		scoreText.text = score.ToString();
		nicknameText.text = Nickname;
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
		if (Time.time > lastShot + shotRate)
		{
			lastShot = Time.time;
			GameObject arrow = Instantiate(arrowPrefab, transform.position, transform.rotation) as GameObject;
			arrow.GetComponent<Rigidbody2D>().velocity = LookingDirection * 13f;
			arrow.GetComponent<ArrowTrigger>().entity = this;
		}
	}

	public bool GotHit(float damage)
	{
		hurtSource.Play();
		
		if (damage > hp)
		{
			score -= 1;
			hp = 100;
			return true;
		}

		hp -= damage;
		return false;
	}
}
