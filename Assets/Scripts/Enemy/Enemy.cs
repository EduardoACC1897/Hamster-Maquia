using System;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Estadisticas base del enemigo")]
    [SerializeField] protected int maxHealth = 3;
    protected int currentHealth;

    [SerializeField] protected float moveSpeed = 2f;

    [Header("Deteccion de Cheeks")]
    [SerializeField] protected float detectionRadius = 4f;
    [SerializeField] protected LayerMask playerLayer;

    protected Rigidbody2D rb;
    protected Transform playerTransform;
    protected bool isPlayerNearby;

    //Metodo padre
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        //Identifica a cheeks en la escena
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            playerTransform = player.transform;
        }
    }

    protected virtual void Update()
    {
        CheckForPlayer();
    }

    //detectar si jugador esta cerca
    protected virtual void CheckForPlayer()
    {
        if (playerTransform == null) return;

        isPlayerNearby = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (isPlayerNearby)
            OnPlayerDetected();
        else
            OnPlayerLost();
    }

    protected virtual void OnPlayerDetected()
    {
        //Debug.Log($"{gameObject.name}: Player detectado");
    }
    protected virtual void OnPlayerLost()
    {
        //logica cuando se aleja
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} recibio {damage} de daño. Vida restante: {currentHealth}");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
   
}
