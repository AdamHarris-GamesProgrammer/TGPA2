using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public Button _MainMenuButton;    
    public Button _HideOutButton;    
    public Button _RespawnButton;    

    public void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }
    
    public void Hideout()
    {
        SceneManager.LoadScene("HideOut");
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
