// using UnityEngine;
// using UnityEngine.SceneManagement;
// using TMPro;

// [RequireComponent(typeof(Rigidbody2D))]
// public class BallController : MonoBehaviour
// {
//     [Header("Movement Settings")]
//     [SerializeField] private float power = 500f;
//     [SerializeField] private float maxSpeed = 400f;
    
//     [Header("Game References")]
//     [SerializeField] private TMP_Text scoreText;
//     [SerializeField] private TMP_Text heartText;
//     [SerializeField] private int maxHearts = 3;
    
//     [Header("Sound Settings")]
//     [SerializeField] private AudioSource audioSource;
//     [SerializeField] private AudioClip ballHitSound;
//     [SerializeField] private AudioClip groundHitSound;
//     [SerializeField] private AudioClip gameOverSound;

//     private Rigidbody2D rb;
//     private bool isDragging = false;
//     private Vector2 startTouchPos;
//     private bool canCollide = true;
//     private int currentHearts;
//     private int currentScore;
//     private bool isGameOver = false;

//     private void Awake()
//     {
//         rb = GetComponent<Rigidbody2D>();
        
//         // اگر بازی جدید است (نه تغییر مرحله)، مقادیر را ریست کن
//         if (!PlayerPrefs.HasKey("IsStageChanged"))
//         {
//             currentHearts = maxHearts;
//             currentScore = 0;
//             SaveGameState();
//         }
//         else
//         {
//             // اگر از مرحله قبلی آمده‌ایم، مقادیر را بارگذاری کن
//             LoadGameState();
//             PlayerPrefs.DeleteKey("IsStageChanged");
//         }
        
//         if (scoreText == null)
//             scoreText = GameObject.Find("ScoreText")?.GetComponent<TMP_Text>();
//         if (heartText == null)
//             heartText = GameObject.Find("HeartText")?.GetComponent<TMP_Text>();
//     }

//     private void Start()
//     {
//         if (heartText != null)
//             heartText.gameObject.SetActive(true);
            
//         UpdateUI();
        
//         if (audioSource == null)
//         {
//             audioSource = gameObject.AddComponent<AudioSource>();
//             audioSource.playOnAwake = false;
//         }
//     }

//     private void Update()
//     {
//         if (isGameOver) return;

//         HandleTouchInput();
//         LimitVelocity();
//     }

//     private void HandleTouchInput()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             isDragging = true;
//             startTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//         }

//         if (Input.GetMouseButtonUp(0) && isDragging)
//         {
//             Vector2 endTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//             Vector2 forceDirection = (endTouchPos - startTouchPos).normalized;
            
//             ApplyForce(forceDirection);
//             AddScore(1);
//             PlaySound(ballHitSound);
//             isDragging = false;
//         }
//     }

//     private void ApplyForce(Vector2 direction)
//     {
//         rb.AddForce(direction * power, ForceMode2D.Impulse);
//     }

//     private void LimitVelocity()
//     {
//         if (rb.velocity.magnitude > maxSpeed)
//         {
//             rb.velocity = rb.velocity.normalized * maxSpeed;
//         }
//     }

//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("Ground") && canCollide)
//         {
//             HandleGroundCollision();
//         }
//     }

//     private void HandleGroundCollision()
//     {
//         canCollide = false;
//         LoseHeart();
//         PlaySound(groundHitSound);
//         Invoke(nameof(ResetCollision), 0.5f);
//     }

//     private void ResetCollision()
//     {
//         canCollide = true;
//     }

//     private void AddScore(int points)
//     {
//         currentScore += points;
//         SaveGameState();
//         UpdateUI();
//         CheckStageTransition();
//     }

//     private void LoseHeart()
//     {
//         currentHearts--;
//         SaveGameState();
//         UpdateUI();

//         if (currentHearts <= 0)
//         {
//             GameOver();
//         }
//     }

//     private void CheckStageTransition()
//     {
//         string currentScene = SceneManager.GetActiveScene().name;
        
