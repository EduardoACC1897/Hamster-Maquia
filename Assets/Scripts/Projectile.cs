using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    #region References

    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;

    #endregion

    #region Settings

    [SerializeField] private float speed = 8f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private int damage = 1;

    #endregion

    #region State

    private Vector2 direction = Vector2.right;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = direction.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // IGNORAR PLAYER
        if (other.GetComponent<PlayerController>() != null)
            return;

        int layerMask = 1 << other.gameObject.layer;

        // GROUND → destruir
        if ((groundLayer & layerMask) != 0)
        {
            Destroy(gameObject);
            return;
        }

        // ENEMY → daño + destruir
        if ((enemyLayer & layerMask) != 0)
        {
            if (other.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }

    #endregion

    #region Public Methods

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    #endregion
}