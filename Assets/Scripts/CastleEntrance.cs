using UnityEngine;

public class CastleEntrance : MonoBehaviour
{
    [SerializeField] private Collider2D blockingCollider;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Flagpole.Instance != null && Flagpole.Instance.activated)
            {
                blockingCollider.enabled = false;
            }
        }
    }
}
