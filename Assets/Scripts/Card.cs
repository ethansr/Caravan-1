using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {
	float doubleClickStart = 0;
	bool rotating = false;
	bool toBack = true;
	public Sprite cardBack;
	public float flipDuration = 2.0f;
	public float flipScale = 1.2f;
	Sprite cardFront;

	void Start() {
		//assume we start with card front
		cardFront = GetComponent<SpriteRenderer>().sprite;
	}

	void OnMouseUp()
	{
		if ((Time.time - doubleClickStart) < 0.3f)
		{
			this.OnDoubleClick();
			doubleClickStart = -1;
		}
		else
		{
			doubleClickStart = Time.time;
		}
	}

	IEnumerator ChangeCardSprite(float delay) {
		yield return new WaitForSeconds(delay);
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		if (toBack) {
			sr.sprite = cardBack;
		} else {
			sr.sprite = cardFront;
		}
	}

	void UpdateRotating(float startTime) {
		float progress = Time.time - startTime;
		if (progress >= flipDuration/2.0f) {
			SpriteRenderer sr = GetComponent<SpriteRenderer>();
			if (toBack) {
				sr.sprite = cardBack;
			} else {
				sr.sprite = cardFront;
			}
		}
	}

	void FinishedRotating() {
		rotating = false;
	}
	
	void OnDoubleClick()
	{
		if (!rotating) {
			SpriteRenderer sr = GetComponent<SpriteRenderer>();
			toBack = sr.sprite == cardFront;
			rotating = true;
			float scaleTime = flipDuration/3.0f;
			iTween.ScaleBy(gameObject, iTween.Hash ("x",flipScale, "y",flipScale, "time",scaleTime));
			iTween.RotateTo(gameObject, iTween.Hash("y",toBack?180.0f:0.0f, "delay", scaleTime, "time", flipDuration, "easetype", "linear",
			                                        "oncomplete", "FinishedRotating"));//, "onupdate")); //, "UpdateRotating", "onupdateparams", Time.time));
			iTween.ScaleBy(gameObject, iTween.Hash ("x",1/flipScale, "y",1/flipScale, "time",scaleTime, "delay",flipDuration + scaleTime));
			StartCoroutine(ChangeCardSprite(flipDuration/2 + scaleTime));
		}
	}
}
