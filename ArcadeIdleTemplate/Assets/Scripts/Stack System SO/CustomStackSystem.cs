using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomStackSystem : MonoBehaviour, IStackSystem
{
    private Transform materialDropPosition;

    [SerializeField] private List<Transform> dropTransforms;

    private int index = 0;
    private int height = 0;
    private void Awake()
    {
        GameObject obj = new("NewGameObject");
        obj.transform.parent = transform;
        materialDropPosition = obj.transform;

        materialDropPosition.
            SetPositionAndRotation(dropTransforms[0].position,
            dropTransforms[0].rotation);
    }

    public void DropPointHandle()
    {
        index++;
        if(index >= dropTransforms.Count)
        {
            index = 0;
            height++;
            materialDropPosition.
                SetPositionAndRotation(dropTransforms[index].position + new Vector3(0,.25f*height,0),
                dropTransforms[index].rotation);
        }
        else if(height> 0) 
        {
            materialDropPosition.
                SetPositionAndRotation(dropTransforms[index].position + new Vector3(0,.25f*height,0),
                dropTransforms[index].rotation);
        }
        else
        {
            materialDropPosition.
                          SetPositionAndRotation(dropTransforms[index].position,
                          dropTransforms[index].rotation);
        }

    }

    public Transform MaterialDropPositon()
    {
        return materialDropPosition;
    }

    public void SetTheStackPositonBack(int stackMaterialCount)
    {
        if (stackMaterialCount <= 0)
        {
            materialDropPosition.SetPositionAndRotation(dropTransforms[0].position,
                dropTransforms[0].rotation);

            height = 0;
            index = 0;
            return;
        }

        height = stackMaterialCount/dropTransforms.Count;
        index = (stackMaterialCount % dropTransforms.Count);
         

        materialDropPosition.SetPositionAndRotation
            (dropTransforms[index].position + new Vector3(0, .25f * height, 0),
            dropTransforms[index].rotation);
    }
}
