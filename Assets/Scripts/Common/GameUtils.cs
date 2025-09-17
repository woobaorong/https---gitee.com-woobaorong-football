using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_WEBGL
using TTSDK;
using WeChatWASM;
#endif

public class GameUtils
{

    public static MiniGamePlatform miniGamePlatform = MiniGamePlatform.DOU_YIN;  

    /// <summary>
    /// 结果可能包含min和max
    /// </summary>
    public static int GetRandom(int min, int max)
    {
        int randomInt = UnityEngine.Random.Range(min, max + 1);
        return randomInt;
    }

    public static string GetId()
    {
        Guid uuid = Guid.NewGuid();
        string uuidString = uuid.ToString();
        //Debug.Log("GetId: " + uuidString);
        return uuidString;
    }

    public static float GetDistance(Vector3 a, Vector3 b)
    {
        return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
    }

    public static void SaveData(string key, string data)
    {

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
#if UNITY_WEBGL
            if (miniGamePlatform ==  MiniGamePlatform.DOU_YIN)
            {
                TT.Save(data, key);
            }
            else
            {
                //UnityEngine.PlayerPrefs.SetString(key, data);
                WX.StorageSetStringSync(key, data);
            }
#endif
        }
        else
        {
            UnityEngine.PlayerPrefs.SetString(key, data);
        }
    }

    public static string LoadData(string key)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
#if UNITY_WEBGL
            if (miniGamePlatform ==  MiniGamePlatform.DOU_YIN)
            {
                return TT.LoadSaving<string>(key);
            }
            else
            {
                return WX.StorageGetStringSync(key, "");
            }
#endif
            return UnityEngine.PlayerPrefs.GetString(key);
        }
        else
        {
            return UnityEngine.PlayerPrefs.GetString(key);
        }
    }

    public static void SaveIntData(string key, int data)
    {
        SaveData(key, data + "");
    }

    public static int LoadIntData(string key, int defaultValue)
    {
        string value = LoadData(key);
        Debug.Log("获取本地缓存: " + key + " === " + value + " end");
        int number;
        bool success = int.TryParse(value, out number);
        if (success)
        {
            return number;
        }
        else
        {
            return defaultValue;
        }
    }

    public static void SaveFloatData(string key, float data)
    {
        SaveData(key, data + "");
    }

    public static float LoadFloatData(string key, float defaultValue)
    {
        string value = LoadData(key);
        Debug.Log("获取本地缓存: " + key + " === " + value + " end");
        float number;
        bool success = float.TryParse(value, out number);
        if (success)
        {
            return number;
        }
        else
        {
            return defaultValue;
        }
    }

    public static GameObject LoadPrefabFromRes(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.LogError("LoadPrefabFromRes: 路径不能为空！");
            return null;
        }
        // 从 Resources 加载
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null)
        {
            Debug.LogError($"LoadPrefabFromRes: 预制体加载失败，路径不存在或资源不是 GameObject: {path}");
            return null;
        }
        return prefab;
    }

    public static bool isPlayerPlayAuidoEffect = true;

    public static void PlayerAudioEffect(string path, GameObject go)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audios/" + path);
        if (go.GetComponent<AudioSource>() == null) go.AddComponent<AudioSource>();
        var audioSource = go.GetComponent<AudioSource>();
        if (clip != null)
        {
            audioSource.clip = clip;
            if (isPlayerPlayAuidoEffect) audioSource.Play();
        }
        else
        {
            Debug.LogError("音频文件未找到: " + path);
        }
    }

    public static void LoadSpriteAndRander(string path, Image image)
    {
        //可以是本地也可以是网络

    }

    public static GameObject Find(string path)
    {
        string[] paths = path.Split('/');
        //Debug.Log(paths);
        int index = 0;
        int step = 0;
        GameObject parent = null;
        while (paths.Length > index && paths[index] != null)
        {
            step++;
            if (step > 20) return null;  //防止死循环
            if (parent && parent.transform.Find(paths[index]))
            {
                parent = parent.transform.Find(paths[index]).gameObject;
                index++;
            }
            else if (!parent && GameObject.Find(paths[index]) && index == 0)
            {
                parent = GameObject.Find(paths[index]);
                index++;
            }
        }
        return parent;
    }

    public static void ExchangeScene(string sceneName)
    {
        EventBus.GetInstance().RemoveAll();
        SceneManager.LoadScene(sceneName);
    }

    private static Tween timer;

    public static void Toast(string str, float time = 2)
    {
        Debug.Log("Toast: " + str);
        if (timer != null) timer.Kill();
        var toastNode = Find("Canvas/Toast");
        if (toastNode)
        {
            toastNode.transform.SetSiblingIndex(9999);
            var textComponent = toastNode.transform.Find("Text").GetComponent<Text>();
            var bg = toastNode.transform.Find("Bg");
            textComponent.text = str;

            // 调整文本组件的宽度为首选宽度
            float preferredWidth = textComponent.preferredWidth + 20;
            RectTransform rectTransform = textComponent.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(preferredWidth, rectTransform.sizeDelta.y);
            RectTransform rectTransform2 = bg.GetComponent<RectTransform>();
            rectTransform2.sizeDelta = new Vector2(preferredWidth + 30, rectTransform2.sizeDelta.y);

            if (toastNode.activeSelf == false)
            {
                toastNode.SetActive(true);
            }
            timer = Invoke(OnTimer, 2);
        }
    }

    public static void OnTimer()
    {
        if (timer != null) timer.Kill();
        var toastNode = Find("Canvas/Toast");
        if (toastNode) toastNode.SetActive(false);
    }

    public static Tween Invoke(TweenCallback callback, float time, TweenCallback Update = null)
    {
        GameObject g = null;
        if (!Find("InvokeObject"))
        {
            g = new GameObject("InvokeObject");
            g.transform.parent = Find("Canvas").transform.parent;
        }
        else
        {
            g = Find("InvokeObject");
        }
        if (Update != null)
        {
            Tween timer = g.transform.DOScale(new Vector3(1, 1, 1), time).OnComplete(callback).OnUpdate(Update);
            return timer;
        }
        else
        {
            Tween timer = g.transform.DOScale(new Vector3(1, 1, 1), time).OnComplete(callback);
            return timer;
        }

    }

    public static GameObject getClosetObj(GameObject t, GameObject[] tools)
    {
        // 如果数组为空或null，返回null
        if (tools == null || tools.Length == 0)
        {
            Debug.LogWarning("Tools array is empty or null");
            return null;
        }
        // 使用LINQ找到距离最近的对象
        GameObject closestObj = tools
            .Where(obj => obj != null) // 过滤掉null对象
            .OrderBy(obj => (obj.transform.position - t.transform.position).sqrMagnitude) // 按距离平方排序
            .FirstOrDefault(); // 获取第一个（最近的）
        return closestObj;
    }

}







public enum MiniGamePlatform
{
    DOU_YIN, WEI_XIN
}