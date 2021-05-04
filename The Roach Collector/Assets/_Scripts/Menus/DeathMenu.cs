using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    private bool isDead;
    private Health _health;
    private void Awake()
    {
        _health = GetComponent<Health>();
    }
    private void Update()
    {
        if (_health.IsDead())
        {
            SceneManager.LoadScene("DeathScreen");
        }
    }
    public void Respawn()
    {
        SceneManager.LoadScene("_Dev");
        Debug.Log("Button pressed");
    }
}
