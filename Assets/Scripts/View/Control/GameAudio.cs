using UnityEngine;

namespace View.Control
{
    /// <summary>
    /// The main controller for the game's audio. Handles SFX along with looping music.
    /// (MODIFIED to be a persistent Singleton and own its music AudioSource)
    /// </summary>
    [RequireComponent(typeof(AudioSource))] // Tự động yêu cầu một AudioSource
    public class GameAudio : MonoBehaviour
    {
        // --- BIẾN TOÀN CỤC (SINGLETON) ---
        public static GameAudio Instance { get; private set; }

        private const string MusicStatusKey = "music.status";
        private const string SfxStatusKey = "sfx.status";
        
        private const float MusicVolume = 0.2f;
        
        public AudioClip[] MusicClips;
        public AudioClip[] SfxClips;

        // --- AUDIO SOURCES ---
        // _musicSource sẽ là component trên chính GameObject này (vĩnh viễn)
        private AudioSource _musicSource;
        // Chúng ta vẫn có thể sử dụng LeanAudio cho SFX (fire-and-forget)
        
        private int _musicVolumeTweenId;
        private bool _musicEnabled = true;
        public bool MusicEnabled
        {
            get { return _musicEnabled; }
            set {
                if (_musicEnabled != value) {
                    LeanTween.cancel(_musicVolumeTweenId);
                }
                
                _musicEnabled = value;
                
                // Thêm kiểm tra null an toàn
                if (_musicSource != null) 
                {
                    _musicSource.volume = value ? MusicVolume : 0f;
                    // Bổ sung: Bật/tắt nhạc ngay lập tức
                    if (value && !_musicSource.isPlaying) _musicSource.Play();
                    else if (!value) _musicSource.Stop();
                }
                PlayerPrefs.SetInt(MusicStatusKey, value ? 0 : 1);
            }
        }

        private bool _sfxEnabled = true;
        public bool SfxEnabled
        {
            get { return _sfxEnabled; }
            set {
                _sfxEnabled = value;
                PlayerPrefs.SetInt(SfxStatusKey, value ? 0 : 1);
            }
        }

        // --- AWAKE (Hàm quan trọng nhất) ---
        private void Awake()
        {
            // --- Thiết lập Singleton Pattern ---
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                // Nếu một GameAudio đã tồn tại, hãy phá hủy cái mới này
                Destroy(gameObject);
                return;
            }

            // --- Thiết lập Music Source ---
            // Lấy AudioSource vĩnh viễn trên chính GameObject này
            _musicSource = GetComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.playOnAwake = false;
        }

        private void Start()
        {
            // Tải cài đặt trước
            if (!PlayerPrefs.HasKey(MusicStatusKey)) {
                PlayerPrefs.SetInt(MusicStatusKey, 0);
            }
            if (!PlayerPrefs.HasKey(SfxStatusKey)) {
                PlayerPrefs.SetInt(SfxStatusKey, 0);
            }

            MusicEnabled = PlayerPrefs.GetInt(MusicStatusKey) == 0;
            SfxEnabled = PlayerPrefs.GetInt(SfxStatusKey) == 0;
            
            // Bắt đầu nhạc SAU KHI đã tải cài đặt
            StartMusic();
        }

        /// <summary>
        /// Plays the given sound clip with the specified parameters.
        /// (Hàm này vẫn ổn, LeanAudio rất tốt cho SFX)
        /// </summary>
        public void Play(GameClip clip, float delay = 0f, float volume = 1f, float startTime = 0f)
        {
            if (!enabled || !SfxEnabled) {
                return;
            }
            
            var audioClip = SfxClips[(uint) clip];
            LeanAudio.play(audioClip, volume, delay, time: startTime);
        }
        
        /// <summary>
        /// Plays the given music clip with the specifide parameters.
        /// (ĐÃ SỬA LỖI: Bây giờ sử dụng _musicSource vĩnh viễn)
        /// </summary>
        public void Play(MusicClip clip, float fadeTime = 0f, float delay = 0f, float volume = 1f, float startTime = 0f)
        {
            if (!enabled || _musicSource == null) {
                return;
            }
            
            var audioClip = MusicClips[(uint) clip];
            
            // --- XÓA DÒNG GÂY LỖI ---
            // _musicSource = LeanAudio.play(audioClip, 0f, delay, true, startTime);

            // --- THAY BẰNG LOGIC NÀY ---
            _musicSource.clip = audioClip;
            _musicSource.time = startTime;
            _musicSource.volume = 0f; // Bắt đầu từ 0 để fade-in

            // Sử dụng delayedCall để xử lý độ trễ
            LeanTween.delayedCall(delay, () => {
                if (MusicEnabled) // Kiểm tra lại phòng khi người dùng tắt nhạc trong lúc chờ
                {
                    _musicSource.Play();
                }

                // Hủy bất kỳ tween âm lượng nào *trước đó*
                LeanTween.cancel(_musicVolumeTweenId);

                // Tạo tween mới trên GameObject *này* (GameObject này là vĩnh viễn)
                _musicVolumeTweenId = LeanTween.value(gameObject, 0f, volume, fadeTime)
                    .setEase(LeanTweenType.easeInOutSine)
                    .setOnUpdate(v => {
                        // _musicSource bây giờ được đảm bảo tồn tại
                        _musicSource.volume = v;
                    })
                    .id;
            });
        }

        private void StartMusic()
        {
            // TODO: make configurable
            const float fadeTime = 3f;
            const float startTime = 32f;
            Play(MusicClip.Ambient02, fadeTime: fadeTime, volume: MusicVolume, startTime: startTime);
        }
    }

    /// <summary>
    /// A one-to-one map of all sound clips
    /// </summary>
    public enum GameClip
    {
        GameStart,
        WinBoard,
        NodeEnter,
        NodeLeave,
        MovePushHigh,
        MovePullHigh,
        MovePullMid,
        MovePushMid,
        MovePullLow,
        MovePushLow,
        ArcMoveHigh,
        NodeRotate90,
        InvalidRotate,
        MenuSelect,
        GameEnd,
        LevelEnable,
        ArcMoveMid,
        ArcMoveLow
    }

    /// <summary>
    /// A one-to-one map of all music clips
    /// </summary>
    public enum MusicClip
    {
        Ambient01,
        Ambient02
    }
}
