using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitRoot
{
    /// <summary>
    /// 初始化根
    /// </summary>
    /// <param name="parent">根</param>
    /// <returns>数组</returns>
    public static Transform[] InitTarget(Transform parent)
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