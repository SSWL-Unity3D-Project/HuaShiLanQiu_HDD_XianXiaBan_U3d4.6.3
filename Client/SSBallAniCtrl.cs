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
    /// <summary>
    /// 玩家索引.
    /// </summary>
    [HideInInspector]
    public SSGameDataCtrl.PlayerIndex IndexPlayer;
    /// <summary>
    /// 是否为开始加分篮球.
    /// </summary>
    [HideInInspector]
    public bool IsStartJiaFenLanQiu = false;
    public void Init(SSGameDataCtrl.PlayerIndex index)
    {
        //Debug.Log("index == " + index);
        IndexPlayer = index;
        if (SSGameDataCtrl.GetInstance().m_PlayerData[(int)index].Score > 0)
        {
            IsStartJiaFenLanQiu = true;
        }

        GameObject ballObj = (GameObject)Instantiate(m_BallData.m_PuTongBallPrefab, m_BallData.m_BallSpawnTr);
        m_BallMove.Init(this, ballObj.transform);
    }

    public IEnumerator DelayDestroyThis(float time)
    {
        SSBallAniCtrl ballAni = GetComponent<SSBallAniCtrl>();
        if (ballAni != null)
        {
            if (ballAni.IsStartJiaFenLanQiu && ballAni.m_BallMove != null && !ballAni.m_BallMove.IsDeFenQiu)
            {
                int scoreVal = SSGameDataCtrl.GetInstance().m_PlayerData[(int)ballAni.IndexPlayer].Score;
                if (scoreVal > 0)
                {
                    //分数大于零后,接篮球有失误时则关闭该玩家的控制权.
                    SSGameDataCtrl.GetInstance().SetActivePlayer(ballAni.IndexPlayer, false);
                }
            }
        }
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}