using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMaterialSave 
{

    private Vector3 outRotation;
    private MachineController _machineController;
    private List<GameObject> singleMaterial;
    private string _saveName;


    public GetMaterialSave(Vector3 _outRotation,MachineController machineController,List<GameObject> singleMaterialList,string saveName)
    {
        outRotation = _outRotation;
        _machineController = machineController;
        singleMaterial = singleMaterialList;
        _saveName = saveName;
    }


    public void HaircutDeskSave()
    {
        int savedAmount = PlayerPrefs.GetInt(_saveName);
        if (savedAmount <= 0) return;
        for (int i = 0; i < savedAmount; i++)
        {
            // var createdSavedObj = Object.Instantiate(UIManager.instance.dirtyAnimalList[Random.Range(0, 6)],
            //     _machineController._stackSystem.materialDropPos.position, Quaternion.Euler(outRotation));
            //
            // singleMaterial.Add(createdSavedObj);
            // createdSavedObj.GetComponent<IAnimal>().SetAnimalProgression(1);
            // var childObjects = createdSavedObj.GetComponent<IAnimal>().AnimalParentObject();
            // childObjects.transform.GetChild(0).gameObject.SetActive(false);
            // _machineController._stackSystem.DropPointHandle();
            // childObjects.transform.GetChild(0).transform.GetComponent<SkinnedMeshRenderer>().material = _machineController.CageOutMaterial();
            // var allHair = childObjects.transform.GetChild(1); // Cutted The Hair
            // foreach (Transform hair in allHair)
            //     hair.gameObject.SetActive(false);
        }
    }

    public void WashingAreaSave()
    {
        int savedAmount = PlayerPrefs.GetInt(_saveName);
        if (savedAmount <= 0) return;
        for (int i = 0; i < savedAmount; i++)
        {
            // var createdSavedObj = Object.Instantiate(UIManager.instance.dirtyAnimalList[Random.Range(0, 5)],
            //     _machineController._stackSystem.materialDropPos.position, Quaternion.Euler(outRotation));
            //
            // singleMaterial.Add(createdSavedObj);
            //
            // createdSavedObj.GetComponent<IAnimal>().SetAnimalProgression(2);
            // var childObjects = createdSavedObj.GetComponent<IAnimal>().AnimalParentObject();
            // childObjects.transform.GetChild(0).gameObject.SetActive(false);
            //
            // childObjects.transform.GetChild(0).transform.GetComponent<SkinnedMeshRenderer>().material = _machineController.CageOutMaterial();
            //
            // _machineController._stackSystem.DropPointHandle();
            //
            // var allHair = childObjects.transform.GetChild(1); // Cutted The Hair
            // foreach (Transform hair in allHair)
            //     hair.gameObject.SetActive(false);
            //
            //
            // _machineController.WashingEffects(childObjects);
        }
    }

    
}
