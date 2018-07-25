using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏文本消息UI管理.
/// </summary>
public class SSGameTextManage : SSGameMono
{
    public enum GameTextState
    {
        Null = -1,
        /// <summary>
        /// 请等待对方同意PK.
        /// </summary>
        DengDuiFangTongYiPK = 0,
        /// <summary>
        /// 开始双人PK模式.
        /// </summary>
        KaiShiShuangRenPK = 1,
        /// <summary>
        /// 对方不敢应战,请等待对方本场游戏结束.
        /// </summary>
        DuiFangBuYingZhan_DengDuiFangJieShu = 2,
        /// <summary>
        /// 请等待对方本场游戏结束.
        /// </summary>
        DengDuiFangJieShu = 3,
        /// <summary>
        /// 对方不敢应战,请继续等待.
        /// </summary>
        DuiFangBuYingZhan_JiXuDengDai = 4,
        /// <summary>
        /// 对方已离开,切换为单人模式.
        /// </summary>
        DuiFangYiLiKai_QieWeiDanRen = 5,
    }

    [System.Serializable]
    public class TextDate
    {
        public GameTextState m_TextState = GameTextState.Null;
        public GameObject m_GameTextUI;
        public TextDate(GameTextState textState, GameObject obj)
        {
            m_TextState = textState;
            m_GameTextUI = obj;
        }
    }
    public List<TextDate> m_TextDtList = new List<TextDate>();

    /// <summary>
    /// 通过文本类型来查找对象.
    /// </summary>
    public TextDate FindGameTextUIByType(GameTextState type)
    {
        TextDate dtFind = m_TextDtList.Find((dt) => { return dt.m_TextState.Equals(type); });
        return dtFind;
    }

    /// <summary>
    /// 产生游戏文本信息UI.
    /// </summary>
    public void SpawnGameTextUI(GameTextState textState, Transform uiParent)
    {
        TextDate textDt = m_TextDtList.Find((dt) => { return dt.m_TextState.Equals(textState); });
        if (textDt != null)
        {
            UnityLogWarning("SpawnGameTextUI -> The TextUI have been created! textState ==== " + textState);
            return;
        }

        string uiTextPrefabPath = "";
        switch (textState)
        {
            case GameTextState.DengDuiFangTongYiPK:
                {
                    uiTextPrefabPath = "Prefabs/GUI/GameTextInfo/DengDaiDuiFangTongYiPK";
                    break;
                }
            case GameTextState.KaiShiShuangRenPK:
                {
                    uiTextPrefabPath = "Prefabs/GUI/GameTextInfo/KaiShiShuangRenPK";
                    break;
                }
            case GameTextState.DuiFangBuYingZhan_DengDuiFangJieShu:
                {
                    uiTextPrefabPath = "Prefabs/GUI/GameTextInfo/DuiFangBuGanYingZhan_DengDuiFangJieShu";
                    break;
                }
            case GameTextState.DengDuiFangJieShu:
                {
                    uiTextPrefabPath = "Prefabs/GUI/GameTextInfo/DengDuiFangJieShu";
                    break;
                }
            case GameTextState.DuiFangBuYingZhan_JiXuDengDai:
                {
                    uiTextPrefabPath = "Prefabs/GUI/GameTextInfo/DuiFangBuGanYingZhan_JiXuDeng";
                    break;
                }
            case GameTextState.DuiFangYiLiKai_QieWeiDanRen:
                {
                    uiTextPrefabPath = "Prefabs/GUI/GameTextInfo/DuiFangYiLiKai_QieWeiDanRen";
                    break;
                }
            default:
                {
                    UnityLogWarning("SpawnGameTextUI -> have not registered textState ==== " + textState);
                    return;
                }
        }

        GameObject gmDataPrefab = (GameObject)Resources.Load(uiTextPrefabPath);
        if (gmDataPrefab != null)
        {
            UnityLog("SpawnGameTextUI -> textState ==== " + textState);
            GameObject obj = (GameObject)Instantiate(gmDataPrefab, uiParent);
            m_TextDtList.Add(new TextDate(textState, obj));
        }
        else
        {
            UnityLogWarning("SpawnGameTextUI -> gmDataPrefab was null, textState ==== " + textState);
        }
    }

    /// <summary>
    /// 删除游戏文本信息UI.
    /// </summary>
    internal void RemoveGameTextUI(GameTextState textState)
    {
        TextDate textDt = m_TextDtList.Find((dt) => { return dt.m_TextState.Equals(textState); });
        if (textDt != null)
        {
            if (textDt.m_GameTextUI != null)
            {
                Destroy(textDt.m_GameTextUI);
            }
            m_TextDtList.Remove(textDt);
        }
    }
}