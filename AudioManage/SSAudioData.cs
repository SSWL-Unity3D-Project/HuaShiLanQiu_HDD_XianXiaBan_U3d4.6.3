using UnityEngine;

public class SSAudioData : SSAudioManager
{
    /// <summary>
    /// 接球失误音效.
    /// </summary>
	public AudioSource m_JieQiuShiWuAd;
    /// <summary>
    /// 引导界面音效.
    /// </summary>
    public AudioSource m_YinDaoJieMianAd;
    /// <summary>
    /// 游戏背景音效.
    /// </summary>
    public AudioSource[] m_GameBeiJingAd;
    public void Init()
    {
        ResetGameAudioSource();
        PlayYinDaoJieMianAudio(YinDaoAudioState.Null);
    }

    void ResetGameAudioSource()
    {
        int max = m_GameBeiJingAd.Length;
        for (int i = 0; i < max; i++)
        {
            CloseAudioSourceOnAwake(m_GameBeiJingAd[i]);
        }

        CloseAudioSourceOnAwake(m_YinDaoJieMianAd);
        CloseAudioSourceOnAwake(m_JieQiuShiWuAd);
    }

    void CloseAudioSourceOnAwake(AudioSource asVal)
    {
        if (asVal == null)
        {
            return;
        }
        asVal.playOnAwake = false;
    }

    /// <summary>
    /// 播放接球失误音效.
    /// </summary>
    public void PlayJieQiuShiWuAudio()
    {
        if (m_JieQiuShiWuAd != null)
        {
            PlayAudio(m_JieQiuShiWuAd, AudioState.StopToPlay);
        }
    }
    
    public enum YinDaoAudioState
    {
        Null = -1,
        /// <summary>
        /// 停止背景音效.
        /// </summary>
        StopBeiJingAudio = 0,
    }

    /// <summary>
    /// 播放引导界面音效.
    /// </summary>
    public void PlayYinDaoJieMianAudio(YinDaoAudioState key = YinDaoAudioState.StopBeiJingAudio)
    {
        if (key == YinDaoAudioState.StopBeiJingAudio)
        {
            if (m_GameBeiJingAd.Length > 0)
            {
                //停止背景音乐.
                StopAudio(m_GameBeiJingAd[m_IndexBeiJing]);
                m_IndexBeiJing++;
                m_IndexBeiJing %= m_GameBeiJingAd.Length;
                //UnityLog("PlayYinDaoJieMianAudio -> m_IndexBeiJing ====== " + m_IndexBeiJing);
            }
        }

        if (m_YinDaoJieMianAd != null)
        {
            PlayAudio(m_YinDaoJieMianAd, AudioState.Loop);
        }
    }

    /// <summary>
    /// 停止引导界面音乐.
    /// </summary>
    public void StopYinDaoAudio()
    {
        //停止引导界面音乐.
        StopAudio(m_YinDaoJieMianAd);
    }

    /// <summary>
    /// 背景音效索引.
    /// </summary>
    int m_IndexBeiJing = 0;
    /// <summary>
    /// 播放引导界面音效.
    /// </summary>
    public void PlayGameBeiJingAudio()
    {
        if (m_GameBeiJingAd.Length > 0)
        {
            PlayAudio(m_GameBeiJingAd[m_IndexBeiJing], AudioState.Loop);
        }
    }
}