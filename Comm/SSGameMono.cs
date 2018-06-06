using UnityEngine;

public class SSGameMono : MonoBehaviour
{
    /// <summary>
    /// 产生预制.
    /// </summary>
    public Object Instantiate(GameObject prefab, Transform parent)
    {
        GameObject obj = (GameObject)Instantiate(prefab);
        if (parent != null)
        {
            obj.transform.SetParent(parent);
            obj.transform.localScale = prefab.transform.localScale;
            obj.transform.localPosition = prefab.transform.localPosition;
        }
        return obj;
    }

    public void UnityLog(object msg)
    {
        Debug.Log("Unity: " + msg);
    }

    public void UnityLogWarning(object msg)
    {
        Debug.LogWarning("Unity: " + msg);
    }

    public void UnityLogError(object msg)
    {
        Debug.LogError("Unity: " + msg);
    }

    /// <summary>
    /// 动画结束事件.
    /// </summary>
    public void OnEndAnimationEvent()
    {
        OnEndAnimationTrigger();
    }

    /// <summary>
    /// 动画结束事件触发器.
    /// </summary>
    public virtual void OnEndAnimationTrigger()
    {
    }
}