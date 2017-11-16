using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper {
    
    /// <summary>
    /// Check if a layer is inside a LayerMask.
    /// 
    /// Credit: http://answers.unity3d.com/answers/1332280/view.html
    /// </summary>
    /// 
    /// <param name="mask">The layer mask.</param>
    /// <param name="layer">The layer.</param>
    /// <returns>True if the layer is inside a LayerMask.</returns>
    public static bool Contains(this LayerMask mask, int layer) {
        return mask == (mask | (1 << layer));
    }

}
