using System;
using UnityEngine;

public class FuHuoDaoJu : SSGameMono
{
    public enum FuHuoCiShuState
    {
        /// <summary>
        /// 复活3次.
        /// </summary>
        Num03 = 0,
        /// <summary>
        /// 复活5次.
        /// </summary>
        Num05 = 1,
        /// <summary>
        /// 复活5次.
        /// </summary>
        Num07 = 2,
        /// <summary>
        /// 复活9次.
        /// </summary>
        Num09 = 3,
    }
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
                    //取消购买复活道具.
                    //删除复活道具购买界面.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.RemoveGameFuHuoDaoJuPanel(m_PlayerIndex);
                    break;
                }
            case SSGameDataCtrl.XuanXiangState.XuanXiang01:
            case SSGameDataCtrl.XuanXiangState.XuanXiang02:
            case SSGameDataCtrl.XuanXiangState.XuanXiang03:
            case SSGameDataCtrl.XuanXiangState.XuanXiang04:
                {
                    SetIsLockPanel(true);
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameFuHuoDaoJu_ZhiFuPanel(m_PlayerIndex);
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

        SSGameDataCtrl.PlayerIndex indexPlayerReverse = SSGameDataCtrl.GetInstance().GetReversePlayerIndex(m_PlayerIndex);
        if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)indexPlayerReverse].IsActiveGame)
        {
            //对方激活了游戏.
            if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].GameState == SSGameDataCtrl.PlayerGameState.YouXiQian)
            {
                //游戏前.
                //是否接受对方挑战.
                SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameShiFouJieShouTiaoZhanPanel(m_PlayerIndex);
            }
            else if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].GameState == SSGameDataCtrl.PlayerGameState.YouXiZhong)
            {
                //游戏中.
                if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsJieShouTiaoZhan)
                {
                    //接受挑战对方.
                    //产生开始双人PK提示.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnKaiShiShuangRenPK();
                }
                else
                {
                    //拒绝挑战对方.
                    //显示游戏倒计时界面.
                    SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameDaoJiShiUI(m_PlayerIndex);
                }
            }
        }
        else
        {
            //对方没有激活游戏.
            //显示游戏倒计时界面.
            SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameDaoJiShiUI(m_PlayerIndex);
        }
        
        InputEventCtrl.GetInstance().ClickTVYaoKongEnterBtEvent -= ClickTVYaoKongEnterBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongLeftBtEvent -= ClickTVYaoKongLeftBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongRightBtEvent -= ClickTVYaoKongRightBtEvent;
        Destroy(gameObject);
    }
}