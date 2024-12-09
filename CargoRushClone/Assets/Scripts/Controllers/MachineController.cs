using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class MachineController : MonoBehaviour, IMachineController
{
    [HideInInspector] public List<GameObject> convertedMaterials;

    [SerializeField] protected Transform material_machine_enter_pos;

    public ISave getMaterialSave;

    public IStackSystem _stackSystem;

    [HideInInspector] public Animator anim;

    [HideInInspector] public AddMaterialToMachine _addMaterialToMachine;

    [HideInInspector] public GetMaterialFromMachine _getMaterialFromMachine;

    protected float animStartValue;

    public List<IStackSystem> _stackSystems;

    public ISave AddMaterialSaveCollectable;
    
    

    public virtual void StartWashing()
    {
    }

    public virtual void Press_Finished()
    {
    }

    public void MachineUpgradeUpdate(int currentLevel)
    {
        anim.SetFloat(TagManager.ANIM_SPEED_FLOAT, animStartValue + currentLevel);
    }



    // function 


}