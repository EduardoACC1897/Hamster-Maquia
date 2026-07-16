using System.Collections;
using UnityEngine;

public class IngredientEssenceOrb : MonoBehaviour
{
    #region Settings

    [Header("Essence")]

    [SerializeField]
    private IngredientEssenceType essenceType;

    [SerializeField]
    private int amount = 1;

    [Header("Absorption")]

    [SerializeField]
    private float autoAbsorbDelay = 3f;

    [SerializeField]
    private float magnetRadius = 2f;

    [SerializeField]
    private float absorbSpeed = 8f;

    [SerializeField]
    private float touchAbsorbSpeedMultiplier = 1.5f;

    [SerializeField]
    private float destroyDistance = 0.08f;

    [SerializeField]
    private float shrinkDistance = 1f;

    [SerializeField]
    private float shrinkSpeed = 6f;

    #endregion

    #region References

    private Transform player;

    private PlayerController playerController;

    private PlayerIngredientEssence playerEssence;

    #endregion

    #region State

    private bool isAbsorbing;

    private float currentAbsorbSpeed;

    private Coroutine autoAbsorbCoroutine;

    #endregion

    #region Unity Messages

    private void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject == null)
        {
            DestroyOrb();
            return;
        }

        player = playerObject.transform;

        playerController =
            playerObject.GetComponent<PlayerController>();

        playerEssence =
            playerObject.GetComponent<PlayerIngredientEssence>();

        autoAbsorbCoroutine =
            StartCoroutine(
                AutoAbsorbRoutine());
    }

    private void Update()
    {
        if (playerController == null ||
            playerController.IsDead)
        {
            DestroyOrb();
            return;
        }

        float distance =
            Vector2.Distance(
                transform.position,
                player.position);

        if (!isAbsorbing)
        {
            if (distance <= magnetRadius)
            {
                StartAbsorption(true);
            }

            return;
        }

        transform.position =
            Vector3.MoveTowards(
                transform.position,
                player.position,
                currentAbsorbSpeed * Time.deltaTime);

        if (distance <= shrinkDistance)
        {
            transform.localScale =
                Vector3.MoveTowards(
                    transform.localScale,
                    Vector3.zero,
                    shrinkSpeed * Time.deltaTime);
        }

        if (distance <= destroyDistance)
        {
            playerEssence.AddEssence(
                essenceType,
                amount);

            DestroyOrb();
        }
    }

    #endregion

    #region Coroutines

    private IEnumerator AutoAbsorbRoutine()
    {
        yield return new WaitForSeconds(
            autoAbsorbDelay);

        StartAbsorption(false);
    }

    #endregion

    #region Private Methods

    private void StartAbsorption(
        bool playerNearby)
    {
        if (isAbsorbing)
            return;

        isAbsorbing = true;

        if (autoAbsorbCoroutine != null)
        {
            StopCoroutine(
                autoAbsorbCoroutine);
        }

        currentAbsorbSpeed = absorbSpeed;

        if (playerNearby)
        {
            currentAbsorbSpeed *=
                touchAbsorbSpeedMultiplier;
        }
    }

    private void DestroyOrb()
    {
        Destroy(gameObject);
    }

    #endregion
}