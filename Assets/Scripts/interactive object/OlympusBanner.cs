using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class OlympusBanner : MonoBehaviour
{
    #region Settings

    [SerializeField]
    private string sceneName;

    [SerializeField]
    private float delay = 3f;

    [SerializeField]
    private PauseManager pauseManager;

    #endregion

    #region State

    private bool activated;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(
        Collider2D other)
    {
        if (activated)
            return;

        if (!other.CompareTag("Player"))
            return;

        PlayerInputHandler input =
            other.GetComponent<PlayerInputHandler>();

        StartCoroutine(
            ActivateRoutine(input));
    }

    #endregion

    #region Coroutines

    private IEnumerator ActivateRoutine(
    PlayerInputHandler input)
    {
        activated = true;

        input?.SetInputEnabled(false);

        pauseManager.IgnoreTimeScale = true;

        Time.timeScale = 0f;

        float timer = 0f;

        while (timer < delay)
        {
            if (pauseManager == null ||
                !pauseManager.JuegoPausado)
            {
                timer += Time.unscaledDeltaTime;
            }

            yield return null;
        }

        pauseManager.IgnoreTimeScale = false;

        Time.timeScale = 1f;

        SceneManager.LoadScene(sceneName);
    }

    #endregion
}