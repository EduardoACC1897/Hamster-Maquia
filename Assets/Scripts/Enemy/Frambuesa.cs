using UnityEngine;
using System.Collections;

public class Frambuesa : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Posicion de Vuelo")]
    [SerializeField] private Transform puntoA;
    [SerializeField] private Transform puntoB;
    [SerializeField] private float tiempoEspera = 3f;

    [Header("Configuracion de Ataques")]
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private Transform direccion;
    [SerializeField] private float tiempoEntreAtaques = 2f; //cadencia

    private bool estaAtacando = false;

    protected override void Start()
    {
        base.Start();
        
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if(puntoA != null && puntoB != null)
        {
            transform.position = puntoA.position;
            StartCoroutine(Patrullaje());
        }
        else
        {
            Debug.LogError("Punto A o Punto B no asignados en el inspector.");
        }
    }
    protected override void Update()
    {
        base.Update();
    }

    private IEnumerator Patrullaje()
    {
        while (true)
        {
            //Estado: llega al punto A, espera y ataca
            yield return StartCoroutine(llegaPunto());
            //Estado: viaja al otro punto
            yield return StartCoroutine(Viaje(puntoB.position));
            //Estado: llega al punto B, espera y ataca
            yield return StartCoroutine(llegaPunto());
            //Estado: viaja al otro punto
            yield return StartCoroutine(Viaje(puntoA.position));


        }
    }

    private IEnumerator Viaje(Vector2 destino)
    {
        while(Vector2.Distance(transform.position, destino) > 0.1f)
        {
            Vector2 newPosition = Vector2.MoveTowards(transform.position, destino, moveSpeed * Time.deltaTime);
            rb.MovePosition(newPosition);

            yield return null;//siguiente frame

        }
        transform.position = destino;
    }

    private IEnumerator llegaPunto()
    {
        estaAtacando = true;
        float tiempoTranscurrido = 0f;

        Debug.Log($"{gameObject.name} Atacando");
        while (tiempoTranscurrido < tiempoEspera)
        {
            if (isPlayerNearby && playerTransform != null)
            {
                DispararSemilla();
            }
            
            yield return new WaitForSeconds(tiempoEntreAtaques);
            tiempoTranscurrido += tiempoEntreAtaques;
        }
        estaAtacando = false;
    }

    protected void DispararSemilla()
    {
        if (proyectilPrefab == null || direccion == null) return;

        GameObject clonProyectil = Instantiate(proyectilPrefab, direccion.position, Quaternion.identity);

        Vector2 direccionDisparo = playerTransform.position - direccion.position;

        clonProyectil.GetComponent<proyectilesFrambuesa>().InicializarDireccion(direccionDisparo);

    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        if(puntoA != null && puntoB != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(puntoA.position, puntoB.position);
            //prueba, elimiaar despues
            Gizmos.DrawWireSphere(puntoA.position, 0.3f);
            Gizmos.DrawWireSphere(puntoB.position, 0.3f);
        }
    }
}
