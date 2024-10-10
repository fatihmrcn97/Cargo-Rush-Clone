using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsFunctionController : MonoBehaviour
{

    private MachineController machineController;

    private void Start()
    {
        machineController = GetComponentInParent<MachineController>();
    }



    public void PressFinished()
    {
        machineController.Press_Finished();
    }

}
