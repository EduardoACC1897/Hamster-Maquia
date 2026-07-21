using UnityEngine;
using System.Collections;

public class Frambuesa : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Patrullaje")]
    [Tooltip("Distancia que se movera a la derecha e izquierda desde la posicion de la frambuesa")]
    [SerializeField] private float distanciaPatrulla = 4f;
    [SerializeField] private float speed = 3f;

    [Header("Configuracion de Ataques")]
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private Transform direccion;
    [SerializeField] private float tiempoEntreAtaques = 2f; //cadencia

    
    [SerializeField] private Animator bodyAnimator;
    [SerializeField] private SpriteRenderer spriteRendererCuerpo;
    [SerializeField] private SpriteRenderer spriteRendererHojas;

    private Rigidbody2D rb;
    private Vector3 puntoA;
    private Vector3 puntoB;
    private Vector3 destinoActual;
    private float contadorAtaque = 0f;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;

        puntoA = transform.position + Vector3.left * distanciaPatrulla;
        puntoB = transform.position + Vector3.right * distanciaPatrulla;

        destinoActual = puntoB;
    }
    protected override void Update()
    {
        base.Update();

        if (!isPlayerNearby)
        {
            ApuntarJugador();
            ManejarAtaque();
        }
        else
        {
            contadorAtaque = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (!isPlayerNearby)
        {
            Patrullaje();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void Patrullaje()
    {
        Vector2 nuevaPosicion = Vector2.MoveTowards(rb.position, destinoActual, speed * Time.fixedDeltaTime);
      
        rb.MovePosition(nuevaPosicion);

        if (Vector2.Distance(rb.position, destinoActual) < 0.1f)
        {
            destinoActual = (destinoActual == puntoA) ? puntoB : puntoA;
            GirarSpriteHorizontal(destinoActual.x - rb.position.x);
        }
    }

    private void ApuntarJugador()
    {
        if(playerTransform != null)
        {
            float direccionX = playerTransform.position.x - transform.position.x;
            GirarSpriteHorizontal(direccionX);
        }
    }
    private void GirarSpriteHorizontal(float direccionX)
    {
        if (direccionX != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (direccionX > 0 ? 1 : -1);
            transform.localScale = scale;
        }
    }

    private void ManejarAtaque()
    {
        contadorAtaque += Time.deltaTime;

        if(contadorAtaque >= tiempoEntreAtaques)
        {
            contadorAtaque = 0f;
            DispararSemilla();
        }
    }

    protected void DispararSemilla()
    {
        if (proyectilPrefab == null || direccion == null || playerTransform == null) return;

        if (bodyAnimator != null)
        {
            bodyAnimator.SetTrigger("Attack");
        }

        GameObject clonProyectil = Instantiate(proyectilPrefab, direccion.position, Quaternion.identity);
        Vector2 direccionDisparo = (playerTransform.position - direccion.position).normalized;

        if (clonProyectil.TryGetComponent(out proyectilesFrambuesa scriptProyectil))
        {
            scriptProyectil.InicializarDireccion(direccionDisparo);
        }

    }
   
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Vector3 posA = Application.isPlaying ? puntoA : transform.position + Vector3.left * distanciaPatrulla;
        Vector3 posB = Application.isPlaying ? puntoB : transform.position + Vector3.right * distanciaPatrulla;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(posA, 0.3f);
        Gizmos.DrawWireSphere(posB, 0.3f);
        Gizmos.DrawLine(posA, posB);
    }
    
}
