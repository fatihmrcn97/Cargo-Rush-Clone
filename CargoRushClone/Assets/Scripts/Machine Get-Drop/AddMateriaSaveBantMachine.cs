using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AddMateriaSaveBantMachine : ISave
{
    private List<int> saveItems;
    private IStackSystem _stackSystem;
    private MachineController _machineController;

    private string FilePathBantMachine;

    public AddMateriaSaveBantMachine(IStackSystem stackSystem, MachineController machineController, string saveName)
    {
        FilePathBantMachine = Path.Combine(Application.persistentDataPath, saveName + "saveBantMachine.json");
        _stackSystem = stackSystem;
        _machineController = machineController;
        saveItems = LoadFromFile();
    }

    private void SaveToFile()
    {
        string json = JsonUtility.ToJson(new SaveDataClass { Items = saveItems });
        File.WriteAllText(FilePathBantMachine, json);
    }
    
    private List<int> LoadFromFile()
    {
        if (File.Exists(FilePathBantMachine))
        {
            string json = File.ReadAllText(FilePathBantMachine);
            SaveDataClass dataClass = JsonUtility.FromJson<SaveDataClass>(json);
            saveItems = dataClass.Items ?? new List<int>();

            return saveItems;
        }
        else
        {
            saveItems = new List<int>();
            return new List<int>();
        }
    }


    public void SaveData(int prefabIndex)
    {
        saveItems.Add(prefabIndex);
        SaveToFile();
    }

    public void RemoveData(int prefabIndex)
    {
        saveItems.Remove(prefabIndex);
        SaveToFile();
    }

    public void LoadData()
    {
        BantItemisCreateSavedObjects();
    }

    private void BantItemisCreateSavedObjects()
    {
        int savedAmount = saveItems.Count;
        if (savedAmount <= 0) return;
        for (int i = 0; i < savedAmount; i++)
        {
            var createdSavedObj = Object.Instantiate(
                UIManager.instance.boxItems[saveItems[i]],
                _stackSystem.MaterialDropPositon().position,
                Quaternion.Euler(Vector3.zero));
            
            createdSavedObj.GetComponentInChildren<Animator>().SetTrigger("close");
            createdSavedObj.GetComponent<IItem>().SetStatus(ItemStatus.boxedItem,TapedItemStatus.nonTapped);
            _machineController.convertedMaterials.Add(createdSavedObj);
            _stackSystem.DropPointHandle();
        }
    }

    private class SaveDataClass
    {
        public List<int> Items;
    }
    
}