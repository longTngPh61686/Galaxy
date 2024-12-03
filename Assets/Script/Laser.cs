using UnityEngine;

public class Laser : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra xem đối tượng va chạm có tag là "Enemy" hay không
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Destroy(gameObject);
        }
    }
}
