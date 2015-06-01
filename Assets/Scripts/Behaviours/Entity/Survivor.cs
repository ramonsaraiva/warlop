using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Survivor : Living
{
	#region UnityFields
	[SerializeField]
	private Animator animator;
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
	private float shotStartTime;
	private float arrowStrength;
	private bool shooting;

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

	public float ArrowStrength
	{
		get { return arrowStrength; }
	}
	#endregion Properties

	#region UnityMethods
	protected override void Awake()
	{
		base.Awake();
		Speed = DataManager.DataConstants.speed;
		hp = 100f;
		lastShot = Time.time - shotRate;
	}

	public void Update()
	{
		canvas.transform.rotation = Quaternion.identity;
		healthBar.fillAmount = hp / 100f;
		scoreText.text = score.ToString();
		nicknameText.text = Nickname;

		if (MovementDirection.x != 0 || MovementDirection.y != 0)
			animator.SetBool("Moving", true);
		else
			animator.SetBool("Moving", false);

		if (Time.time > lastShot + shotRate && shooting)
		{
			float deltaShotTime = Time.time - shotStartTime;
			if (deltaShotTime > 2f)
				deltaShotTime = 2f;
			float multiplier = deltaShotTime / 2f;
			arrowStrength = 10 * multiplier;
		}
		else
		{
			arrowStrength = 0;
		}
	}
	#endregion UnityMethods

	public override void SetAction(InputActions action, bool value)
	{
		switch (action)
		{
			case InputActions.Shoot:
				if (value)
				{
					if (Time.time > lastShot + shotRate)
					{
						shotStartTime = Time.time;
						Speed = DataManager.DataConstants.shootingSpeed;
						shooting = true;
					}
				}
				else
				{
					Shoot();
				}
				break;
			case InputActions.Sprint:
				//sprint
				break;
		}
	}

	private void Shoot()
	{
		bool outOfCooldown = Time.time > lastShot + shotRate;

		if (outOfCooldown)
		{
			lastShot = Time.time;
			GameObject arrow = Instantiate(arrowPrefab, transform.position, transform.rotation) as GameObject;
			arrow.GetComponent<Rigidbody2D>().velocity = LookingDirection * (DataManager.DataConstants.arrowBaseForce + arrowStrength);
			arrow.GetComponent<ArrowTrigger>().entity = this;

			Speed = DataManager.DataConstants.speed;
			shooting = false;
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
