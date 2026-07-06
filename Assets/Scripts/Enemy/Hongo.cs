using UnityEngine;

public class Hongo : Enemy
{
    [Header("Hongo Movimiento")]
    [SerializeField] private float patrullaDistance = 3f;
    private Vector2 startPosition;
    private int direction = 1;

    protected override void Start()
    {
        base.Start();
        startPosition = transform.position;
    }

    protected override void Update()
    {
        base.Update();

        if (!isPlayerNearby)
        {
            Patrulla();
        }
    }

    private void Patrulla()
    {
        if(Mathf.Abs(transform.position.x - startPosition.x) >= patrullaDistance)
        {
            direction *= -1; // Cambia la dirección
            //volteo sprite
            transform.localScale = new Vector3(direction, 1, 1);
        }

        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }

    protected override void OnPlayerDetected()
    {
        base.OnPlayerDetected();
        float dirToPlayer = Mathf.Sign(playerTransform.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(dirToPlayer * (moveSpeed * 1.5f), rb.linearVelocity.y);
    }
}
