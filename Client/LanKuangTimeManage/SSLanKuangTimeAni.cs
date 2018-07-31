using UnityEngine;

/// <summary>
/// 该组件必须放置在控制篮筐倒计时的动画控制器对象上.
/// </summary>
public class SSLanKuangTimeAni : SSGameMono
{
    /// <summary>
    /// 篮环UV控制脚本.
    /// </summary>
    public SSLanHuanUV m_LanHuanUV;
    /// <summary>
    /// 倒计时音效.
    /// </summary>
    public AudioSource m_AudioDaoJiShi;
    /// <summary>
    /// 倒计时个位的缩放脚本.
    /// </summary>
    public TweenScale[] m_DaoJiShiScale = new TweenScale[2];
    /// <summary>
    /// 是否打开倒计时缩放.
    /// </summary>
    bool IsOpenDaoJiShiScale = false;

    public enum DaoJuState
    {
        /// <summary>
        /// 篮筐放大道具.
        /// </summary>
        LanKuangFangDa,
        /// <summary>
        /// 篮球减速道具.
        /// </summary>
        BallSlowSpeed,
    }

    public enum DaoJiShiState
    {
        /// <summary>
        /// 10秒倒计时.
        /// </summary>
        Time10 = 0,
        /// <summary>
        /// 15秒倒计时.
        /// </summary>
        Time15 = 1,
        /// <summary>
        /// 20秒倒计时.
        /// </summary>
        Time20 = 2,
        /// <summary>
        /// 25秒倒计时.
        /// </summary>
        Time25 = 3,
    }

    Animator m_Animator;
    /// <summary>
    /// 动画配置信息.
    /// m_RuntimeAniArray[0] - 5秒.
    /// m_RuntimeAniArray[1] - 10秒.
    /// m_RuntimeAniArray[2] - 15秒.
    /// m_RuntimeAniArray[3] - 20秒.
    /// </summary>
    public RuntimeAnimatorController[] m_RuntimeAniArray = new RuntimeAnimatorController[4];
    /// <summary>
    /// 玩家1时间对象.
    /// </summary>
    public GameObject m_TimeP1;
    /// <summary>
    /// 玩家2时间对象.
    /// </summary>
    public GameObject m_TimeP2;
    /// <summary>
    /// 那种道具的倒计时.
    /// </summary>
    DaoJuState m_DaoJuState;
    /// <summary>
    /// 玩家索引.
    /// </summary>
    SSGameDataCtrl.PlayerIndex m_PlayerIndex;
    /// <summary>
    /// 是否播放道具倒计时.
    /// </summary>
    [HideInInspector]
    public bool IsPlayTimeAni = false;

    public void Init()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        if (m_Animator != null)
        {
            m_Animator.enabled = false;
            m_Animator.runtimeAnimatorController = null;
        }
        SetActivePlayerTime(SSGameDataCtrl.PlayerIndex.Player01, false);
        SetActivePlayerTime(SSGameDataCtrl.PlayerIndex.Player02, false);

        if (m_LanHuanUV != null)
        {
            m_LanHuanUV.ResetUV();
        }

