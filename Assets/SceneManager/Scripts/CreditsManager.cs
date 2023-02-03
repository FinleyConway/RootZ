using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CreditsManager : MonoBehaviour
{
    public void closeCredits()
    {
        SceneManager.LoadScene(0); //Value must be set to menu value
    }
}
