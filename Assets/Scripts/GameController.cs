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
 *       - private static ints for score / combo amounts + better condition text update
 */
public class GameController : MonoBehaviour {

    // Constants that define the length of the game
    private static float TIME_BONUS_AMT = 2000.0f;
    private static float TOTAL_TIME = 20000f;
    private static float SIGN_TIME = 20000f;

    private int goal; // sum, quotient, etc
    private System.Diagnostics.Stopwatch timer;
    private System.Diagnostics.Stopwatch signTimer;
    private float timeBonus; // gain bonus time on each correct answer, in milliseconds
    private float totalTimeFiller; // the radial clock animation
    private float signTimeFiller;
    private int curSignIndex; // which sign we're currently using
    private bool gameOver;
    private bool gameWin; // condition cleared
    private bool conditionWait; // waiting for user to enter
    private bool showCombo;

    // Set of variables that track the win condition
    private int combo;
    private int maxCombo;
    private int score;
    private int correctAmt;

    public Image timerImg; // game clock timer
    public Image signTimerImg; // current sign timer
    public Image signImg;
    public Sprite[] signs;
    public Text scoreText;

    public Canvas gameWinCanvas;
    public Canvas gameOverCanvas; // elements to display during gameOver
    public Canvas conditionCanvas;
    public Text conditionText;
    public AudioSource goodSound;

	void Start () {
        timer = new System.Diagnostics.Stopwatch();
        signTimer = new System.Diagnostics.Stopwatch();

        timeBonus = 0;
        gameOver = false;
        gameWin = false;
        showCombo = false;
        conditionWait = true;
        goal = 10;
        curSignIndex = 0;
        maxCombo = 0;
        combo = 0;
        correctAmt = 0;
        score = 0;
        scoreText.text = "Score: " + score.ToString().PadLeft(5, '0');
        Enable(false);
        DetermineConditionText();
	}


    // Wipe the timerImg in a radial fashion
    void Update()
    {
        if (conditionWait)
        {
            conditionCanvas.enabled = true;
            if (Input.GetKeyDown(KeyCode.Space))
            {                
                timer.Start();
                signTimer.Start();

                conditionWait = false;
                conditionCanvas.enabled = false;
            }
        } else
        {
            if (!gameOver & !gameWin)
            {
                if (showCombo)
                {
                    scoreText.fontSize = 150;
                    scoreText.text = "Max Combo: " + maxCombo.ToString() + " | Current: " + combo.ToString();
                } else
                {
                    scoreText.fontSize = 200;
                    scoreText.text = "Score: " + score.ToString().PadLeft(5, '0');
                }
                
                totalTimeFiller = Mathf.Clamp01(1 - ((timer.ElapsedMilliseconds - timeBonus) / TOTAL_TIME));
                signTimeFiller = Mathf.Clamp01(1 - (signTimer.ElapsedMilliseconds / SIGN_TIME));

                timerImg.fillAmount = totalTimeFiller;
                signTimerImg.fillAmount = signTimeFiller;

                // Change signs and restart the timer
                if (signTimeFiller == 0)
                {
                    ChangeSign();
                }

                // check every frame to see if game is over
                CheckWinCondition();
            }
            else
            {
                Enable(true);
                if (Input.GetKeyDown(KeyCode.R))
                {
                    SceneManager.LoadScene(1);
                }
            }
        }
    }

    // Hide / show game over/win UI elements
    void Enable(bool en)
    {
        if (en && gameOver)
        {
            gameOverCanvas.enabled = true;
        } else if (en && gameWin)
        {
            gameWinCanvas.enabled = true;
        } else if (!gameOver && !gameWin)
        {
            gameOverCanvas.enabled = false;
            gameWinCanvas.enabled = false;
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
            goodSound.Play();
            combo++;
            if (combo > maxCombo)
            {
                maxCombo = combo;
            }
            return true;
        } else
        {
            combo = 0;
        }
        return false;
    }

    /*  Compares the current scene number with the goal to determine game over / win
     *  Changes the boolean values gameOver and gameWin as necessary
     *  Conditions are (before time runs out):
     *      1) Certain combo
     *      2) Achieve score
     *      3) x amount of correct guesses before the sign switches
     *      
     *  Level 0 -> score
     *  Level 1 -> combo
     *  Level 2 -> ... to implement later
     */
    void CheckWinCondition()
    {
        if (totalTimeFiller == 0)
        {
            gameOver = true;
        } else
        {
            switch (SceneManager.GetActiveScene().buildIndex)
            {                
                case 2:
                    if(score == 3000)
                    {                        
                        gameWin = true;
                    }
                    break;
                case 3:
                    showCombo = true;
                    if(maxCombo == 10)
                    {
                        gameWin = true;
                    }
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
            }
        }

        if (gameWin)
        {
            GameObject.Find("LevelTracker").GetComponent<LevelTracker>().RemoveLocked(SceneManager.GetActiveScene().buildIndex - 1 );
        }

    }


    // Correctly updates the condition text 
    void DetermineConditionText()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 2:
                conditionText.text = "Get 3000 points to pass";
                break;
            case 3:
                conditionText.text = "Get 10 combo to pass";
                break;
        }
    }

    // 0 == +, 1 == -
    public int GetCurSignIndex()
    {
        return curSignIndex;
    }

    public bool GetGameWin()
    {
        return gameWin;
    }

    public bool GetGameOver()
    {
        return gameOver;
    }

    public bool GetConditionWait()
    {
        return conditionWait;
    }

}
