using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QiuYuanCtrl : MonoBehaviour
{

    public FootballGameCtrl gc;
    
    void Start()
    {

    }

    public void Invoke_InteractWithBall()
    {
        //EditorApplication.isPaused = true;
        gc.Kick();
    }
}
