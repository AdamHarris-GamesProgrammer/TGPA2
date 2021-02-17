using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MC_Controller : MonoBehaviour
{

    public float AxisLR;
    public float AxisUD;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AxisLR = Input.GetAxis("Horizontal");
        animator.SetFloat("MovingLR", AxisLR);

        AxisUD = Input.GetAxis("Vertical");
        animator.SetFloat("MovingUD", AxisUD);

    }
}
