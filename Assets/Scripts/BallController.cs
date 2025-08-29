using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;   // برای Button
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

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

 
    [Header("Cheat Settings (Hidden Button)")]
    [SerializeField] private Button secretButton;       
    [SerializeField] private int tapsToActivateCheat = 3;   
    [SerializeField] private int cheatScoreCeiling = 60;     
    private int secretTapCount = 0;
    private bool cheatActive = false;
    private const string CheatActiveKey = "CheatActive";


    [Header("Ball Skins (2 per type)")]
    [SerializeField] private SpriteRenderer ballRenderer;     
    [SerializeField] private Sprite[] footballSkins;         
    [SerializeField] private Sprite[] basketballSkins;       
    [SerializeField] private Sprite[] tennisSkins;      

    [Header("Pause UI")]
    [SerializeField] private GameObject pauseMenuPanel;    
    private bool isPaused = false;


    [Header("Revive UI")]
    [SerializeField] private GameObject revivePanel;         
    private bool isSoftGameOver = false;              
    private bool waitingForReviveAd = false;

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
        
        backgroundAudioSource = gameObject.AddComponent<AudioSource>();
        backgroundAudioSource.loop = true;
        backgroundAudioSource.playOnAwake = false;
        backgroundAudioSource.volume = backgroundVolume;

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

 cheatActive = false;


        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TMP_Text>();
        if (heartText == null)
            heartText = GameObject.Find("HeartText")?.GetComponent<TMP_Text>();
    }

    private void Start()
    {
        if (heartText != null) heartText.gameObject.SetActive(true);
        UpdateUI();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

 
        if (secretButton != null)
            secretButton.onClick.AddListener(SecretButtonClick);


        if (AdsManager.Instance != null)
            AdsManager.RewardGranted += OnRewardedAdCompleted;

        PlaySceneBackgroundSound();
        

        ApplyBallSkinForCurrentScene_Simple();
    }

    private void OnDestroy()
    {
        if (secretButton != null)
            secretButton.onClick.RemoveListener(SecretButtonClick);
        AdsManager.RewardGranted -= OnRewardedAdCompleted;
    }

    private void Update()
    {
        if (isGameOver || isSoftGameOver || isPaused) return;

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

    public GameObject cheatActivatedResponse;
    public void SecretButtonClick()
    {
        if ( SceneManager.GetActiveScene().name == "Scene3_tennis" && currentScore > 100)
            return;
        secretTapCount++;
        if (!cheatActive && secretTapCount >= tapsToActivateCheat)
        {
            cheatActive = true;
            cheatActivatedResponse.SetActive(true);
            PlayerPrefs.SetInt(CheatActiveKey, 1);
            PlayerPrefs.Save();
            StartCoroutine(CheatTurnOff());
        }
    }

public IEnumerator CheatTurnOff()
    {
        yield return new WaitForSeconds(3);
        cheatActivatedResponse.SetActive(false);
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
        if (collision.gameObject.CompareTag("Ground") && canCollide && !isSoftGameOver)
        {
            HandleGroundCollision();
        }
    }

    private void HandleGroundCollision()
    {
        canCollide = false;

        if (cheatActive && currentScore < cheatScoreCeiling)
        {
            PlaySound(groundHitSound);
            Invoke(nameof(ResetCollision), 0.4f);
            return;
        }


        currentHearts--;
        PlaySound(groundHitSound);

        if (currentHearts <= 0)
        {
            EnterSoftGameOver();
        }
        else
        {
            SaveGameState();
            UpdateUI();
            Invoke(nameof(ResetCollision), 0.4f);
        }
    }

    private void ResetCollision() => canCollide = true;

    private void AddScore(int points)
    {
        currentScore += points;

        if (cheatActive && currentScore >= cheatScoreCeiling)
        {
            cheatActive = false;
            PlayerPrefs.SetInt(CheatActiveKey, 0);
        }

        SaveGameState();
        UpdateUI();
        CheckStageTransition();
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
        if (scoreText != null) scoreText.text = currentScore.ToString();
        if (heartText != null) heartText.text = currentHearts.ToString();
    }


private int VariantFromChosen(string chosenKey)
{
    // اگر کلید انتخاب‌شده، ورژن 1 باشد → وریانت 1
    if (chosenKey == GlobalValue.FootBall1 ||
        chosenKey == GlobalValue.BasketBall1 ||
        chosenKey == GlobalValue.TennisBall1)
        return 1;
    return 0; // ورژن عادی
}

// گرفتن بانک اسپریت و انتخاب وریانت
private Sprite GetSkinByTypeAndVariant(string typeKey, int variantIdx)
{
    Sprite[] bank = null;
    if (typeKey == GlobalValue.Football || typeKey == GlobalValue.FootBall1)
        bank = footballSkins;
    else if (typeKey == GlobalValue.Basketball || typeKey == GlobalValue.BasketBall1)
        bank = basketballSkins;
    else if (typeKey == GlobalValue.TennisBall || typeKey == GlobalValue.TennisBall1)
        bank = tennisSkins;

    if (bank == null || bank.Length == 0) return null;
    variantIdx = Mathf.Clamp(variantIdx, 0, bank.Length - 1);
    return bank[variantIdx];
}


private void ApplyBallSkinForCurrentScene_Simple()
{
    if (ballRenderer == null) return;

    string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

    if (scene == "Scene1_foot")
    {
        string chosen = PlayerPrefs.GetString(GlobalValue.ChoosenBall, string.Empty);
        if (!string.IsNullOrEmpty(chosen) && GlobalValue.GetItems(chosen))
        {
            int variant = VariantFromChosen(chosen);

            string baseType = GlobalValue.Football; 
            if (chosen == GlobalValue.Football || chosen == GlobalValue.FootBall1) baseType = GlobalValue.Football;
            else if (chosen == GlobalValue.Basketball || chosen == GlobalValue.BasketBall1) baseType = GlobalValue.Basketball;
            else if (chosen == GlobalValue.TennisBall || chosen == GlobalValue.TennisBall1) baseType = GlobalValue.TennisBall;

            Sprite s = GetSkinByTypeAndVariant(baseType, variant);
            if (s != null) { ballRenderer.sprite = s; return; }
        }

        ballRenderer.sprite = RandomOrFirst(footballSkins, false);
    }
    else if (scene == "Scene2_basket")
    {
        ballRenderer.sprite = RandomOrFirst(basketballSkins, true);
    }
    else if (scene == "Scene3_tennis")
    {
        ballRenderer.sprite = RandomOrFirst(tennisSkins, true);
    }
    else
    {
        ballRenderer.sprite = RandomOrFirst(footballSkins, false);
    }
}
    private Sprite GetFirstSkinByType(string typeKey)
    {
        if (typeKey == GlobalValue.Football)   return RandomOrFirst(footballSkins, false);
        if (typeKey == GlobalValue.Basketball) return RandomOrFirst(basketballSkins, false);
        if (typeKey == GlobalValue.TennisBall) return RandomOrFirst(tennisSkins, false);
        return null;
    }

    private Sprite RandomOrFirst(Sprite[] bank, bool useRandom)
    {
        if (bank == null || bank.Length == 0) return null;
        if (useRandom) return bank[Random.Range(0, bank.Length)];
        return bank[0];
    }

    public void OnPauseButton()
    {
        if (isGameOver || isSoftGameOver) return;
        isPaused = true;
        Time.timeScale = 0f;
        if (pauseMenuPanel) pauseMenuPanel.SetActive(true);
    }

    public void OnResumeButton()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pauseMenuPanel) pauseMenuPanel.SetActive(false);
    }

    public void OnRestartButton()
    {
        Time.timeScale = 1f;
        ManualReset();
    }

    public void OnExitButton()
    {
        Time.timeScale = 1f;
        if (Application.CanStreamedLevelBeLoaded("MainMenu"))
            SceneManager.LoadScene("MainMenu");
        else
            Application.Quit();
    }

    private void EnterSoftGameOver()
    {
        isSoftGameOver = true;


        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.isKinematic = true;

        Time.timeScale = 0f;
        GameOver();
        if (revivePanel) revivePanel.SetActive(true);
    }

        public void OnReviveYes_WatchAd()
    {
        if (waitingForReviveAd) return;
        if (AdsManager.Instance == null) { OnReviveNo_GameOver(); return; }

        waitingForReviveAd = true;
        AdsManager.Instance.ShowRewardedAd(); 
    }


    private void OnRewardedAdCompleted()
    {
        if (!isSoftGameOver) return;

        waitingForReviveAd = false;
        currentHearts = Mathf.Max(currentHearts, 0) + 1;
        SaveGameState();
        UpdateUI();

        if (revivePanel) revivePanel.SetActive(false);
        isSoftGameOver = false;
        Time.timeScale = 1f;

        rb.isKinematic = false;
        Invoke(nameof(ResetCollision), 0.1f);
    }

   
    public void OnReviveNo_GameOver()
    {
        // waitingForReviveAd = false;
        // if (revivePanel) revivePanel.SetActive(false);
        // Time.timeScale = 1f;
        // GameOver();
    }

    private void PlaySceneBackgroundSound()
    {
        if (backgroundAudioSource.isPlaying) backgroundAudioSource.Stop();

        string currentScene = SceneManager.GetActiveScene().name;
        AudioClip clipToPlay = null;

        if (currentScene == "Scene1_foot") clipToPlay = scene1BackgroundSound;
        else if (currentScene == "Scene2_basket") clipToPlay = scene2BackgroundSound;
        else if (currentScene == "Scene3_tennis") clipToPlay = scene3BackgroundSound;

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
    public TMP_Text EndScoreText;
    private void GameOver()
    {
        isGameOver = true;
        PlaySound(gameOverSound);

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        // rb.isKinematic = true;

        if (backgroundAudioSource.isPlaying) backgroundAudioSource.Stop();

        Debug.Log($"Game Over! Final Score: {currentScore}");
        EndScoreText.text  = $"Points: {scoreText.text}";
        PlayerPrefs.DeleteKey("CurrentScore");
        PlayerPrefs.DeleteKey("CurrentHearts");
        PlayerPrefs.DeleteKey("IsStageChanged");
        PlayerPrefs.DeleteKey(CheatActiveKey);
    }

    public void ManualReset()
    {
        PlayerPrefs.DeleteKey("CurrentScore");
        PlayerPrefs.DeleteKey("CurrentHearts");
        PlayerPrefs.DeleteKey("IsStageChanged");
        PlayerPrefs.DeleteKey(CheatActiveKey);
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
