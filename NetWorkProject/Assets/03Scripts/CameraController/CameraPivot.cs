using UnityEngine;

public class ThirdPersoncamera : MonoBehaviour
{
    public Transform target;          // 플레이어
    public Transform pivot;           // CameraPivot
    public float distance = 5f;

    public float mouseSensitivity = 200f;
    public float minY = -30f;
    public float maxY = 60f;

    float yaw;   // 좌우
    float pitch; // 상하

     
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;

        pitch = Mathf.Clamp(pitch, minY, maxY);

        // 1. 플레이어 기준 좌우 회전
        target.rotation = Quaternion.Euler(0, yaw, 0);

        // 2. 피벗 상하 회전
        pivot.localRotation = Quaternion.Euler(pitch, 0, 0);

        // 3. 카메라 위치
        transform.position = pivot.position - pivot.forward * distance;

        // 4. 카메라 바라보기
        transform.LookAt(pivot);
    }
}