using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour
{
	private Survivor entity;
	private Rigidbody2D rigidbody;


	private Vector3 lastDirection;
	private Vector3 direction;
	private Vector3 lastPosition;

	private Vector3 lastMouseWorldPosition;
	private Vector3 mouseWorldPosition;

	private bool[] lastActions;
	private bool actionChanged;
	private short inputs;

    void Awake()
    {
		entity = GetComponent<Survivor>();
		rigidbody = GetComponent<Rigidbody2D>();
		
		lastActions = new bool[DataManager.DataConstants.Actions.Length];

		Camera2DFollow cameraFollow = Camera.main.gameObject.AddComponent<Camera2DFollow>();
		cameraFollow.target = transform;
    }

    void Update()
    {
		direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		entity.UpdateDirection(direction);

		mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		entity.UpdateRotation(Mathf.Atan2(transform.position.y - mouseWorldPosition.y, transform.position.x - mouseWorldPosition.x) * Mathf.Rad2Deg, 90f);
		entity.LookingDirection = mouseWorldPosition - transform.position;

		if (direction != lastDirection)
		{
			lastDirection = direction;
			ClientManager.SendInput(direction);
		}

		if (transform.position != lastPosition)
		{
			lastPosition = transform.position;
			ClientManager.SendPosition(transform.position);
		}

		if (mouseWorldPosition != lastMouseWorldPosition)
		{
			lastMouseWorldPosition = mouseWorldPosition;
			ClientManager.SendPlayerRotation(entity.Angle, entity.LookingDirection);
		}

		inputs = 0;
		actionChanged = false;
		for (int i = 0; i < DataManager.DataConstants.Actions.Length; i++)
		{
			bool action = Input.GetButton(DataManager.DataConstants.Actions[i]);

			if (lastActions[i] != action)
			{
				lastActions[i] = action;
				actionChanged = true;
			}

			if (lastActions[i])
				inputs |= (short) (1 << i);
		}

		if (actionChanged)
			ClientManager.SendActions(inputs);
    }
}
