using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Harris.Saving;
using System;

public class SavingWrapper : MonoBehaviour
{
#if UNITY_EDITOR
    public void Update()
    {
        //Development test keys
        if (Input.GetKeyDown(KeyCode.O)) Save();
        if (Input.GetKeyDown(KeyCode.L)) Load();
        if (Input.GetKeyDown(KeyCode.Delete)) Delete();
    }
#endif 

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
