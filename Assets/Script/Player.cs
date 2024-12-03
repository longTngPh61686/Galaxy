using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f; // Tốc độ di chuyển

    private Rigidbody2D rb;
    private Vector2 movement;

    // Giới hạn di chuyển
    public float minX = -2f;
    public float maxX = 2f;
    public float minY = -4f;
    public float maxY = 2f;

    // Cấu hình bắn đạn
    public Transform shootPoint; // Điểm bắn đạn
    public GameObject bulletPrefab; // Prefab đạn
    public float bulletSpeed = 10f; // Tốc độ đạn

    private Animator animator; // Tham chiếu đến Animator

    void Start()
    {
        // Lấy Rigidbody2D từ GameObject
        rb = GetComponent<Rigidbody2D>();

        // Lấy Animator từ GameObject
        animator = GetComponent<Animator>();

        // Đảm bảo Rigidbody2D là Kinematic
        if (rb.bodyType != RigidbodyType2D.Kinematic)
        {
            Debug.LogError("Rigidbody2D phải được đặt thành Kinematic!");
        }
    }

    void Update()
    {
        // Lấy giá trị di chuyển từ bàn phím
        movement.x = Input.GetAxisRaw("Horizontal"); // Trục ngang (A, D hoặc Left, Right)
        movement.y = Input.GetAxisRaw("Vertical");   // Trục dọc (W, S hoặc Up, Down)

        // Kiểm tra phím Space để bắn đạn
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        // Điều khiển animation và flip X
        HandleAnimationAndFlip();
    }

    void FixedUpdate()
    {
        // Tính vị trí mới
        Vector2 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        // Giới hạn vị trí trong phạm vi
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        // Cập nhật vị trí của Rigidbody
        rb.MovePosition(newPosition);
    }

    void Shoot()
    {
        if (shootPoint == null || bulletPrefab == null)
        {
            Debug.LogError("Shoot Point hoặc Bullet Prefab chưa được gán!");
            return;
        }

        // Tạo đạn tại vị trí shootPoint
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);

        // Gắn tốc độ cho đạn
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            bulletRb.linearVelocity = Vector2.up * bulletSpeed; // Đạn di chuyển hướng lên trên
        }

        // Tùy chọn: Hủy đạn sau 5 giây nếu cần
        Destroy(bullet, 5f);
    }

    // Hàm điều khiển animation và flip X
    void HandleAnimationAndFlip()
    {
        // Kiểm tra di chuyển theo trục X
        if (movement.x != 0)
        {
            // Cập nhật trạng thái animation cho chạy
            animator.SetBool("isRunning", true); // Cập nhật animation "isRunning"

            // Flip nhân vật nếu di chuyển theo trục X
            if (movement.x > 0)
            {
                // Flip nhân vật sang phải (giữ nguyên kích thước ban đầu)
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (movement.x < 0)
            {
                // Flip nhân vật sang trái (giữ nguyên kích thước ban đầu)
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

        }
        else
        {
            // Không di chuyển
            animator.SetBool("isRunning", false); // Dừng animation "isRunning"
        }
    }
}


