using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;


public class BasePannelCtrl : BaseComponet
{

    public GameObject pannel;

    public List<Transform> ignoreNodes;

    public static bool isShowingPannel = false;

    public bool isCloseWhenClickBg = false;

    public static int siblingIndex = 100;

    public void InitPannel()
    {
        //设置背景点击事件
        if (isCloseWhenClickBg)
        {
            SetClickEvent(pannel.transform.Find("Bg").gameObject, () =>
            {
                PlayerClickAudio();
                HidePannel();
            });
        }
    }

    List<Transform> animNodes;

    public virtual void ShowPannel()
    {
        pannel.SetActive(true);
        animNodes = new List<Transform>();
        for (int i = 0; i < pannel.transform.Find("Pannel").transform.childCount; i++)
        {
            var item = pannel.transform.Find("Pannel").transform.GetChild(i);
            if (item.name == "Bg" || item.name == "Title") continue;
            bool ignore = false;
            foreach (var ignoreNode in ignoreNodes)
            {
                if (ignoreNode == item)
                {
                    ignore = true;
                }
            }
            if (ignore) continue;
            if (item.gameObject.activeSelf)
            {
                item.gameObject.SetActive(false);
                animNodes.Add(item);
            }
        }
        pannel.transform.Find("Pannel").localScale = new Vector3(0, 0, 0);

        Sequence sq = DOTween.Sequence();
        sq.Append(pannel.transform.Find("Pannel").DOScale(1.12f, 0.2f));
        sq.Append(pannel.transform.Find("Pannel").DOScale(1f, 0.1f));
        sq.OnComplete(() =>
        {
            int i = 0;
            foreach (var item in animNodes)
            {
                if (item.name == "Bg" || item.name == "Title") continue;
                item.gameObject.SetActive(true);
                item.localScale = Vector3.zero;
                Sequence seq = DOTween.Sequence();
                seq.AppendInterval(i * 0.05f);
                seq.Append(item.DOScale(1.2f, 0.1f));
                seq.Append(item.DOScale(1f, 0.05f));
                seq.OnComplete(() => Debug.Log("动画完成！"));
                i++;
            }
        });
        
        siblingIndex++;
        pannel.transform.SetSiblingIndex(siblingIndex);
        isShowingPannel = true;
    }

    public virtual void HidePannel()
    {
        isShowingPannel = false;
        Sequence seq = DOTween.Sequence();
        seq.Append(pannel.transform.Find("Pannel").DOScale(1.12f, 0.1f));
        seq.Append(pannel.transform.Find("Pannel").DOScale(0f, 0.2f));
        seq.OnComplete(() => pannel.SetActive(false));
    }

    public void SetButtonEvent(Action act)
    {
        DelayAction(2, () =>
        {

        });
    }

    void SetClickEvent(GameObject target, Action act)
    {
        EventTrigger trigger = target.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = target.AddComponent<EventTrigger>();
        }

        // 移除所有PointerClick类型的Entry
        trigger.triggers.RemoveAll(entry =>
            entry.eventID == EventTriggerType.PointerClick);
        EventTrigger.Entry clickEntry = new EventTrigger.Entry();
        clickEntry.eventID = EventTriggerType.PointerClick;
        clickEntry.callback.AddListener((data) => { act(); });
        trigger.triggers.Add(clickEntry);
    }
}