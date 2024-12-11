using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class AddMaterialSaveCollectable : ISave
{
    private List<int> saveCollectableAddMaterialItems;
    private IStackSystem _stackSystem;
    private MachineController _machineController;

    private string FilePathCollectableAddMachine =>
        Path.Combine(Application.persistentDataPath, "saveCollectableAddMaterialItems.json");

    public AddMaterialSaveCollectable(IStackSystem stackSystem, MachineController machineController)
    {
        _stackSystem = stackSystem;
        _machineController = machineController;
        saveCollectableAddMaterialItems = LoadFromFile();
    }

    private void CollectableConverterCreateSavedObjects()
    {
        int savedAmount = saveCollectableAddMaterialItems.Count;
        if (savedAmount <= 0) return;
        for (int i = 0; i < savedAmount; i++)
        {
            var createdSavedObj = Object.Instantiate(
                UIManager.instance.collectablePrefabs[saveCollectableAddMaterialItems[i]],
                _stackSystem.MaterialDropPositon().position,
                Quaternion.Euler(new Vector3(Random.Range(0, 90), Random.Range(0, 190), Random.Range(0, 290))));
            createdSavedObj.GetComponent<Rigidbody>().isKinematic = true;
            createdSavedObj.GetComponent<Rigidbody>().useGravity = false;
            createdSavedObj.GetComponent<BoxCollider>().isTrigger = true;
            createdSavedObj.transform.tag = TagManager.PACKABLE_ITEM;
            _machineController.convertedMaterials.Add(createdSavedObj);
            _stackSystem.DropPointHandle();
        }
    }

    private void SaveToFile()
    {
        string json = JsonUtility.ToJson(new SaveDataClass { Items = saveCollectableAddMaterialItems });
        File.WriteAllText(FilePathCollectableAddMachine, json);
    }

    private List<int> LoadFromFile()
    {
        if (File.Exists(FilePathCollectableAddMachine))
        {
            string json = File.ReadAllText(FilePathCollectableAddMachine);
            SaveDataClass dataClass = JsonUtility.FromJson<SaveDataClass>(json);
            saveCollectableAddMaterialItems = dataClass.Items ?? new List<int>();

            return saveCollectableAddMaterialItems;
        }
        else
        {
            saveCollectableAddMaterialItems = new List<int>();
            return new List<int>();
        }
    }

    public void SaveData(int prefabIndex)
    {
        saveCollectableAddMaterialItems.Add(prefabIndex);
        SaveToFile();
    }

    public void RemoveData(int prefabIndex)
    {
        saveCollectableAddMaterialItems.Remove(prefabIndex);
        SaveToFile();
    }

    public void LoadData()
    {
        CollectableConverterCreateSavedObjects();
    }
 
    private class SaveDataClass
    {
        public List<int> Items;
    }
}