using UnityEngine;

public class LanKuangFangDa : SSGameMono
{
    /// <summary>
    /// 取消框.
    /// </summary>
    public GameObject m_QuXiaoKuang;
    /// <summary>
    /// 选项框.
    /// </summary>
    public Transform m_XuanXiangKuang;
    /// <summary>
    /// 选项框坐标.
    /// </summary>
    public Vector2[] m_XuanXiangPosArray = new Vector2[5];
    [HideInInspector]
    public SSGameDataCtrl.XuanXiangState m_XuanXiangState = SSGameDataCtrl.XuanXiangState.XuanXiang04;
    /// <summary>
    /// 选项索引.
    /// </summary>
    int IndexXuanXiang = 0;
    /// <summary>
    /// 是否锁定界面.
    /// </summary>
    bool IsLockPanel = false;

    bool IsRemoveSelf = false;
    SSGameDataCtrl.PlayerIndex m_PlayerIndex = SSGameDataCtrl.PlayerIndex.Null;
    // Use this for initialization
    public void Init(SSGameDataCtrl.PlayerIndex indexVal)
    {
        m_PlayerIndex = indexVal;
        IndexXuanXiang = (int)m_XuanXiangState;
        SetActiveQuXiaoKuang(false);
        InputEventCtrl.GetInstance().ClickTVYaoKongEnterBtEvent += ClickTVYaoKongEnterBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongLeftBtEvent += ClickTVYaoKongLeftBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongRightBtEvent += ClickTVYaoKongRightBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongUpBtEvent += ClickTVYaoKongUpBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongDownBtEvent += ClickTVYaoKongDownBtEvent;
    }

    private void ClickTVYaoKongEnterBtEvent(InputEventCtrl.ButtonState val)
    {
        if (val == InputEventCtrl.ButtonState.DOWN)
        {
            return;
        }
        
        if (SSGameDataCtrl.GetInstance().m_SSUIRoot.m_ExitGameUI != null)
        {
            //退出游戏界面存在时,不响应消息.
            return;
        }

        if (IsLockPanel)
        {
            return;
        }

        switch (m_XuanXiangState)
        {
            case SSGameDataCtrl.XuanXiangState.XuanXiang05:
                {
                    //取消购买篮筐放大道具.
                    //删除购买篮筐放大道具界面.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameLanKuangFangDaPanel();
                    //SSGameDataCtrl.GetInstance().m_SSUIRoot.SetActiveLanKuangFangDaDingBu(true);
                    //延迟显示顶部购买提示.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.IsCloseDaoJuBuyDingBu = true;
                    break;
                }
            case SSGameDataCtrl.XuanXiangState.XuanXiang01:
            case SSGameDataCtrl.XuanXiangState.XuanXiang02:
            case SSGameDataCtrl.XuanXiangState.XuanXiang03:
            case SSGameDataCtrl.XuanXiangState.XuanXiang04:
                {
                    SetIsLockPanel(true);
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameLanKuangFangDa_ZhiFuPanel(m_PlayerIndex);
                    break;
                }

        }
    }

    private void ClickTVYaoKongLeftBtEvent(InputEventCtrl.ButtonState val)
    {
        if (val == InputEventCtrl.ButtonState.DOWN)
        {
            return;
        }
        
        if (SSGameDataCtrl.GetInstance().m_SSUIRoot.m_ExitGameUI != null)
        {
            //退出游戏界面存在时,不响应消息.
            return;
        }
        SetXuanXiangKuangPos(SSGameDataCtrl.XuanXiangMoveDir.Left);
    }

    private void ClickTVYaoKongRightBtEvent(InputEventCtrl.ButtonState val)
    {
        if (val == InputEventCtrl.ButtonState.DOWN)
        {
            return;
        }
        
        if (SSGameDataCtrl.GetInstance().m_SSUIRoot.m_ExitGameUI != null)
        {
            //退出游戏界面存在时,不响应消息.
            return;
        }
        SetXuanXiangKuangPos(SSGameDataCtrl.XuanXiangMoveDir.Right);
    }
    
    private void ClickTVYaoKongUpBtEvent(InputEventCtrl.ButtonState val)
    {
        if (val == InputEventCtrl.ButtonState.DOWN)
        {
            return;
        }

        if (SSGameDataCtrl.GetInstance().m_SSUIRoot.m_ExitGameUI != null)
        {
            //退出游戏界面存在时,不响应消息.
            return;
        }
        SetXuanXiangKuangVerticalPos(SSGameDataCtrl.XuanXiangMoveDir.Up);
    }

