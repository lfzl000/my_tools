using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllChild : MonoBehaviour
{
    void Start()
    {
        var allChild = InitRoot.InitTarget(transform);
        for (int i = 0; i < allChild.Length; i++)
        {
            Destroy(allChild[i].gameObject);
            Debug.Log("已删除" + allChild[i]);
        }
    }
}