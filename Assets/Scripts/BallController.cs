using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BallController : MonoBehaviour
{
    // Ball movement settings
    [Header("Ball movement")]
    public float power = 35f;
    public float maxSpeed = 30f;
    private Rigidbody2D rb;
    private bool isDragging = false;
    private Vector2 startTouchPos;
    private bool canCollide = true;

    // Game settings
    [Header("Game settings")]
    public TMP_Text scoreText;
    public TMP_Text heartsText;
    public int maxHearts = 3;

    // Sound settings
    [Header("Sound settings")]
    public AudioSource audioSource;
    public AudioClip ballHitSound;
    public AudioClip groundHitSound;
    private int currentScore = 0;
    private int currentHearts;
    private bool canMove = true;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHearts = maxHearts;
        UpdateUI();

        // Create an AudioSource if AudioSource is not set
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    void Update()
    {
        if (!canMove) return;

        HandleTouch();
        LimitVelocity();
    }

    void HandleTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startTouchPos = GetTouchPosition();
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            Vector2 endTouchPos = GetTouchPosition();
            Vector2 swipeDirection = (endTouchPos - startTouchPos).normalized;

            rb.AddForce(swipeDirection * power, ForceMode2D.Impulse);
            AddScore(1);

            // Play sound when hitting the ball
            PlaySound(ballHitSound);

            isDragging = false;
        }
    }

    Vector2 GetTouchPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void LimitVelocity()
    {
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && canCollide)
        {
            canCollide = false;
            LoseHeart();

            // Play sound when hitting the ground
            PlaySound(groundHitSound);

            Invoke("ResetCollision", 0.5f);
        }
    }

    void ResetCollision()
    {
        canCollide = true;
    }

    void LoseHeart()
    {
        currentHearts--;
        UpdateUI();

        if (currentHearts <= 0)
        {
            GameOver();
        }
    }

    void AddScore(int points)
    {
        currentScore += points;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = Mathf.Max(0, currentScore).ToString();

        if (heartsText != null)
            heartsText.text = Mathf.Max(0, currentHearts).ToString();
    }

    void GameOver()
    {
        canMove = false;
        Debug.Log("Game over! Final score: " + currentScore);
    }

    // Method for playing sound
    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}