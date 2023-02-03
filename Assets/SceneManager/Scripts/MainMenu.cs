using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        SceneManager.LoadScene(1); //Value must be set to that of the game scene in editor (Set Gameplay to 1 in build!!!!)
    }
    public void QuitGame()
    {
         Application.Quit();
    }
    public void CreditScreen()
    {
        SceneManager.LoadScene(2); //Value must be set to that of the credits scene!!
    }
}
