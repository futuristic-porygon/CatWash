using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
  
    public void OnStartClick()
    {
        SceneManager.LoadScene("Main");           
    }
    public void OnClickExit()
    {
       Application.Quit();
    }
    public void OnClickCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void OnClickTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void OnClickMainManu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
