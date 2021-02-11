using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizeData : MonoBehaviour
{
    public GameObject cubePrefab;

    GameObject[] cubes = new GameObject[1024];

    private void Start()
    {
        for(int i = 0; i<1024;i++)
        {
            GameObject instanceCube = (GameObject)Instantiate(cubePrefab);
            instanceCube.transform.position = this.transform.position;
            instanceCube.transform.parent = this.transform;
            instanceCube.name = "Cube " + i;
            this.transform.eulerAngles = new Vector3(0, (float)(-0.3515625 * i), 0);
            instanceCube.transform.position = Vector3.forward * 100;
            cubes[i] = instanceCube;
        }
    }

    private void Update()
    {
        for(int i = 0; i<cubes.Length; i++)
        {
            if(cubes!= null)
            {
                cubes[i].transform.localScale = new Vector3(10, (AudioController.samples[i] * 100000) + 1, 10);
            }
        }
    }

}
