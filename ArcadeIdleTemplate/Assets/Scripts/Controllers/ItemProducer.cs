using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ItemProducer : MonoBehaviour
{
    [SerializeField] Transform instantiatePos;

    [SerializeField] private GameObject materialPrefab;

    [HideInInspector] public StackSystem _stackSystem;

    private bool isTruckAlreadyMoving;

    private int createdPackageCount;
    [SerializeField] private int maxPackageCount = 10;

    private MaterialCollectPoint _materialPlaceController;

    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        InvokeRepeating(nameof(TruckMovement), 1, 10);
        _stackSystem = GetComponent<StackSystem>();
        _materialPlaceController = GetComponent<MaterialCollectPoint>();
    }


    private void TruckMovement()
    {
        if (isTruckAlreadyMoving) return;
        if (_materialPlaceController.singleMaterial.Count >= maxPackageCount) return;
        isTruckAlreadyMoving = true;

        StartCoroutine(DropPackages());

    }
    private IEnumerator DropPackages()
    {
        yield return new WaitForSeconds(.2f);
        GameObject material = Instantiate(materialPrefab, instantiatePos.position, Quaternion.identity, null);
        material.transform.DOJump(_stackSystem.materialDropPos.position, .4f, 1, .4f).OnComplete(() =>
        {
            _materialPlaceController.singleMaterial.Add(material);
            material.transform.position = _stackSystem.materialDropPos.position;
            _stackSystem.DropPointHandle();
            createdPackageCount++;
            if (createdPackageCount < maxPackageCount)
            {
                StartCoroutine(DropPackages());
            }
            else
            {
                TruckGoFirstPlace();
            }
        });
    }
    private void TruckGoFirstPlace()
    { 
        isTruckAlreadyMoving = false;
        createdPackageCount = 0; 
    }

}