using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShouMenYuanCtrl : MonoBehaviour
{

    Animator anim;


    void Start()
    {
        //anim = GetComponent<Animator>();
        this.Jump();
    }

    void Update()
    {

    }

    void Jump()
    {
        //anim.SetBool("sasd", true);
        var pos = transform.position;
        transform.DOLocalMove(new Vector3(1.5f, pos.y, pos.z), 1.2f);
    }

    public void OnAnimFinshed()
    {

    }
}
