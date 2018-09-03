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
    /// 场景贴图.
    /// m_SceneImgArray[0] -> 黑白,没有激活游戏.
    /// m_SceneImgArray[1] -> 彩色,激活游戏.
    /// </summary>
    public Texture[] m_SceneImgArray = new Texture[2];
    [System.Serializable]
    public class ScreenImgData
    {
        /// <summary>
        /// 场景贴图.
        /// m_SceneImgArray[0] -> 黑白,没有激活游戏.
        /// m_SceneImgArray[1] -> 彩色,激活游戏.
        /// </summary>
        public Texture[] SceneImgArray = new Texture[2];
    }
    /// <summary>
    /// 游戏场景背景图数据.
    /// </summary>
    public ScreenImgData[] m_ScreenImgData = new ScreenImgData[2];
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
    //RuntimeAnimatorController m_RuntimeAni;
    /// <summary>
    /// 初始化.
    /// </summary>
    public void Init()
    {
        m_TeXiaoAni = gameObject.GetComponent<Animator>();
        //m_RuntimeAni = m_TeXiaoAni.runtimeAnimatorController;
        if (m_TeXiaoAni != null)
        {
            m_TeXiaoAni.enabled = false;
        }

        if (m_MatTX != null)
        {
            m_MatTX.color = new Color(1f, 1f, 1f, 1f);
        }
        ChangePlayerSceneImg(false);
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
            m_TeXiaoAni.ResetTrigger(aniTrigger);
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

    /// <summary>
    /// 改变玩家当前游戏场景的背景贴图.
    /// </summary>
    internal void ChangePlayerSceneImg(bool isActiveGame)
    {
        if (m_MatTX != null && m_SceneImgArray.Length >= 2)
        {
            Texture img = m_SceneImgArray[isActiveGame == true ? 1 : 0];
            if (img != null)
            {
                m_MatTX.mainTexture = img;
            }
        }
    }

    /// <summary>
    /// 设置游戏背景图的资源信息.
    /// </summary>
    public void SetScreenImgInfo(int index)
    {
        int indexTmp = index % m_ScreenImgData.Length;
        if (m_ScreenImgData[indexTmp] != null)
        {
            for (int i = 0; i < m_ScreenImgData[indexTmp].SceneImgArray.Length; i++)
            {
                if (m_ScreenImgData[indexTmp].SceneImgArray[i] != null)
                {
                    m_SceneImgArray[i] = m_ScreenImgData[indexTmp].SceneImgArray[i];
                }
            }
            ChangePlayerSceneImg(false);
        }
    }
}