using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;      // Ссылка на игрока
    public Vector3 offset = new Vector3(0, 15, -10); // Смещение камеры
    public float followSpeed = 5f;

    void LateUpdate()
    {
        if (target == null) return;

        // Следим только по X и Z, Y остаётся фиксированным (высота камеры)
        Vector3 targetPosition = new Vector3(target.position.x, 0f, target.position.z) + offset;
        Vector3 currentPosition = transform.position;

        // Y остаётся тем, что было задано в offset
        targetPosition.y = offset.y;

        // Плавное следование
        transform.position = Vector3.Lerp(currentPosition, targetPosition, followSpeed * Time.deltaTime);
    }
}
