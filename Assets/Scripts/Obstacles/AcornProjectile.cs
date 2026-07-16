using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class AcornProjectile : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float maxLifetime = 10f;

    private void Start()
    {
        Destroy(gameObject, maxLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int otherLayer = collision.gameObject.layer;

        // Toca el suelo
        if (((1 << otherLayer) & groundLayer) != 0)
        {
            Destroy(gameObject);
            return;
        }

        // Toca al jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}