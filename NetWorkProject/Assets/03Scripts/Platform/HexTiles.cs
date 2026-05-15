using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
public class HexTiles : MonoBehaviour
{
    private int myTileID = -1;
    private bool isTriggered = false;
    public GameObject parent;
    [Header("Components")]
    [SerializeField] private Renderer m_renderer;
    [SerializeField] private List<Collider> m_colliders=new List<Collider>();

    public Renderer renderer;
    private void Start()
    {
        // 게임이 시작되면 MapManager에게 나를 등록하고 번호표를 받음
        if (MapManager.Instance != null)
        {
            myTileID = MapManager.Instance.RegisterTile(this);
        }
        renderer=this.gameObject.GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTriggered) return;

        // 플레이어가 밟았을 때
        if (collision.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
            // 밟힌 타일은 빨간색으로 표시
            ChangeColor();
            //myAnimator.SetTrigger("OnPlatform");
            StartCoroutine(Push());
            Invoke(nameof(SendHideRequest), 1f);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("계속 밟고있음");
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
        if (m_colliders != null) 
            foreach (var collider in m_colliders)
            {
               
                collider.enabled = false;
             
            }
    }
    private void ChangeColor()
    {
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();

        renderer.GetPropertyBlock(mpb);
        mpb.SetColor("_BaseColor", Color.red);
        renderer.SetPropertyBlock(mpb);
    }
    private IEnumerator Push()
    {
        Vector3 startPos = transform.position;
        Vector3 downPos = startPos + (Vector3.down*0.2f);

        float duration = 0.5f;

        // 🔽 내려가기 (0.5초)
        float t = 0f;
        while (t < duration)
        {
            yield return new WaitForFixedUpdate(); // 물리 연산 직전에 실행
            t += Time.fixedDeltaTime;             // 물리 프레임 시간만큼 증가
            float lerp = t / duration;
            transform.position = Vector3.Lerp(startPos, downPos, lerp);
            yield return null;
        }

        // 🔼 올라오기 (0.5초)
        t = 0f;
        while (t < duration)
        {
            yield return new WaitForFixedUpdate();
            t += Time.fixedDeltaTime;
            float lerp = t / duration;
            transform.position = Vector3.Lerp(downPos, startPos, lerp);
            yield return null;
        }
    }
}