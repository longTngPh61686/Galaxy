using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Để sử dụng SceneManager

public class Manager : MonoBehaviour
{
    public Button restartButton; // Nút Restart
    public GameObject player; // Đối tượng Player

    private void Start()
    {
        // Ẩn nút restart và panel game over khi bắt đầu trò chơi
        restartButton.gameObject.SetActive(false);
        // Đảm bảo nút restart gọi hàm RestartGame khi được click
        restartButton.onClick.AddListener(RestartGame);
    }

    private void Update()
    {
        // Kiểm tra xem player có bị destroy không
        if (player == null)
        {
            ShowGameOver();
        }
    }

    void ShowGameOver()
    {
        // Hiển thị panel Game Over và nút Restart
        restartButton.gameObject.SetActive(true);
    }

    void RestartGame()
    {
        // Lại scene hiện tại, sẽ bắt đầu lại từ đầu
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

