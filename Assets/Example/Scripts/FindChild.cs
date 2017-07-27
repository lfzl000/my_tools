using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindChild : MonoBehaviour
{
    public string childName;

    void Start()
    {
        var child = FindChildTool.FindChild<Renderer>(transform, childName);
        if (child)
        {
            child.material.color = Color.red;
            Debug.Log("找到物体" + childName);
        }
        else
            Debug.Log("没有找到物体" + childName);
    }
}