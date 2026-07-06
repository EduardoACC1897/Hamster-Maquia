using UnityEngine;
using System.Collections;
using System;

public class Esparrago : Enemy
{
    [Header("Configuracion de Ataque")]
    [SerializeField] private Transform contenedorTemportal;
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private Transform spawnPoint;//spawn jabalina
    [SerializeField] private float attackCooldown = 2f;

    [Range(0.1f, 5f)]
    [SerializeField] private float margenError = 1.5f; //mantener paralelo al piso

    private bool canAttack = true;
    private bool beInclinado = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        //Se mantiene estatico
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnPlayerDetected()
    {
        if (playerTransform == null || !canAttack) return;
        //apunta al jugador cuando esta cerca
        seePlayer();

        //verificar si estan en la misma posicion
        float diferenciaY = Mathf.Abs(playerTransform.position.y - transform.position.y);

        if (diferenciaY <= margenError)
        {
            StartCoroutine(SecuenciaAtaque());
        }            
        
    }

    private void seePlayer()
    {
        if (beInclinado) return;
        if(playerTransform.position.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    private IEnumerator SecuenciaAtaque()
    {
        canAttack = false;
        beInclinado = true;
        //animacion de ataque

        float anguloInclinacion = (transform.eulerAngles.y == 180) ? 15f : -15f;
        contenedorTemportal.localRotation = Quaternion.Euler(0, transform.eulerAngles.y, anguloInclinacion);
        Debug.Log("Inclinacion True");

        yield return new WaitForSeconds(1.5f); //espera antes de disparar

        //DISPARO
        if(proyectilPrefab != null && spawnPoint != null) 
        {
            //Asegurar trayectoria de la jabalina paralela al piso
            float rotacionY = (transform.eulerAngles.y == 180) ? 180f : 0f;
            Quaternion rotacionParalelo = Quaternion.Euler(0, rotacionY, 0);

            Instantiate(proyectilPrefab, spawnPoint.position, rotacionParalelo);
            Debug.Log("Disparo true");
        }

        //regresar posicion
        contenedorTemportal.localRotation = Quaternion.identity;
        beInclinado = false;

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;


    }
    //Dibujo de una linea horizontal para revisar el trayecto de la jabalina
    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.yellow;
        Vector3 rangoLeft = transform.position + Vector3.left * detectionRadius;
        Vector3 rangoRight = transform.position + Vector3.right * detectionRadius;

        Gizmos.DrawLine(rangoLeft + Vector3.up * margenError, rangoRight + Vector3.up * margenError);
        Gizmos.DrawLine(rangoLeft + Vector3.down * margenError, rangoRight + Vector3.down * margenError);

        //cierre 
        Gizmos.DrawLine(rangoLeft + Vector3.up * margenError, rangoLeft + Vector3.down * margenError);
        Gizmos.DrawLine(rangoRight + Vector3.up * margenError, rangoRight + Vector3.down * margenError);
    }
}


