using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class BaseComponet : MonoBehaviour
{

    public void DelayAction(float delay, Action action)
    {
        StartCoroutine(DelayCoroutine(delay, action));
    }

    IEnumerator DelayCoroutine(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    public void Log(string str)
    {
        Debug.Log(str);
    }

    public void LogError(string str)
    {
        Debug.LogError(str);
    }

    // 方法1：将十六进制颜色字符串转换为Unity的Color
    public static Color HexToColor(string hex)
    {
        // 解析RGB值
        float r = HexToFloat(hex.Substring(0, 2));
        float g = HexToFloat(hex.Substring(2, 2));
        float b = HexToFloat(hex.Substring(4, 2));
        // 默认alpha为1（不透明）
        return new Color(r, g, b, 1f);
    }

    private static float HexToFloat(string hex)
    {
        // 将十六进制字符串转换为0-255的整数值
        int value = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
        // 转换为0-1范围的浮点数
        return Mathf.Clamp01((float)value / 255f);
    }

    public void PlayerClickAudio()
    {
        GameUtils.PlayerAudioEffect("click", gameObject);
    }

    public static float progressPercentage = 0;

    public void WatchAd(Action<int> act)
    {
        GameUtils.Toast("开始模拟看广告,2秒后获取收益");
        //模拟看广告
        DelayAction(2, () =>
        {
            int result = 1;
            if (result == 0) GameUtils.Toast("广告异常关闭，未获得奖励");
            act(result);
        });
    }

    public void ButtonJump(Transform targetButton)
    {
        targetButton.DOKill();
        targetButton.DOPunchScale(Vector3.one * 0.2f, 0.3f, 4)
            .SetEase(Ease.OutQuad)
            .SetLoops(3, LoopType.Restart);
    }

}
