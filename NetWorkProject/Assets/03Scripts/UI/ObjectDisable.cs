using UnityEngine;

public class ObjectDisable : MonoBehaviour,IDisable
{
    [SerializeField] private GameObject m_object;
    [SerializeField] private RectTransform m_transform;
    public void Disable()
    {
        m_object.SetActive(false);
    }
    public void Start()
    {
        m_object.SetActive(false);
    }
    public void OnEnable()
    {
        m_transform.localPosition = new Vector3(0, 0, m_transform.localPosition.z);
    }
}
