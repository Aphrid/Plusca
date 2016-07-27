using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


/*
 *   Maintains the goal amount, timers for the game and sign, and current sign.
 *   To be added:
 *       - randomizing goal amount
 *       - shuffle button
 *       - score
 */
public class GameController : MonoBehaviour {

    // Constants that define the length of the game
    private static float TIME_BONUS_AMT = 2000.0f;
    private static float TOTAL_TIME = 10000f; //45000f;
    private static float SIGN_TIME = 20000f;

    private int goal; // sum, quotient, etc
    private System.Diagnostics.Stopwatch timer;
    private System.Diagnostics.Stopwatch signTimer;
    private float timeBonus; // gain bonus time on each correct answer, in milliseconds
    private float totalTimeFiller; // the radial clock animation
    private float signTimeFiller;
    private int curSignIndex; // which sign we're currently using
    private int score;
    private bool gameOver;

    public Image timerImg; // game clock timer
    public Image signTimerImg; // current sign timer
    public Image signImg;
    public Sprite[] signs;
    public Text scoreText;

    public Image[] gameOverImg; // elements to display during gameOver
    public Text[] gameOverText;

	void Start () {
        timer = new System.Diagnostics.Stopwatch();
        timer.Start();

        signTimer = new System.Diagnostics.Stopwatch();
        signTimer.Start();

        timeBonus = 0;
        gameOver = false;
        goal = 10;
        curSignIndex = 0;
        score = 0;
        scoreText.text = "Score: " + score.ToString().PadLeft(5, '0');
        Enable(false);
	}

    // Wipe the timerImg in a radial fashion
    void Update()
    {
        if (!gameOver)
        {
            scoreText.text = "Score: " + score.ToString().PadLeft(5, '0');

            totalTimeFiller = Mathf.Clamp01(1 - ((timer.ElapsedMilliseconds - timeBonus) / TOTAL_TIME));
            signTimeFiller = Mathf.Clamp01(1 - (signTimer.ElapsedMilliseconds / SIGN_TIME));

            timerImg.fillAmount = totalTimeFiller;
            signTimerImg.fillAmount = signTimeFiller;

            // Change signs and restart the timer
            if (signTimeFiller == 0)
            {
                ChangeSign();
            }
            else if (totalTimeFiller == 0)
            {
                gameOver = true;                
            }
        } else
        {
            Enable(true);
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

    }

    void Enable(bool en)
    {
        if (en)
        {
            for(int i = 0; i < gameOverImg.Length; i++)
            {
                gameOverImg[i].enabled = true;
            }

            for(int i = 0; i < gameOverText.Length; i++)
            {
                gameOverText[i].enabled = true;
            }
        } else
        {
            for (int i = 0; i < gameOverImg.Length; i++)
            {
                gameOverImg[i].enabled = false;
            }

            for (int i = 0; i < gameOverText.Length; i++)
            {
                gameOverText[i].enabled = false;
            }
        }
    }

    // Change the sign between + and -, including the image and timer
    void ChangeSign()
    {
        signTimer = new System.Diagnostics.Stopwatch();
        curSignIndex++;
        if (curSignIndex == 2)
        {
            curSignIndex = 0;
        }
        signImg.sprite = signs[curSignIndex];

        signTimer.Start();
        signTimeFiller = Mathf.Clamp01(1 - (signTimer.ElapsedMilliseconds / SIGN_TIME));
    }

    // Compare the user answer with the generated number and increment score
    public bool CheckGoal(int ans){
        if (ans == goal)
        {
            timeBonus += TIME_BONUS_AMT;
            score += 500;
            return true;
        }
        return false;
    }


    // 0 == +, 1 == -
    public int GetCurSignIndex()
    {
        return curSignIndex;
    }

    public bool GetGameOver()
    {
        return gameOver;
    }

}
