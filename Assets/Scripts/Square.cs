using UnityEngine;
using System.Collections;
using UnityEngine.UI;


/*
 *  Attaches a numeric value via UI text object to the square (includes tracking location of gameObject).
 *  To do:
 *      - change random number generation to be dependent on goal
 */
public class Square : MonoBehaviour {

    private GameObject spawner;
    private GameObject boardManager;
    private GameController gameController;
    private Camera cam;
    private Text valueText;
    private bool mouseOver;
    private bool selected;
    private int xPos, yPos;
    private int value;
    private int sign;
    private Sprite originalSprite;

    public Text valuePrefab;
    public Font valueFont;
    public Sprite selectedSprite;

    // Find the camera, and then create a new text component
    void Awake () {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        originalSprite = GetComponent<SpriteRenderer>().sprite;
        mouseOver = false;
        selected = false;
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        boardManager = GameObject.Find("BoardManager");

        valueText = Instantiate(valuePrefab, transform.position, transform.rotation) as Text;
        valueText.font = valueFont;
        valueText.rectTransform.SetParent(GameObject.Find("Canvas").GetComponent<RectTransform>());
        valueText.rectTransform.sizeDelta = new Vector2(160.0f, 125.0f);
        valueText.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Check the sign to generate sufficiently large numbers
        sign = gameController.GetCurSignIndex();
        SetValueRange();
        
        valueText.text = value.ToString();
    }

    void Update()
    {
        if (!gameController.GetGameOver())
        {
            if (gameController.GetCurSignIndex() != sign)
            {
                sign = gameController.GetCurSignIndex();
                SetValueRange();
            }

            if (mouseOver && Input.GetMouseButtonDown(0) && !selected)
            {
                GetComponent<AudioSource>().Play();
                selected = true;
                GetComponent<SpriteRenderer>().sprite = selectedSprite;
                boardManager.GetComponent<BoardManager>().SelectSq(xPos, yPos);
                Debug.Log("Value is: " + value.ToString());
            }
            else if (mouseOver && Input.GetMouseButtonDown(0) && selected)
            {
                selected = false;
                GetComponent<SpriteRenderer>().sprite = originalSprite;
                boardManager.GetComponent<BoardManager>().DeselectSq(xPos, yPos);
            }

            valueText.transform.position = cam.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y - 0.18f, transform.position.z));
        } else
        {
            valueText.text = "";
        }
    }

    public Text GetValueText()
    {
        return valueText;
    }

    public int GetValue()
    {
        return value;
    }

    public void SetPos(int x, int y)
    {
        xPos = x;
        yPos = y;
    }

    void SetValueRange()
    {
        if (sign == 0)
        {
            value = Mathf.RoundToInt(Random.Range(1.0f, 9.0f));
        }
        else
        {
            value = Mathf.RoundToInt(Random.Range(1.0f, 20.0f));
        }
    }

    // when mouse hovers over button
    void OnMouseOver()
    {
        mouseOver = true;
    }

    // when mouse stops hovering over button
    void OnMouseExit()
    {
        mouseOver = false;
    }

    public void Test()
    {
        Debug.Log("(" + xPos.ToString() + "," + yPos.ToString() + ") was checked");
    }
}
