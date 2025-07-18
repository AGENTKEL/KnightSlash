using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Ссылка на игрока
    public Vector3 offset = new Vector3(0, 15, -10); // Смещение камеры
    public float followSpeed = 5f;
    public float minDistance = 3f; // минимальное приближение камеры
    public float maxDistance = 10f; // максимальная дистанция (исходная)
    public LayerMask obstacleLayer; // тут установить слой "Map"

    private float currentDistance;

    void Start()
    {
        currentDistance = offset.magnitude;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 direction = offset.normalized;
        Vector3 desiredCameraPos = target.position + direction * currentDistance;
        Vector3 lookDir = desiredCameraPos - target.position;

        // Проверка на препятствия между камерой и игроком
        if (Physics.Raycast(target.position, direction, out RaycastHit hit, maxDistance, obstacleLayer))
        {
            // Приближаем камеру к точке перед препятствием
            currentDistance = Mathf.Clamp(hit.distance - 0.5f, minDistance, maxDistance);
        }
        else
        {
            // Плавно возвращаем к исходной дистанции
            currentDistance = Mathf.Lerp(currentDistance, maxDistance, Time.deltaTime * followSpeed);
        }

        // Новая позиция камеры
        Vector3 targetPosition = target.position + direction * currentDistance;

        // Фиксируем высоту камеры как в offset
        targetPosition.y = offset.y;

        // Плавное движение
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

}
