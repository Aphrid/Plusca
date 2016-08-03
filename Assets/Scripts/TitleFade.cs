using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Fades in the title text components after the given delay
// Automatically tweens alpha and loads the level select screen
public class TitleFade : MonoBehaviour {

    public Button[] buttons;
    public Image neutral; // level fade overlay
    public float fadeDelay;

    private Color origColor;

    void Start () {
        origColor = GetComponent<Text>().color;
        GetComponent<Text>().color = new Color(origColor.r, origColor.g, origColor.b, 0.01f);
        StartCoroutine(FadeIn(fadeDelay));
	}
	
	IEnumerator FadeIn(float fade)
    {
        yield return new WaitForSeconds(fade);
        GetComponent<Text>().CrossFadeAlpha(255, 0.5f, false);
        yield return new WaitForSeconds(0.5f);
        GetComponent<Text>().color = new Color(origColor.r, origColor.g, origColor.b, 1f);
    }

    // Set the animation trigger to animate text from R->L and load the level select scene
    public void LevelSelect(int sceneNum)
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        StartCoroutine(OverlayFade(sceneNum));
    }

    IEnumerator OverlayFade(int sceneNum)
    {
        neutral.GetComponent<RectTransform>().localScale = new Vector3(1.0f, 7.0f, 1.0f);
        while(neutral.color.a < 1.0f)
        {
            neutral.color = new Color(neutral.color.r, neutral.color.g, neutral.color.b, neutral.color.a + 0.15f);
            yield return new WaitForSeconds(0.03f);
        }
        SceneManager.LoadScene(sceneNum);
    }

    // Exit game button
    public void ExitGame()
    {
        Application.Quit();
    }
}
