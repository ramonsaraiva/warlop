using UnityEngine;
public class NetworkCorrectionBehaviour : MonoBehaviour
{
	private Vector3 serverPosition;
	private float correctionSpeed;
    private bool forciblyDisabled;

	void Awake()
	{
		enabled = false;
	}

	void Update()
	{
		transform.position = Vector3.Lerp(transform.position, serverPosition, correctionSpeed);

		float distance = (transform.position - serverPosition).sqrMagnitude;

		if (distance < 0.05f)
			enabled = false;
	}

	public void NewServerPosition(Vector3 position)
	{
        if (forciblyDisabled)
            return;

		float distance = (transform.position - position).sqrMagnitude;
		if (distance < 0.05f)
		{
			enabled = false;
		}
		else if (distance > 1.0f)
		{
			transform.position = position;
			enabled = false;
		}
		else if (distance > 0.5f)
		{
			correctionSpeed = 0.4f;
			enabled = true;
			serverPosition = position;
		}
		else
		{
			correctionSpeed = 0.1f;
			enabled = true;
			serverPosition = position;
		}
	}

    public void ForciblyDisable()
    {
        forciblyDisabled = true;
        enabled = false;
    }

    public void ForciblyEnable()
    {
        forciblyDisabled = false;
    }
}
