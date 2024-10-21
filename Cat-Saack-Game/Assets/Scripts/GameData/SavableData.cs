using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SavableData
{
    // save & load dynamic variables using SaveManager
    public void SaveData();
    public void LoadData();
}
