using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AIStackController : MonoBehaviour , IItemList
{
    public List<GameObject> stackedMaterials; 

    public List<Transform> stackTransform;

    [SerializeField] private int maxStackCountBase = 3;

    [HideInInspector] public int maxStackCount;
    
    
    private void Start()
    {
        maxStackCount = maxStackCountBase;
    }

    public void MaxStackCountUpdated()
    {
        // maxStackCount = maxStackCountBase + PlayerPrefs.GetInt("maxCollected");
    }
    
    private void CheckMax()
    {
        Vibration.Vibrate(10);
        UIManager.instance.maxUI.SetActive(stackedMaterials.Count >= maxStackCount);
    }

    #region INTERFACE IMPLEMENTATIONS

    public List<GameObject> StackedMaterialList()
    {
        return stackedMaterials;
    }

    public List<Transform> StackTransforms()
    {
        return stackTransform;
    }
    public int MaxStackCount()
    {
        return maxStackCount;
    }

    public void StackPositionHandler()
    {
        for (int i = 0; i < stackedMaterials.Count; i++)
        {
            stackedMaterials[i].transform.SetParent(stackTransform[i]);
            stackedMaterials[i].transform.DOLocalMove(Vector3.zero, .1f);
        }
    }
    public bool CheckPlayerHandMax()
    {
        return StackedMaterialList().Count >= MaxStackCount();
    }

   

    #endregion


}
