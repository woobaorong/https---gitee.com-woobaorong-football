using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCtrl : BaseComponet
{
    void Start()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.name == "Plane")
            {
                Debug.Log(hit.point.x + " --xxx-- " + hit.point.z);
            }
        }
    }

    int index = 0;

    float time = 0;

    void Update()
    {
        index++;
        time += Time.deltaTime;
        if (time > 1)
        {
            //Log("fps:" + index);
            index = 0;
            time = 0;
        }
    }

    private Vector3 offset;

    void OnMouseDown()
    {
        Log("xxxx" + transform.gameObject.name);
        // 计算物体与点击点的偏移量
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject.name == "Plane")
            {
                Debug.Log(hit.point.x + " --- " + hit.point.z);
            }
        }
    }

    void OnMouseDrag()
    {
        //Log("----------OnMouseDrag---------");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {

        }
    }

    void OnMouseUp()
    {

    }


}
