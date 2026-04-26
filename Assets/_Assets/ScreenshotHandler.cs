using UnityEngine;

namespace FalconGames
{
    using UnityEngine;
    using System.IO;

    public class ScreenshotHandler : MonoBehaviour
    {
        void Awake()
        {
            // Giữ object này không bị xóa khi load scene mới
            DontDestroyOnLoad(gameObject);
        }

        [Header("Cấu hình phím tắt")] public KeyCode screenshotKey = KeyCode.K; // Nhấn phím K để chụp

        [Header("Cấu hình thư mục")] public string folderName = "Screenshots";

        void Update()
        {
            // Kiểm tra nếu người dùng nhấn phím tắt
            if (Input.GetKeyDown(screenshotKey))
            {
                CaptureFixedRatio();
            }
        }

        public int width = 1920;
        public int height = 1080;
        public Camera targetCamera; // Kéo Camera chính vào đây

        public void CaptureFixedRatio()
        {
            if (targetCamera == null) targetCamera = Camera.main;

            // 1. Tạo một RenderTexture với đúng độ phân giải mong muốn
            RenderTexture rt = new RenderTexture(width, height, 24);
            targetCamera.targetTexture = rt;

            // 2. Tạo Texture2D để đọc dữ liệu từ RenderTexture
            Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);

            // 3. Render Camera
            targetCamera.Render();
            RenderTexture.active = rt;

            // 4. Đọc pixel và lưu thành file
            screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            targetCamera.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);

            byte[] bytes = screenShot.EncodeToPNG();
            
            // Đặt tên khác nhau mỗi lần chụp bằng cách thêm thời gian hiện tại
            string timeStamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string filename = Path.Combine(Application.dataPath, $"Screenshot_{timeStamp}.png");
            File.WriteAllBytes(filename, bytes);

            Debug.Log($"Đã chụp ảnh đúng tỉ lệ {width}x{height} tại: {filename}");
        }
    }
}