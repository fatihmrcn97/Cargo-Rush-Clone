using System;
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

    private bool _isInTrigger;
    private Animator anim;
    private void Start()
    {
        maxStackCount = maxStackCountBase;
    }

    public void MaxStackCountUpdated()
    {
        // maxStackCount = maxStackCountBase + PlayerPrefs.GetInt("maxCollected");
    }

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
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
        anim.SetBool(TagManager.CARRY_BOOL_ANIM,stackedMaterials.Count>0);
        return StackedMaterialList().Count >= MaxStackCount();
    }

    public bool IsInTrigger { get; set; }

    #endregion


}
