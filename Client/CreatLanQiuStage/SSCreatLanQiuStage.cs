using UnityEngine;

public class SSCreatLanQiuStage : SSGameMono
{
    /// <summary>
    /// 创建篮球的阶段索引.
    /// </summary>
    [HideInInspector]
    public int IndexCreatBallJieDuan = 0;
    /// <summary>
    /// 控制篮球发球阶段的时间控制器.
    /// </summary>
    SSTimeUpCtrl m_TimeUpCreatBallStageCom;
    public void Init()
    {
        if (SSGameDataCtrl.GetInstance().m_LastOverPlayerData != null)
        {
            //恢复继续游戏玩家的数据.
            IndexCreatBallJieDuan = SSGameDataCtrl.GetInstance().m_LastOverPlayerData.IndexCreatBallJieDuan;
            SSGameDataCtrl.GetInstance().IsCreatLanQiuJianSuDaoJuBuy = SSGameDataCtrl.GetInstance().m_LastOverPlayerData.IsCreatLanQiuJianSuDaoJuBuy;
            if (SSGameDataCtrl.GetInstance().IsCreatLanQiuJianSuDaoJuBuy
                || IndexCreatBallJieDuan > SSGameDataCtrl.GetInstance().IndexJieDuanFangDaLanKuang - 1)
            {
                //开启道具顶部购买提示事件.
                SSGameDataCtrl.GetInstance().m_SSUIRoot.CreatGameDaoJuTimeEvent();
            }
            //清理最后一个结束游戏玩家的数据.
            SSGameDataCtrl.GetInstance().CleanLastOverPlayerData();
        }
        else
        {
            IndexCreatBallJieDuan = 0;
        }
    }

    /// <summary>
    /// 重置信息,清理TimeUp组件.
    /// </summary>
    public void Reset()
    {
        if (m_TimeUpCreatBallStageCom != null)
        {
            Destroy(m_TimeUpCreatBallStageCom);
        }
    }

    /// <summary>
    /// 创建篮球时间节点时间组件.
    /// </summary>
    public void CreatBallJieDuanTimeUp()
    {
        //UnityLog("CreatBallJieDuanTimeUp -> IndexCreatBallJieDuan == " + IndexCreatBallJieDuan + ", time " + Time.time);
        m_TimeUpCreatBallStageCom = gameObject.AddComponent<SSTimeUpCtrl>();
        m_TimeUpCreatBallStageCom.Init(SSGameDataCtrl.GetInstance().GetBallCreatRuleDt(IndexCreatBallJieDuan).TimeVal);
        m_TimeUpCreatBallStageCom.OnTimeUpOverEvent += OnCreatBallTimeUpOverEvent;
    }

    private void OnCreatBallTimeUpOverEvent()
    {
        //if (!SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsActiveGame)
        //{
        //    return;
        //}

        if (IndexCreatBallJieDuan < SSGameDataCtrl.GetInstance().m_BallCreatRule.Length - 1)
        {
            bool isCreatBallJieDuanTimeUp = true;
            if (IndexCreatBallJieDuan == SSGameDataCtrl.GetInstance().IndexJieDuanFangDaLanKuang - 1)
            {
                isCreatBallJieDuanTimeUp = false;
                SSGameDataCtrl.GetInstance().IsStopCreatBall = true;
                //SSGameDataCtrl.GetInstance().m_BallSpawnPointDaoJu = this;
            }


            if (!SSGameDataCtrl.GetInstance().IsCreatLanQiuJianSuDaoJuBuy)
            {
                if (SSGameDataCtrl.GetInstance().m_LianKuangTimeAni != null
                    && !SSGameDataCtrl.GetInstance().m_LianKuangTimeAni.IsPlayTimeAni)
                {
                    //玩家现在没有用篮筐放大道具.
                    if (IndexCreatBallJieDuan >= SSGameDataCtrl.GetInstance().IndexJieDuanLanQiuJianSu - 1)
                    {
                        SSGameDataCtrl.GetInstance().IsCreatLanQiuJianSuDaoJuBuy = true;
                        isCreatBallJieDuanTimeUp = false;
                        SSGameDataCtrl.GetInstance().IndexJieDuanLanQiuJianSu = IndexCreatBallJieDuan + 1;
                        SSGameDataCtrl.GetInstance().IsStopCreatBall = true;
                        //SSGameDataCtrl.GetInstance().m_BallSpawnPointDaoJu = this;
                    }
                }
            }

            if (isCreatBallJieDuanTimeUp)
            {
                AddIndexCreatBallJieDuan();
            }
        }
    }

    /// <summary>
    /// 增加发球阶段索引.
    /// </summary>
    public void AddIndexCreatBallJieDuan()
    {
        //if (!SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsActiveGame
        //    || !SSGameDataCtrl.GetInstance().m_PlayerData[(int)m_PlayerIndex].IsCreateGameBall)
        //{
        //    return;
        //}

        if (IndexCreatBallJieDuan < SSGameDataCtrl.GetInstance().m_BallCreatRule.Length - 1)
        {
            IndexCreatBallJieDuan++;
            CreatBallJieDuanTimeUp();
        }
    }

    /// <summary>
    /// 检测产生道具购买UI界面.
    /// </summary>
    public void CheckSpawnDaoJuGouMaiUI()
    {
        SSGameDataCtrl.PlayerIndex index = GetPlayerIndexDaoJu();
        if (IndexCreatBallJieDuan == SSGameDataCtrl.GetInstance().IndexJieDuanFangDaLanKuang - 1)
        {
            //创建篮筐放大道具购买界面.
            SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameLanKuangFangDaPanel(index);
        }

        if (IndexCreatBallJieDuan == SSGameDataCtrl.GetInstance().IndexJieDuanLanQiuJianSu - 1)
        {
            //创建篮球减速道具购买界面.
            SSGameDataCtrl.GetInstance().m_SSUIRoot.SpawnGameLanQiuJianSuPanel(index);
        }
    }

    /// <summary>
    /// 获取道具界面产生的玩家索引.
    /// </summary>
    public SSGameDataCtrl.PlayerIndex GetPlayerIndexDaoJu()
    {
        SSGameDataCtrl.PlayerIndex index = SSGameDataCtrl.PlayerIndex.Null;
        int count = 0;
        for (int i = 0; i < SSGameDataCtrl.GetInstance().m_PlayerData.Length; i++)
        {
            if (SSGameDataCtrl.GetInstance().m_PlayerData[i].IsActiveGame
                && SSGameDataCtrl.GetInstance().m_PlayerData[i].IsCreateGameBall)
            {
                index = (SSGameDataCtrl.PlayerIndex)i;
                count++;
            }
        }

        if (count == SSGameDataCtrl.GetInstance().m_PlayerData.Length)
        {
            index = SSGameDataCtrl.PlayerIndex.Null;
        }
        return index;
    }
}