using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FootballGameCtrl : BaseComponet
{
    public GameObject[] path;

    public Transform lookat;

    public Transform smy;

    public Transform ball;

    void Start()
    {
        Time.timeScale = 0.2f;
        //Time.timeScale = 1f;
        ball.GetComponent<FootballCtrl>().KickToTarget(2f, -3.6f, -54);//+-3.46  53-56
        DelayAction(1, () =>
        {
            SmyJump(1.5f, 1.2f);
        });
        DelayAction(1, () =>
        {
            SetAnim(0);
        });

    }

    void MoveAround()
    {
        Vector3[] points = new Vector3[path.Length];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = path[i].transform.position;
        }
        //transform.DOKill();
        transform.DOPath(points, 3, PathType.CatmullRom, PathMode.Full3D)
        //.SetEase(Ease.Linear)
        .SetLookAt(lookat.position)  //lookat z轴
        .OnComplete(() =>
        {
            Debug.Log("---OnComplete---");
        });
    }

    void SmyJump(float x, float time)
    {

        var pos = smy.transform.position;
        smy.transform.DOLocalMove(new Vector3(x, pos.y, pos.z), time);
    }

    void SetAnim(int type)
    {
        var anim = smy.Find("ShouMenYuan").GetComponent<Animator>();
        if (type == -3)
        {
            anim.SetBool("jump_left_fail", true);
        }
        else if (type == -2)
        {
            anim.SetBool("jump_up_left", true);
        }
        else if (type == -1)
        {
            anim.SetBool("jump_left", true);
        }
        else if (type == 0)
        {
            anim.SetBool("jump_up", true);
        }
        else if (type == 1)
        {
            anim.SetBool("jump_right", true);
        }
        else if (type == 2)
        {
            anim.SetBool("jump_up_right", true);
        }
        else if (type == 3)
        {
            anim.SetBool("jump_right_fail", true);
        }
    }

}
