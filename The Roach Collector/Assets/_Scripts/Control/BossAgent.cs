using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    static Animator thisAnim;
    [SerializeField]
    FieldOfView fov;
    [SerializeField]
    AIHealth _brickHealth;

    // Start is called before the first frame update
    void Awake()
    {
        thisAnim = GetComponent<Animator>();
        fov = GetComponent<FieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fov.IsEnemyInFOV)
        {
            Debug.Log("player detected!");
            thisAnim.SetTrigger("isDetected");
        }
    }
}
