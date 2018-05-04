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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SetActivePlayer(PlayerIndex.Player01, true);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            SetActivePlayer(PlayerIndex.Player02, true);
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
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
}