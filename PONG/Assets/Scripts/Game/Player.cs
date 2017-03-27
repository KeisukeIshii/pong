using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private const float MOVE_SPEED = 200.0f;
	private const float MAX_Y = 55.0f;
	private const float MIN_Y = -81.0f;

	[SerializeField]
	private KeyCode moveUp;
	[SerializeField]
	private KeyCode moveDown;
	[SerializeField]
	RectTransform rectTransform;

	public string playerName = "";
	public int count = 0;

	public void Initiarize(Vector2 position) {
		this.rectTransform.anchoredPosition = 
			new Vector2(position.x,position.y) ;
		this.count = 0;
	}

	private float GetMoveY()
	{
		float moveY = 0;
		if (Input.GetKey (this.moveUp))
		{
			moveY += MOVE_SPEED;
		}
		if (Input.GetKey (this.moveDown))
		{
			moveY -= MOVE_SPEED;
		}
		return moveY;
	}

	public void OnUpdate (float deltaTime)
	{
		Vector2 position = new Vector2 (rectTransform.anchoredPosition.x,
			rectTransform.anchoredPosition.y + GetMoveY() * deltaTime);
		rectTransform.anchoredPosition = position;
		LimitArea ();
	}
	
	private void LimitArea()
	{
		if (rectTransform.anchoredPosition.y > MAX_Y) {
			rectTransform.anchoredPosition = new Vector2 (
				rectTransform.anchoredPosition.x,
				MAX_Y);
		}
		if (rectTransform.anchoredPosition.y < MIN_Y) {
			rectTransform.anchoredPosition = new Vector2 (
				rectTransform.anchoredPosition.x,
				MIN_Y);
		}
	}
}
