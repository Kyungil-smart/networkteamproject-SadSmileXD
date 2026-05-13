using UnityEngine;

public class playerRotation : MonoBehaviour
{
    [Header("회전 속도 설정")]
    [SerializeField] private float mouseSensitivity = 100f; // 마우스 민감도

    private float _yRotation = 0f;

   
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    void Update()
    {
        // 1. 마우스의 가로 이동량 가져오기
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        // 2. 현재 Y축 회전 값에 마우스 이동량 더하기
        _yRotation += mouseX;

        // 3. 플레이어의 로컬 회전값(Local Rotation)에 적용
        transform.localRotation = Quaternion.Euler(0f, _yRotation, 0f);
    }
}