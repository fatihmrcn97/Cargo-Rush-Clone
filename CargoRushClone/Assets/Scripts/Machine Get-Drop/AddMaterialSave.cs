using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMaterialSave
{

     
    private string _saveName;


    private Vector3 outRotation;
    private StackSystem _stackSystem;
    private MachineController _machineController;
    public AddMaterialSave(Vector3 _outRotation,StackSystem stackSystem,MachineController machineController,string saveName)
    {
        outRotation = _outRotation;
        _stackSystem = stackSystem;
        _machineController = machineController;
        _saveName = saveName;
    }


    public void HaircutDeskSave()
    {
        int savedAmount = PlayerPrefs.GetInt(_saveName);
        if (savedAmount <= 0) return;
        for (int i = 0; i < savedAmount; i++)
        {
            // var createdSavedObj = Object.Instantiate(UIManager.instance.dirtyAnimalList[Random.Range(0, 6)],
            //     _stackSystem.materialDropPos.position, Quaternion.Euler(outRotation));
            //
            // _machineController.convertedMaterials.Add(createdSavedObj);
            // createdSavedObj.GetComponent<IAnimal>().SetAnimalProgression(0);
            // var childObjects = createdSavedObj.GetComponent<IAnimal>().AnimalParentObject();
            // childObjects.transform.GetChild(0).gameObject.SetActive(false);
            // _stackSystem.DropPointHandle();
        }
    }

    public void WashingAreaSave()
    {
        int savedAmount = PlayerPrefs.GetInt(_saveName);
        if (savedAmount <= 0) return;
        for (int i = 0; i < savedAmount; i++)
        {
            // var createdSavedObj = Object.Instantiate(UIManager.instance.dirtyAnimalList[Random.Range(0, 6)],
            //     _stackSystem.materialDropPos.position, Quaternion.Euler(outRotation));
            //
            // _machineController.convertedMaterials.Add(createdSavedObj);
            //
            // var childObjects = createdSavedObj.GetComponent<IAnimal>().AnimalParentObject();
            // childObjects.transform.GetChild(0).gameObject.SetActive(false);
            // _stackSystem.DropPointHandle();
            //
            // createdSavedObj.GetComponent<IAnimal>().SetAnimalProgression(1);
            // var allHair = childObjects.transform.GetChild(1); // Cutted The Hair
            // foreach (Transform hair in allHair)
            //     hair.gameObject.SetActive(false);
        }
    }  
 
 
}