//         if (currentScore >= 40 && currentScene != "Scene3_tennis")
//         {
//             PlayerPrefs.SetInt("IsStageChanged", 1); // علامت گذاری برای تغییر مرحله
//             SceneManager.LoadScene("Scene3_tennis");
//         }
//         else if (currentScore >= 20 && currentScene == "Scene1_foot")
//         {
//             PlayerPrefs.SetInt("IsStageChanged", 1); // علامت گذاری برای تغییر مرحله
//             SceneManager.LoadScene("Scene2_basket");
//         }
//     }

//     private void UpdateUI()
//     {
//         if (scoreText != null)
//             scoreText.text = currentScore.ToString();
        
//         if (heartText != null)
//             heartText.text = currentHearts.ToString();
//     }

//     private void LoadGameState()
//     {
//         currentScore = PlayerPrefs.GetInt("CurrentScore", 0);
//         currentHearts = PlayerPrefs.GetInt("CurrentHearts", maxHearts);
//     }

//     private void SaveGameState()
//     {
//         PlayerPrefs.SetInt("CurrentScore", currentScore);
//         PlayerPrefs.SetInt("CurrentHearts", currentHearts);
//         PlayerPrefs.Save();
//     }

//     private void GameOver()
//     {
//         isGameOver = true;
//         PlaySound(gameOverSound);
        
//         // توقف فیزیک توپ
//         rb.velocity = Vector2.zero;
//         rb.angularVelocity = 0f;
//         rb.isKinematic = true;
        
//         Debug.Log($"Game Over! Final Score: {currentScore}");
        
//         // ریست کامل برای بازی جدید
//         PlayerPrefs.DeleteKey("CurrentScore");
//         PlayerPrefs.DeleteKey("CurrentHearts");
//         PlayerPrefs.DeleteKey("IsStageChanged");
//     }

//     // برای ریست دستی بازی
//     public void ManualReset()
//     {
//         PlayerPrefs.DeleteKey("CurrentScore");
//         PlayerPrefs.DeleteKey("CurrentHearts");
//         PlayerPrefs.DeleteKey("IsStageChanged");
//         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//     }

//     private void PlaySound(AudioClip clip)
//     {
//         if (audioSource != null && clip != null)
//         {
//             audioSource.PlayOneShot(clip);
//         }
//     }
// }












// using UnityEngine;
// using UnityEngine.SceneManagement;
// using TMPro;

// [RequireComponent(typeof(Rigidbody2D))]
// public class BallController : MonoBehaviour
// {
//     [Header("Movement Settings")]
//     [SerializeField] private float power = 500f;
//     [SerializeField] private float maxSpeed = 400f;
    
//     [Header("Game References")]
//     [SerializeField] private TMP_Text scoreText;
//     [SerializeField] private TMP_Text heartText;
//     [SerializeField] private int maxHearts = 3;
    
//     [Header("Sound Settings")]
//     [SerializeField] private AudioSource audioSource;
//     [SerializeField] private AudioClip ballHitSound;
//     [SerializeField] private AudioClip groundHitSound;
//     [SerializeField] private AudioClip gameOverSound;

//     [Header("Scene Background Sounds")]
//     [SerializeField] private AudioClip scene1Sound;
//     [SerializeField] private AudioClip scene2Sound;
//     [SerializeField] private AudioClip scene3Sound;

//     private Rigidbody2D rb;
//     private bool isDragging = false;
//     private Vector2 startTouchPos;
//     private bool canCollide = true;
//     private int currentHearts;
//     private int currentScore;
//     private bool isGameOver = false;
//     private AudioSource backgroundAudioSource;

//     private void Awake()
//     {
//         rb = GetComponent<Rigidbody2D>();
        
//         // ایجاد AudioSource برای صدای پس‌زمینه
//         backgroundAudioSource = gameObject.AddComponent<AudioSource>();
//         backgroundAudioSource.loop = true;
//         backgroundAudioSource.playOnAwake = false;

