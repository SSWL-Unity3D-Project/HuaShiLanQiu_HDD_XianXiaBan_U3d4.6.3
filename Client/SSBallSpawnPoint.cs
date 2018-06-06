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
    /// <summary>
    /// 最后一次发球产生点的索引信息.
    /// </summary>
    int m_LastIndexPoint = 0;

    void Awake()
    {
        HiddenLanQiuSpawnPoint();
    }

    /// <summary>
    /// 隐藏篮球产生点.
    /// </summary>
    void HiddenLanQiuSpawnPoint()
    {
        for (int i = 0; i < m_SpawnPointTrArray.Length; i++)
        {
            if (m_SpawnPointTrArray[i] != null)
            {
                m_SpawnPointTrArray[i].gameObject.SetActive(false);
            }
        }
        //if (transform.childCount > 0)
        //{
        //    //隐藏产生点上的篮球.
        //    GameObject obj = transform.GetChild(0).gameObject;
        //    obj.SetActive(false);
        //}
    }

    /// <summary>
    /// 初始化连发球信息.
    /// </summary>
    void InitLianFaBallInfo()
    {
        if (!IsLianFaBall)
        {
            IsLianFaBall = true;
            m_LastLianFaTime = Time.time;
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

            m_TimeMinLianFa = SSGameDataCtrl.GetInstance().m_BallCreatRule[IndexCreatBallJieDuan].m_TimeMinLianFa;
            int maxPointVal = SSGameDataCtrl.GetInstance().m_BallCreatRule[IndexCreatBallJieDuan].MaxIndex;
            if (maxPointVal == 0 || maxPointVal > m_SpawnPointTrArray.Length)
            {
                maxPointVal = m_SpawnPointTrArray.Length;
            }
            m_IndexLianFaSpawn = Random.Range(0, 1000) % maxPointVal;
        }
    }

    float m_LastLianFaTime = 0f;
    /// <summary>
    /// 连发最后一球之后是否间隔一会时间.
    /// </summary>
    bool IsLianFaBallJianGe = false;
    public void CheckIsPlayLianFaQiu()
    {
        if (IsLianFaBallJianGe)
        {
            //等待连发间隔时间结束后再继续发球.
            return;
        }

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
    [HideInInspector]
    public int IndexCreatBallJieDuan = 0;
    public void Init()
    {
        IndexCreatBallJieDuan = 0;
        IsLianFaBall = false;
        CreatBallJieDuanTimeUp();
    }

    public void ResetInfo()
    {
        if (m_TimeUpCom != null)
        {
            Destroy(m_TimeUpCom);
        }

        IndexCreatBallJieDuan = 0;
        IsLianFaBall = false;
        IsLianFaBallJianGe = false;
    }

    SSTimeUpCtrl m_TimeUpCom;
    /// <summary>
    /// 创建篮球时间节点时间组件.
    /// </summary>
    void CreatBallJieDuanTimeUp()
    {
        Debug.Log("CreatBallJieDuanTimeUp -> IndexCreatBallJieDuan == " + IndexCreatBallJieDuan + ", time " + Time.time);
        m_TimeUpCom = gameObject.AddComponent<SSTimeUpCtrl>();
        m_TimeUpCom.Init(SSGameDataCtrl.GetInstance().m_BallCreatRule[IndexCreatBallJieDuan].TimeVal);
        m_TimeUpCom.OnTimeUpOverEvent += OnCreatBallTimeUpOverEvent;
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
        if (!SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsActiveGame
            || !SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsCreateGameBall)
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
                    IsLianFaBallJianGe = true;
                    m_LastLianFaTime = Time.time;
                }
            }
        }
        else
        {
            if (IsLianFaBallJianGe)
            {
                if (Time.time - m_LastLianFaTime >= SSGameDataCtrl.GetInstance().m_BallCreatRule[IndexCreatBallJieDuan].m_TimeLianFa)
                {
                    //连发最后一球后,间隔一段时间在发球.
                    IsLianFaBallJianGe = false;
                    m_LastLianFaTime = Time.time;
                    SpawnGameBall();
                }
            }
            else
            {

                if (Time.time - m_LastLianFaTime >= SSGameDataCtrl.GetInstance().m_BallCreatRule[IndexCreatBallJieDuan].m_TimeDanFa)
                {
                    Debug.Log("SpawnPoint -> creat next ball...");
                    m_LastLianFaTime = Time.time;
                    CreateGameBall();
                }
            }
        }
    }

    /// <summary>
    /// 创建篮球.
    /// </summary>
    public void CreateGameBall()
    {
        if (IsLianFaBallJianGe)
        {
            //等待连发间隔时间结束后再继续发球.
            return;
        }
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
        if (indexBallSpawn == -1 && m_LastIndexPoint == randVal)
        {
            //避免和上次的产生点索引相同.
            if (randVal > 0)
            {
                randVal--;
            }
            else
            {
                randVal++;
            }
        }

        if (m_SpawnPointTrArray[randVal] == null)
        {
            Debug.LogWarning("SpawnGameBall -> m_SpawnPointTrArray[" + randVal + "] is null");
            return;
        }

        m_LastIndexPoint = randVal;
        Transform trSpawn = m_SpawnPointTrArray[randVal].transform;
        GameObject obj = (GameObject)Instantiate(m_BallPrefabArray[randBallVal], trSpawn.position, trSpawn.rotation);
        obj.transform.SetParent(SSGameRootCtrl.GetInstance().MissionCleanup);
        SSBallAniCtrl ballAni = obj.GetComponent<SSBallAniCtrl>();
        ballAni.Init(m_PlayerIndex, IndexCreatBallJieDuan);

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