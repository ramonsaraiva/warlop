using UnityEngine;
using System.Collections;

public class Living : Entity
{
	#region UnityFields
	[SerializeField]
	private Rigidbody2D rigidbody;
	[SerializeField]
	private NetworkCorrectionBehaviour networkCorrection;
	#endregion UnityFields

	private Vector3 movementDirection;
	private float speed;
	private float angle;
	private bool flying;
	private bool channeling;

	#region Properties
	public Rigidbody2D Rigidbody
	{
		get { return rigidbody; }
		set { rigidbody = value; }
	}

	public NetworkCorrectionBehaviour NetworkCorrection
	{
		get { return networkCorrection; }
	}

    public Vector3 MovementDirection
    {
        get { return movementDirection; }
        set { movementDirection = value; }
    }

	public float Speed
	{
		get { return speed; }
		set { speed = value; }
	}

	public float Angle
	{
		get { return angle; }
		set { angle = value; }
	}

	public bool Flying
	{
		get { return flying; }
		set { flying = value; }
	}

	public bool Channeling
	{
		get { return channeling; }
		set { channeling = value; }
	}
	#endregion Properties

	public void UpdateDirection(Vector3 direction)
	{
		movementDirection = direction;
	}

	public void UpdateRotation(float angle, float offset)
	{
		this.angle = angle;
		rigidbody.rotation = angle + offset;
	}

	public virtual void SetAction(InputActions action, bool value) {}

	protected virtual void Update()
	{
	}

	protected virtual void FixedUpdate()
	{
		if (channeling)
			return;

		if (flying)
			rigidbody.AddForce(movementDirection * speed, ForceMode2D.Force);
		else
			rigidbody.velocity = movementDirection * speed;
	}
}
