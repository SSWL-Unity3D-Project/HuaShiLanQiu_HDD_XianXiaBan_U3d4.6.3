//#define DRAW_FPS
using UnityEngine;
using System;

public class XKGameFPSCtrl : MonoBehaviour
{
#if DRAW_FPS
    /// <summary>
    /// The update interval.
    /// </summary>
    float UpdateInterval = 0.5f;
	
	/// <summary>
	/// The accum.
	/// </summary>
	private float accum; // FPS accumulated over the interval
	
	/// <summary>
	/// The frames.
	/// </summary>
	private int frames; // Frames drawn over the interval
	
	/// <summary>
	/// The timeleft.
	/// </summary>
	private float timeleft; // Left time for current interval
	float FPSVal = 60f;
	Color FPSColorVal = Color.green;
	void Start()
    {
        this.timeleft = this.UpdateInterval;
	}

    void Update()
    {
        this.timeleft -= Time.deltaTime;
        this.accum += Time.timeScale / Time.deltaTime;
        ++this.frames;

        // Interval ended - update GUI text and start new interval
        if (this.timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            float fps = this.accum / this.frames;
            if (fps < 10f)
            {
                if (FPSColorVal != Color.red)
                {
                    FPSColorVal = Color.red;
                }
            }
            else if (fps < 30f)
            {
                if (FPSColorVal != Color.yellow)
                {
                    FPSColorVal = Color.yellow;
                }
            }
            else
            {
                if (FPSColorVal != Color.green)
                {
                    FPSColorVal = Color.green;
                }
            }

            FPSVal = fps;
            this.timeleft = this.UpdateInterval;
            this.accum = 0.0f;
            this.frames = 0;
        }
    }

	void OnGUI()
    {
        GUI.Box(new Rect(25f, 75f, 100f, 25f), "");
        GUI.color = FPSColorVal;
        GUI.Label(new Rect(25f, 75f, 100f, 25f), String.Format("FPS: {0:F0}", FPSVal));
    }
#endif
}