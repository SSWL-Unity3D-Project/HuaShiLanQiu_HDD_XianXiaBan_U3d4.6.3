using System;
using UnityEngine;

public class SSLanKuangCtrl : SSGameMono
{
    /// <summary>
    /// 玩家索引.
    /// </summary>
    public SSGameDataCtrl.PlayerIndex m_PlayerIndex;
    /// <summary>
    /// 玩家篮球进入篮筐触发器.
    /// </summary>
    public SSTriggerScore m_SSTriggerScore;
    /// <summary>
    /// 篮球运动轨迹测试.
    /// </summary>
    public SSBallMoveCtrl m_BallMoveTest;
    /// <summary>
    /// 篮筐Tr.
    /// </summary>
    public Transform m_RealKuangTr;
    /// <summary>
    /// 得分爆炸产生点.
    /// </summary>
    public Transform m_DeFenExpTr;
    /// <summary>
    /// X轴移动的最大距离.
    /// </summary>
    float m_DisXMax = 2f;
    /// <summary>
    /// 篮筐移动的速度.
    /// </summary>
    float m_SpeedX = 5f;
    
    void Awake()
    {
        if (m_SSTriggerScore != null)
        {
            m_SSTriggerScore.m_PlayerIndex = m_PlayerIndex;
        }
        m_DisXMax = SSGameDataCtrl.GetInstance().m_LanKuangData.m_DisXMax;
        m_SpeedX = SSGameDataCtrl.GetInstance().m_LanKuangData.m_SpeedX;
        SetActiveRealBallKuang(false);

        InputEventCtrl.GetInstance().OnClickLeftHorBtEvent += OnClickLeftHorBtEvent;
        InputEventCtrl.GetInstance().OnClickRightHorBtEvent += OnClickRightHorBtEvent;
        if (pcvr.m_InputDevice == InputEventCtrl.InputDevice.HDD)
        {
            InputEventCtrl.GetInstance().ClickHddPadLanKuangBtEvent += ClickHddPadLanKuangBtEvent;
        }
    }

    private void ClickHddPadLanKuangBtEvent(SSGameDataCtrl.PlayerIndex index, float val)
    {
        if (index == m_PlayerIndex)
        {
            m_InputHorVal = val;
        }
    }

    /// <summary>
    /// 设置玩家篮筐是否可见.
    /// </summary>
    public void SetActiveRealBallKuang(bool isActive)
    {
        SetLanKuangScale(LanKuangScale.Normal);
        //SetLanKuangScale(LanKuangScale.Big); //test.
        m_RealKuangTr.gameObject.SetActive(isActive);
        if (!isActive)
        {
            m_RealKuangTr.localPosition = Vector3.zero;
        }
    }

    public enum LanKuangScale
    {
        /// <summary>
        /// 正常篮筐.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 大篮筐.
        /// </summary>
        Big = 1,
    }

    /// <summary>
    /// 设置篮筐大小.
    /// </summary>
    public void SetLanKuangScale(LanKuangScale type)
    {
        if (type == LanKuangScale.Big)
        {
            if (!SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsActiveGame
                || !SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsCreateGameBall)
            {
                return;
            }
        }

        m_RealKuangTr.gameObject.SetActive(false);
        float scaleLanKuang = SSGameDataCtrl.GetInstance().m_LanKuangData.m_LanKaungScale;
        float bigScaleLanKuang = SSGameDataCtrl.GetInstance().m_LanKuangData.m_BigLanKaungScale;
        switch (type)
        {
            case LanKuangScale.Normal:
                {
                    m_RealKuangTr.localScale = new Vector3(scaleLanKuang, scaleLanKuang, scaleLanKuang);
                    if (!SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsActiveGame
                        || !SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsCreateGameBall)
                    {
                        //玩家没有激活游戏不应该显示篮筐.
                        return;
                    }
                    break;
                }
            case LanKuangScale.Big:
                {
                    m_RealKuangTr.localScale = new Vector3(bigScaleLanKuang, scaleLanKuang, bigScaleLanKuang);
                    break;
                }
        }
        
        m_RealKuangTr.gameObject.SetActive(true);
    }

