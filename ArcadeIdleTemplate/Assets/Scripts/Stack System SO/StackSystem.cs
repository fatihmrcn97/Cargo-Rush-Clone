using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackSystem : MonoBehaviour , IStackSystem
{
    public Transform materialDropPos;

    [SerializeField] StackSO stackData;

    private Vector3 startPosOfDropPos;
    private bool oneTime = true;
    private int totalLineAndColumn;
    private int height;

    private int line = 0, column = 0; // Line Z ' de haraktet eden // Column X ' de haraketer
    [SerializeField] private int _x = 1, _y = 1;
    private void Start()
    {
        startPosOfDropPos = materialDropPos.position;
        Initialize(_x, _y);
    }

    public void Initialize(int x, int y)
    {
        totalLineAndColumn = (x + 1) * (y + 1);
        height = 0;
    }

    public void DropPointHandle()
    {
        if (line < _x)
        {
            // ilk yerleşmeden sonra bu kod çalışıyor 
            line++;
            var currentPosition = materialDropPos.transform.position;
            currentPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + stackData.Z);
            materialDropPos.transform.position = currentPosition;
        }
        else
        {
            if (column < _y)
            {
                column++;
                var currentPosition = materialDropPos.transform.position;
                currentPosition = new Vector3(currentPosition.x + stackData.X, currentPosition.y, currentPosition.z - (line * stackData.Z));
                line = 0;
                materialDropPos.transform.position = currentPosition;
            }
            else
            {
                var currentPosition = materialDropPos.transform.position;
                currentPosition = new Vector3(currentPosition.x - ((column) * stackData.X), currentPosition.y + stackData.Y, currentPosition.z - (line * stackData.Z));
                line = 0;
                column = 0;
                materialDropPos.transform.position = currentPosition;
            }
        }
    }
    public void SetTheStackPositonBack(int stackMaterialCount)
    {
        CalculatePos(stackMaterialCount);
        if (stackMaterialCount == 0)
        {
            line = 0;
            column = 0;
        }
    }


    private void CalculatePos(int stackCount)
    {
        height = stackCount / totalLineAndColumn;
        if ((stackCount + 1) % totalLineAndColumn == 0)
        {
            column = _y;
            line = _x;
        }
        else
        {
            if (column > 0 && line == 0)
            {
                if (column == 0) column = _y;
                column--;
                line = _x;
                oneTime = false;
            }
            if (line > 0 && oneTime) line--;
        }
        oneTime = true;
        float xPos = (stackData.X * column) + startPosOfDropPos.x;
        float yPos = startPosOfDropPos.y + (height * stackData.Y);
        float zPos = (stackData.Z * line) + startPosOfDropPos.z;

        materialDropPos.position = new Vector3(xPos, yPos, zPos);
    }

    public Transform MaterialDropPositon()
    {
        return materialDropPos;
    }
}


