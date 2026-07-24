using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FallZone : MonoBehaviour
{
    [SerializeField]
    private float respawnDelay = 1f;

    [SerializeField]
    private ScreenTransition transition;

    private bool respawning;

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (respawning)
            return;

        if (!other.CompareTag("Player"))
            return;

        PlayerRespawn respawn =
            other.GetComponent<PlayerRespawn>();

        PlayerController controller =
            other.GetComponent<PlayerController>();

        if (respawn == null ||
            controller == null)
        {
            return;
        }

        StartCoroutine(
            RespawnRoutine(
                respawn,
                controller));
    }

    private IEnumerator RespawnRoutine(
    PlayerRespawn respawn,
    PlayerController controller)
    {
        respawning = true;

        controller.IsDead = true;

        yield return new WaitForSeconds(
            respawnDelay);

        if (transition != null)
        {
            yield return transition.FadeOut();
        }

        respawn.Respawn();

        controller.IsDead = false;

        respawning = false;
    }
}