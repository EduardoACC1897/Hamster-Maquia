using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class AcornTree : MonoBehaviour
{
    #region Settings

    [Header("Spawn")]

    [SerializeField]
    private GameObject acornPrefab;

    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private float attackDelay = 2f;

    #endregion

    #region State

    private bool playerInside;

    private float timer;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Update()
    {
        if (!playerInside)
            return;

        timer += Time.deltaTime;

        if (timer >= attackDelay)
        {
            SpawnAcorn();

            timer = 0f;
        }
    }

    #endregion

    #region Private Methods

    private void SpawnAcorn()
    {
        if (acornPrefab == null ||
            spawnPoint == null)
        {
            return;
        }

        Instantiate(
            acornPrefab,
            spawnPoint.position,
            Quaternion.identity);
    }

    #endregion

    #region Trigger

    private void OnTriggerEnter2D(
        Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = true;

        timer = 0f;
    }

    private void OnTriggerExit2D(
        Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;

        timer = 0f;
    }

    #endregion
}