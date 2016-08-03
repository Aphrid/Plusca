using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Manages which levels are loaded, in addition to the corresponding buttons
// Persists between scene loading
public class LevelTracker : MonoBehaviour {
    private static int MAX_LEVELS = 6; // starts at 0
    private static Color LOCKED_COLOR = new Color(1.0f, 0.65f, 0.65f);
    private static Color DEFAULT_COLOR = new Color(0.082f, 0.827f, 1.0f);    

    public Button[] buttons;
    public Image neutral;
    private List<int> lockedLevels;
    private List<int> availableLevels;


    // Use this for initialization
    void Awake () {
        DontDestroyOnLoad(transform.gameObject);
        lockedLevels = new List<int>();
        availableLevels = new List<int>();
        neutral.color = new Color(neutral.color.r, neutral.color.g, neutral.color.b, 1f);

        // by default level "0" the tutorial and level "1" are unlocked
        for (int i = 1; i < MAX_LEVELS; i++)
        {
            lockedLevels.Add(i);
        }
        availableLevels.Add(0);
        //availableLevels.Add(1);                
	}


    // Removes level from the list of locked levels and unlocks it
    public void RemoveLocked(int level)
    {
        Debug.Log("Removing "+level.ToString());
        availableLevels.Add(level);
        lockedLevels.Remove(level);
    }

    // Checks unlocked levels, tints and activates the correct buttons
    public void CheckButtons()
    {
        ColorBlock cb;
        for (int i = 0; i < lockedLevels.Count; i++)
        {
            // Change button interaction colors to match disabled colors
            cb = buttons[lockedLevels[i]].GetComponent<Button>().colors;
            cb.highlightedColor = LOCKED_COLOR;
            cb.normalColor = LOCKED_COLOR;
            cb.disabledColor = LOCKED_COLOR;
            cb.pressedColor = LOCKED_COLOR;
            buttons[lockedLevels[i]].GetComponent<Button>().colors = cb;

            buttons[lockedLevels[i]].interactable = false;
            buttons[lockedLevels[i]].GetComponent<Image>().CrossFadeColor(LOCKED_COLOR, 1.0f, false, false);            
        }

        for (int i = 0; i < availableLevels.Count; i++)
        {
            buttons[availableLevels[i]].GetComponent<Button>().colors = buttons[0].GetComponent<Button>().colors;
            buttons[availableLevels[i]].interactable = true;
            buttons[availableLevels[i]].GetComponent<Image>().CrossFadeColor(DEFAULT_COLOR, 1.0f, false, false);
        }
    }

    //Creates a fading effect on scene loading
    public IEnumerator OverlayFade()
    {
        neutral.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 7.0f, 1.0f);
        while (neutral.color.a > 0.01f)
        {
            neutral.color = new Color(neutral.color.r, neutral.color.g, neutral.color.b, neutral.color.a - 0.1f);
            yield return new WaitForSeconds(0.03f);
        }
        neutral.GetComponent<RectTransform>().localScale = Vector3.zero;
        CheckButtons();
    }

    // Loads the correct level
    public void LoadLevel(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

}
