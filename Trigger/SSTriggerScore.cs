using UnityEngine;

public class SSTriggerScore : SSGameMono
{
    /// <summary>
    /// 玩家索引.
    /// </summary>
    [HideInInspector]
    public SSGameDataCtrl.PlayerIndex m_PlayerIndex = SSGameDataCtrl.PlayerIndex.Null;
    /// <summary>
    /// 是否为空心球得分.
    /// </summary>
    [HideInInspector]
    public bool IsKongXiBallScore = false;

    public void Init()
    {
        IsKongXiBallScore = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsActiveGame)
        {
            return;
        }

        SSBallMoveCtrl ballMove = other.GetComponent<SSBallMoveCtrl>();
        if (ballMove != null)
        {
            if (!ballMove.IsDeFenQiu && !ballMove.IsRemoveSelf && ballMove.IsEnterLanKuang && !ballMove.IsExitLanKuang)
            //if (!ballMove.IsDeFenQiu && !ballMove.IsRemoveSelf && ballMove.m_EnterLanKuangCount >= 1)
            //if (!ballMove.IsDeFenQiu && !ballMove.IsRemoveSelf)
            {
                ballMove.IsDeFenQiu = true;
                bool isKongXiQiu = false;
                bool isHuaQiuKongXin = false;
                if (ballMove.CountOnHit == 0)
                {
                    //UnityLog("SSTriggerScore -> KongXinQiu, m_PlayerIndex == " + m_PlayerIndex);
                    isKongXiQiu = true;
                    SSGameDataCtrl.GetInstance().m_BallSpawnArray[(int)m_PlayerIndex].CheckIsPlayLianFaQiu();
                }
                IsKongXiBallScore = isKongXiQiu;

                int ballScore = 0;
                switch (ballMove.m_BallMoveData.m_LanQiuType)
                {
                    case SSGameDataCtrl.LanQiuType.PuTong:
                        {
                            if (isKongXiQiu)
                            {
                                ballScore = SSGameDataCtrl.GetInstance().m_PuTongBallScoreDt.KongXinQiu;
                                SSGameDataCtrl.GetInstance().PlayKongXinQiuTXAni(SSKongXinQiuTXAni.TeXiaoState.PuTongKongXinQiu, m_PlayerIndex);
                            }
                            else
                            {
                                ballScore = SSGameDataCtrl.GetInstance().m_PuTongBallScoreDt.PuTongQiu;
                            }
                            break;
                        }
                    case SSGameDataCtrl.LanQiuType.HuaShi:
                        {
                            if (isKongXiQiu)
                            {
                                isHuaQiuKongXin = true;
                                ballScore = SSGameDataCtrl.GetInstance().m_HuaShiBallScoreDt.KongXinQiu;
                                SSGameDataCtrl.GetInstance().PlayKongXinQiuTXAni(SSKongXinQiuTXAni.TeXiaoState.HuoQiuKongXinQiu, m_PlayerIndex);
                            }
                            else
                            {
                                ballScore = SSGameDataCtrl.GetInstance().m_HuaShiBallScoreDt.PuTongQiu;
                            }
                            break;
                        }
                    case SSGameDataCtrl.LanQiuType.ZhaDan:
                        {
                            //玩家接住炸弹篮球之后,直接关闭该玩家的篮筐控制权.
                            //UnityLog("Player get zhaDan ball!!! playerIndex == " + m_PlayerIndex);
                            SSGameDataCtrl.GetInstance().SetActivePlayer(m_PlayerIndex, false);
                            ballMove.m_BallAni.CreatZhaDanBallExplosion();
                            return;
                        }
                }

                if (ballMove.m_BallMoveData.IsYanWuTXBall)
                {
                    //烟雾特效篮球分数加倍.
                    ballScore *= SSGameDataCtrl.GetInstance().m_YanWuTXBallScoreBL;
                    ballMove.m_BallAni.RemoveYanWuTXObj();
                }

                if (ballScore > 0)
                {
                    GameObject lanHuanTX = null;
                    Transform expTr = SSGameDataCtrl.GetInstance().m_LanKuang[(int)m_PlayerIndex].m_DeFenExpTr;
                    if (isKongXiQiu)
                    {
                        if (isHuaQiuKongXin)
                        {
                            lanHuanTX = SSGameDataCtrl.GetInstance().m_LanKuangData.m_HuaQiuKongXinQiuExp;
                        }
                        else
                        {
                            lanHuanTX = SSGameDataCtrl.GetInstance().m_LanKuangData.m_KongXinQiuExp;
                        }
                    }
                    else
                    {
                        lanHuanTX = SSGameDataCtrl.GetInstance().m_LanKuangData.m_NoKongXinQiuExp;
                    }

                    if (lanHuanTX != null && expTr != null)
                    {
                        //Instantiate(lanHuanTX, SSGameRootCtrl.GetInstance().MissionCleanup, expTr);
                        GameObject objExp = (GameObject)Instantiate(lanHuanTX, expTr, expTr);
                        SSGameDataCtrl.GetInstance().AddLanHuanExplosionToList(objExp, m_PlayerIndex);
                    }
                    else
                    {
                        UnityLogWarning("lanHuanTX or expTr was null!!!");
                    }

                    SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGamePiaoFenUI(m_PlayerIndex, ballScore);
                }
                SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].Score += ballScore;
                //UnityLog("SSTriggerScore -> Score == " + SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].Score
                //    + ", addScore == " + ballScore
                //    + ", m_PlayerIndex == " + m_PlayerIndex);
            }
            
            //bool isCreatBall = true;
            //if (ballMove.IsLianFaQiu && !ballMove.IsLianFaQiu)
            //{
            //    //是连发球,单不是最后一个连发球,所以不去创建篮球.
            //    isCreatBall = false;
            //}

            //if (SSGameDataCtrl.GetInstance().m_BallSpawnArray[(int)m_PlayerIndex].IsLianFaBall)
            //{
            //    //准备进行连发球.
            //    isCreatBall = false;
            //}

            //if (isCreatBall)
            //{
            //    Debug.Log("SSTriggerScore -> creat next ball...");
            //    SSGameDataCtrl.GetInstance().m_BallSpawnArray[(int)m_PlayerIndex].CreateGameBall();
            //}
        }
    }
}