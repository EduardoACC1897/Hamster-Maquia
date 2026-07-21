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

  
    private Vector2 puntoA;
    private Vector2 puntoB;
    private Vector2 destinoActual;
    private float contadorAtaque = 0f;

    protected override void Start()
    {
        base.Start();

        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0f;
        }

        Vector2 posInicial = transform.position;
        puntoA = posInicial + Vector2.left * distanciaPatrulla;
        puntoB = posInicial + Vector2.right * distanciaPatrulla;

        destinoActual = puntoB;
    }
    protected override void Update()
    {
        base.Update();

        Debug.Log($"Jugador cerca? {isPlayerNearby}");

        if (isPlayerNearby)
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
        if (Mathf.Abs(direccionX) > 0.01f)
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
        Vector2 posInicial = Application.isPlaying ? (puntoA + puntoB) / 2f : (Vector2)transform.position;
        Vector2 posA = Application.isPlaying ? puntoA : posInicial + Vector2.left * distanciaPatrulla;
        Vector2 posB = Application.isPlaying ? puntoB : posInicial + Vector2.right * distanciaPatrulla;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(posA, 0.3f);
        Gizmos.DrawWireSphere(posB, 0.3f);
        Gizmos.DrawLine(posA, posB);
    }
    
}
