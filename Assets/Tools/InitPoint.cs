using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPoint
{
    /// <summary>
    /// 初始化路点
    /// </summary>
    /// <param name="parent">路点根</param>
    /// <returns>路点数组</returns>
    public static Transform[] InitPatrolTargetPos(Transform parent)
    {
        Transform[] children;
        if (parent != null && parent.childCount > 0)
        {
            children = new Transform[parent.childCount];
            for (int i = 0; i < parent.childCount; i++)
                children[i] = parent.GetChild(i);
            return children;
        }
        return null;
    }
}