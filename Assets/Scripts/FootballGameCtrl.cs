using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FootballGameCtrl : MonoBehaviour
{
     public GameObject[] path;

    public Transform lookat;

    void Start()
    {
        Move();
    }

    void Move()
    {
        Vector3[] points = new Vector3[path.Length];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = path[i].transform.position;
        }
        //transform.DOKill();
        transform.DOPath(points, 3, PathType.CatmullRom, PathMode.Full3D)
        //.SetEase(Ease.Linear)
        //.SetLookAt(lookat.position)  //lookat z轴
        .OnComplete(() =>
        {
            Debug.Log("---OnComplete---");
        });
    }

}
