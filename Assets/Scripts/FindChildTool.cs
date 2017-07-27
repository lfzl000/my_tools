using UnityEngine;

/// <summary>
/// 查找物体工具类
/// </summary>
public class FindChildTool
{
    /// <summary>
    /// 查找子物体
    /// </summary>
    /// <param name="trans">要查找的物体</param>
    /// <param name="childName">物体名称</param>
    /// <returns>找到的物体</returns>
	public static GameObject FindChild(Transform trans, string childName)
    {
        Transform child = trans.Find(childName);
        if (child != null)
        {
            return child.gameObject;
        }
        int count = trans.childCount;
        GameObject go = null;
        for (int i = 0; i < count; ++i)
        {
            child = trans.GetChild(i);
            go = FindChild(child, childName);
            if (go != null)
                return go;
        }
        return null;
    }
    public static T FindChild<T>(Transform trans, string childName) where T : Component
    {
        GameObject go = FindChild(trans, childName);
        if (go == null)
            return null;
        return go.GetComponent<T>();
    }

    public static void DeleteAllChild()
    {
        
    }
}