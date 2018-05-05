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
    /// 连发球间隔最短时间控制.
    /// </summary>
    [Range(0.03f, 10f)]
    float m_TimeMinLianFa = 0.3f;
    /// <summary>
    /// 连发球的数量.
    /// </summary>
    int m_LianFaBallNum = 3;
    int m_LianFaBallCount = 0;
    /// <summary>
    /// 连发球产生点索引.
    /// </summary>
    int m_IndexLianFaSpawn = 0;
    /// <summary>
    /// 是否连发篮球.
    /// </summary>
    [HideInInspector]
    public bool IsLianFaBall = false;
    float m_TimeLastBallSpawn = 0f;
    /// <summary>
    /// 产生点数组.
    /// </summary>
    public Transform[] m_SpawnPointTrArray;

    void Start()
    {
        m_TimeMinLianFa = SSGameDataCtrl.GetInstance().m_BallSpawnData.m_TimeMinLianFa;
    }

    /// <summary>
    /// 初始化连发球信息.
    /// </summary>
    void InitLianFaBallInfo()
    {
        if (!IsLianFaBall)
        {
            IsLianFaBall = true;
            m_LianFaBallCount = 0;

            float randVal = Random.Range(0f, 100f) / 100f;
            if (randVal < SSGameDataCtrl.GetInstance().m_BallCreatRule[IndexCreatBallJieDuan].LianFaBallNum02)
            {
                //连发2球.
                m_LianFaBallNum = 2;
            }
            else
            {
                //连发3球.
                m_LianFaBallNum = 3;
            }
            Debug.Log("InitLianFaBallInfo -> m_LianFaBallNum == " + m_LianFaBallNum + ", player == " + m_PlayerIndex);

            int maxPointVal = SSGameDataCtrl.GetInstance().m_BallCreatRule[IndexCreatBallJieDuan].MaxIndex;
            if (maxPointVal == 0 || maxPointVal > m_SpawnPointTrArray.Length)
            {
                maxPointVal = m_SpawnPointTrArray.Length;
            }
            m_IndexLianFaSpawn = Random.Range(0, 1000) % maxPointVal;
        }
    }

    public void CheckIsPlayLianFaQiu()
    {
        if (!IsLianFaBall)
        {
            float randVal = Random.Range(0f, 100f) / 100f;
            if (randVal < SSGameDataCtrl.GetInstance().m_BallCreatRule[IndexCreatBallJieDuan].LianFaBall)
            {
                InitLianFaBallInfo();
            }
        }
    }

    /// <summary>
    /// 创建篮球的阶段索引.
    /// </summary>
    int IndexCreatBallJieDuan = 0;
    public void Init()
    {
        IndexCreatBallJieDuan = 0;
        IsLianFaBall = false;
        CreatBallJieDuanTimeUp();
    }

    /// <summary>
    /// 创建篮球时间节点时间组件.
    /// </summary>
    void CreatBallJieDuanTimeUp()
    {
        Debug.Log("CreatBallJieDuanTimeUp -> IndexCreatBallJieDuan == " + IndexCreatBallJieDuan + ", time " + Time.time);
        SSTimeUpCtrl timeUp = gameObject.AddComponent<SSTimeUpCtrl>();
        timeUp.Init(SSGameDataCtrl.GetInstance().m_BallCreatRule[IndexCreatBallJieDuan].TimeVal);
        timeUp.OnTimeUpOverEvent += OnCreatBallTimeUpOverEvent;
    }

    private void OnCreatBallTimeUpOverEvent()
    {
        if (!SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsActiveGame)
        {
            return;
        }

        if (IndexCreatBallJieDuan < SSGameDataCtrl.GetInstance().m_BallCreatRule.Length - 1)
        {
            IndexCreatBallJieDuan++;
            CreatBallJieDuanTimeUp();
        }
    }

    //Update is called once per frame
    void Update()
    {
        if (!SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsActiveGame)
        {
            return;
        }

        if (IsLianFaBall)
        {
            if (Time.time - m_TimeLastBallSpawn >= m_TimeMinLianFa)
            {
                m_TimeLastBallSpawn = Time.time;
                //连续发射篮球.
                SpawnGameBall(m_IndexLianFaSpawn);
                m_LianFaBallCount++;
                Debug.Log("LianFaQiu -> index == " + m_LianFaBallCount);
                if (m_LianFaBallCount >= m_LianFaBallNum)
                {
                    IsLianFaBall = false;
                }
            }
        }
    }

    /// <summary>
    /// 创建篮球.
    /// </summary>
    public void CreatGameBall()
    {
        SpawnGameBall();
    }

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
            Debug.LogWarning("SpawnGameBall -> m_BallPrefabArray[" + randBallVal + "] is null");
            return;
        }

        if (m_SpawnPointTrArray.Length <= 0)
        {
            Debug.LogWarning("SpawnGameBall -> m_SpawnPointTrArray was wrong!");
            return;
        }

        int maxPointVal = SSGameDataCtrl.GetInstance().m_BallCreatRule[IndexCreatBallJieDuan].MaxIndex;
        if (maxPointVal == 0 || maxPointVal > m_SpawnPointTrArray.Length)
        {
            maxPointVal = m_SpawnPointTrArray.Length;
        }

        int randVal = (indexBallSpawn == -1) ? Random.Range(0, 1000) % maxPointVal : indexBallSpawn;
        if (m_SpawnPointTrArray[randVal] == null)
        {
            Debug.LogWarning("SpawnGameBall -> m_SpawnPointTrArray[" + randVal + "] is null");
            return;
        }

        Transform trSpawn = m_SpawnPointTrArray[randVal].transform;
        GameObject obj = (GameObject)Instantiate(m_BallPrefabArray[randBallVal], trSpawn.position, trSpawn.rotation);
        obj.transform.parent = SSGameRootCtrl.GetInstance().MissionCleanup;
        SSBallAniCtrl ballAni = obj.GetComponent<SSBallAniCtrl>();
        ballAni.Init(m_PlayerIndex);

        if (IsLianFaBall)
        {
            ballAni.m_BallMove.IsLastLianFaQiu = true;
            if (m_LianFaBallCount >= m_LianFaBallNum - 1)
            {
                //连发球的最后一个.
                ballAni.m_BallMove.IsLastLianFaQiu = true;
            }
            else
            {
                ballAni.m_BallMove.IsLastLianFaQiu = false;
            }
        }
    }
}