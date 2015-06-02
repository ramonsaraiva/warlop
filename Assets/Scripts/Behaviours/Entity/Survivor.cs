using UnityEngine;
using UnityEngine.UI;
using Warlop.Constants;

public class Survivor : Living
{
	#region UnityFields
	[SerializeField]
	private Animator animator;
	[SerializeField]
	private GameObject fireballPrefab;
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

	private float lastShot;
	private float cooldown;
	private bool shootFireball;

	private bool applyForce;
	private Vector3 force;
	private float stopFlyingTime;

	private float stopChannelingTime;

	private float lastExplosion;
	private bool makeExplosion;

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

		Speed = WizardConstants.StartingSpeed;
		hp = WizardConstants.StartingHealth;
		lastShot = Time.time - SpellConstants.FireballRate;
		lastExplosion = Time.time - SpellConstants.ExplosionRate;
		lastDamager = this;
		safe = true;
	}

	protected override void Update()
	{
		base.Update();

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

		if (!safe && Time.time > unsafeCounter + EnvironmentConstants.UnsafeAreaRate)
		{
			ServerManager.ApplyDamage(lastDamager, this, EnvironmentConstants.UnsafeAreaDPR);
			unsafeCounter += EnvironmentConstants.UnsafeAreaRate;
		}

		if (Channeling && Time.time > stopChannelingTime)
		{
			Channeling = false;
			makeExplosion = true;
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

			GameObject fireball = Instantiate(fireballPrefab, transform.position, transform.rotation) as GameObject;
			fireball.GetComponent<Rigidbody2D>().velocity = LookingDirection * SpellConstants.FireballSpeed;
			fireball.GetComponent<Rigidbody2D>().AddTorque(400f);
			fireball.GetComponent<FireballTrigger>().entity = this;
		}

		if (makeExplosion)
		{
			makeExplosion = false;
			lastExplosion = Time.time;

			Rigidbody.velocity = Vector3.zero;

			if (ServerManager.IsServer())
			{
				Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, SpellConstants.ExplosionRadius, 1 << LayerMask.NameToLayer("Player"));
				foreach (Collider2D coll in hit)
				{
					if (coll.gameObject == gameObject)
						continue;

					float explosionDistance = Vector3.Distance(transform.position, coll.transform.position);
					Vector3 explosionForce = (coll.transform.position - transform.position).normalized * SpellConstants.ExplosionForce * (2 - explosionDistance);
					ServerManager.ApplyDamageWithForce(this, coll.GetComponent<Survivor>(), SpellConstants.ExplosionDamage, explosionForce);
				}
			}
		}
	}
	#endregion UnityMethods

	public override void SetAction(InputActions action, bool value)
	{
		switch (action)
		{
			case InputActions.Fireball:
				if (!value)
					Shoot();
				break;
			case InputActions.Explosion:
				if (!value)
					Explosion();
				break;
		}
	}

	private void Shoot()
	{
		bool outOfCooldown = Time.time > lastShot + SpellConstants.FireballRate;

		if (outOfCooldown)
			shootFireball = true;
	}

	private void Explosion()
	{
		bool outOfCooldown = Time.time > lastExplosion + SpellConstants.ExplosionRate;

		if (!outOfCooldown)
			return;

		Channeling = true;
		stopChannelingTime = Time.time + SpellConstants.ExplosionChannelingTime;
		animator.SetTrigger("Explosion");
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
		Fly();
	}

	private void Fly()
	{
		Flying = true;
		stopFlyingTime = Time.time + WizardConstants.FlyTime;
	}

	private void Die()
	{
		score -= 1;
		hp = 100;
	}
}
