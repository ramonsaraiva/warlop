using UnityEngine;
using System.Collections.Generic;

public class DataConstants : ScriptableObject
{
    public GameObject PlayerPrefab;
    public float speed = 5;
	public float sprintSpeed = 10;
	public float shootingSpeed = 1f;
	public float arrowBaseForce = 5f;
	public readonly string[] Actions = { "Action0", "Action1" };
}