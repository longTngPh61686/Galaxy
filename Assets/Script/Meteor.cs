using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float fallSpeed = 5f; // Tốc độ rơi của thiên thạch
    public float health = 3f; // Máu của thiên thạch
    private Rigidbody2D rb;
    private Animator animator; // Animator để điều khiển animation
    public float delayBeforeDestroy = 1f; // Thời gian chờ trước khi hủy thiên thạch sau khi animation crack
    private bool isBroken = false; // Kiểm tra xem thiên thạch đã vỡ chưa

    public GameObject[] itemPrefabs; // Mảng các prefab item có thể rơi ra (bao gồm các item laser)
    public float dropChance = 0.3f; // Tỉ lệ rơi item (ví dụ: 30%)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Lấy component Animator

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on Meteor!");
        }

        if (animator == null)
        {
            Debug.LogError("Animator not found on Meteor!");
        }
    }

    void Update()
    {
        // Thiên thạch rơi xuống với tốc độ cố định nếu chưa vỡ
        if (isBroken == false)
        {
            rb.linearVelocity = new Vector2(0, -fallSpeed); // Rơi theo trục Y mà không bị đẩy lùi ngang
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // Dừng chuyển động khi thiên thạch vỡ
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Thiên thạch va vào Player!");

            // Đặt trạng thái thiên thạch đã vỡ
            isBroken = true;
            Destroy(collision.gameObject); // Hủy Player khi va chạm với thiên thạch
            TriggerCrackAnimation(); // Chạy animation vỡ
        }
        else if (collision.gameObject.CompareTag("Laser"))
        {
            // Giảm máu của thiên thạch khi bị bắn trúng
            health -= 1;
            if (health <= 0)
            {
                // Thiên thạch bị phá hủy
                isBroken = true;
                TriggerCrackAnimation(); // Kích hoạt animation vỡ
            }
            rb.linearVelocity = Vector2.zero; // Dừng chuyển động của thiên thạch
        }
        else if (collision.gameObject.CompareTag("NonTouch"))
        {
            Debug.Log("Thiên thạch đã vỡ!");
            isBroken = true;
            // Hủy thiên thạch ngay lập tức
            Destroy(gameObject);
        }
    }

    // Hàm kích hoạt animation crack và hủy thiên thạch sau delay
    private void TriggerCrackAnimation()
    {
        // Dừng thiên thạch rơi (dừng tác động lực)
        rb.linearVelocity = Vector2.zero; // Dừng chuyển động

        // Nếu bạn muốn sử dụng hiệu ứng nổ, có thể bật một animator hoặc particle system ở đây
        if (animator != null)
        {
            animator.SetTrigger("Crack"); // Đảm bảo rằng bạn đã đặt một Trigger "Crack" trong Animator
        }

        // Tạo item ngẫu nhiên sau khi thiên thạch vỡ
        TryDropItem();

        // Hủy thiên thạch sau một khoảng thời gian để hoàn thành animation
        Destroy(gameObject, delayBeforeDestroy); // Thiên thạch sẽ bị hủy sau thời gian delay
    }

    // Hàm kiểm tra tỉ lệ và tạo item ngẫu nhiên
    private void TryDropItem()
    {
        if (Random.value <= dropChance) // Kiểm tra tỉ lệ rơi item
        {
            // Lựa chọn ngẫu nhiên một item từ mảng itemPrefabs
            int randomIndex = Random.Range(0, itemPrefabs.Length);
            GameObject item = Instantiate(itemPrefabs[randomIndex], transform.position, Quaternion.identity); // Tạo item tại vị trí thiên thạch
            item.SetActive(true); // Bật item (nếu bị tắt trong prefab)
        }
    }
}






