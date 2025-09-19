using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class FootballGameCtrl : BaseComponet
{
    public GameObject[] path;

    public Transform lookat;

    public Transform smy;

    public Transform ball;

    public Transform hand;

    public Transform handWorld;

    void Start()
    {

        Time.timeScale = 0.25f;
        Time.timeScale = 0.75f;

        float kickSpeed = 0.9f;  //0.6-0.9
        float v = GameUtils.GetRandom(0, 200) - 100;  //改成方向值
        float value = Math.Abs(v / 100);
        int r = GameUtils.GetRandom(0, 100);
        if (Math.Abs(v) < 30)
        {
            if (r * kickSpeed > 30)
            {
                this.KickSuccess(v > 0 ? 1 : -1, value);
            }
            else
            {
                this.kickFail(v > 0 ? 1 : -1, value);
            }
        }
        else
        {
            if (r * kickSpeed > 70)
            {
                this.KickSuccess(v > 0 ? 1 : -1, value);
            }
            else
            {
                this.kickFail(v > 0 ? 1 : -1, value);
            }
        }
    }

    void kickFail(int dir, float value)
    {
        //int dir = 1;
        //float value = 1f;
        ball.GetComponent<FootballCtrl>().KickToPos(1.6f, -3.46f * value * dir, -54);     //+-3.46  53-56
        float time = 0.05f;
        DelayAction(time + 0.1f, () =>
        {
            SmyJump(0, 0.8f);
        });
        DelayAction(time, () =>
        {
            SetAnim(-3 * dir);
        });
    }

    void KickSuccess(int dir, float value)
    {
        //int dir = -1;
        //float value = 1f;
        handWorld.position += new Vector3(-5.5f * dir * value, 0, 0);
        ball.GetComponent<FootballCtrl>().KickToTarget(handWorld, hand, 0.6f, 0.4f * value);
        float time = 0.05f;
        DelayAction(time + 0.1f, () =>
        {
            SmyJump(-3.5f * dir * value, 1.2f);
        });
        DelayAction(time, () =>
        {
            if (value < 0.2f)
            {
                SetAnim(0);
            }
            else
            {
                SetAnim(-2 * dir);
            }

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
