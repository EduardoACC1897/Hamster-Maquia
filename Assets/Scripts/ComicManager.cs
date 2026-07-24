using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ComicManager : MonoBehaviour
{
    [SerializeField] private Image comicImage;
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button nextSceneButton;
    [SerializeField] private Button previousSceneButton;

    [SerializeField] private Sprite[] pages;

    private int currentPage = 0;

    void Start()
    {
        ShowPage();
    }

    void ShowPage()
    {
        if (currentPage == 0)
        {
            previousButton.gameObject.SetActive(false);
            previousSceneButton.gameObject.SetActive(true);
        }
        else
        {
            previousButton.gameObject.SetActive(true);
            previousSceneButton.gameObject.SetActive(false);
        }

        if (currentPage == pages.Length - 1)
        {
            nextButton.gameObject.SetActive(false);
            nextSceneButton.gameObject.SetActive(true);
        }
        else
        {
            nextButton.gameObject.SetActive(true);
            nextSceneButton.gameObject.SetActive(false);
        }
        comicImage.sprite = pages[currentPage];
    }

    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            ShowPage();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage();
        }
    }

    public void TutorialSceneBtn()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void MainMenuSceneBtn()
    {
        SceneManager.LoadScene("MainMenu");
    }
}