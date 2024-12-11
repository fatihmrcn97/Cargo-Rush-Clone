using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper  
{

    public static string GetPoolName(CollectableTypes collectableTypes)
    {
        return collectableTypes switch
        {
            CollectableTypes.pinkduck => "pinkDuck",
            CollectableTypes.duck => "duck",
            CollectableTypes.blueduck => "blueDuck",
            _ => ""
        };
    }  
    
    public static TapedItemStatus GetRandomTaped(int totalCollectable)
    {
        int i = Random.Range(0, totalCollectable);
        return i switch
        {
            0 => TapedItemStatus.yellowBox,
            1 => TapedItemStatus.pinkBox,
            2 => TapedItemStatus.blueBox,
            _ => TapedItemStatus.yellowBox
        };
    }

    public static Sprite GetRandomSpriteAccordiongToTapedBox(TapedItemStatus tapedItemStatus)
    {
        return tapedItemStatus switch
        {
            TapedItemStatus.yellowBox => UIManager.instance.boxSprites[0],
            TapedItemStatus.pinkBox => UIManager.instance.boxSprites[1],
            TapedItemStatus.blueBox => UIManager.instance.boxSprites[2], 
            _ => UIManager.instance.boxSprites[0]
        };
    }
}
