using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 踢球前球的轨迹和结果已经确定
/// 成功-三个动作并设置位移
/// 失败-反方向
/// </summary>
public class FootballCtrl : MonoBehaviour
{


    void Start()
    {
        //KickToTarget(2f, -3.6f, -54);//+-3.46  53-56
    }

    public void KickToPos(float speed, float x, float z)
    {
        Vector3[] pathPoints = CalculateParabolaPath(transform.position, new Vector3(x, 0.13f, z), Math.Abs(transform.position.z - z) * 0.13f, 10);
        transform.DOKill();
        transform.DOPath(pathPoints, speed, PathType.CatmullRom, PathMode.Full3D)
            .SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                // 模拟旋转（足球滚动）
                transform.Rotate(Vector3.right, 180f * Time.deltaTime, Space.World);
            })
            .OnComplete(() =>
            {
                Debug.Log("Ball reached target!");
                // 落地后的反弹效果
                BounceAfterLanding();
            });
    }

    Vector3[] CalculateParabolaPath(Vector3 start, Vector3 end, float maxHeight, int resolution)
    {
        Vector3[] points = new Vector3[resolution];

        for (int i = 0; i < resolution; i++)
        {
            float t = (float)i / (resolution - 1);
            Vector3 point = CalculateParabolaPoint(start, end, maxHeight, t);
            points[i] = point;
        }

        return points;
    }

    Vector3 CalculateParabolaPoint(Vector3 start, Vector3 end, float maxHeight, float t)
    {
        // 抛物线公式
        float x = Mathf.Lerp(start.x, end.x, t);
        float z = Mathf.Lerp(start.z, end.z, t);

        // 二次函数 y = a(x - h)² + k
        float midPoint = 0.5f;
        float heightFactor = 4f * maxHeight;
        float y = heightFactor * (-(t - midPoint) * (t - midPoint) + midPoint * midPoint);
        y += Mathf.Lerp(start.y, end.y, t);

        return new Vector3(x, y, z);
    }

    void BounceAfterLanding()
    {
        // 落地后的小弹跳
        transform.DOJump(transform.position + Vector3.forward * 0.5f, 0.3f, 1, 0.5f)
            .SetEase(Ease.OutQuad);
    }

    //------------------------------------------------------------

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <param name="duration">时间</param>
    /// <param name="delta">偏移,正数就是想左偏</param>
    public void KickToTarget(Transform targetWorld, Transform target, float duration, float delta)
    {
        this.target = target;
        this.targetWorld = targetWorld;
        this.duration = duration;
        this.delta = delta;
        startPosition = transform.position;
        startPosition2 = targetWorld.transform.position;
        elapsedTime = 0f;
        isBegin = true;
    }

    public Transform target;
    public Transform targetWorld;
    public float duration = 2f;
    private Vector3 startPosition;
    private Vector3 startPosition2;
    private float elapsedTime;
    bool isBegin;
    float delta;

    void Update()
    {
        if (!isBegin) return;
        if (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / duration;

            Vector3 currentPos2 = CalculateParabolicPosition(
              startPosition2,
              target.position,
              normalizedTime
            );
            targetWorld.position = currentPos2;

            // 抛物线运动公式
            Vector3 currentPos = CalculateParabolicPosition(
                startPosition,
                targetWorld.position,
                normalizedTime
            );
            transform.position = currentPos;
        }
        else
        {
            transform.position = target.position;
        }
    }

    Vector3 CalculateParabolicPosition(Vector3 start, Vector3 end, float t)
    {
        // 水平方向线性插值
        Vector3 horizontal = Vector3.Lerp(start, end, t);
        return horizontal;
    }
}
