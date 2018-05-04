using UnityEngine;

public class SSBallSpawnPoint : MonoBehaviour
{
    /// <summary>
    /// 玩家索引.
    /// </summary>
    public SSGameDataCtrl.PlayerIndex m_PlayerIndex;
    /// <summary>
    /// 篮球预制.
    /// </summary>
    public GameObject[] m_BallPrefabArray;
    /// <summary>
    /// 产生篮球的间隔时间控制.
    /// </summary>
    [Range(0.03f, 10f)]
    public float m_TimeMinSpawn = 2f;
    float m_TimeBallSpawnRecord = 0f;
    /// <summary>
    /// 连发球间隔最短时间控制.
    /// </summary>
    [Range(0.03f, 10f)]
    public float m_TimeMinLianFa = 0.3f;
    public int m_MaxLianFaBallNum = 5;
    public int m_MinLianFaBallNum = 2;
    int m_LianFaBallNum = 3;
    int m_LianFaBallCount = 0;
    /// <summary>
    /// 连发球产生点索引.
    /// </summary>
    int m_IndexLianFaSpawn = 0;
    /// <summary>
    /// 是否连发篮球.
    /// </summary>
    bool IsLianFaBall = false;
    SSTimeDownCtrl m_TimeDownCom;

    float m_TimeLastBallSpawn = 0f;
    void Start()
    {
        m_TimeBallSpawnRecord = m_TimeMinSpawn;
        m_TimeDownCom = gameObject.AddComponent<SSTimeDownCtrl>();
        m_TimeDownCom.Init(3600f, 3f);
        m_TimeDownCom.OnTimeDownStepEvent += OnTimeDownStepLianFaBallEvent;
    }

    /// <summary>
    /// 连发球时间到达事件响应.
    /// </summary>
    void OnTimeDownStepLianFaBallEvent(float timeVal)
    {
        if (!IsLianFaBall)
        {
            if (Random.Range(0, 100) % 2 == 0)
            {
                IsLianFaBall = true;
                m_LianFaBallCount = 0;
                m_TimeMinSpawn = m_TimeMinLianFa;
                m_LianFaBallNum = (Random.Range(0, 100) % (m_MaxLianFaBallNum - m_MinLianFaBallNum + 1)) + m_MinLianFaBallNum;
                m_IndexLianFaSpawn = Random.Range(0, 1000) % m_SpawnPointTrArray.Length;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsActiveGame)
        {
            return;
        }

        if (Time.time - m_TimeLastBallSpawn >= m_TimeMinSpawn)
        {
            m_TimeLastBallSpawn = Time.time;
            if (IsLianFaBall)
            {
                //连续发射篮球.
                SpawnGameBall(m_IndexLianFaSpawn);
                m_LianFaBallCount++;
                if (m_LianFaBallCount >= m_LianFaBallNum)
                {
                    IsLianFaBall = false;
                    m_TimeMinSpawn = m_TimeBallSpawnRecord;
                }
            }
            else
            {
                SpawnGameBall();
            }
        }
	}
    
    public Transform[] m_SpawnPointTrArray;
    void SpawnGameBall(int indexBallSpawn = -1)
    {
        if (m_BallPrefabArray.Length <= 0)
        {
            Debug.LogWarning("m_BallPrefabArray -> m_BallPrefab is wrong!");
            return;
        }

        int randBallVal = Random.Range(0, 100) % m_BallPrefabArray.Length;
        if (m_BallPrefabArray[randBallVal] == null)
        {
            return;
        }

        if (m_SpawnPointTrArray.Length <= 0)
        {
            Debug.LogWarning("SpawnGameBall -> m_SpawnPointTrArray was wrong!");
            return;
        }

        int randVal = (indexBallSpawn == -1) ? Random.Range(0, 100) % m_SpawnPointTrArray.Length : indexBallSpawn;
        if (m_SpawnPointTrArray[randVal] == null)
        {
            return;
        }

        Transform trSpawn = m_SpawnPointTrArray[randVal].transform;
        GameObject obj = (GameObject)Instantiate(m_BallPrefabArray[randBallVal], trSpawn.position, trSpawn.rotation);
        obj.transform.parent = SSGameRootCtrl.GetInstance().MissionCleanup;
        SSBallAniCtrl ballAni = obj.GetComponent<SSBallAniCtrl>();
        ballAni.Init(m_PlayerIndex);
    }
}