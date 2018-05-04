using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pcvr : MonoBehaviour
{

    static pcvr _Instance;
    private SSBoxPostNet m_SSBoxPostNet;

    public static pcvr GetInstance()
    {
        if (_Instance == null)
        {
            GameObject obj = new GameObject("_PCVR");
            DontDestroyOnLoad(obj);
            _Instance = obj.AddComponent<pcvr>();

            if (SSGameDataCtrl.GetInstance() != null && InputEventCtrl.GetInstance().m_InputDevice == InputEventCtrl.InputDevice.HDD)
            {
                GameObject websocketObj = SSGameDataCtrl.GetInstance().SpawnWebSocketBox(obj.transform);
                _Instance.m_SSBoxPostNet = websocketObj.GetComponent<SSBoxPostNet>();
            }
        }
        return _Instance;
    }

    void Start()
    {
        if (m_SSBoxPostNet != null)
        {
            WebSocketSimpet webSocketSimpet = m_SSBoxPostNet.m_WebSocketSimpet;
            if (webSocketSimpet != null)
            {
                Debug.Log("add webSocketSimpet event...");
                webSocketSimpet.OnEventPlayerLoginBox += OnEventPlayerLoginBox;
                webSocketSimpet.OnEventPlayerExitBox += OnEventPlayerExitBox;
                webSocketSimpet.OnEventDirectionAngle += OnEventDirectionAngle;
                webSocketSimpet.OnEventActionOperation += OnEventActionOperation;
            }
            else
            {
                Debug.Log("webSocketSimpet is null!");
            }
        }
    }

    class GamePlayerData
    {
        /// <summary>
        /// 玩家在游戏中的索引.
        /// </summary>
        public int Index = -1;
        /// <summary>
        /// 是否退出微信.
        /// </summary>
        public bool IsExitWeiXin = false;
        /// <summary>
        /// 玩家的微信数据信息.
        /// </summary>
        public WebSocketSimpet.PlayerWeiXinData m_PlayerWeiXinData;
    }
    List<GamePlayerData> m_GamePlayerData = new List<GamePlayerData>();

    /// <summary>
    /// 玩家激活游戏状态.
    /// </summary>
    enum PlayerActiveState
    {
        WeiJiHuo = 0, //未激活.
        JiHuo = 1, //激活.
    }

    /// <summary>
    /// 红点点微信手柄发射按键.
    /// </summary>
    enum PlayerShouBingFireBt
    {
        fireX,
        fireY,
        fireA,
        fireB,
    }

    /// <summary>
    /// 红点点微信手柄方向.
    /// </summary>
    enum PlayerShouBingDir
    {
        Null = 0, //没有操作方向盘.
        Left = 1,
        LeftDown = 2,
        Down = 3,
        RightDown = 4,
        Right = 5,
        RightUp = 6,
        Up = 7,
        LeftUp = 8,
    }

    /// <summary>
    /// 2个玩家激活游戏的列表状态(0 未激活, 1 激活).
    /// </summary>
    [HideInInspector]
    public byte[] m_IndexPlayerActiveGameState = new byte[2];
    public void SetIndexPlayerActiveGameState(int index, byte activeState)
    {
        m_IndexPlayerActiveGameState[index] = activeState;
        if (activeState == (int)PlayerActiveState.WeiJiHuo)
        {
            GamePlayerData playerDt = m_GamePlayerData.Find((dt) => { return dt.Index.Equals(index); });
            if (playerDt != null)
            {
                if (playerDt.IsExitWeiXin)
                {
                    //玩家已经退出微信.
                    Debug.Log("player have exit weiXin! clean the player data...");
                    m_GamePlayerData.Remove(playerDt);
                }
                else
                {
                    //玩家血值耗尽应该续费了,找到玩家数据.
                    Debug.Log("player should buy game coin!");
                }
            }
        }
    }

    /// <summary>
    /// 获取未激活玩家的索引. returnVal == -1 -> 所有玩家都处于激活状态.
    /// </summary>
    int GetActivePlayerIndex()
    {
        int indexPlayer = -1;
        for (int i = 0; i < m_IndexPlayerActiveGameState.Length; i++)
        {
            if (m_IndexPlayerActiveGameState[i] == 0)
            {
                //未激活玩家索引.
                indexPlayer = i;
                break;
            }
        }
        return indexPlayer;
    }

    /// <summary>
    /// 发射按键响应.
    /// </summary>
    private void OnEventActionOperation(string val, int userId)
    {
        //Debug.Log("pcvr::OnEventActionOperation -> userId " + userId + ", val " + val);
        GamePlayerData playerDt = m_GamePlayerData.Find((dt) => { return dt.m_PlayerWeiXinData.userId.Equals(userId); });
        if (playerDt != null && playerDt.Index > -1 && playerDt.Index < 4)
        {
            //Debug.Log("OnEventActionOperation -> playerIndex == " + playerDt.Index);
            playerDt.IsExitWeiXin = false;
            if (m_IndexPlayerActiveGameState[playerDt.Index] == (int)PlayerActiveState.JiHuo)
            {
                //处于激活状态的玩家才可以进行游戏操作.
                if (val == PlayerShouBingFireBt.fireA.ToString()
                    || val == PlayerShouBingFireBt.fireX.ToString())
                {
                    switch (playerDt.Index)
                    {
                        case 0:
                            {
                                //InputEventCtrl.GetInstance().ClickFireBtOne(ButtonState.DOWN);
                                break;
                            }
                        case 1:
                            {
                                //InputEventCtrl.GetInstance().ClickFireBtTwo(ButtonState.DOWN);
                                break;
                            }
                        case 2:
                            {
                                //InputEventCtrl.GetInstance().ClickFireBtThree(ButtonState.DOWN);
                                break;
                            }
                        case 3:
                            {
                                //InputEventCtrl.GetInstance().ClickFireBtFour(ButtonState.DOWN);
                                break;
                            }
                    }
                }

                if (val == PlayerShouBingFireBt.fireB.ToString()
                    || val == PlayerShouBingFireBt.fireY.ToString())
                {
                    switch (playerDt.Index)
                    {
                        case 0:
                            {
                                //InputEventCtrl.GetInstance().ClickDaoDanBtOne(ButtonState.DOWN);
                                break;
                            }
                        case 1:
                            {
                                //InputEventCtrl.GetInstance().ClickDaoDanBtTwo(ButtonState.DOWN);
                                break;
                            }
                        case 2:
                            {
                                //InputEventCtrl.GetInstance().ClickDaoDanBtThree(ButtonState.DOWN);
                                break;
                            }
                        case 3:
                            {
                                //InputEventCtrl.GetInstance().ClickDaoDanBtFour(ButtonState.DOWN);
                                break;
                            }
                    }
                }
            }
        }
    }

    private void OnEventDirectionAngle(int val, int userId)
    {
        //Debug.Log("pcvr::OnEventDirectionAngle -> userId " + userId + ", val " + val);
        GamePlayerData playerDt = m_GamePlayerData.Find((dt) => { return dt.m_PlayerWeiXinData.userId.Equals(userId); });
        if (playerDt != null && playerDt.Index > -1 && playerDt.Index < 4)
        {
            //Debug.Log("OnEventDirectionAngle -> playerIndex == " + playerDt.Index);
            playerDt.IsExitWeiXin = false;
            if (m_IndexPlayerActiveGameState[playerDt.Index] == (int)PlayerActiveState.JiHuo)
            {
                //处于激活状态的玩家才可以进行游戏操作.
                int dirVal = 0;
                if (val >= -22.5f && val < 22.5f)
                {
                    dirVal = 1;
                }
                else if (val >= -67.5f && val < -22.5f)
                {
                    dirVal = 2;
                }
                else if (val >= -112.5f && val < -67.5f)
                {
                    dirVal = 3;
                }
                else if (val >= -157.5f && val < -112.5f)
                {
                    dirVal = 4;
                }
                else if ((val >= -180f && val < -157.5f)
                    || (val <= 180f && val > 157.5f))
                {
                    dirVal = 5;
                }
                else if (val > 112.5f && val <= 157.5f)
                {
                    dirVal = 6;
                }
                else if (val > 67.5f && val <= 112.5f)
                {
                    dirVal = 7;
                }
                else if (val >= 22.5f && val <= 67.5f)
                {
                    dirVal = 8;
                }

                PlayerShouBingDir dirState = (PlayerShouBingDir)dirVal;
                if (dirState != PlayerShouBingDir.Null)
                {
                    StopCoroutine(DelayResetPlayerShouBingDir(playerDt.Index));
                    StartCoroutine(DelayResetPlayerShouBingDir(playerDt.Index));
                }

                switch (dirState)
                {
                    case PlayerShouBingDir.Left:
                        {
                            switch (playerDt.Index)
                            {
                                case 0:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP1(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP1(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP1(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP1(ButtonState.UP);
                                        break;
                                    }
                                case 1:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP2(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP2(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP2(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP2(ButtonState.UP);
                                        break;
                                    }
                                case 2:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP3(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP3(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP3(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP3(ButtonState.UP);
                                        break;
                                    }
                                case 3:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP4(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP4(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP4(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP4(ButtonState.UP);
                                        break;
                                    }
                            }
                            break;
                        }
                    case PlayerShouBingDir.LeftDown:
                        {
                            switch (playerDt.Index)
                            {
                                case 0:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP1(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP1(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP1(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP1(ButtonState.UP);
                                        break;
                                    }
                                case 1:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP2(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP2(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP2(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP2(ButtonState.UP);
                                        break;
                                    }
                                case 2:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP3(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP3(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP3(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP3(ButtonState.UP);
                                        break;
                                    }
                                case 3:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP4(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP4(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP4(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP4(ButtonState.UP);
                                        break;
                                    }
                            }
                            break;
                        }
                    case PlayerShouBingDir.Down:
                        {
                            switch (playerDt.Index)
                            {
                                case 0:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP1(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP1(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP1(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP1(ButtonState.UP);
                                        break;
                                    }
                                case 1:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP2(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP2(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP2(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP2(ButtonState.UP);
                                        break;
                                    }
                                case 2:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP3(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP3(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP3(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP3(ButtonState.UP);
                                        break;
                                    }
                                case 3:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP4(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP4(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP4(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP4(ButtonState.UP);
                                        break;
                                    }
                            }
                            break;
                        }
                    case PlayerShouBingDir.RightDown:
                        {
                            switch (playerDt.Index)
                            {
                                case 0:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP1(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP1(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP1(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP1(ButtonState.DOWN);
                                        break;
                                    }
                                case 1:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP2(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP2(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP2(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP2(ButtonState.DOWN);
                                        break;
                                    }
                                case 2:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP3(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP3(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP3(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP3(ButtonState.DOWN);
                                        break;
                                    }
                                case 3:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP4(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP4(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP4(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP4(ButtonState.DOWN);
                                        break;
                                    }
                            }
                            break;
                        }
                    case PlayerShouBingDir.Right:
                        {
                            switch (playerDt.Index)
                            {
                                case 0:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP1(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP1(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP1(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP1(ButtonState.DOWN);
                                        break;
                                    }
                                case 1:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP2(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP2(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP2(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP2(ButtonState.DOWN);
                                        break;
                                    }
                                case 2:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP3(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP3(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP3(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP3(ButtonState.DOWN);
                                        break;
                                    }
                                case 3:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP4(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP4(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP4(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP4(ButtonState.DOWN);
                                        break;
                                    }
                            }
                            break;
                        }
                    case PlayerShouBingDir.RightUp:
                        {
                            switch (playerDt.Index)
                            {
                                case 0:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP1(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP1(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP1(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP1(ButtonState.DOWN);
                                        break;
                                    }
                                case 1:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP2(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP2(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP2(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP2(ButtonState.DOWN);
                                        break;
                                    }
                                case 2:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP3(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP3(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP3(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP3(ButtonState.DOWN);
                                        break;
                                    }
                                case 3:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP4(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP4(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP4(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP4(ButtonState.DOWN);
                                        break;
                                    }
                            }
                            break;
                        }
                    case PlayerShouBingDir.Up:
                        {
                            switch (playerDt.Index)
                            {
                                case 0:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP1(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP1(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP1(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP1(ButtonState.UP);
                                        break;
                                    }
                                case 1:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP2(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP2(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP2(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP2(ButtonState.UP);
                                        break;
                                    }
                                case 2:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP3(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP3(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP3(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP3(ButtonState.UP);
                                        break;
                                    }
                                case 3:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP4(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP4(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP4(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP4(ButtonState.UP);
                                        break;
                                    }
                            }
                            break;
                        }
                    case PlayerShouBingDir.LeftUp:
                        {
                            switch (playerDt.Index)
                            {
                                case 0:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP1(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP1(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP1(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP1(ButtonState.UP);
                                        break;
                                    }
                                case 1:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP2(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP2(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP2(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP2(ButtonState.UP);
                                        break;
                                    }
                                case 2:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP3(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP3(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP3(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP3(ButtonState.UP);
                                        break;
                                    }
                                case 3:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP4(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP4(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP4(ButtonState.DOWN);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP4(ButtonState.UP);
                                        break;
                                    }
                            }
                            break;
                        }
                    case PlayerShouBingDir.Null:
                        {
                            switch (playerDt.Index)
                            {
                                case 0:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP1(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP1(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP1(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP1(ButtonState.UP);
                                        break;
                                    }
                                case 1:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP2(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP2(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP2(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP2(ButtonState.UP);
                                        break;
                                    }
                                case 2:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP3(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP3(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP3(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP3(ButtonState.UP);
                                        break;
                                    }
                                case 3:
                                    {
                                        //InputEventCtrl.GetInstance().ClickFangXiangUBtP4(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangDBtP4(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangLBtP4(ButtonState.UP);
                                        //InputEventCtrl.GetInstance().ClickFangXiangRBtP4(ButtonState.UP);
                                        break;
                                    }
                            }
                            break;
                        }
                }
            }
        }
    }

    IEnumerator DelayResetPlayerShouBingDir(int index)
    {
        yield return new WaitForSeconds(1f);
        switch (index)
        {
            case 0:
                {
                    //InputEventCtrl.GetInstance().ClickFangXiangUBtP1(ButtonState.UP);
                    //InputEventCtrl.GetInstance().ClickFangXiangDBtP1(ButtonState.UP);
                    //InputEventCtrl.GetInstance().ClickFangXiangLBtP1(ButtonState.UP);
                    //InputEventCtrl.GetInstance().ClickFangXiangRBtP1(ButtonState.UP);
                    break;
                }
            case 1:
                {
                    //InputEventCtrl.GetInstance().ClickFangXiangUBtP2(ButtonState.UP);
                    //InputEventCtrl.GetInstance().ClickFangXiangDBtP2(ButtonState.UP);
                    //InputEventCtrl.GetInstance().ClickFangXiangLBtP2(ButtonState.UP);
                    //InputEventCtrl.GetInstance().ClickFangXiangRBtP2(ButtonState.UP);
                    break;
                }
            case 2:
                {
                    //InputEventCtrl.GetInstance().ClickFangXiangUBtP3(ButtonState.UP);
                    //InputEventCtrl.GetInstance().ClickFangXiangDBtP3(ButtonState.UP);
                    //InputEventCtrl.GetInstance().ClickFangXiangLBtP3(ButtonState.UP);
                    //InputEventCtrl.GetInstance().ClickFangXiangRBtP3(ButtonState.UP);
                    break;
                }
            case 3:
                {
                    //InputEventCtrl.GetInstance().ClickFangXiangUBtP4(ButtonState.UP);
                    //InputEventCtrl.GetInstance().ClickFangXiangDBtP4(ButtonState.UP);
                    //InputEventCtrl.GetInstance().ClickFangXiangLBtP4(ButtonState.UP);
                    //InputEventCtrl.GetInstance().ClickFangXiangRBtP4(ButtonState.UP);
                    break;
                }
        }
    }

    private void OnEventPlayerExitBox(int userId)
    {
        Debug.Log("pcvr::OnEventPlayerExitBox -> userId " + userId);
        GamePlayerData playerDt = m_GamePlayerData.Find((dt) => { return dt.m_PlayerWeiXinData.userId.Equals(userId); });
        if (playerDt != null)
        {
            playerDt.IsExitWeiXin = true;
            if (playerDt.Index > -1 && playerDt.Index < 4)
            {
                if (m_IndexPlayerActiveGameState[playerDt.Index] == (int)PlayerActiveState.WeiJiHuo)
                {
                    //玩家血值耗尽,清理玩家微信数据.
                    m_GamePlayerData.Remove(playerDt);
                    Debug.Log("OnEventPlayerExitBox -> clear player weiXin data...");
                }
            }
        }
    }

    private void OnEventPlayerLoginBox(WebSocketSimpet.PlayerWeiXinData val)
    {
        Debug.Log("pcvr::OnEventPlayerLoginBox -> userName " + val.userName + ", userId " + val.userId);
        GamePlayerData playerDt = m_GamePlayerData.Find((dt) => {
            if (dt.m_PlayerWeiXinData != null)
            {
                return dt.m_PlayerWeiXinData.userId.Equals(val.userId);
            }
            return dt.m_PlayerWeiXinData.Equals(val);
        });

        int indexPlayer = -1;
        bool isActivePlayer = false;
        if (playerDt == null)
        {
            indexPlayer = GetActivePlayerIndex();
            if (indexPlayer > -1 && indexPlayer < 4)
            {
                Debug.Log("Active player, indexPlayer == " + indexPlayer);
                playerDt = new GamePlayerData();
                playerDt.m_PlayerWeiXinData = val;
                playerDt.Index = indexPlayer;
                m_GamePlayerData.Add(playerDt);
                isActivePlayer = true;
            }
            else
            {
                Debug.Log("have not empty player!");
            }
        }
        else
        {
            Debug.Log("player have active game!");
            playerDt.IsExitWeiXin = false;
            if (playerDt.Index > -1 && playerDt.Index < 4)
            {
                if (m_IndexPlayerActiveGameState[playerDt.Index] == (int)PlayerActiveState.WeiJiHuo)
                {
                    isActivePlayer = true;
                    indexPlayer = playerDt.Index;
                }
            }
        }

        if (isActivePlayer)
        {
            switch (indexPlayer)
            {
                case 0:
                    {
                        //InputEventCtrl.GetInstance().ClickStartBtOne(ButtonState.DOWN);
                        //InputEventCtrl.GetInstance().ClickStartBtOne(ButtonState.UP);
                        break;
                    }
                case 1:
                    {
                        //InputEventCtrl.GetInstance().ClickStartBtTwo(ButtonState.DOWN);
                        //InputEventCtrl.GetInstance().ClickStartBtTwo(ButtonState.UP);
                        break;
                    }
                case 2:
                    {
                        //InputEventCtrl.GetInstance().ClickStartBtThree(ButtonState.DOWN);
                        //InputEventCtrl.GetInstance().ClickStartBtThree(ButtonState.UP);
                        break;
                    }
                case 3:
                    {
                        //InputEventCtrl.GetInstance().ClickStartBtFour(ButtonState.DOWN);
                        //InputEventCtrl.GetInstance().ClickStartBtFour(ButtonState.UP);
                        break;
                    }
            }
        }
    }
}