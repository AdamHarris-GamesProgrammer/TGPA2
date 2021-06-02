using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    Animator _transition;


    void Awake()
    {
        
        _transition = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        FindObjectOfType<SavingWrapper>().Load();
    }


    public void LoadLevel(string name)
    {
        StartCoroutine(LoadLevelAnim(name));
    }

    IEnumerator LoadLevelAnim(string name)
    {
        //Plays the animation
        _transition.SetTrigger("start");

        Time.timeScale = 1.0f;
        //Waits for the animation to be finished (1 second)
        yield return new WaitForSeconds(1.0f);

        //Saves the game
        FindObjectOfType<SavingWrapper>().Save();
        //Loads the new scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }
}
