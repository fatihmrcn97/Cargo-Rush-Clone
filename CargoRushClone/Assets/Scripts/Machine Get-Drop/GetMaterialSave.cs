using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GetMaterialSave : ISave
{
    private List<int> savedMaterials;

    private string FilePath;

    private Vector3 outRotation = Vector3.zero;
    private MachineController _machineController;
    private List<GameObject> singleMaterial;
    private BoxTapingMachine _boxTapingMachine;
    private string _saveName;

    public GetMaterialSave(BoxTapingMachine boxTapingMachine, MachineController machineController,
        List<GameObject> singleMaterialList, string saveName)
    {
        if (boxTapingMachine != null)
            _boxTapingMachine = boxTapingMachine;
        _machineController = machineController;
        singleMaterial = singleMaterialList;
        _saveName = saveName;
        FilePath = Path.Combine(Application.persistentDataPath, saveName + "getMaterialItems.json");
        savedMaterials = LoadFromFile();
    }


    private void CollectableGetMaterials()
    {
        int savedAmount = savedMaterials.Count;
        if (savedAmount <= 0) return;
        // Save Name 0-1-2 => MaterialConverter altında 3 tane GetMaterialFromMachine var onları temsil ediyor |||||  3-4-5 Bant machine GetMaterial temsil ediyor |||| 6,7... bu şekilde sahnelerde indexledilklerimle alaklı bu sayılar

        switch (_saveName)
        {
            case "0":
            {
                int saveIndex = 0;
                for (int i = 0; i < savedAmount; i++)
                {
                    var createdSavedObj = Object.Instantiate(UIManager.instance.boxItems[saveIndex],
                        _machineController._stackSystems[saveIndex].MaterialDropPositon().position,
                        Quaternion.Euler(outRotation));
                    createdSavedObj.GetComponentInChildren<Animator>().SetTrigger("close"); 
                    createdSavedObj.GetComponent<IItem>().SetStatus(ItemStatus.boxedItem, TapedItemStatus.yellowBox);
                    singleMaterial.Add(createdSavedObj);
                    _machineController._stackSystems[saveIndex].DropPointHandle();
                }

                break;
            }
            case "1":
            {
                int saveIndex = 1;
                for (int i = 0; i < savedAmount; i++)
                {
                    var createdSavedObj = Object.Instantiate(UIManager.instance.boxItems[saveIndex],
                        _machineController._stackSystems[saveIndex].MaterialDropPositon().position,
                        Quaternion.Euler(outRotation));
                    createdSavedObj.GetComponentInChildren<Animator>().SetTrigger("close");
                    createdSavedObj.GetComponent<IItem>().SetStatus(ItemStatus.boxedItem, TapedItemStatus.pinkBox);
                    singleMaterial.Add(createdSavedObj);
                    _machineController._stackSystems[saveIndex].DropPointHandle();
                }

                break;
            }
            case "2":
            {
                int saveIndex = 2;
                for (int i = 0; i < savedAmount; i++)
                {
                    var createdSavedObj = Object.Instantiate(UIManager.instance.boxItems[saveIndex],
                        _machineController._stackSystems[saveIndex].MaterialDropPositon().position,
                        Quaternion.Euler(outRotation));
                    createdSavedObj.GetComponentInChildren<Animator>().SetTrigger("close");
                    createdSavedObj.GetComponent<IItem>().SetStatus(ItemStatus.boxedItem, TapedItemStatus.blueBox);
                    singleMaterial.Add(createdSavedObj);
                    _machineController._stackSystems[saveIndex].DropPointHandle();
                }

                break;
            }
            case "3":
            {
                int saveIndex = 3;
                for (int i = 0; i < savedAmount; i++)
                {
                    var createdSavedObj = Object.Instantiate(UIManager.instance.boxItems[saveIndex - 3],
                        _machineController._stackSystem.MaterialDropPositon().position, Quaternion.Euler(outRotation));
                    createdSavedObj.GetComponentInChildren<Animator>().SetTrigger("close");
                    createdSavedObj.GetComponent<IItem>().SetStatus(ItemStatus.boxedAndTaped, TapedItemStatus.yellowBox);
                    singleMaterial.Add(createdSavedObj);
                    _boxTapingMachine.CheckExtraPaletAddOrDelete(true);
                    _machineController._stackSystem.DropPointHandle();
                }
            }
                break;
            case "4":
            {
                int saveIndex = 4;
                for (int i = 0; i < savedAmount; i++)
                {
                    var createdSavedObj = Object.Instantiate(UIManager.instance.boxItems[saveIndex - 3],
                        _machineController._stackSystem.MaterialDropPositon().position, Quaternion.Euler(outRotation));
                    createdSavedObj.GetComponentInChildren<Animator>().SetTrigger("close");
                    createdSavedObj.GetComponent<IItem>().SetStatus(ItemStatus.boxedAndTaped, TapedItemStatus.pinkBox);
                    singleMaterial.Add(createdSavedObj);
                    _boxTapingMachine.CheckExtraPaletAddOrDelete(true);
                    _machineController._stackSystem.DropPointHandle();
                }
            }
                break;
            case "5":
            {
                int saveIndex = 5;
                for (int i = 0; i < savedAmount; i++)
                {
                    var createdSavedObj = Object.Instantiate(UIManager.instance.boxItems[saveIndex - 3],
                        _machineController._stackSystem.MaterialDropPositon().position, Quaternion.Euler(outRotation));
                    createdSavedObj.GetComponent<IItem>().SetStatus(ItemStatus.boxedAndTaped, TapedItemStatus.blueBox);
                    createdSavedObj.GetComponentInChildren<Animator>().SetTrigger("close");
                    singleMaterial.Add(createdSavedObj);
                    _boxTapingMachine.CheckExtraPaletAddOrDelete(true);
                    _machineController._stackSystem.DropPointHandle();
                }
            }
                break;
        }
    }


    private void SaveToFile()
    {
        string json = JsonUtility.ToJson(new SaveDataClass { Items = savedMaterials });
        File.WriteAllText(FilePath, json);
    }

    private List<int> LoadFromFile()
    {
        if (File.Exists(FilePath))
        {
            string json = File.ReadAllText(FilePath);
            SaveDataClass dataClass = JsonUtility.FromJson<SaveDataClass>(json);
            savedMaterials = dataClass.Items ?? new List<int>();

            return savedMaterials;
        }
        else
        {
            savedMaterials = new List<int>();
            return new List<int>();
        }
    }


    public void SaveData(int prefabIndex = 0)
    {
        savedMaterials.Add(prefabIndex);
        SaveToFile();
    }

    public void RemoveData(int prefabIndex = 0)
    {
        savedMaterials.Remove(prefabIndex);
        SaveToFile();
    }

    public void LoadData()
    {
        CollectableGetMaterials();
    }

    private class SaveDataClass
    {
        public List<int> Items;
    }
}