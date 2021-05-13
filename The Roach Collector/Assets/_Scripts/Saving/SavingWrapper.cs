using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.Saving;

namespace TGP.Saving
{

}
public class SavingWrapper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save() {
        FindObjectOfType<SavingSystem>().Save("save.dat");
    }

    public void Load() {
        FindObjectOfType<SavingSystem>().Load("save.dat");
    }
}
