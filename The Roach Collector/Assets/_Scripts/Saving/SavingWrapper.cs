using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.Saving;
using System;

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

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Delete();
        }
    }

    private void Delete()
    {
        GetComponent<SavingSystem>().Delete(Application.persistentDataPath + "save.sav");
    }

    public void Save()
    {
        GetComponent<SavingSystem>().Save(Application.persistentDataPath + "save.sav");
    }

    public void Load()
    {
        GetComponent<SavingSystem>().Load(Application.persistentDataPath + "save.sav");
    }
}
