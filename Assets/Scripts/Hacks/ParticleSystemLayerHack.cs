using UnityEngine;

public class ParticleSystemLayerHack : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer spriteRenderer;
	[SerializeField]
	private ParticleSystem particleSystem;

	void Start()
	{
		Renderer particleRenderer = particleSystem.GetComponent<Renderer>();
		particleRenderer.sortingLayerID = spriteRenderer.sortingLayerID;
		particleRenderer.sortingOrder = spriteRenderer.sortingOrder;
	}
}
