using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper  
{

    public static string GetPoolName(TapedItemStatus tapedItemStatus)
    {
        return tapedItemStatus switch
        {
            TapedItemStatus.pinkBox => "pinkDuck",
            TapedItemStatus.yellowBox => "duck",
            TapedItemStatus.nonTapped => "",
            _ => ""
        };
    }  
    
}
