using UnityEngine;
using System.Collections;

public class Plane : MonoBehaviour
{
    public float moveSpeed = 5f; // Tốc độ di chuyển
    public GameObject laserPrefab; // Prefab của laser
    public float laserSpeed = 10f; // Tốc độ laser
    public Transform laserSpawnPoint; // Điểm bắn laser
    private int laserType = 1; // Mặc định là laser đơn

    // Giới hạn di chuyển
    public float minX = -2f;
    public float maxX = 2f;
    public float minY = -4f;
    public float maxY = 2f;

    private Rigidbody2D rb;
    private Vector2 movement;

    private Animator animator; // Tham chiếu đến Animator

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Lấy component Rigidbody2D
        animator = GetComponent<Animator>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on Plane!");
        }
    }

    private void Update()
    {
        // Di chuyển máy bay
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        movement = new Vector2(horizontalInput, verticalInput);

        // Bắn laser
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootLaser();
        }
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

    void ShootLaser()
    {
        if (laserPrefab && laserSpawnPoint)
        {
            if (laserType == 1)
            {
                // Bắn laser đơn ngay lập tức
                ShootSingleLaser();
            }
            else if (laserType == 2)
            {
                // Bắn 2 viên laser, cách nhau 0.5 giây
                StartCoroutine(ShootDoubleLaser());
            }
            else if (laserType == 3)
            {
                // Laser ba hướng
                ShootTripleLaser();
            }
        }
    }

    void ShootSingleLaser()
    {
        // Bắn laser đơn
        GameObject laser = Instantiate(laserPrefab, laserSpawnPoint.position, laserSpawnPoint.rotation);
        Rigidbody2D laserRb = laser.GetComponent<Rigidbody2D>();
        if (laserRb)
        {
            laserRb.linearVelocity = Vector2.up * laserSpeed; // Laser đơn di chuyển lên
        }
    }

    IEnumerator ShootDoubleLaser()
    {
        // Bắn viên laser đầu tiên ngay lập tức
        ShootSingleLaser();

        // Chờ 0.5 giây trước khi bắn viên laser thứ hai
        yield return new WaitForSeconds(0.1f);

        // Bắn viên laser thứ hai
        ShootSingleLaser();
    }

    void ShootTripleLaser()
    {
        // Bắn laser ba hướng (ngang, lên, và xuống)
        InstantiateLaser(1);
        InstantiateLaser(0);
        InstantiateLaser(-1);
    }

    void InstantiateLaser(float angleOffset)
    {
        GameObject laser = Instantiate(laserPrefab, laserSpawnPoint.position, laserSpawnPoint.rotation);
        Rigidbody2D laserRb = laser.GetComponent<Rigidbody2D>();
        if (laserRb)
        {
            float angle = transform.rotation.eulerAngles.z + angleOffset * 15; // Điều chỉnh góc
            laser.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            laserRb.linearVelocity = laser.transform.up * laserSpeed;
        }
    }

    public void UpgradeLaser()
    {
        if (laserType == 1)
        {
            laserType = 2; // Chuyển sang laser đôi
        }
        else if (laserType == 2)
        {
            laserType = 3; // Chuyển sang laser ba hướng
        }
    }
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



