using System;
using UnityEngine;

/// <summary>
/// 空心球/火球空心球/玩家接球失误动画特效.
/// </summary>
public class SSKongXinQiuTXAni : SSGameMono
{
    /// <summary>
    /// 特效材质.
    /// </summary>
    public Material m_MatTX;
    /// <summary>
    /// 动画播放时间控制器.
    /// </summary>
    SSTimeUpCtrl m_TimeUpCom;
    public enum TeXiaoState
    {
        /// <summary>
        /// 普通空心球.
        /// </summary>
        PuTongKongXinQiu = 0,
        /// <summary>
        /// 火球空心球.
        /// </summary>
        HuoQiuKongXinQiu = 1,
        /// <summary>
        /// 接球失误.
        /// </summary>
        JieQiuShiWu = 2,
    }
    /// <summary>
    /// 动画控制器.
    /// </summary>
    Animator m_TeXiaoAni;
    public void Init()
    {
        m_TeXiaoAni = gameObject.GetComponent<Animator>();
        if (m_TeXiaoAni != null)
        {
            m_TeXiaoAni.enabled = false;
        }

        if (m_MatTX != null)
        {
            m_MatTX.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    /// <summary>
    /// 播放游戏进球特效.
    /// </summary>
    public void PlayGameTeXiaoAni(TeXiaoState type)
    {
        if (m_TeXiaoAni != null)
        {
            if (type == TeXiaoState.HuoQiuKongXinQiu
                || type == TeXiaoState.PuTongKongXinQiu)
            {
                //部分模式现在去掉闪屏逻辑.
                //修改为产生粒子特效.
                return;
            }
            m_TeXiaoAni.enabled = true;
            string aniTrigger = type.ToString();
            m_TeXiaoAni.SetTrigger(aniTrigger);

            /*if (m_TimeUpCom != null)
            {
                Destroy(m_TimeUpCom);
            }
            m_TimeUpCom = gameObject.AddComponent<SSTimeUpCtrl>();
            m_TimeUpCom.Init(0.8f);
            m_TimeUpCom.OnTimeUpOverEvent += OnTimeUpOverEvent;*/
        }
    }

    private void OnTimeUpOverEvent()
    {
        //强制更换材质颜色.
        if (m_MatTX != null)
        {
            Color white = new Color(1f, 1f, 1f, 1f);
            if (m_MatTX.color != white)
            {
                //Debug.Log("*********************************************change color...");
                m_MatTX.color = white;
            }
        }
    }

    /// <summary>
    /// 动画结束事件触发器.
    /// </summary>
    public override void OnEndAnimationTrigger()
	{
        /*if (m_TeXiaoAni != null)
        {
            m_TeXiaoAni.enabled = false;
        }*/

        if (m_MatTX != null)
        {
            m_MatTX.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}