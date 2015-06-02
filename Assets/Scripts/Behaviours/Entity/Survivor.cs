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
	private Survivor lastDamager;

	private bool safe;
	private float unsafeCounter;
	private float unsafeRate = 1f;
	private float unsafeDamage = 10f;

	private float lastShot;
	private float shotRate = 3f;
	float cooldown;
	bool shootFireball;

	bool applyForce;
	Vector3 force;
	float stopFlyingTime;
	float flyTime = 2f;


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

	public Survivor LastDamager
	{
		get { return lastDamager; }
		set { lastDamager = value; }
	}

	public float Cooldown
	{
		get { return cooldown; }
	}

	#endregion Properties

	#region UnityMethods
	protected override void Awake()
	{
		base.Awake();

		Speed = DataManager.DataConstants.speed;
		hp = 100f;
		lastShot = Time.time - shotRate;
		lastDamager = this;
		safe = true;
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

		if (Flying && Time.time > stopFlyingTime)
			Flying = false;

		if (Time.time - lastShot <= 3f)
			cooldown = -(Time.time - lastShot - 3);
		else
			cooldown = 3f;

		if (!safe && Time.time > unsafeCounter + unsafeRate)
		{
			ServerManager.ApplyDamage(lastDamager, this, unsafeDamage);
			unsafeCounter += unsafeRate;
		}
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		if (applyForce)
		{
			Rigidbody.velocity = force;
			applyForce = false;
		}

		if (shootFireball)
		{
			shootFireball = false;

			lastShot = Time.time;
			GameObject arrow = Instantiate(arrowPrefab, transform.position, transform.rotation) as GameObject;
			arrow.GetComponent<Rigidbody2D>().velocity = LookingDirection * DataManager.DataConstants.arrowBaseForce;
			arrow.GetComponent<Rigidbody2D>().AddTorque(200f);
			arrow.GetComponent<FireballTrigger>().entity = this;
		}
	}
	#endregion UnityMethods

	public override void SetAction(InputActions action, bool value)
	{
		switch (action)
		{
			case InputActions.Shoot:
				if (!value)
					Shoot();
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
			shootFireball = true;
	}

	public bool ReceivedDamage(float damage)
	{
		hurtSource.Play();
		return ApplyDamage(damage);
	}

	public bool ReceivedDamageWithForce(float damage, Vector3 force)
	{
		hurtSource.Play();
		ApplyForce(force);
		return ApplyDamage(damage);
	}

	public void EnteredSafeArea()
	{
		safe = true;
	}

	public void LeftSafeArea()
	{
		safe = false;
		unsafeCounter = Time.time;
	}

	private bool ApplyDamage(float damage)
	{
		if (damage > hp)
		{
			Die();
			return true;
		}

		hp -= damage;
		return false;
	}

	private void ApplyForce(Vector3 force)
	{
		applyForce = true;
		this.force = force;
		Flying = true;
		stopFlyingTime = Time.time + flyTime;
	}

	private void Die()
	{
		score -= 1;
		hp = 100;
	}
}
