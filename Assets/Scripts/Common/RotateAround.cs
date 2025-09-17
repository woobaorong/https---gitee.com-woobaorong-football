using UnityEngine;

public class RotateAroundZAxis : MonoBehaviour
{
    [Header("旋转设置")]
    public float rotationSpeed = 30f; // 旋转速度（度/秒）
    public Space relativeTo = Space.Self; 
    
    void Update()
    {
        // 每帧绕Z轴旋转
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime, relativeTo);
    }
}