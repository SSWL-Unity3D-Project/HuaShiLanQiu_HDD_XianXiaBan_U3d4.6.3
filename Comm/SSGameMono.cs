using UnityEngine;

public class SSGameMono : MonoBehaviour
{
    /// <summary>
    /// 产生预制.
    /// </summary>
    public Object Instantiate(GameObject prefab, Transform parent, Transform trPosRot = null)
    {
        GameObject obj = (GameObject)Instantiate(prefab, new Vector3(0f, -9999f, 0f), Quaternion.identity);
        if (parent != null)
        {
            obj.transform.SetParent(parent);
            if (trPosRot == null)
            {
                obj.transform.localScale = prefab.transform.localScale;
                obj.transform.localPosition = prefab.transform.localPosition;
            }
            else
            {
                obj.transform.position = trPosRot.position;
                obj.transform.rotation = trPosRot.rotation;
            }
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