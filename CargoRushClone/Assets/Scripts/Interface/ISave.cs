using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISave
{
     void SaveData(int prefabIndex);
     void RemoveData(int prefabIndex);
     void LoadData();
}
