using System.Threading.Tasks;
using DG.Tweening;
using BasePuzzle.PuzzlePackages.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PuzzleGames
{
    using System;
    using System.Collections;
    using BasePuzzle.Modules.UI.Transition.Runtime;

    public class LoadSceneManager : PersistentSingleton<LoadSceneManager>
    {
        [SerializeField] private UITransitionManager transitionManager;

        private string _previousScene;
        private bool   _isInProgress;
        // private bool   _isFirstLoad; // Không cần thiết với logic mới

        protected override void Awake()
        {
            _previousScene = SceneManager.GetActiveScene().name;

            base.Awake();
        }

        public void LoadScene(string sceneName)
        {
            ResourceManager.Instance.ReleaseUI();
            StartCoroutine(CoLoadSceneAsync(sceneName));
        }

        private IEnumerator CoLoadSceneAsync(string sceneName)
        {
            if (_isInProgress) yield break;
            
            Time.timeScale = 1;
            _isInProgress  = true;

            yield return null;

            // --- ĐÃ BỎ QUA LOGIC _isFirstLoad ---
            
            // 1. Chờ hiệu ứng chuyển cảnh (Wipe In)
            var wipeInAsync = transitionManager.IrisWipeInAsync;
            yield return new WaitUntil(() => wipeInAsync.IsCompleted);
            
            // 2. Tải scene mới (NON-ADDITIVE)
            // Đây là thay đổi quan trọng nhất. 
            // LoadSceneAsync (không có Additive) sẽ tự động hủy scene cũ
            // và bảo toàn các đối tượng DontDestroyOnLoad.
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName); 
            
            while (loadOperation is { isDone: false })
            {
                yield return null;
            }

            // --- TOÀN BỘ LOGIC UNLOAD ĐÃ BỊ XÓA ---
            // Vì SceneManager.LoadSceneAsync đã tự xử lý.

            // 3. Kích hoạt scene mới (nên làm)
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            
            yield return null;

            // 4. Chạy hiệu ứng chuyển cảnh ra (Wipe Out)
            transitionManager.IrisWipeOut(null);

            // 5. --- THÊM SỬA LỖI ---
            // Ẩn LoadingCanvas (với thanh progress bar)
            // Đây là dòng bị thiếu
            transitionManager.ProgressBarOut(null); 

            _previousScene = sceneName;
            _isInProgress  = false;
        }
    }
}

