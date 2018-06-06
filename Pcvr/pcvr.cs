using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pcvr : MonoBehaviour
{
    /// <summary>
    /// 输入设备控制.
    /// </summary>
    public static InputEventCtrl.InputDevice m_InputDevice = InputEventCtrl.InputDevice.PC;
    [HideInInspector]
    public BarcodeCam m_BarcodeCam;
    [HideInInspector]
    public SSBoxPostNet m_SSBoxPostNet;
    static pcvr _Instance;
    public static pcvr GetInstance()
    {
        if (_Instance == null)
        {
            GameObject obj = new GameObject("_PCVR");
            _Instance = obj.AddComponent<pcvr>();
            DontDestroyOnLoad(obj);
            _Instance.Init();
        }
        return _Instance;
    }
    
    void Init()
    {
        try
        {
            if (SSGameDataCtrl.GetInstance() != null && InputEventCtrl.GetInstance().m_InputDevice == InputEventCtrl.InputDevice.HDD)
            {
                GameObject websocketObj = SSGameDataCtrl.GetInstance().SpawnWebSocketBox(gameObject.transform);
                _Instance.m_SSBoxPostNet = websocketObj.GetComponent<SSBoxPostNet>();
                _Instance.m_SSBoxPostNet.Init();
                _Instance.m_BarcodeCam = gameObject.AddComponent<BarcodeCam>();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("Unity: " + ex.ToString());
        }
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
        fireXDown,
        fireYDown,
        fireADown,
        fireBDown,
        fireXUp,
        fireYUp,
        fireAUp,
        fireBUp,
    }

    /// <summary>
    /// 红点点微信手柄方向.
    /// </summary>
    enum PlayerShouBingDir
    {
        up = 0, //没有操作方向盘.
        DirLeft = 1,
        DirLeftDown = 2,
        DirDown = 3,
        DirRightDown = 4,
        DirRight = 5,
        DirRightUp = 6,
        DirUp = 7,
        DirLeftUp = 8,
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
                if (val == PlayerShouBingFireBt.fireADown.ToString()
                    || val == PlayerShouBingFireBt.fireXDown.ToString())
                {
                    InputEventCtrl.GetInstance().ClickStartBt((SSGameDataCtrl.PlayerIndex)playerDt.Index, InputEventCtrl.ButtonState.DOWN);
                }

                if (val == PlayerShouBingFireBt.fireBDown.ToString()
                    || val == PlayerShouBingFireBt.fireYDown.ToString())
                {
                    InputEventCtrl.GetInstance().ClickStartBt((SSGameDataCtrl.PlayerIndex)playerDt.Index, InputEventCtrl.ButtonState.DOWN);
                }
            }
        }
    }

    private void OnEventDirectionAngle(string dirValStr, int userId)
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
                if (dirValStr == PlayerShouBingDir.up.ToString())
                {
                    //玩家手指离开摇杆(摇杆回中了).
                }
                else
                {
                    int val = System.Convert.ToInt32(dirValStr);
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
                }

                PlayerShouBingDir dirState = (PlayerShouBingDir)dirVal;
                //if (dirState != PlayerShouBingDir.Null)
                //{
                //    StopCoroutine(DelayResetPlayerShouBingDir(playerDt.Index));
                //    StartCoroutine(DelayResetPlayerShouBingDir(playerDt.Index));
                //}

                switch (dirState)
                {
                    case PlayerShouBingDir.DirUp:
                    case PlayerShouBingDir.DirLeftUp:
                    case PlayerShouBingDir.DirLeft:
                    case PlayerShouBingDir.DirLeftDown:
                        {
                            switch (playerDt.Index)
                            {
                                case 0:
                                case 1:
                                    {
                                        InputEventCtrl.GetInstance().ClickLeftHorBt((SSGameDataCtrl.PlayerIndex)playerDt.Index, InputEventCtrl.ButtonState.DOWN);
                                        break;
                                    }
                            }
                            break;
                        }
                    case PlayerShouBingDir.DirDown:
                    case PlayerShouBingDir.DirRightDown:
                    case PlayerShouBingDir.DirRight:
                    case PlayerShouBingDir.DirRightUp:
                        {
                            switch (playerDt.Index)
                            {
                                case 0:
                                case 1:
                                    {
                                        InputEventCtrl.GetInstance().ClickRightHorBt((SSGameDataCtrl.PlayerIndex)playerDt.Index, InputEventCtrl.ButtonState.DOWN);
                                        break;
                                    }
                            }
                            break;
                        }
                    case PlayerShouBingDir.up:
                        {
                            switch (playerDt.Index)
                            {
                                case 0:
                                case 1:
                                    {
                                        InputEventCtrl.GetInstance().ClickLeftHorBt((SSGameDataCtrl.PlayerIndex)playerDt.Index, InputEventCtrl.ButtonState.UP);
                                        InputEventCtrl.GetInstance().ClickRightHorBt((SSGameDataCtrl.PlayerIndex)playerDt.Index, InputEventCtrl.ButtonState.UP);
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
            case 1:
                {
                    InputEventCtrl.GetInstance().ClickLeftHorBt((SSGameDataCtrl.PlayerIndex)index, InputEventCtrl.ButtonState.UP);
                    InputEventCtrl.GetInstance().ClickRightHorBt((SSGameDataCtrl.PlayerIndex)index, InputEventCtrl.ButtonState.UP);
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
                case 1:
                    {
                        InputEventCtrl.GetInstance().ClickStartBt((SSGameDataCtrl.PlayerIndex)indexPlayer, InputEventCtrl.ButtonState.DOWN);
                        break;
                    }
            }
        }
    }
}