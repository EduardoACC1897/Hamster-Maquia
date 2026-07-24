using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]

public class proyectilesFrambuesa : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private float velocidad = 8f;
    //[SerializeField] private int damage = 1;
    [SerializeField] private float tiempodeVida = 5f;

    private Rigidbody2D rb;
   
    public void InicializarDireccion(Vector2 direccion)
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponent<BoxCollider2D>().isTrigger = true;

        rb.linearVelocity = direccion.normalized * velocidad;

        //Rotacion de la semilla dependiendo de su direccion
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angulo);

        Destroy(gameObject, tiempodeVida);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(1, transform.position);
            Debug.Log("Cheeks golpeado");
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            Destroy(gameObject);//destruir si toca el escenario
        }
    }
}
