using UnityEngine;

public class SSLanKuangCtrl : MonoBehaviour
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

        InputEventCtrl.GetInstance().OnClickLeftHorBtEvent += OnClickLeftHorBtEvent;
        InputEventCtrl.GetInstance().OnClickRightHorBtEvent += OnClickRightHorBtEvent;
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
    void Update()
    {
        if (!SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsActiveGame)
        {
            return;
        }

        float inputHorVal = m_InputHorVal;
        Vector3 pos = m_RealKuangTr.localPosition;
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
                    pos.x += Mathf.Sign(inputHorVal) * (m_SpeedX * Time.deltaTime);
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
        m_RealKuangTr.localPosition = pos;
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