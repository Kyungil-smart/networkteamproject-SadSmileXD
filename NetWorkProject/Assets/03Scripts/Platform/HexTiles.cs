using UnityEngine;

 
public class HexTiles : MonoBehaviour
{
    private int myTileID = -1;
    private bool isTriggered = false;
    public Animator myAnimator;
    [Header("Components")]
    [SerializeField] private Renderer m_renderer;
    [SerializeField] private Collider m_collider;

    private void Start()
    {
        // 게임이 시작되면 MapManager에게 나를 등록하고 번호표를 받음
        if (MapManager.Instance != null)
        {
            myTileID = MapManager.Instance.RegisterTile(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTriggered) return;

        // 플레이어가 밟았을 때
        if (collision.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
            // 밟힌 타일은 빨간색으로 표시
            GetComponent<Renderer>().material.color = Color.gold;
            myAnimator.SetTrigger("OnPlatform");
            Invoke(nameof(SendHideRequest), 1f);
        }
    }

    private void SendHideRequest()
    {
        // 1초가 지나면 매니저에게 "내 번호 지워주세요" 라고 요청
        if (MapManager.Instance != null && myTileID != -1)
        {
            MapManager.Instance.RequestHideTileRpc(myTileID);
        }
    }

    // 매니저의 명령을 받아 실제로 내 컴퓨터 화면에서 발판을 끄는 함수
    public void HideTileLocally()
    {
        if (m_renderer != null) m_renderer.enabled = false;
        if (m_collider != null) m_collider.enabled = false;
    }
}