    private void OnClickLeftHorBtEvent(SSGameDataCtrl.PlayerIndex index, InputEventCtrl.ButtonState val)
    {
        if (index == m_PlayerIndex)
        {
            switch (index)
            {
                case SSGameDataCtrl.PlayerIndex.Player01:
                    {
                        if (val == InputEventCtrl.ButtonState.DOWN)
                        {
                            GetInputHorValP1(ArrowBtState.LeftArrowDown);
                        }

                        if (val == InputEventCtrl.ButtonState.UP)
                        {
                            GetInputHorValP1(ArrowBtState.LeftArrowUp);
                        }
                        break;
                    }
                case SSGameDataCtrl.PlayerIndex.Player02:
                    {
                        if (val == InputEventCtrl.ButtonState.DOWN)
                        {
                            GetInputHorValP2(ArrowBtState.LeftArrowDown);
                        }

                        if (val == InputEventCtrl.ButtonState.UP)
                        {
                            GetInputHorValP2(ArrowBtState.LeftArrowUp);
                        }
                        break;
                    }
            }
        }
    }

    private void OnClickRightHorBtEvent(SSGameDataCtrl.PlayerIndex index, InputEventCtrl.ButtonState val)
    {
        if (index == m_PlayerIndex)
        {
            switch (index)
            {
                case SSGameDataCtrl.PlayerIndex.Player01:
                    {
                        if (val == InputEventCtrl.ButtonState.DOWN)
                        {
                            GetInputHorValP1(ArrowBtState.RightArrowDown);
                        }

                        if (val == InputEventCtrl.ButtonState.UP)
                        {
                            GetInputHorValP1(ArrowBtState.RightArrowUp);
                        }
                        break;
                    }
                case SSGameDataCtrl.PlayerIndex.Player02:
                    {
                        if (val == InputEventCtrl.ButtonState.DOWN)
                        {
                            GetInputHorValP2(ArrowBtState.RightArrowDown);
                        }

                        if (val == InputEventCtrl.ButtonState.UP)
                        {
                            GetInputHorValP2(ArrowBtState.RightArrowUp);
                        }
                        break;
                    }
            }
        }
    }

    enum ArrowBtState
    {
        LeftArrowDown = 0,
        LeftArrowUp = 1,
        RightArrowDown = 2,
        RightArrowUp = 3,
    }

    byte[] KeyCodeState = new byte[2];
    float m_InputHorVal = 0f;
    float m_InputHorValLerp = 0f;
    float GetInputHorValP1(ArrowBtState valBt)
    {
        switch (valBt)
        {
            case ArrowBtState.LeftArrowDown:
                {
                    m_InputHorVal = -1f;
                    KeyCodeState[0] = 1;
                    break;
                }
            case ArrowBtState.LeftArrowUp:
                {
                    if (KeyCodeState[1] == 0)
                    {
                        m_InputHorVal = 0f;
                    }
                    else if (KeyCodeState[1] == 1)
                    {
                        m_InputHorVal = 1f;
                    }
                    KeyCodeState[0] = 0;
                    break;
                }
            case ArrowBtState.RightArrowDown:
                {
                    m_InputHorVal = 1f;
                    KeyCodeState[1] = 1;
                    break;
                }
            case ArrowBtState.RightArrowUp:
                {
                    if (KeyCodeState[0] == 0)
                    {
                        m_InputHorVal = 0f;
                    }
                    else if (KeyCodeState[0] == 1)
                    {
                        m_InputHorVal = -1f;
                    }
                    KeyCodeState[1] = 0;
                    break;
                }
        }
        return m_InputHorVal;
    }