    private void ClickTVYaoKongDownBtEvent(InputEventCtrl.ButtonState val)
    {
        if (val == InputEventCtrl.ButtonState.DOWN)
        {
            return;
        }

        if (SSGameDataCtrl.GetInstance().m_SSUIRoot.m_ExitGameUI != null)
        {
            //退出游戏界面存在时,不响应消息.
            return;
        }
        SetXuanXiangKuangVerticalPos(SSGameDataCtrl.XuanXiangMoveDir.Down);
    }

    /// <summary>
    /// 垂直方向选项框索引.
    /// </summary>
    int IndexXuanXiangVer = 4;
    /// <summary>
    /// 设置垂直方向选项框的位置.
    /// </summary>
    void SetXuanXiangKuangVerticalPos(SSGameDataCtrl.XuanXiangMoveDir dir)
    {
        if (IsLockPanel || m_XuanXiangKuang == null)
        {
            return;
        }

        int offsetVal = dir == SSGameDataCtrl.XuanXiangMoveDir.Down ? 1 : -1;
        IndexXuanXiangVer += offsetVal;
        if (IndexXuanXiangVer < (int)SSGameDataCtrl.XuanXiangState.XuanXiang01)
        {
            //到达下极限.
            IndexXuanXiangVer = (int)SSGameDataCtrl.XuanXiangState.XuanXiang05;
        }

        if (IndexXuanXiangVer > (int)SSGameDataCtrl.XuanXiangState.XuanXiang05)
        {
            //到达上极限.
            IndexXuanXiangVer = (int)SSGameDataCtrl.XuanXiangState.XuanXiang01;
        }

        switch (IndexXuanXiangVer)
        {
            case 0:
                {
                    IndexXuanXiang = 0;
                    break;
                }
            case 1:
                {
                    IndexXuanXiang = 2;
                    break;
                }
            case 2:
                {
                    IndexXuanXiang = 4;
                    break;
                }
            case 3:
                {
                    IndexXuanXiang = 1;
                    break;
                }
            case 4:
                {
                    IndexXuanXiang = 3;
                    break;
                }
        }

        m_XuanXiangState = (SSGameDataCtrl.XuanXiangState)IndexXuanXiang;
        switch (m_XuanXiangState)
        {
            case SSGameDataCtrl.XuanXiangState.XuanXiang05:
                {
                    //取消框.
                    SetActiveQuXiaoKuang(true);
                    break;
                }
            default:
                {
                    //支付选项.
                    SetActiveQuXiaoKuang(false);
                    break;
                }
        }
        m_XuanXiangKuang.localPosition = m_XuanXiangPosArray[IndexXuanXiang];
        UnityLog("SetXuanXiangKuangVerticalPos -> dir == " + dir + ", m_XuanXiangState == " + m_XuanXiangState);
    }

    void SetXuanXiangKuangPos(SSGameDataCtrl.XuanXiangMoveDir dir)
    {
        if (IsLockPanel || m_XuanXiangKuang == null)
        {
            return;
        }

        int offsetVal = dir == SSGameDataCtrl.XuanXiangMoveDir.Right ? 1 : -1;
        IndexXuanXiang += offsetVal;
        if (IndexXuanXiang < (int)SSGameDataCtrl.XuanXiangState.XuanXiang01)
        {
            //到达右极限.
            IndexXuanXiang = (int)SSGameDataCtrl.XuanXiangState.XuanXiang05;
        }

        if (IndexXuanXiang > (int)SSGameDataCtrl.XuanXiangState.XuanXiang05)
        {
            //到达左极限.
            IndexXuanXiang = (int)SSGameDataCtrl.XuanXiangState.XuanXiang01;
        }
        m_XuanXiangState = (SSGameDataCtrl.XuanXiangState)IndexXuanXiang;
        
        switch (IndexXuanXiang)
        {
            case 0:
                {
                    IndexXuanXiangVer = 0;
                    break;
                }
            case 1:
                {
                    IndexXuanXiangVer = 3;
                    break;
                }
            case 2:
                {
                    IndexXuanXiangVer = 1;
                    break;
                }
            case 3:
                {
                    IndexXuanXiangVer = 4;
                    break;
                }
            case 4:
                {
                    IndexXuanXiangVer = 2;
                    break;
                }
        }

        switch (m_XuanXiangState)
        {
            case SSGameDataCtrl.XuanXiangState.XuanXiang05:
                {
                    //取消框.
                    SetActiveQuXiaoKuang(true);
                    break;
                }
            default:
                {
                    //支付选项.
                    SetActiveQuXiaoKuang(false);
                    break;
                }
        }
        m_XuanXiangKuang.localPosition = m_XuanXiangPosArray[IndexXuanXiang];
        UnityLog("SetXuanXiangKuangPos -> dir == " + dir + ", m_XuanXiangState == " + m_XuanXiangState);
    }

