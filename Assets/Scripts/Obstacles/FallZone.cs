using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FallZone : MonoBehaviour
{
    [SerializeField]
    private float respawnDelay = 1f;

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

        PlayerInputHandler input =
            other.GetComponent<PlayerInputHandler>();

        if (respawn == null)
            return;

        StartCoroutine(
            RespawnRoutine(
                respawn,
                input));
    }

    private IEnumerator RespawnRoutine(
        PlayerRespawn respawn,
        PlayerInputHandler input)
    {
        respawning = true;

        input?.SetInputEnabled(false);

        yield return new WaitForSeconds(
            respawnDelay);

        respawn.Respawn();

        input?.SetInputEnabled(true);

        respawning = false;
    }
}