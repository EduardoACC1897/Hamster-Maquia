using System;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Jabalina : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float speed = 12f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 5f;

    [Header("Ajuste Visual de la Jabalina")]
    [Tooltip("Ajusta este valor si la jabalina no sale como queres")]
    [SerializeField] private float rotationOffset = -90f;

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Detecta colision pero no empuja
        if(TryGetComponent(out BoxCollider2D boxCollider))
        {
            boxCollider.isTrigger = true;
        }
        moveDirection = new Vector2(transform.right.x, 0f).normalized;

        transform.Rotate(0f, 0f, rotationOffset);
        //Destruir entidad
        Destroy(gameObject, lifeTime);

    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage, transform.position);
            Debug.Log("Checks fue empalado");
            //collision.GetComponent<PlayerHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            //Si choca con una pared, se destruye
            Destroy(gameObject);
        }
    } 
}