    float GetInputHorValP2(ArrowBtState valBt)
    {
        switch (valBt)
        {
            case ArrowBtState.LeftArrowDown:
                {
                    m_InputHorVal = -1f;
                    KeyCodeState[0] = 1;
                    break;
                }
            case ArrowBtState.LeftArrowUp:
                {
                    if (KeyCodeState[1] == 0)
                    {
                        m_InputHorVal = 0f;
                    }
                    else if (KeyCodeState[1] == 1)
                    {
                        m_InputHorVal = 1f;
                    }
                    KeyCodeState[0] = 0;
                    break;
                }
            case ArrowBtState.RightArrowDown:
                {
                    m_InputHorVal = 1f;
                    KeyCodeState[1] = 1;
                    break;
                }
            case ArrowBtState.RightArrowUp:
                {
                    if (KeyCodeState[0] == 0)
                    {
                        m_InputHorVal = 0f;
                    }
                    else if (KeyCodeState[0] == 1)
                    {
                        m_InputHorVal = -1f;
                    }
                    KeyCodeState[1] = 0;
                    break;
                }
        }
        return m_InputHorVal;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (SSGameDataCtrl.GetInstance().m_SSUIRoot.IsPlayGameDaoJiShi)
        {
            //播放游戏倒计时阶段.
        }
        else
        {
            if (!SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsActiveGame
                || !SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsCreateGameBall)
            {
                return;
            }

            if (SSGameDataCtrl.GetInstance().m_SSUIRoot.m_ExitGameUI != null)
            {
                //退出游戏界面存在时,不允许移动篮筐.
                return;
            }

            if (SSGameDataCtrl.GetInstance().IsPauseGame)
            {
                return;
            }
        }
        m_RealKuangTr.localPosition = GetRealLanKuangPosition();
    }
    
    /// <summary>
    /// 获取篮筐的移动坐标.
    /// </summary>
    Vector3 GetRealLanKuangPosition()
    {
        float inputHorVal = m_InputHorVal;
        Vector3 pos = m_RealKuangTr.localPosition;
        if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsHddWxPadPlayer)
        {
            //红点点微信手柄激活游戏的玩家.
            //inputHorVal -> [-1f, 1f].
            if (m_InputHorValLerp != inputHorVal)
            {
                //平滑过渡处理微信手柄玩家篮筐输入信息.
                m_InputHorValLerp = Mathf.MoveTowards(m_InputHorValLerp, inputHorVal, 0.2f);
                inputHorVal = m_InputHorValLerp;
            }
            pos.x = inputHorVal * m_DisXMax;
        }
        else
        {
            //遥控器或键盘激活游戏的玩家.
            if (inputHorVal != 0f)
            {
                //硬件版篮筐的移动方案可能和软件版不一样.
                //软件版移动篮筐方案.
                if (Mathf.Abs(pos.x) <= m_DisXMax)
                {
                    bool isMoveLK = false;
                    if (Mathf.Abs(pos.x) != m_DisXMax)
                    {
                        isMoveLK = true;
                    }
                    else
                    {
                        if (Mathf.Sign(inputHorVal) == -1f && pos.x == m_DisXMax)
                        {
                            isMoveLK = true;
                        }

                        if (Mathf.Sign(inputHorVal) == 1f && pos.x == -m_DisXMax)
                        {
                            isMoveLK = true;
                        }
                    }

                    if (isMoveLK)
                    {
                        pos.x += Mathf.Sign(inputHorVal) * (m_SpeedX * Time.fixedDeltaTime);
                    }

                    if (pos.x > m_DisXMax)
                    {
                        pos.x = m_DisXMax;
                    }

                    if (pos.x < -m_DisXMax)
                    {
                        pos.x = -m_DisXMax;
                    }
                }
            }
        }
        return pos;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            return;
        }

        if (m_BallMoveTest != null)
        {
            m_BallMoveTest.DrawPath();
        }
    }
#endif
}