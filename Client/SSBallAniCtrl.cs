using System.Collections;
using UnityEngine;

public class SSBallAniCtrl : SSGameMono
{
    /// <summary>
    /// 篮球运动控制脚本.
    /// </summary>
    public SSBallMoveCtrl m_BallMove;
    [System.Serializable]
    public class BallData
    {
        /// <summary>
        /// 普通篮球模型预制.
        /// </summary>
        public GameObject m_PuTongBallPrefab;
        /// <summary>
        /// 花式篮球模型预制.
        /// </summary>
        public GameObject m_HuaShiBallPrefab;
        /// <summary>
        /// 篮球产生点.
        /// </summary>
        public Transform m_BallSpawnTr;
    }
    public BallData m_BallData;

    [System.Serializable]
    public class YanWuTXData
    {
        /// <summary>
        /// 烟雾特效预制.
        /// </summary>
        public GameObject m_YanWuTXPrefab;
        public Transform m_YanWuTXSpawnTr;
    }
    public YanWuTXData m_YanWuTXDt;
    /// <summary>
    /// 玩家索引.
    /// </summary>
    [HideInInspector]
    public SSGameDataCtrl.PlayerIndex IndexPlayer;
    public void Init(SSGameDataCtrl.PlayerIndex index)
    {
        //Debug.Log("index == " + index);
        IndexPlayer = index;
        bool isYanWuTxBall = SSGameDataCtrl.GetInstance().m_LanKuang[(int)index].m_SSTriggerScore.IsKongXiBallScore;
        if (isYanWuTxBall)
        {
            if (m_YanWuTXDt.m_YanWuTXPrefab != null && m_YanWuTXDt.m_YanWuTXSpawnTr != null)
            {
                //产生烟雾特效.
                Instantiate(m_YanWuTXDt.m_YanWuTXPrefab, m_YanWuTXDt.m_YanWuTXSpawnTr);
            }
        }
        GameObject ballObj = (GameObject)Instantiate(m_BallData.m_PuTongBallPrefab, m_BallData.m_BallSpawnTr);
        SSBallMoveCtrl.BallMoveData ballMoveDt = new SSBallMoveCtrl.BallMoveData(SSGameDataCtrl.LanQiuType.PuTong,
                                                        ballObj.transform, this, isYanWuTxBall);
        m_BallMove.Init(ballMoveDt);
    }

    public IEnumerator DelayDestroyThis(float time)
    {
        bool isCreatBall = false;
        if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)IndexPlayer].Score > 0)
        {
            if (m_BallMove != null && !m_BallMove.IsDeFenQiu)
            {
                int scoreVal = SSGameDataCtrl.GetInstance().m_PlayerData[(int)IndexPlayer].Score;
                if (scoreVal > 0)
                {
                    //分数大于零后,接篮球有失误时则关闭该玩家的控制权.
                    SSGameDataCtrl.GetInstance().SetActivePlayer(IndexPlayer, false);
                }
            }
        }
        else
        {
            //还没有得分.
            isCreatBall = true;
        }

        if (isCreatBall)
        {
            Debug.Log("DelayDestroyThis -> creat next ball...");
            SSGameDataCtrl.GetInstance().m_BallSpawnArray[(int)IndexPlayer].CreatGameBall();
        }
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}