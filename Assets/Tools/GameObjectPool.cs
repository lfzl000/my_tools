using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// 对象池工具类
/// </summary>
public class GameObjectPool : MonoSingleton<GameObjectPool>
{
    /// <summary>
    /// 对象缓存
    /// </summary>
    private Dictionary<string, List<GameObject>> Cache = new Dictionary<string, List<GameObject>>();

    /// <summary>
    /// 将对象加入对象池
    /// </summary>
    /// <param name="key"></param>
    /// <param name="go"></param>
    private void Add(string key, GameObject go)
    {
        //将对象引用加入cache
        if (!Cache.ContainsKey(key))
            Cache.Add(key, new List<GameObject>());
        Cache[key].Add(go);
    }

    /// <summary>
    /// 销毁指定键对应的对象
    /// </summary>
    /// <param name="key"></param>
    public void Clear(string key)
    {
        //销毁指定键对应的对象
        if (Cache.ContainsKey(key))
        {
            while (Cache[key].Count > 0)
            {
                Destroy(Cache[key][0]);
                Cache[key].RemoveAt(0);
            }
            Cache.Remove(key);
        }
    }

    /// <summary>
    /// 销毁池中所有对象
    /// </summary>
    public void ClearAll()
    {
        //将池中所有对象销毁 并且清空缓存中所有对象引用
        var list = new List<string>(Cache.Keys);
        while (list.Count > 0)
        {
            Clear(list[0]);
            list.RemoveAt(0);
        }
    }

    /// <summary>
    /// 从池中创建对象
    /// </summary>
    /// <param name="key">对象所属的键</param>
    /// <param name="go">对象原型</param>
    /// <param name="postion">位置</param>
    /// <param name="quaternion">朝向</param>
    public GameObject CreateObject(string key, GameObject go, Vector3 position, Quaternion quaternion)
    {
        //如果池中有可用的对象(未激活的对象)，直接返回，
        GameObject tempGo = FindUsable(key);
        if (tempGo != null)
        {
            tempGo.SetActive(true);
            tempGo.transform.position = position;
            tempGo.transform.rotation = quaternion;
        }
        else //没有先创建，放入池，再返回
        {
            tempGo = Instantiate(go, position, quaternion) as GameObject;
            Add(key, tempGo);
        }
        tempGo.transform.parent = transform;
        return tempGo;
    }

    /// <summary>
    /// 在对象池中查找可用对象
    /// </summary>
    /// <param name="key">指定的键</param>
    private GameObject FindUsable(string key)
    {
        if (Cache.ContainsKey(key))
        {
            Cache[key].RemoveAll(p => p == null);
            return Cache[key].Find(p => !p.activeSelf);
        }
        return null;
    }

    /// <summary>
    /// 立即回收不在使用的对象
    /// </summary>
    /// <param name="go">回收的对象</param>
    public void CollectObject(GameObject go)
    {
        //将对象设为非活动状态
        go.SetActive(false);
    }

    /// <summary>
    /// 延时回收对象
    /// </summary>
    /// <param name="go">回收的对象</param>
    /// <param name="delay">延迟的时间</param>
    public void CollectObject(GameObject go, float delay)
    {
        //等待delay时间后，再设为非活动状态
        StartCoroutine(DelayCollect(go, delay));
    }

    /// <summary>
    /// 延时回收协程工作方法
    /// </summary>
    private IEnumerator DelayCollect(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        CollectObject(go);
    }
}