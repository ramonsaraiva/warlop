using UnityEngine;
using UnityEngine.UI;
using Warlop.Constants;

using System.Collections.Generic;

public class Survivor : Living
{
	#region UnityFields
	[SerializeField]
	private Animator animator;
	[SerializeField]
	public GameObject fireballPrefab;
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

	private bool applyForce;
	private Vector3 force;
	private float stopFlyingTime;

	private List<SpellBehaviour> spells;

	#region Properties
	public Animator Animator
	{
		get { return animator; }
	}

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
	#endregion Properties

	#region UnityMethods
	protected override void Awake()
	{
		base.Awake();

		Speed = WizardConstants.StartingSpeed;
		hp = WizardConstants.StartingHealth;
		lastDamager = this;
		safe = true;

		spells = new List<SpellBehaviour>();
		spells.Add(gameObject.AddComponent<FireballBehaviour>());
		spells.Add(gameObject.AddComponent<KnockbackBehaviour>());
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

		if (!safe && Time.time > unsafeCounter + EnvironmentConstants.UnsafeAreaRate)
		{
			ServerManager.ApplyDamage(lastDamager, this, EnvironmentConstants.UnsafeAreaDPR);
			unsafeCounter += EnvironmentConstants.UnsafeAreaRate;
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
	}
	#endregion UnityMethods

	public override void SetAction(InputActions action, bool value)
	{
		if (Channeling)
			return;

		switch (action)
		{
			case InputActions.PrimarySkill:
				if (!value)
					spells[0].Use();
				break;
			case InputActions.SecondarySkill:
				if (!value)
					spells[1].Use();
				break;
		}
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
