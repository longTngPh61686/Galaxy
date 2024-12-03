using UnityEngine;

public class LaserItem : MonoBehaviour
{
    public float moveSpeed = 2f;

    void Update()
    {
        // Di chuyển item xuống
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            // Nếu người chơi ăn được item, tăng cường laser
            col.gameObject.GetComponent<Plane>().UpgradeLaser();
            Destroy(gameObject); // Item biến mất
        }
    }
}
