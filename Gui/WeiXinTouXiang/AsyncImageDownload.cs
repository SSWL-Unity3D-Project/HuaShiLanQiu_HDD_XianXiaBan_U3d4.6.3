using UnityEngine;
using System.Collections;

/// <summary>
/// 动态下载玩家微信头像的控制脚本.
/// </summary>
public class AsyncImageDownload : MonoBehaviour
{
    public void LoadPlayerHeadImg(string url, UITexture image)
    {
        if (url != null && url != "" && url.Length > 5)
        {
            Debug.Log("Unity: url == " + url);
            StartCoroutine(DownloadImage(url, image));
        }
    }

    /// <summary>
    /// 加载微信头像.
    /// </summary>
    IEnumerator DownloadImage(string url, UITexture image)
    {
        Texture2D tex2d = null;
        //Debug.Log("Unity:"+"downloading new image:" + url.GetHashCode());//url转换HD5作为名字.
        WWW www = null;
        try
        {
            www = new WWW(url);
        }
        catch (System.Exception)
        {
        }
        yield return www;

        try
        {
            tex2d = www.texture;
        }
        catch (System.Exception)
        {
        }

        if (tex2d != null && image != null)
        {
            Debug.Log("Unity: DownloadImage...");
            image.mainTexture = tex2d;
        }
    }
    
    public void LoadPlayerHeadImg(string url, Material image)
    {
        if (url != null && url != "" && url.Length > 5)
        {
            Debug.Log("Unity: url == " + url);
            StartCoroutine(DownloadImage(url, image));
        }
    }

    /// <summary>
    /// (indexVal 小于 0 或 indexVal 大于 3)时,不去对PlayerWXDtAy进行操作.
    /// indexVal = [0, 3].
    /// </summary>
    IEnumerator DownloadImage(string url, Material image)
    {
        Texture2D tex2d = null;
        //Debug.Log("Unity:"+"downloading new image:" + url.GetHashCode());//url转换HD5作为名字.
        WWW www = null;
        try
        {
            www = new WWW(url);
        }
        catch (System.Exception)
        {
        }
        yield return www;

        try
        {
            tex2d = www.texture;
        }
        catch (System.Exception)
        {
        }

        if (tex2d != null && image != null)
        {
            Debug.Log("Unity: DownloadImage...");
            image.mainTexture = tex2d;
        }
    }
}