        CloseDaoJiShiScale();
    }
    
    public void PlayDaoJiShi(SSGameDataCtrl.PlayerIndex index, DaoJiShiState type, DaoJuState daoJu)
    {
        HiddenAllPlayerTime();
        IsPlayTimeAni = true;
        m_DaoJuState = daoJu;
        m_PlayerIndex = index;
        RuntimeAnimatorController daoJiShiRuntime = m_RuntimeAniArray[(int)type];
        switch (index)
        {
            case SSGameDataCtrl.PlayerIndex.Player01:
                {
                    SetActivePlayerTime(SSGameDataCtrl.PlayerIndex.Player01, true);
                    SetActivePlayerTime(SSGameDataCtrl.PlayerIndex.Player02, false);
                    break;
                }
            case SSGameDataCtrl.PlayerIndex.Player02:
                {
                    SetActivePlayerTime(SSGameDataCtrl.PlayerIndex.Player01, false);
                    SetActivePlayerTime(SSGameDataCtrl.PlayerIndex.Player02, true);
                    break;
                }
            case SSGameDataCtrl.PlayerIndex.Null:
                {
                    bool isActiveP1 = SSGameDataCtrl.GetInstance().m_PlayerData[(int)SSGameDataCtrl.PlayerIndex.Player01].IsActiveGame;
                    bool isActiveP2 = SSGameDataCtrl.GetInstance().m_PlayerData[(int)SSGameDataCtrl.PlayerIndex.Player02].IsActiveGame;
                    SetActivePlayerTime(SSGameDataCtrl.PlayerIndex.Player01, isActiveP1);
                    SetActivePlayerTime(SSGameDataCtrl.PlayerIndex.Player02, isActiveP2);
                    break;
                }
        }

        if (m_Animator != null && daoJiShiRuntime != null)
        {
            m_Animator.runtimeAnimatorController = daoJiShiRuntime;
            m_Animator.enabled = true;
        }

        if (m_LanHuanUV != null)
        {
            m_LanHuanUV.InitPlayUVAni(type);
        }
    }

    /// <summary>
    /// 动画结束事件.
    /// </summary>
    public override void OnEndAnimationTrigger()
    {
        HiddenAllPlayerTime();
        ResetPlayerDaoJuState();
    }

    void ResetPlayerDaoJuState()
    {
        //显示篮筐放大顶部提示.
        SSGameDataCtrl.GetInstance().m_SSUIRoot.m_DaoJuState = SSUIRootCtrl.DaoJuState.LanKuangFangDa;
        SSGameDataCtrl.GetInstance().m_SSUIRoot.SetActiveLanKuangFangDaDingBu(true);

        IsPlayTimeAni = false;
        switch (m_PlayerIndex)
        {
            case SSGameDataCtrl.PlayerIndex.Player01:
            case SSGameDataCtrl.PlayerIndex.Player02:
                {
                    if (m_DaoJuState == DaoJuState.LanKuangFangDa)
                    {
                        SSGameDataCtrl.GetInstance().m_LanKuang[(int)m_PlayerIndex].SetLanKuangScale(SSLanKuangCtrl.LanKuangScale.Normal);
                    }
                    else if (m_DaoJuState == DaoJuState.BallSlowSpeed)
                    {
                        SSGameDataCtrl.GetInstance().SetLanQiuMoveSpeedType(SSGameDataCtrl.LanQiuMoveSpeed.Normal);
                    }
                    break;
                }
            case SSGameDataCtrl.PlayerIndex.Null:
                {
                    if (m_DaoJuState == DaoJuState.LanKuangFangDa)
                    {
                        SSGameDataCtrl.GetInstance().m_LanKuang[(int)SSGameDataCtrl.PlayerIndex.Player01].SetLanKuangScale(SSLanKuangCtrl.LanKuangScale.Normal);
                        SSGameDataCtrl.GetInstance().m_LanKuang[(int)SSGameDataCtrl.PlayerIndex.Player02].SetLanKuangScale(SSLanKuangCtrl.LanKuangScale.Normal);
                    }
                    else if (m_DaoJuState == DaoJuState.BallSlowSpeed)
                    {
                        SSGameDataCtrl.GetInstance().SetLanQiuMoveSpeedType(SSGameDataCtrl.LanQiuMoveSpeed.Normal);
                    }
                    break;
                }
        }
    }

    /// <summary>
    /// 隐藏所有玩家的倒计时.
    /// </summary>
    public void HiddenAllPlayerTime()
    {
        IsPlayTimeAni = false;
        if (m_Animator != null)
        {
            m_Animator.enabled = false;
            m_Animator.runtimeAnimatorController = null;
        }
        SetActivePlayerTime(SSGameDataCtrl.PlayerIndex.Player01, false);
        SetActivePlayerTime(SSGameDataCtrl.PlayerIndex.Player02, false);

        if (m_LanHuanUV != null)
        {
            m_LanHuanUV.ResetUV();
        }
        CloseDaoJiShiScale();
    }
    
    /// <summary>
    /// 设置玩家倒计时现实状态
    /// </summary>
    public void SetActivePlayerTime(SSGameDataCtrl.PlayerIndex index, bool isActive)
    {
        if (isActive)
        {
            if (!SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].IsActiveGame
                || !SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].IsCreateGameBall)
            {
                return;
            }
        }

        switch (index)
        {
            case SSGameDataCtrl.PlayerIndex.Player01:
                {
                    if (m_TimeP1 != null)
                    {
                        m_TimeP1.SetActive(isActive);
                    }
                    break;
                }
            case SSGameDataCtrl.PlayerIndex.Player02:
                {
                    if (m_TimeP2 != null)
                    {
                        m_TimeP2.SetActive(isActive);
                    }
                    break;
                }
        }
    }

    /// <summary>
    /// 5秒倒计时触发器信息.
    /// </summary>
    public void OnTriggerDaoJiShiScale()
    {
        OpenDaoJiShiScale();
    }

    /// <summary>
    /// 打开倒计时缩放脚本.
    /// </summary>
    void OpenDaoJiShiScale()
    {
        if (IsOpenDaoJiShiScale)
        {
            return;
        }
        IsOpenDaoJiShiScale = true;

        if (m_AudioDaoJiShi != null)
        {
            m_AudioDaoJiShi.enabled = true;
            m_AudioDaoJiShi.Play();
        }

        for (int i = 0; i < m_DaoJiShiScale.Length; i++)
        {
            if (m_DaoJiShiScale[i] != null)
            {
                m_DaoJiShiScale[i].ResetToBeginning();
                m_DaoJiShiScale[i].enabled = true;
                m_DaoJiShiScale[i].PlayForward();
            }
        }
    }

    /// <summary>
    /// 关闭倒计时缩放脚本.
    /// </summary>
    void CloseDaoJiShiScale()
    {
        IsOpenDaoJiShiScale = false;
        if (m_AudioDaoJiShi != null)
        {
            m_AudioDaoJiShi.enabled = false;
        }

        for (int i = 0; i < m_DaoJiShiScale.Length; i++)
        {
            if (m_DaoJiShiScale[i] != null)
            {
                m_DaoJiShiScale[i].ResetToBeginning();
                m_DaoJiShiScale[i].enabled = false;
                m_DaoJiShiScale[i].transform.localScale = m_DaoJiShiScale[i].from;
            }
        }
    }
}