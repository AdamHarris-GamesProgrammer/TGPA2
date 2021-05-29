using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    Animator _transition;


    void Awake()
    {
        FindObjectOfType<SavingWrapper>().Load();
        _transition = GetComponentInChildren<Animator>();
    }

    public void LoadLevel(string name)
    {
        StartCoroutine(LoadLevelAnim(name));
    }

    IEnumerator LoadLevelAnim(string name)
    {
        //Plays the animation
        _transition.SetTrigger("start");

        //Waits for the animation to be finished (1 second)
        yield return new WaitForSeconds(1);

        //Saves the game
        Debug.Log("Saving From SceneLoader.cs");
        FindObjectOfType<SavingWrapper>().Save();

        //Loads the new scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }
}