//         if (!PlayerPrefs.HasKey("IsStageChanged"))
//         {
//             currentHearts = maxHearts;
//             currentScore = 0;
//             SaveGameState();
//         }
//         else
//         {
//             LoadGameState();
//             PlayerPrefs.DeleteKey("IsStageChanged");
//         }
        
//         if (scoreText == null)
//             scoreText = GameObject.Find("ScoreText")?.GetComponent<TMP_Text>();
//         if (heartText == null)
//             heartText = GameObject.Find("HeartText")?.GetComponent<TMP_Text>();
//     }

//     private void Start()
//     {
//         if (heartText != null)
//             heartText.gameObject.SetActive(true);
            
//         UpdateUI();
        
//         if (audioSource == null)
//         {
//             audioSource = gameObject.AddComponent<AudioSource>();
//             audioSource.playOnAwake = false;
//         }

//         // پخش صدای مخصوص صحنه فعلی
//         PlaySceneBackgroundSound();
//     }

//     private void PlaySceneBackgroundSound()
//     {
//         // توقف صدای قبلی اگر در حال پخش است
//         if (backgroundAudioSource.isPlaying)
//         {
//             backgroundAudioSource.Stop();
//         }

//         // تشخیص صحنه فعلی و پخش صدای مناسب
//         string currentScene = SceneManager.GetActiveScene().name;
//         AudioClip clipToPlay = null;

//         if (currentScene == "Scene1_foot")
//         {
//             clipToPlay = scene1Sound;
//         }
//         else if (currentScene == "Scene2_basket")
//         {
//             clipToPlay = scene2Sound;
//         }
//         else if (currentScene == "Scene3_tennis")
//         {
//             clipToPlay = scene3Sound;
//         }

//         // پخش صدای جدید
//         if (clipToPlay != null)
//         {
//             backgroundAudioSource.clip = clipToPlay;
//             backgroundAudioSource.Play();
//         }
//     }

//     private void Update()
//     {
//         if (isGameOver) return;

//         HandleTouchInput();
//         LimitVelocity();
//     }

//     private void HandleTouchInput()
//     {
//         if (Input.GetMouseButtonDown(0))
//         {
//             isDragging = true;
//             startTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//         }

//         if (Input.GetMouseButtonUp(0) && isDragging)
//         {
//             Vector2 endTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//             Vector2 forceDirection = (endTouchPos - startTouchPos).normalized;
            
//             ApplyForce(forceDirection);
//             AddScore(1);
//             PlaySound(ballHitSound);
//             isDragging = false;
//         }
//     }

//     private void ApplyForce(Vector2 direction)
//     {
//         rb.AddForce(direction * power, ForceMode2D.Impulse);
//     }

//     private void LimitVelocity()
//     {
//         if (rb.velocity.magnitude > maxSpeed)
//         {
//             rb.velocity = rb.velocity.normalized * maxSpeed;
//         }
//     }

//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         if (collision.gameObject.CompareTag("Ground") && canCollide)
//         {
//             HandleGroundCollision();
//         }
//     }

//     private void HandleGroundCollision()
//     {
//         canCollide = false;
//         LoseHeart();
//         PlaySound(groundHitSound);
//         Invoke(nameof(ResetCollision), 0.5f);
//     }

//     private void ResetCollision()
//     {
//         canCollide = true;
//     }

//     private void AddScore(int points)
//     {
//         currentScore += points;
//         SaveGameState();
//         UpdateUI();
//         CheckStageTransition();
//     }

//     private void LoseHeart()
//     {
//         currentHearts--;
//         SaveGameState();
//         UpdateUI();

//         if (currentHearts <= 0)
//         {
//             GameOver();
//         }
//     }

//     private void CheckStageTransition()
//     {
//         string currentScene = SceneManager.GetActiveScene().name;
        
//         if (currentScore >= 40 && currentScene != "Scene3_tennis")
//         {
//             PlayerPrefs.SetInt("IsStageChanged", 1);
//             SceneManager.LoadScene("Scene3_tennis");
//         }
//         else if (currentScore >= 20 && currentScene == "Scene1_foot")
//         {
//             PlayerPrefs.SetInt("IsStageChanged", 1);
//             SceneManager.LoadScene("Scene2_basket");
//         }
//     }

