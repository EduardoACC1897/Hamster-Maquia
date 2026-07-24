using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RetryBtn()
    {
        if(PlayerDataManager.Instance != null)
        {
            PlayerDataManager.Instance.ResetRun();
        }
        SceneManager.LoadScene("Level");
    }
    public void ExitToMenuBtn()
    {
        if (PlayerDataManager.Instance != null)
        {
            PlayerDataManager.Instance.ResetRun();
        }
        SceneManager.LoadScene("MainMenu");
    }
}
