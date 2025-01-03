using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class LightingEditorVideoPlayer : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    private bool paused = true;
    public Animator[] pauseIconAnimators;
    public Slider[] videoSliders;
    private bool dragging;
    private int draggingIndex;
    private double draggedTime;
    private bool justDragged = false;
    
    
    public void SetUrl(string url)
    {
        videoPlayer.url = url;
        videoPlayer.Play();
        StartCoroutine(PauseAfterShortTime());
    }
    
    private IEnumerator PauseAfterShortTime()
    {
        yield return new WaitForSeconds(0.1f);
        videoPlayer.Pause();
    }
    
    public void PlayPause()
    {
        SetPaused(!paused);
    }


    public void SetPaused(bool value)
    {
        paused = value;
        if (!paused)
        {
            videoPlayer.Play();
            SetPauseIcon(false);
        }
        else
        {
            videoPlayer.Pause();
            SetPauseIcon(true);
        }
        LightsPreviewInterpreter.instance.RefreshExecutedKeyframes();
    }

    private void SetPauseIcon(bool paused)
    {
        foreach (Animator pauseIconAnimator in pauseIconAnimators)
        {
            pauseIconAnimator.SetBool("Paused", paused);
        }
    }
    
    public void BeginDrag(int index)
    {
        dragging = true;
        draggingIndex = index;
    }
    
    public void EndDrag(int index)
    {
        dragging = false;
        draggingIndex = index;
        justDragged = true;
        LightsPreviewInterpreter.instance.RefreshExecutedKeyframes(true, (float)draggedTime);
    }
    
    public float GetVideoTime()
    {
        return justDragged ? (float)draggedTime : (float)videoPlayer.time; // this is because when you set the time, the video player has to load for a little bit before actually changing the time
    }

    public bool IsPlaying()
    {
        return !paused;
    }
    
    public float GetVideoLength()
    {
        return (float)videoPlayer.length;
    }

    private void Update()
    {
        if (dragging)
        {
            draggedTime = videoPlayer.length * videoSliders[draggingIndex].value;
            videoPlayer.time = draggedTime;
        } else
        {
            float v = (float)(videoPlayer.time / videoPlayer.length);
            if (!float.IsNaN(v))
            {
                SetSliders(v);
            }
        }

        
        if (justDragged && Mathf.Abs((float)videoPlayer.time - (float)draggedTime) < 0.3f)
        {
            justDragged = false;
        }

    }
    
    
    private void SetSliders(float value)
    {
        foreach (Slider slider in videoSliders)
        {
            slider.value = value;
        }
    }
}
