using UnityEngine;
using System.Collections;
using System;

public class Esparrago : Enemy
{
    [Header("Configuracion de Ataque")]
    //[SerializeField] private Transform contenedorTemportal;
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private Transform spawnPoint;//spawn jabalina
    [SerializeField] private float attackCooldown = 2f;

    [Range(0.1f, 5f)]
    [SerializeField] private float margenError = 1.5f; //mantener paralelo al piso

    [Header("Animaciones y Componentes")]
    [SerializeField] private Animator bodyAnimator;

    private bool canAttack = true;
    //private bool beInclinado = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        //Se mantiene estatico
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        /*if(bodyAnimator == null)
        {
            bodyAnimator = GetComponentInChildren<Animator>();
            if(bodyAnimator == null )
            {
                Debug.LogError("Animator no asignado en el inspector ni encontrado en los hijos del objeto.");
            }
        }*/
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnPlayerDetected()
    {
        base.OnPlayerDetected();

        /*if(bodyAnimator != null)
        {
            bodyAnimator.SetBool("Emerging", true);
        }*/

        if (playerTransform == null || !canAttack) return;

        //verificar si estan en la misma posicion
        float diferenciaY = Mathf.Abs(playerTransform.position.y - transform.position.y);

        if (diferenciaY <= margenError)
        {
            StartCoroutine(SecuenciaAtaque());
        }            
        
    }
    /*
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
    */

    protected override void OnPlayerLost()
    {
        base.OnPlayerLost();

        /*if (bodyAnimator != null)
        {
            bodyAnimator.SetBool("Emerging", false);
        }*/
        
    }

    private IEnumerator SecuenciaAtaque()
    {
        canAttack = false;

        // Breve tiempo de preparación antes del disparo
        yield return new WaitForSeconds(0.3f);

        if (proyectilPrefab != null && spawnPoint != null && isPlayerNearby)
        {
            if (bodyAnimator != null)
            {
                bodyAnimator.SetTrigger("Attack");
            }

            float dirX = (playerTransform.position.x >= transform.position.x) ? 1f : -1f;
            Quaternion rotacionDisparo = (dirX < 0) ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;

            Instantiate(proyectilPrefab, spawnPoint.position, rotacionDisparo);
        }

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


