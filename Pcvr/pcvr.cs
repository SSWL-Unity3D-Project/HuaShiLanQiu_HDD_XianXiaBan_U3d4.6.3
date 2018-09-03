using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pcvr : MonoBehaviour
{
    /// <summary>
    /// 输入设备控制.
    /// </summary>
    public static InputEventCtrl.InputDevice m_InputDevice = InputEventCtrl.InputDevice.PC;
    /// <summary>
    /// 微信小程序虚拟手柄.
    /// </summary>
    public SSBoxPostNet.WeiXinShouBingEnum m_WXShouBingType
    {
        get
        {
            SSBoxPostNet.WeiXinShouBingEnum type = SSBoxPostNet.WeiXinShouBingEnum.XiaoChengXu;
            //if (m_TVGamePayType == SSGamePayUICtrl.TVGamePayState.DianXinApk)
            //{
            //    type = SSBoxPostNet.WeiXinShouBingEnum.XiaoChengXu;
            //}
            return type;
        }
    }
    /// <summary>
    /// 二维码创建脚本.
    /// </summary>
    [HideInInspector]
    public BarcodeCam m_BarcodeCam;
    /// <summary>
    /// 电视盒子控制.
    /// </summary>
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
            _Instance.InitInfo();
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
    
    void InitInfo()
    {
        for (int i = 0; i < m_GmWXLoginDt.Length; i++)
        {
            m_GmWXLoginDt[i] = new GameWeiXinLoginData();
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
            AddTVYaoKongBtEvent();
        }
    }

    /// <summary>
    /// 添加遥控器按键信息响应事件.
    /// </summary>
    public void AddTVYaoKongBtEvent()
    {
        InputEventCtrl.GetInstance().ClickTVYaoKongEnterBtEvent += ClickTVYaoKongEnterBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongLeftBtEvent += ClickTVYaoKongLeftBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongRightBtEvent += ClickTVYaoKongRightBtEvent;
        InputEventCtrl.GetInstance().ClickTVYaoKongUpBtEvent += ClickTVYaoKongUpBtEvent;
    }

    public enum GamePadType
    {
        Null,
        /// <summary>
        /// 电视遥控器.
        /// </summary>
        TV_YaoKongQi,
        /// <summary>
        /// 微信虚拟手柄.
        /// </summary>
        WeiXin_ShouBing,
    }

    /// <summary>
    /// 电视遥控器激活玩家时的数据信息.
    /// </summary>
    public class TVYaoKongPlayerData
    {
        /// <summary>
        /// 最后一个血值耗尽的玩家索引信息.
        /// </summary>
        public int Index = -1;
        /// <summary>
        /// 游戏外设操作设备.
        /// </summary>
        public GamePadType m_GamePadType = GamePadType.Null;
        public TVYaoKongPlayerData(int indexVal, GamePadType pad)
        {
            Index = indexVal;
            m_GamePadType = pad;
        }

        public void Reset()
        {
            Index = -1;
            m_GamePadType = GamePadType.Null;
        }
    }
    /// <summary>
    /// 遥控器确定键激活玩家数据列表.
    /// 按照谁最后挂掉优先激活谁的顺序排列.
    /// </summary>
    List<TVYaoKongPlayerData> m_TVYaoKongPlayerDt = new List<TVYaoKongPlayerData>();
    /// <summary>
    /// 游戏电视遥控器登陆的玩家信息.
    /// </summary>
    [HideInInspector]
    public TVYaoKongPlayerData m_GmTVLoginDt;

    public class GameWeiXinLoginData
    {
        /// <summary>
        /// 是否登陆微信手柄.
        /// </summary>
        public bool IsLoginWX = false;
        /// <summary>
        /// 是否激活游戏.
        /// </summary>
        public bool IsActiveGame = false;
        /// <summary>
        /// 游戏外设操作设备.
        /// </summary>
        public GamePadType m_GamePadType = GamePadType.Null;
        public void Reset()
        {
            IsLoginWX = false;
            IsActiveGame = false;
            m_GamePadType = GamePadType.Null;
        }
    }
    /// <summary>
    /// 游戏微信手柄登陆信息.
    /// </summary>
    public GameWeiXinLoginData[] m_GmWXLoginDt = new GameWeiXinLoginData[2];
    int m_MaxPlayerNum = 2;

    private void ClickTVYaoKongEnterBtEvent(InputEventCtrl.ButtonState val)
    {
        if (SSGameDataCtrl.GetInstance().m_SSUIRoot.m_GameErWeiMa == null)
        {
            return;
        }

        if (val == InputEventCtrl.ButtonState.UP)
        {
            Debug.Log("Unity: pcvr -> ClickTVYaoKongEnterBtEvent...");
            CheckActiveTvPlayer();
        }
    }

    /// <summary>
    /// 检测激活电视遥控器玩家.
    /// </summary>
    void CheckActiveTvPlayer()
    {
        int count = m_TVYaoKongPlayerDt.Count;
        if (count > 0)
        {
            TVYaoKongPlayerData playerDt = m_TVYaoKongPlayerDt[count - 1];
            int indexPlayer = playerDt.Index;
            //清理最后一个血值耗尽的玩家信息.
            m_TVYaoKongPlayerDt.RemoveAt(count - 1);

            if (indexPlayer > -1 && indexPlayer < m_MaxPlayerNum)
            {
                switch (playerDt.m_GamePadType)
                {
                    case GamePadType.TV_YaoKongQi:
                        {
                            if (m_GmWXLoginDt[indexPlayer].IsLoginWX)
                            {
                                if (!m_GmWXLoginDt[indexPlayer].IsActiveGame)
                                {
                                    Debug.Log("Unity: click TVYaoKong EnterBt -> active TV_YaoKongQi " + indexPlayer + " player!");
                                    m_GmWXLoginDt[indexPlayer].IsActiveGame = true;
                                    InputEventCtrl.GetInstance().ClickStartBt((SSGameDataCtrl.PlayerIndex)indexPlayer, InputEventCtrl.ButtonState.DOWN);
                                }
                            }
                            break;
                        }
                    case GamePadType.WeiXin_ShouBing:
                        {
                            if (m_GmWXLoginDt[indexPlayer].IsLoginWX)
                            {
                                if (!m_GmWXLoginDt[indexPlayer].IsActiveGame)
                                {
                                    Debug.Log("Unity: click TVYaoKong EnterBt -> active " + indexPlayer + " player!");
                                    m_GmWXLoginDt[indexPlayer].IsActiveGame = true;
                                    InputEventCtrl.GetInstance().ClickStartBt((SSGameDataCtrl.PlayerIndex)indexPlayer, InputEventCtrl.ButtonState.DOWN);
                                }
                            }
                            break;
                        }
                }
            }
        }
        else
        {
            if (SSGameDataCtrl.GetInstance().m_SSUIRoot.m_ExitGameUI == null)
            {
                //没有退出游戏界面出现时,可以用遥控器进入游戏.
                if (m_GmTVLoginDt == null)
                {
                    //遥控器激活玩家.
                    int index = GetActivePlayerIndex();
                    if (index < m_MaxPlayerNum && index > -1)
                    {
                        Debug.Log("Unity: click TVYaoKong EnterBt -> --> active TV_YaoKongQi " + index + " player!");
                        m_GmWXLoginDt[index].IsLoginWX = true;
                        m_GmWXLoginDt[index].IsActiveGame = true;
                        m_GmWXLoginDt[index].m_GamePadType = GamePadType.TV_YaoKongQi;
                        SSGameDataCtrl.GetInstance().m_PlayerData[index].PlayerHeadUrl = "";
                        m_GmTVLoginDt = new TVYaoKongPlayerData(index, GamePadType.TV_YaoKongQi);
                        InputEventCtrl.GetInstance().ClickStartBt((SSGameDataCtrl.PlayerIndex)index, InputEventCtrl.ButtonState.DOWN);
                    }
                }
                else
                {
                    if (m_GmTVLoginDt.Index > -1 && m_GmTVLoginDt.Index < m_MaxPlayerNum)
                    {
                        //Debug.Log("OnEventActionOperation -> playerIndex == " + playerDt.Index);
                        if (m_IndexPlayerActiveGameState[m_GmTVLoginDt.Index] == (int)PlayerActiveState.JiHuo)
                        {
                            //处于激活状态的玩家才可以进行游戏操作.
                            InputEventCtrl.GetInstance().ClickStartBt((SSGameDataCtrl.PlayerIndex)m_GmTVLoginDt.Index, InputEventCtrl.ButtonState.DOWN);
                        }
                    }
                }
            }

        }
    }

    private void ClickTVYaoKongLeftBtEvent(InputEventCtrl.ButtonState val)
    {
        if (m_GmTVLoginDt != null)
        {
            int index = m_GmTVLoginDt.Index;
            InputEventCtrl.GetInstance().ClickLeftHorBt((SSGameDataCtrl.PlayerIndex)index, val);
        }
    }

    private void ClickTVYaoKongRightBtEvent(InputEventCtrl.ButtonState val)
    {
        if (m_GmTVLoginDt != null)
        {
            int index = m_GmTVLoginDt.Index;
            InputEventCtrl.GetInstance().ClickRightHorBt((SSGameDataCtrl.PlayerIndex)index, val);
        }
    }

    private void ClickTVYaoKongUpBtEvent(InputEventCtrl.ButtonState val)
    {
        if (SSGameDataCtrl.GetInstance().m_SSUIRoot.m_GameErWeiMa != null)
        {
            //有引导界面时,不允许使用遥控器上键激活玩家.
            return;
        }

        if (SSGameDataCtrl.GetInstance().m_SSUIRoot.m_FuHuoDaoJu != null)
        {
            //有复活道具购买界面时,不允许使用遥控器上键激活玩家.
            return;
        }

        if (SSGameDataCtrl.GetInstance().m_SSUIRoot.m_LanKuangFangDa != null)
        {
            //有篮筐放大道具购买界面时,不允许使用遥控器上键激活玩家.
            return;
        }

        if (SSGameDataCtrl.GetInstance().m_SSUIRoot.m_LanQiuJianSu!= null)
        {
            //有篮球减速道具购买界面时,不允许使用遥控器上键激活玩家.
            return;
        }

        if (val == InputEventCtrl.ButtonState.UP)
        {
            Debug.Log("Unity: pcvr -> ClickTVYaoKongUpBtEvent...");
            CheckActiveTvPlayer();
        }
    }

    void CheckRemoveGmTVLoginDt(int indexPlayer)
    {
        if (m_GmTVLoginDt != null)
        {
            if (m_GmTVLoginDt.Index == indexPlayer)
            {
                //清除遥控器激活玩家的数据信息.
                InputEventCtrl.GetInstance().ClickLeftHorBt((SSGameDataCtrl.PlayerIndex)indexPlayer, InputEventCtrl.ButtonState.UP);
                InputEventCtrl.GetInstance().ClickRightHorBt((SSGameDataCtrl.PlayerIndex)indexPlayer, InputEventCtrl.ButtonState.UP);
                m_GmTVLoginDt = null;
                m_GmWXLoginDt[indexPlayer].Reset();
                Debug.Log("Unity: CheckRemoveGmTVLoginDt -> indexPlayer == " + indexPlayer);
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
    public enum PlayerActiveState
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
                    //清理玩家微信数据,必须重新扫码才可继续游戏.
                    m_GamePlayerData.Remove(playerDt);
                }
            }
            CheckRemoveGmTVLoginDt(index);
        }
    }

    /// <summary>
    /// 获取未激活玩家的索引. returnVal == -1 -> 所有玩家都处于激活状态.
    /// </summary>
    int GetActivePlayerIndex()
    {
        int indexPlayer = -1;
        for (int i = 0; i < SSGameDataCtrl.GetInstance().m_PlayerData.Length; i++)
        {
            if (!SSGameDataCtrl.GetInstance().m_PlayerData[i].IsActiveGame
                && SSGameDataCtrl.GetInstance().m_PlayerData[i].GameState == SSGameDataCtrl.PlayerGameState.YouXiQian)
            {
                //玩家没有激活游戏,并且没有处于游戏中的阶段.
                indexPlayer = i;
                break;
            }
        }
        //for (int i = 0; i < m_IndexPlayerActiveGameState.Length; i++)
        //{
        //    if (m_IndexPlayerActiveGameState[i] == (int)PlayerActiveState.WeiJiHuo)
        //    {
        //        if (!m_GmWXLoginDt[i].IsLoginWX)
        //        {
        //            //未激活且未登陆过微信手柄的玩家索引.
        //            indexPlayer = i;
        //            break;
        //        }
        //    }
        //}
        return indexPlayer;
    }

    /// <summary>
    /// 发射按键响应.
    /// </summary>
    private void OnEventActionOperation(string val, int userId)
    {
        //Debug.Log("pcvr::OnEventActionOperation -> userId " + userId + ", val " + val);
        GamePlayerData playerDt = m_GamePlayerData.Find((dt) => { return dt.m_PlayerWeiXinData.userId.Equals(userId); });
        if (playerDt != null && playerDt.Index > -1 && playerDt.Index < m_MaxPlayerNum)
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
        //Debug.Log("pcvr::OnEventDirectionAngle -> userId " + userId + ", val " + dirValStr);
        GamePlayerData playerDt = m_GamePlayerData.Find((dt) => { return dt.m_PlayerWeiXinData.userId.Equals(userId); });
        if (playerDt != null && playerDt.Index > -1 && playerDt.Index < m_MaxPlayerNum)
        {
            //Debug.Log("OnEventDirectionAngle -> playerIndex == " + playerDt.Index);
            playerDt.IsExitWeiXin = false;
            if (m_IndexPlayerActiveGameState[playerDt.Index] == (int)PlayerActiveState.JiHuo)
            {
                //处于激活状态的玩家才可以进行游戏操作.
                //传递玩家移动篮筐的数据信息.
                //dirValStr -> [0f, 100f].
                if (dirValStr == PlayerShouBingDir.up.ToString())
                {
                    //雷霆战车中玩家手指离开摇杆(摇杆回中了).
                    return;
                }

                float maxVal = 50f;
                float valTmp = System.Convert.ToInt32(dirValStr);
                //向左平移maxVal单位.
                valTmp -= maxVal;
                valTmp = Mathf.Clamp(valTmp, -maxVal, maxVal);
                float inputVal = valTmp == 0f ? 0f : (valTmp / maxVal);
                InputEventCtrl.GetInstance().ClickHddPadLanKuangBt((SSGameDataCtrl.PlayerIndex)playerDt.Index, inputVal);
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
            if (playerDt.Index > -1 && playerDt.Index < m_MaxPlayerNum)
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
            if (indexPlayer > -1 && indexPlayer < m_MaxPlayerNum)
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
            if (playerDt.Index > -1 && playerDt.Index < m_MaxPlayerNum)
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
                        SSGameDataCtrl.GetInstance().m_PlayerData[indexPlayer].PlayerHeadUrl = playerDt.m_PlayerWeiXinData.headUrl;
                        InputEventCtrl.GetInstance().ClickStartBt((SSGameDataCtrl.PlayerIndex)indexPlayer, InputEventCtrl.ButtonState.DOWN);
                        break;
                    }
            }
        }
    }
}