//     private void UpdateUI()
//     {
//         if (scoreText != null)
//             scoreText.text = currentScore.ToString();
        
//         if (heartText != null)
//             heartText.text = currentHearts.ToString();
//     }

//     private void LoadGameState()
//     {
//         currentScore = PlayerPrefs.GetInt("CurrentScore", 0);
//         currentHearts = PlayerPrefs.GetInt("CurrentHearts", maxHearts);
//     }

//     private void SaveGameState()
//     {
//         PlayerPrefs.SetInt("CurrentScore", currentScore);
//         PlayerPrefs.SetInt("CurrentHearts", currentHearts);
//         PlayerPrefs.Save();
//     }

//     private void GameOver()
//     {
//         isGameOver = true;
//         PlaySound(gameOverSound);
        
//         // توقف فیزیک توپ
//         rb.velocity = Vector2.zero;
//         rb.angularVelocity = 0f;
//         rb.isKinematic = true;
        
//         Debug.Log($"Game Over! Final Score: {currentScore}");
        
//         // ریست کامل برای بازی جدید
//         PlayerPrefs.DeleteKey("CurrentScore");
//         PlayerPrefs.DeleteKey("CurrentHearts");
//         PlayerPrefs.DeleteKey("IsStageChanged");

//         // توقف صدای پس‌زمینه
//         if (backgroundAudioSource.isPlaying)
//         {
//             backgroundAudioSource.Stop();
//         }
//     }

//     public void ManualReset()
//     {
//         PlayerPrefs.DeleteKey("CurrentScore");
//         PlayerPrefs.DeleteKey("CurrentHearts");
//         PlayerPrefs.DeleteKey("IsStageChanged");
//         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//     }

//     private void PlaySound(AudioClip clip)
//     {
//         if (audioSource != null && clip != null)
//         {
//             audioSource.PlayOneShot(clip);
//         }
//     }
// }



























