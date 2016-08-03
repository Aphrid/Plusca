using UnityEngine;
using System.Collections;


// Used by the level select screen to call the level check method
public class CallButtonCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(GameObject.Find("LevelTracker").GetComponent<LevelTracker>().OverlayFade());
	}    

}
