using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 消息收发器
/// </summary>
public class EventBus
{
    private static EventBus instance;

    public static EventBus GetInstance()
    {
        if (instance == null)
        {
            instance = new EventBus();
        }
        return instance;
    }

    public Dictionary<string, MessageSuber> funsMap = new Dictionary<string, MessageSuber>();

    /// <summary>
    /// 组件订阅消息
    /// </summary>
    /// <param name="fun">回调函数指针 Action<string, string> 第一个str为传过来的消息，第二个为订阅者id，方便在回调里直接取消订阅</param>
    /// <param name="t">消息枚举类型</param>
    /// <param name="comp">订阅组件对象</param>
    /// <param name="isOnce">是否收到消息后自动取消订阅，即一次性订阅，可选，默认为false</param>
    /// <returns>组件需要持有的消息key-id字符串</returns>
    public string Sub(Action<MessageObj, string> fun, EventType t, MonoBehaviour comp, bool isOnce = false)
    {
        string id = Guid.NewGuid().ToString();
        MessageSuber o = new MessageSuber
        {
            t = t,
            comp = comp,
            fun = fun,
            id = id,
            isOnce = isOnce
        };
        funsMap.Add(id, o);
        return id;
    }

    /// <summary>
    /// 在场景跳转是要调用此方法来清空所有订阅者
    /// </summary>
    public void RemoveAll()
    {
        funsMap.Clear();
    }

    /// <summary>
    /// 取消订阅某消息
    /// </summary>
    /// <param name="id">订阅时获取的id</param>
    public void UnSub(string id)
    {
        if (funsMap.ContainsKey(id))
        {
            funsMap.Remove(id);
        }
        else
        {
            Debug.Log("此消息订阅对象已被移出");
        }

    }

    /// <summary>
    /// 广播消息
    /// </summary>
    /// <param name="e">消息枚举类型</param>
    /// <param name="p">消息参数</param>
    public void Board(EventType e, MessageObj p)
    {
        List<string> ids = new List<string>();
        foreach (var obj in funsMap)
        {
            if (obj.Value != null)
            {
                MessageSuber item = obj.Value;
                if (item.fun != null && item.comp != null && item.t == e)
                {

                    //if (item.comp.enabled == false || item.comp.gameObject.activeSelf == false) Debug.Log("组件不可见");

                    //此处无需指定this和ts不一样c#能保证委托方法里的this为委托指向的方法所属的对象
                    item.fun(p, item.id);
                    if (item.isOnce)
                    {
                        ids.Add(item.id);
                    }
                }

                //else if (item.t == e) {
                //Debug.Log("组件不可见");
                //}





            }
        }
        foreach (var id in ids)
        {
            UnSub(id);
        }
    }
}

/// <summary>
/// 消息订阅对象
/// </summary>
public class MessageSuber
{
    /// <summary>
    /// 前一个参数是广播的消息带的参数，后一个是消息id
    /// 注意--解绑不能直接写在回调里,因为forech里不能增删容器
    /// 可以考虑在invoke中解除回调
    /// </summary>
    public Action<MessageObj, string> fun;
    public EventType t;
    public string id;
    public MonoBehaviour comp;
    public bool isOnce;
}

/// <summary>
/// 消息对象
/// </summary>
public class MessageObj
{
    public string msg;
    public Vector3 pos;
    public GameObject g;
    public int code;
    public float f;

    public static MessageObj GetIns()
    {
        return new MessageObj();
    }
}

/// <summary>
/// 消息类型枚举,手动添加
/// </summary>
public enum EventType
{
    net,//网络事件
    net_error,//网络错误
    action,//动作
    cut_number,
    win_or_lose
}