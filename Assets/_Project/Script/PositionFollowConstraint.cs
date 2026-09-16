using UnityEngine;

/// <summary>
/// Bir kaynak kemiðin (targetBone) pozisyon deðiþimini,
/// belirlenen aðýrlýk oranýnda bu objenin (head_helper) pozisyonuna uygular.
/// Örn: bone_nose saða kaydýkça head_helper ters yönde %30 kayar.
/// </summary>
[ExecuteAlways]
public class PositionFollowConstraint : MonoBehaviour
{
    [Header("Hedef Kemik (örn: bone_nose)")]
    public Transform targetBone;

    [Header("Tepki Ayarý")]
    [Tooltip("Hedefin pozisyon deðiþimine tepki oraný. -0.3 = %30 ters yönde takip. Pozitif deðer ayný yönde takip demektir.")]
    [Range(-1f, 1f)]
    public float positionWeight = -0.3f;

    [SerializeField, HideInInspector] private Vector3 targetInitialPos;
    [SerializeField, HideInInspector] private Vector3 myInitialPos;
    [SerializeField, HideInInspector] private bool isInitialized;

    private void OnEnable()
    {
        if (!isInitialized)
            BindCurrentPose();
    }

    [ContextMenu("Bind Current Pose")]
    public void BindCurrentPose()
    {
        if (targetBone == null) return;

        targetInitialPos = targetBone.localPosition;
        myInitialPos = transform.localPosition;
        isInitialized = true;
    }

    private void LateUpdate()
    {
        if (!isInitialized || targetBone == null) return;

        Vector3 delta = targetBone.localPosition - targetInitialPos;

        // Sayýsal güvenlik kontrolü (NaN/Infinity oluþumunu engeller)
        if (!IsValid(delta)) return;

        Vector3 newPos = myInitialPos + delta * positionWeight;
        if (IsValid(newPos))
        {
            transform.localPosition = newPos;
        }
    }

    private static bool IsValid(Vector3 v)
    {
        return !float.IsNaN(v.x) && !float.IsNaN(v.y) && !float.IsNaN(v.z)
            && !float.IsInfinity(v.x) && !float.IsInfinity(v.y) && !float.IsInfinity(v.z);
    }
}
