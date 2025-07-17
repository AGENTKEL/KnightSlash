using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    public float rotationSpeed = 90f;
    public float magnetDistance = 3f;        // Радиус притяжения
    public float maxSpeed = 10f;             // Максимальная скорость
    public float minSpeed = 2f;              // Минимальная скорость (на краю радиуса)

    private Transform player;

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        // Вращение по оси Y
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);

        // Притяжение к игроку
        if (player != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);

            if (dist <= magnetDistance)
            {
                // Расчёт скорости: чем ближе — тем быстрее
                float t = 1f - Mathf.Clamp01(dist / magnetDistance); // от 0 до 1
                float speed = Mathf.Lerp(minSpeed, maxSpeed, t);

                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * speed * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.AddCoin(coinValue);
            CoinPoolManager.Instance.ReturnCoin(gameObject);
        }
    }
}
