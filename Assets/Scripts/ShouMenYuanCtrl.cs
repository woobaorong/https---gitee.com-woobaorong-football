using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShouMenYuanCtrl : MonoBehaviour
{

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {

    }

    void Jump()
    {
        anim.SetBool("sasd", true);
    }

    public void OnAnimFinshed()
    {

    }
}
