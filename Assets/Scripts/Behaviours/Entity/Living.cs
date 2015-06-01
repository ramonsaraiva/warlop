using UnityEngine;
using System.Collections;

public class Living : Entity
{
	[SerializeField]
	private Rigidbody2D rigidbody;
	[SerializeField]
	private NetworkCorrectionBehaviour networkCorrection;

    private Vector3 movementDirection;
	private float speed;
	private float angle;

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
	}

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

	private void FixedUpdate()
	{
		rigidbody.velocity = movementDirection * speed;
	}
}
