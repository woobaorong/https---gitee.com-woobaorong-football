using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FootballGameCtrl : MonoBehaviour
{
    public GameObject[] path;

    public Transform lookat;

    public Transform smy;

    public Transform ball;

    void Start()
    {
        Time.timeScale = 0.2f;
        //Time.timeScale = 1f;
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

    void SmyJump()
    {
        var anim = smy.Find("ShouMenYuan").GetComponent<Animator>();
        anim.SetBool("sasd", true);
        var pos = smy.transform.position;
        smy.transform.DOLocalMove(new Vector3(1.5f, pos.y, pos.z), 1.2f);
    }

}
