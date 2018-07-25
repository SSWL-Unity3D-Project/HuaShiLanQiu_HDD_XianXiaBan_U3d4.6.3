using UnityEngine;

public class SSAudioManager : SSGameMono
{
    public enum AudioState
    {
        /// <summary>
        /// 强制停止音效然后再重新播放..
        /// </summary>
        StopToPlay = 0,
        /// <summary>
        /// 音效停止时播放音效.
        /// </summary>
        OnPlayAfterStop = 1,
        /// <summary>
        /// 循环播放音效.
        /// </summary>
        Loop = 2,
    }

    public void StopAudio(AudioSource asVal)
    {
        if (asVal == null)
        {
            return;
        }
        asVal.Stop();
    }

    public void PlayAudio(AudioSource asVal, AudioState key)
    {
        if (asVal == null)
        {
            return;
        }

        switch (key)
        {
            case AudioState.StopToPlay:
                {
                    asVal.Stop();
                    break;
                }

            case AudioState.OnPlayAfterStop:
                {
                    if (asVal.isPlaying)
                    {
                        return;
                    }
                    break;
                }

            case AudioState.Loop:
                {
                    if (!asVal.loop)
                    {
                        asVal.loop = true;
                    }

                    if (asVal.isPlaying)
                    {
                        asVal.Stop();
                    }
                    break;
                }
        }
        asVal.Play();
    }
}