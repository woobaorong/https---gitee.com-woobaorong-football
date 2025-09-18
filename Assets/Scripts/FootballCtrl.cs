using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FootballCtrl : MonoBehaviour
{
    public Transform target; // 目标位置
    public float height = 5f; // 抛物线高度
    public float duration = 2f; // 飞行时间

    void Start()
    {
        if (target != null)
        {
            KickToTarget();
        }
    }

    void KickToTarget()
    {
        Vector3[] pathPoints = CalculateParabolaPath(transform.position, target.position, height, 10);

        transform.DOKill();
        transform.DOPath(pathPoints, duration, PathType.CatmullRom, PathMode.Full3D)
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
}
