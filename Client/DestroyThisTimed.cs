using UnityEngine;
using System.Collections;

public class DestroyThisTimed : MonoBehaviour
{
    public enum DestroyState
    {
        Null = -1,
        /// <summary>
        /// 篮环爆炸粒子.
        /// </summary>
        LanHuanExp = 0,
    }
    /// <summary>
    /// 删除对象的类型.
    /// </summary>
    DestroyState m_DestroyState;
    /// <summary>
    /// 删除时间.
    /// </summary>
	[Range(0f, 100f)]
    public float TimeRemove = 5f;
    // Use this for initialization
    void Start()
	{
		//Debug.Log("DestroyThisTimed -> objName "+gameObject.name);
		//Destroy(gameObject, TimeRemove);
        //Invoke("DelayDestroyThis", TimeRemove);
        StartCoroutine(DelayDestroyThis(TimeRemove));
	}

    public void Init(DestroyState type)
    {
        m_DestroyState = type;
    }

    IEnumerator DelayDestroyThis(float time)
    {
        yield return new WaitForSeconds(time);
        switch (m_DestroyState)
        {
            case DestroyState.LanHuanExp:
                {
                    SSGameDataCtrl.GetInstance().RemoveLanHuanExplosionToList(gameObject);
                    break;
                }
        }
        Destroy(gameObject);
    }
}
