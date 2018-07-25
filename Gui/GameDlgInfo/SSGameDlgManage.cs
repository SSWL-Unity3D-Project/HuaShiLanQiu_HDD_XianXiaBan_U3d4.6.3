using System.Collections.Generic;
using UnityEngine;

public class SSGameDlgManage : SSGameMono
{
    [System.Serializable]
    public class DlgData
    {
        public SSGameDlgUI.GameDlgState m_DlgState = SSGameDlgUI.GameDlgState.Null;
        public SSGameDlgUI m_GameDlgUI;
        public DlgData(SSGameDlgUI.GameDlgState dlgState, SSGameDlgUI dlgUI)
        {
            m_DlgState = dlgState;
            m_GameDlgUI = dlgUI;
        }
    }
    public List<DlgData> m_DlgDtList = new List<DlgData>();
    
    /// <summary>
    /// 产生游戏对话框文本信息UI.
    /// </summary>
    public void SpawnGameDlgUI(SSGameDlgUI.GameDlgState dlgState, SSGameDataCtrl.PlayerIndex playerIndex, Transform uiParent)
    {
        DlgData dlgDt = m_DlgDtList.Find((dt) => { return dt.m_DlgState.Equals(dlgState); });
        if (dlgDt != null)
        {
            UnityLogWarning("SpawnGameDlgUI -> The DlgUI have been created! dlgState ==== " + dlgState);
            return;
        }

        string uiPrefabPath = "";
        switch (dlgState)
        {
            case SSGameDlgUI.GameDlgState.ShiFouJieShouTiaoZhan:
                {
                    uiPrefabPath = "Prefabs/GUI/GameDlgInfo/ShiFouJieShouTiaoZhan";
                    break;
                }
            case SSGameDlgUI.GameDlgState.ShiFouChongXinKaiShi:
                {
                    uiPrefabPath = "Prefabs/GUI/GameDlgInfo/ShiFouChongXinKaiShi";
                    break;
                }
            case SSGameDlgUI.GameDlgState.FuHuoDaoJu_ZhiFu:
                {
                    uiPrefabPath = "Prefabs/GUI/FuHuoDaoJu/FuHuoDaoJu_ZhiFu";
                    break;
                }
            case SSGameDlgUI.GameDlgState.LanKuangFangDa_ZhiFu:
                {
                    uiPrefabPath = "Prefabs/GUI/LanKuangFangDa/LanKuangFangDa_ZhiFu";
                    break;
                }
            case SSGameDlgUI.GameDlgState.LanQiuJianSu_ZhiFu:
                {
                    uiPrefabPath = "Prefabs/GUI/LanQiuJianSu/LanQiuJianSu_ZhiFu";
                    break;
                }
            case SSGameDlgUI.GameDlgState.ShiFouJieShouTiaoZhan_ZhiFu:
                {
                    uiPrefabPath = "Prefabs/GUI/GameDlgInfo/ShiFouJieShouTiaoZhan_zhiFu";
                    break;
                }
            case SSGameDlgUI.GameDlgState.ShiFouChongXinKaiShi_ZhiFu:
                {
                    uiPrefabPath = "Prefabs/GUI/GameDlgInfo/ShiFouChongXinKaiShi_zhiFu";
                    break;
                }
            default:
                {
                    UnityLogWarning("SpawnGameDlgUI -> have not registered textState ==== " + dlgState);
                    return;
                }
        }

        GameObject gmDataPrefab = (GameObject)Resources.Load(uiPrefabPath);
        if (gmDataPrefab != null)
        {
            UnityLog("SpawnGameDlgUI -> dlgState ==== " + dlgState);
            GameObject obj = (GameObject)Instantiate(gmDataPrefab, uiParent);
            SSGameDlgUI dlgUI = obj.GetComponent<SSGameDlgUI>();
            dlgUI.Init(playerIndex, dlgState);
            m_DlgDtList.Add(new DlgData(dlgState, dlgUI));
        }
        else
        {
            UnityLogWarning("SpawnGameDlgUI -> gmDataPrefab was null, dlgState ==== " + dlgState);
        }
    }

    /// <summary>
    /// 通过对话框类型来查找对象.
    /// </summary>
    public SSGameDlgUI FindGameDlgByType(SSGameDlgUI.GameDlgState dlgState)
    {
        DlgData dlgDt = m_DlgDtList.Find((dt) => { return dt.m_DlgState.Equals(dlgState); });
        if (dlgDt != null)
        {
            return dlgDt.m_GameDlgUI;
        }
        return null;
    }

    /// <summary>
    /// 删除游戏对话框文本信息UI.
    /// </summary>
    internal void RemoveGameDlgUI(SSGameDlgUI.GameDlgState dlgState)
    {
        DlgData dlgDt = m_DlgDtList.Find((dt) => { return dt.m_DlgState.Equals(dlgState); });
        if (dlgDt != null)
        {
            if (dlgDt.m_GameDlgUI != null)
            {
                dlgDt.m_GameDlgUI.RemoveSelf();
            }
            m_DlgDtList.Remove(dlgDt);
        }
    }
}