using UnityEngine;
using UnityEngine.SceneManagement;

public class NavManager : MonoBehaviour
{

    public GameObject creditsPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBtn()
    {
        SceneManager.LoadScene("ComicScene1");
    }
    public void SettingsBtn()
    {
        SceneManager.LoadScene("Config");
    }
    public void CreditsBtn()
    {
        creditsPanel.SetActive(true);
    }
    public void BackBtn()
    {
        creditsPanel.SetActive(false);
    }
    public void ExitBtn()
    {
        Application.Quit();
    }

}
