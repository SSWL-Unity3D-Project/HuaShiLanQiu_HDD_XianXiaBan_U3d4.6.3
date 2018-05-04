using System;
using UnityEngine;

public class SSGameDataCtrl : MonoBehaviour
{
    public enum PlayerIndex
    {
        Player01 = 0,
        Player02 = 1,
    }

    public class PlayerData
    {
        /// <summary>
        /// 是否激活了游戏.
        /// </summary>
        public bool IsActiveGame = false;
        /// <summary>
        /// 玩家分数信息.
        /// </summary>
        public int Score = 0;
    }
    public PlayerData[] m_PlayerData = new PlayerData[2];

    public SSLanKuangCtrl[] m_LanKuang = new SSLanKuangCtrl[2];

    static SSGameDataCtrl _Instance;
    public static SSGameDataCtrl GetInstance()
    {
        return _Instance;
    }

    void Awake()
    {
        Screen.SetResolution(1280, 720, false);
        m_PlayerData[0] = new PlayerData();
        m_PlayerData[1] = new PlayerData();
        _Instance = this;
        
        //SetActivePlayer(PlayerIndex.Player01, true); //test
        InputEventCtrl.GetInstance().OnClickStartBtEvent += OnClickStartBtEvent;
    }

    private void OnClickStartBtEvent(PlayerIndex index, InputEventCtrl.ButtonState val)
    {
        if (val == InputEventCtrl.ButtonState.DOWN)
        {
            SetActivePlayer(index, true);
        }
    }

    public void SetActivePlayer(PlayerIndex index, bool isActive)
    {
        if (m_PlayerData[(int)index].IsActiveGame == isActive)
        {
            return;
        }

        Debug.Log("SetActivePlayer -> index " + index + ", isActive " + isActive);
        m_PlayerData[(int)index].IsActiveGame = isActive;
        if (isActive)
        {
            //重置信息.
            m_PlayerData[(int)index].Score = 0;
            m_LanKuang[(int)index].ActiveLanQiuCollider();
        }
    }

    /// <summary>
    /// Websocket预制.
    /// </summary>
    public GameObject m_WebSocketBoxPrefab;
    /// <summary>
    /// 产生Websocket预制.
    /// </summary>
    public GameObject SpawnWebSocketBox(Transform tr)
    {
        Debug.Log("SpawnWebSocketBox...");
        GameObject obj = (GameObject)Instantiate(m_WebSocketBoxPrefab);
        obj.transform.parent = tr;
        return obj;
    }
}