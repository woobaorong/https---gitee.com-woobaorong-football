using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QiuYuanCtrl : MonoBehaviour
{

    public FootballGameCtrl gc;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    public void Begin()
    {
        anim.SetBool("begin", true);
    }

    public void Invoke_InteractWithBall()
    {
        if (!FootballGameCtrl.isPalyback) GameUtils.PlayerAudioEffect("kick", gameObject);
        //EditorApplication.isPaused = true;
        gc.Kick();
    }
}
