using UnityEngine;
using System.Collections;

public class Egg : MonoBehaviour
{
    private Animator animator; // Animator để điều khiển animation
    public float delayBeforeDestroy = 1f; // Thời gian chờ trước khi phá hủy
    private bool isBroken = false; // Trạng thái kiểm tra nếu trứng đã chạm NonTouch

    void Start()
    {
        // Lấy Animator từ GameObject
        animator = GetComponent<Animator>();

        // Kiểm tra Animator có tồn tại không
        if (animator == null)
        {
            Debug.LogError("Không tìm thấy Animator trên GameObject!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Nếu trứng đã vỡ, bỏ qua các xử lý khác
        if (isBroken) return;

        if (collision.gameObject.CompareTag("NonTouch"))
        {
            Debug.Log("Trứng đã vỡ!");
            isBroken = true;
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Debug.Log("Trứng đã vỡ!");
            isBroken = true;
            StartCoroutine(PlayAnimationAndDestroy());
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Destroy(rb);
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Đã va vào Player");

            // Tìm tất cả các đối tượng có tag "Player"
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                Destroy(player); // Phá hủy từng đối tượng
            }

            // Đặt trạng thái trứng đã vỡ
            isBroken = true;

            // Bắt đầu Coroutine để play animation và phá hủy trứng
            StartCoroutine(PlayAnimationAndDestroy());

            // Loại bỏ tương tác vật lý cho đối tượng trứng
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Destroy(rb); // Loại bỏ Rigidbody2D để đối tượng không còn bị ảnh hưởng bởi vật lý
            }
        }

    }

    private IEnumerator PlayAnimationAndDestroy()
    {
        // Chạy animation (trong Animator phải có trigger "Break")
        Debug.Log("Trigger 'Crack' được kích hoạt!");
        animator.SetTrigger("Crack");

        // Chờ delayBeforeDestroy giây
        yield return new WaitForSeconds(delayBeforeDestroy);

        // Phá hủy đối tượng
        Destroy(gameObject);
    }
}



