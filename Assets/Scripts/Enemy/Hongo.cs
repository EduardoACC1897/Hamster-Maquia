using UnityEngine;

public class Hongo : Enemy
{
    [Header("Hongo Movimiento")]
    [SerializeField] private float patrullaDistance = 3f;
    private Vector2 startPosition;
    private int direction = 1;

    [Header("Deteccion de Vacio y Parederes")]
    [Tooltip("Capa de las plataformas y suelos")]
    [SerializeField] private LayerMask groundLayer;
    [Tooltip("Distancia hacia el frente para detectar el vacio y paredes")] 
    [SerializeField] private float detectionDistance = 0.6f;
    [Tooltip("Distancia hacia abajo para detectar el vacio y paredes")]
    [SerializeField] private float groundDetectionDistance = 1.2f;


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
            InvertirDireccion();
        }
        else if (HayVacio(direction))
        {
            InvertirDireccion();
        }

        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }

    protected override void OnPlayerDetected()
    {
        base.OnPlayerDetected();
        float dirToPlayer = Mathf.Sign(playerTransform.position.x - transform.position.x);
        int intDir = (int)dirToPlayer;

        // Orientar el sprite hacia el jugador
        ActualizarOrientacion(intDir);

        // Si detecta un precipicio persiguiendo al jugador, se frena para no suicidarse
        if (HayVacio(intDir))
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        
        rb.linearVelocity = new Vector2(dirToPlayer * (moveSpeed * 1.5f), rb.linearVelocity.y);
    }

    private bool HayVacio(float dir)
    {
        Vector2 origin = (Vector2)transform.position + new Vector2(dir * detectionDistance, 0f);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundDetectionDistance, groundLayer);
        return hit.collider == null;
    }

    private void InvertirDireccion()
    {
        direction *= -1;
        ActualizarOrientacion(direction);
    }

    private void ActualizarOrientacion(int dir)
    {
        if (direction == 0) return;
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * dir;
        transform.localScale = scale;
    }
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.yellow;
        float currentDir = Application.isPlaying ? direction : 1;
        Vector3 rayOrigin = transform.position + new Vector3(currentDir * detectionDistance, 0f, 0f);

        Gizmos.DrawLine(rayOrigin, rayOrigin + Vector3.down * groundDetectionDistance);
    }
}
