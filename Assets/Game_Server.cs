using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class ColorData
{
    public string ColorName;
    public Color ColorValue;
}

public class Game_Server : MonoBehaviour
{
    public List<ColorData> AvailableColors;
    public TextMeshProUGUI TargetColorNameDisplay;
    public Image[] ColorButtons;

    private int targetColorIndex;
    private int playerScore;
    private float timeRemaining;
    private bool isGameRunning;
    private int correctColorIndex;

    [Header("UI Elements")]
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TimeText;
    public GameObject WinScreen;
    public GameObject LoseScreen;

    [Header("Audio")]
    public AudioSource CorrectSound;
    public AudioSource WrongSound; 
    public AudioSource Win_Sound;
    public AudioSource Lost_Sound;


    [Header("Settings")]
    public float roundDuration = 5f;
    public float gameStartDelay = 3f;

    private float elapsedTime;

    void Start()
    {
        InitializeGame();
        ScoreText.text = "0";
    }

    private void InitializeGame()
    {
        playerScore = 0;
        StartCoroutine(StartGameAfterDelay(gameStartDelay));
    }

    private IEnumerator StartGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetupColorButtons();
        isGameRunning = true;
        timeRemaining = roundDuration;
    }

    private List<int> usedIndices = new List<int>();
    private void SetupColorButtons()
    {
        for (int i = 0; i < ColorButtons.Length; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = UnityEngine.Random.Range(0, AvailableColors.Count);
            } while (usedIndices.Contains(randomIndex));

            usedIndices.Add(randomIndex);

            ColorButtons[i].color = AvailableColors[randomIndex].ColorValue;
            ColorButtons[i].GetComponent<Item_Controler>().AssignedColorName = AvailableColors[randomIndex].ColorName;
        }

        targetColorIndex = UnityEngine.Random.Range(0, usedIndices.Count);

        TargetColorNameDisplay.text = "Choose a cookie which is: "+  AvailableColors[usedIndices[targetColorIndex]].ColorName;

        correctColorIndex = usedIndices[targetColorIndex];
        usedIndices.Clear();
    }

    void Update()
    {
        if (isGameRunning)
        {
            elapsedTime += Time.deltaTime;
            timeRemaining -= Time.deltaTime;
            TimeText.text = timeRemaining.ToString("N2");

            if (timeRemaining <= 0)
            {
                OnTimeOut();
            }
        }
    }

    public void VerifyColorChoice(string selectedColor)
    {
        if (selectedColor == AvailableColors[correctColorIndex].ColorName)
        {
            HandleCorrectChoice();
        }
        else
        {
            HandleIncorrectChoice();
        }
    }

    private void HandleCorrectChoice()
    {
        CorrectSound.Play();
        UpdateScore(5);
        ResetRound();
    }

    private void HandleIncorrectChoice()
    {
        WrongSound.Play();
        UpdateScore(-5);
        ResetRound();
    }

    private void OnTimeOut()
    {
        LoseScreen.SetActive(true);
        isGameRunning = false;
    }

    private void UpdateScore(int points)
    {
        playerScore += points;
        ScoreText.text = playerScore.ToString() ;

        if (playerScore >= 100)
        {
            WinScreen.SetActive(true);
            Win_Sound.Play();
             isGameRunning = false;
        }
        else if (playerScore <= 0)
        {
            playerScore = 0;
            LoseScreen.SetActive(true);
            Lost_Sound.Play();
            isGameRunning = false;
        }
    }

    private void ResetRound()
    {
        if (isGameRunning)
        {
            timeRemaining = roundDuration;
            SetupColorButtons();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
