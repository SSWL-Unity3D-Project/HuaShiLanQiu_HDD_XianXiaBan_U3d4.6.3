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
        /// 普通着火篮球模型预制.
        /// </summary>
        public GameObject m_PuTongHuoBallPrefab;
        /// <summary>
        /// 花式着火篮球模型预制.
        /// </summary>
        public GameObject m_HuaShiHuoBallPrefab;
        /// <summary>
        /// 炸弹篮球模型预制.
        /// </summary>
        public GameObject m_ZhaDanBallPrefab;
        /// <summary>
        /// 炸弹篮球爆炸粒子预制.
        /// </summary>
        public GameObject m_ExpZhaDanBallPrefab;
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
    [HideInInspector]
    public GameObject m_YanWuTXObj;

    /// <summary>
    /// 玩家索引.
    /// </summary>
    [HideInInspector]
    public SSGameDataCtrl.PlayerIndex IndexPlayer;
    bool IsDestroySelf = false;
    public void Init(SSGameDataCtrl.PlayerIndex index, int indexSpawnBallJieDuan)
    {
        //Debug.Log("index == " + index);
        IndexPlayer = index;
        bool isYanWuTxBall = false;
        float rv = Random.Range(0, 100) / 100f;
        GameObject ballPrefab = null;
        SSGameDataCtrl.LanQiuType lanQiuType = SSGameDataCtrl.LanQiuType.Null;
        bool isLianFaQiu = SSGameDataCtrl.GetInstance().m_BallSpawnArray[(int)index].IsLianFaBall;
        if (!isLianFaQiu && rv < SSGameDataCtrl.GetInstance().m_BallCreatRule[indexSpawnBallJieDuan].ZhaDanBall)
        {
            //产生炸弹篮球.
            lanQiuType = SSGameDataCtrl.LanQiuType.ZhaDan;
            ballPrefab = m_BallData.m_ZhaDanBallPrefab;
            if (ballPrefab == null)
            {
                UnityLogWarning("m_ZhaDanBallPrefab was null!!!");
                return;
            }
        }
        else
        {
            isYanWuTxBall = SSGameDataCtrl.GetInstance().m_LanKuang[(int)index].m_SSTriggerScore.IsKongXiBallScore;
            if (isYanWuTxBall)
            {
                if (m_YanWuTXDt.m_YanWuTXPrefab != null && m_YanWuTXDt.m_YanWuTXSpawnTr != null)
                {
                    //产生烟雾特效.
                    m_YanWuTXObj = (GameObject)Instantiate(m_YanWuTXDt.m_YanWuTXPrefab, m_YanWuTXDt.m_YanWuTXSpawnTr);
                }
            }

            rv = Random.Range(0, 100) / 100f;
            if (rv < SSGameDataCtrl.GetInstance().m_BallCreatRule[indexSpawnBallJieDuan].PuTongBall)
            {
                //产生普通篮球.
                lanQiuType = SSGameDataCtrl.LanQiuType.PuTong;
                if (isYanWuTxBall)
                {
                    ballPrefab = m_BallData.m_PuTongHuoBallPrefab;
                }
                else
                {
                    ballPrefab = m_BallData.m_PuTongBallPrefab;
                }
            }
            else
            {
                //产生花式篮球.
                if (isYanWuTxBall)
                {
                    ballPrefab = m_BallData.m_HuaShiHuoBallPrefab;
                }
                else
                {
                    ballPrefab = m_BallData.m_HuaShiBallPrefab;
                }
                lanQiuType = SSGameDataCtrl.LanQiuType.HuaShi;
            }
        }

        GameObject ballObj = (GameObject)Instantiate(ballPrefab, m_BallData.m_BallSpawnTr);
        SSBallMoveCtrl.BallMoveData ballMoveDt = new SSBallMoveCtrl.BallMoveData(lanQiuType, ballObj.transform, this, isYanWuTxBall);
        m_BallMove.Init(ballMoveDt);
    }

    public IEnumerator DelayDestroyThis(float time)
    {
        if (IsDestroySelf == true)
        {
            yield break;
        }
        IsDestroySelf = true;

        //bool isCreatBall = false;
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
        //else
        //{
        //    if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)IndexPlayer].IsActiveGame)
        //    {
        //        //还没有得分.
        //        isCreatBall = true;
        //    }
        //}

        //if (isCreatBall)
        //{
        //    Debug.Log("DelayDestroyThis -> creat next ball...");
        //    SSGameDataCtrl.GetInstance().m_BallSpawnArray[(int)IndexPlayer].CreateGameBall();
        //}
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    /// <summary>
    /// 删除篮球烟雾特效.
    /// </summary>
    public void RemoveYanWuTXObj()
    {
        if (m_YanWuTXObj != null)
        {
            Destroy(m_YanWuTXObj);
        }
    }

    /// <summary>
    /// 创建炸弹篮球的爆炸粒子特效.
    /// </summary>
    public void CreatZhaDanBallExplosion()
    {
        if (m_BallData.m_ExpZhaDanBallPrefab != null)
        {
            GameObject obj = (GameObject)Instantiate(m_BallData.m_ExpZhaDanBallPrefab, transform.position, transform.rotation);
            obj.transform.SetParent(SSGameRootCtrl.GetInstance().MissionCleanup);
        }
        else
        {
            UnityLogWarning("m_ExpZhaDanBallPrefab was null!!!");
        }
    }
}