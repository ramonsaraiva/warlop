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

    private float lastTeleport;
    private bool teleporting;

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

    public bool Teleporting
    {
        get { return teleporting; }
        set
        {
            teleporting = true;
            lastTeleport = Time.time;
        }
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
        // network hack? lol
        if (teleporting && Time.time > lastTeleport + 1f)
            teleporting = false;
	}

	protected virtual void FixedUpdate()
	{
		if (channeling)
			return;

        if (flying)
            rigidbody.AddForce(movementDirection * speed, ForceMode2D.Force);
        else
        {
            //rigidbody.velocity = movementDirection * speed;
            transform.position += movementDirection * speed * Time.deltaTime;
            /*
             * swap velocity to position?
             * i don't lik the "slow movement" effect that velocity does
             */
        }
	}
}