using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float power = 500f;
    [SerializeField] private float maxSpeed = 400f;
    
    [Header("Game References")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text heartText;
    [SerializeField] private int maxHearts = 3;
    
    [Header("Sound Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip ballHitSound;
    [SerializeField] private AudioClip groundHitSound;
    [SerializeField] private AudioClip gameOverSound;

    [Header("Scene Background Sounds")]
    [SerializeField] private AudioClip scene1BackgroundSound;
    [SerializeField] private AudioClip scene2BackgroundSound;
    [SerializeField] private AudioClip scene3BackgroundSound;
    [SerializeField] [Range(0, 1)] private float backgroundVolume = 0.5f;

    private Rigidbody2D rb;
    private bool isDragging = false;
    private Vector2 startTouchPos;
    private bool canCollide = true;
    private int currentHearts;
    private int currentScore;
    private bool isGameOver = false;
    private AudioSource backgroundAudioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // ایجاد AudioSource برای صدای بک‌گراند
        backgroundAudioSource = gameObject.AddComponent<AudioSource>();
        backgroundAudioSource.loop = true;
        backgroundAudioSource.playOnAwake = false;
        backgroundAudioSource.volume = backgroundVolume;

        // مدیریت وضعیت بازی
        if (!PlayerPrefs.HasKey("IsStageChanged"))
        {
            currentHearts = maxHearts;
            currentScore = 0;
            SaveGameState();
        }
        else
        {
            LoadGameState();
            PlayerPrefs.DeleteKey("IsStageChanged");
        }
        
        // پیدا کردن المان‌های UI اگر در Inspector تنظیم نشده باشند
        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TMP_Text>();
        if (heartText == null)
            heartText = GameObject.Find("HeartText")?.GetComponent<TMP_Text>();
    }

    private void Start()
    {
        // فعال کردن نمایش قلب‌ها
        if (heartText != null)
            heartText.gameObject.SetActive(true);
            
        UpdateUI();
        
        // ایجاد AudioSource اگر وجود نداشته باشد
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        // پخش صدای بک‌گراند صحنه فعلی
        PlaySceneBackgroundSound();
    }

    private void Update()
    {
        if (isGameOver) return;

        HandleTouchInput();
        LimitVelocity();
    }

    private void HandleTouchInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            Vector2 endTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 forceDirection = (endTouchPos - startTouchPos).normalized;
            
            ApplyForce(forceDirection);
            AddScore(1);
            PlaySound(ballHitSound);
            isDragging = false;
        }
    }

    private void ApplyForce(Vector2 direction)
    {
        rb.AddForce(direction * power, ForceMode2D.Impulse);
    }

    private void LimitVelocity()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && canCollide)
        {
            HandleGroundCollision();
        }
    }

    private void HandleGroundCollision()
    {
        canCollide = false;
        LoseHeart();
        PlaySound(groundHitSound);
        Invoke(nameof(ResetCollision), 0.5f);
    }

    private void ResetCollision()
    {
        canCollide = true;
    }

    private void AddScore(int points)
    {
        currentScore += points;
        SaveGameState();
        UpdateUI();
        CheckStageTransition();
    }

    private void LoseHeart()
    {
        currentHearts--;
        SaveGameState();
        UpdateUI();

        if (currentHearts <= 0)
        {
            GameOver();
        }
    }

    private void CheckStageTransition()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        
        if (currentScore >= 40 && currentScene != "Scene3_tennis")
        {
            PlayerPrefs.SetInt("IsStageChanged", 1);
            SceneManager.LoadScene("Scene3_tennis");
        }
        else if (currentScore >= 20 && currentScene == "Scene1_foot")
        {
            PlayerPrefs.SetInt("IsStageChanged", 1);
            SceneManager.LoadScene("Scene2_basket");
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = currentScore.ToString();
        
        if (heartText != null)
            heartText.text = currentHearts.ToString();
    }

    private void PlaySceneBackgroundSound()
    {
        // توقف صدای قبلی اگر در حال پخش است
        if (backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.Stop();
        }

        // تشخیص صحنه فعلی و پخش صدای مناسب
        string currentScene = SceneManager.GetActiveScene().name;
        AudioClip clipToPlay = null;

        if (currentScene == "Scene1_foot")
        {
            clipToPlay = scene1BackgroundSound;
        }
        else if (currentScene == "Scene2_basket")
        {
            clipToPlay = scene2BackgroundSound;
        }
        else if (currentScene == "Scene3_tennis")
        {
            clipToPlay = scene3BackgroundSound;
        }

        // پخش صدای جدید
        if (clipToPlay != null)
        {
            backgroundAudioSource.clip = clipToPlay;
            backgroundAudioSource.Play();
        }
    }

    private void LoadGameState()
    {
        currentScore = PlayerPrefs.GetInt("CurrentScore", 0);
        currentHearts = PlayerPrefs.GetInt("CurrentHearts", maxHearts);
    }

    private void SaveGameState()
    {
        PlayerPrefs.SetInt("CurrentScore", currentScore);
        PlayerPrefs.SetInt("CurrentHearts", currentHearts);
        PlayerPrefs.Save();
    }

    private void GameOver()
    {
        isGameOver = true;
        PlaySound(gameOverSound);
        
        // توقف فیزیک توپ
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;
        
        Debug.Log($"Game Over! Final Score: {currentScore}");
        
        // توقف صدای بک‌گراند
        if (backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.Stop();
        }
        
        // ریست کامل برای بازی جدید
        PlayerPrefs.DeleteKey("CurrentScore");
        PlayerPrefs.DeleteKey("CurrentHearts");
        PlayerPrefs.DeleteKey("IsStageChanged");
    }

    public void ManualReset()
    {
        PlayerPrefs.DeleteKey("CurrentScore");
        PlayerPrefs.DeleteKey("CurrentHearts");
        PlayerPrefs.DeleteKey("IsStageChanged");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}