using System;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Jabalina : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private float speed = 12f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float lifeTime = 5f;

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //Detecta colision pero no empuja
        GetComponent<BoxCollider2D>().isTrigger = true;
        //Destruir entidad
        Destroy(gameObject, lifeTime);

    }

    void FixedUpdate()
    {
        rb.linearVelocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