    /// <summary>
    /// 设置取消狂显示状态.
    /// </summary>
    void SetActiveQuXiaoKuang(bool isActive)
    {
        if (m_QuXiaoKuang != null)
        {
            m_QuXiaoKuang.SetActive(isActive);
        }

        if (m_XuanXiangKuang != null)
        {
            m_XuanXiangKuang.gameObject.SetActive(!isActive);
        }
    }

    /// <summary>
    /// 设置界面锁定状态.
    /// </summary>
    public void SetIsLockPanel(bool isLock)
    {
        IsLockPanel = isLock;
        gameObject.SetActive(!isLock);
    }

    public void RemoveSelf()
    {
        if (IsRemoveSelf)
        {
            return;
        }
        IsRemoveSelf = true;

        if (m_PlayerIndex != SSGameDataCtrl.PlayerIndex.Null)
        {
            if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsJieShouTiaoZhan)
            {
                //玩家PK模式下.
                for (int i = 0; i < SSGameDataCtrl.GetInstance().m_PlayerData.Length; i++)
                {
                    if (SSGameDataCtrl.GetInstance().m_BallSpawnArray[i].IndexCreatBallJieDuan == SSGameDataCtrl.GetInstance().IndexJieDuanFangDaLanKuang - 1)
                    {
                        //发球阶段在2时,增加发球阶段索引.
                        //SSGameDataCtrl.GetInstance().m_BallSpawnArray[i].AddIndexCreatBallJieDuan();
                        SSGameDataCtrl.GetInstance().m_CreatLanQiuStage.AddIndexCreatBallJieDuan();
                    }
                }
            }
            else
            {
                if (SSGameDataCtrl.GetInstance().m_BallSpawnArray[(int)m_PlayerIndex].IndexCreatBallJieDuan == SSGameDataCtrl.GetInstance().IndexJieDuanFangDaLanKuang - 1)
                {
                    //发球阶段在2时,增加发球阶段索引.
                    //SSGameDataCtrl.GetInstance().m_BallSpawnArray[(int)m_PlayerIndex].AddIndexCreatBallJieDuan();
                    SSGameDataCtrl.GetInstance().m_CreatLanQiuStage.AddIndexCreatBallJieDuan();
                }
            }
            //SSGameDataCtrl.GetInstance().TestResetPlayerLanKuang(m_PlayerIndex); //test.
        }
        else
        {
            //玩家PK或者玩家主动触发道具购买.
            for (int i = 0; i < SSGameDataCtrl.GetInstance().m_PlayerData.Length; i++)
            {
                if (SSGameDataCtrl.GetInstance().m_BallSpawnArray[i].IndexCreatBallJieDuan == SSGameDataCtrl.GetInstance().IndexJieDuanFangDaLanKuang - 1)
                {
                    //发球阶段在2时,增加发球阶段索引.
                    //SSGameDataCtrl.GetInstance().m_BallSpawnArray[i].AddIndexCreatBallJieDuan();
                    SSGameDataCtrl.GetInstance().m_CreatLanQiuStage.AddIndexCreatBallJieDuan();
                }
            }
        }

        SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameLanKuangFangDaDingBuPanel();
        SSGameDataCtrl.GetInstance().IsPauseGame = false;

        InputEventCtrl.GetInstance().ClickTVYaoKongEnterBtEvent -= ClickTVYaoKongEnterBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongLeftBtEvent -= ClickTVYaoKongLeftBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongRightBtEvent -= ClickTVYaoKongRightBtEvent;
        Destroy(gameObject);
    }
}