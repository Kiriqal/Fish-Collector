using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private string gameSceneName; 
    public GameObject optionsPanel;
    public GameObject Information;
    public GameObject Exit;

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName); // tukar sesuai nama scene nanti
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }
    public void OpenInformation()
    {
        Information.SetActive(true);
    }

    public void CloseInformation()
    {
        Information.SetActive(false);
    }
    public void OpenExit()
    {
        Exit.SetActive(true);
    }

    public void CloseExit()
    {
        Exit.SetActive(false);
    }
    public void ExitGame()
    {
        Debug.Log("Exit Game Called");
        Application.Quit();
    }
}
