using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


// A hacky implementation of a singleton used for the button canvas and the level tracker
// Destroys all instances of the gameObject past the first, and hides it on other levels
public class DontDestroyOnLoad : MonoBehaviour {

    private int origScene;
    private Vector3 origScale;

    public void Awake()
    {
        int count = 0;
        DontDestroyOnLoad(this);

        foreach (GameObject gameObj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (gameObj.name.Equals(gameObject.name) && count == 0)
            {                
                count++;
            } else if (gameObj.name.Equals(gameObject.name) && count == 1)
            {
                Destroy(gameObj);
            }
        }
    }

    void Start()
    {
        origScene = SceneManager.GetActiveScene().buildIndex;
        if (GetComponent<RectTransform>() != null)
        {
            origScale = GetComponent<RectTransform>().localScale;
        }
    }

    // Compares the current scene with the original scene
    void Update()
    {
        if (!gameObject.name.Equals("LevelTracker") && SceneManager.GetActiveScene().buildIndex != origScene)
        {
            gameObject.GetComponent<Canvas>().enabled = false;
        } else if (!gameObject.name.Equals("LevelTracker") && SceneManager.GetActiveScene().buildIndex == origScene && GetComponent<Canvas>() != null)
        {
            gameObject.GetComponent<Canvas>().enabled = true;
        }
    }
}
