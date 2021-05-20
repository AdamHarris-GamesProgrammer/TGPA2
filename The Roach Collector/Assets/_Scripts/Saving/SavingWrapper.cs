using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.Saving;

public class SavingWrapper : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
    }

    public void Save()
    {
        GetComponent<SavingSystem>().Save(Application.persistentDataPath + "save.sav");
    }

    public void Load()
    {
        GetComponent<SavingSystem>().Load(Application.persistentDataPath + "save.sav");
    }

    public void Save() {
        FindObjectOfType<SavingSystem>().Save("save.dat");
    }

    public void Load() {
        FindObjectOfType<SavingSystem>().Load("save.dat");
    }
}
