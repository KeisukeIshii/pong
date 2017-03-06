using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleCanvasController : MonoBehaviour {
	
	private Scene gameScene = SceneManager.GetSceneByName("Game");

	/// <summary>ボタンをクリックした際に呼ばれるイベント
	/// ゲームシーンをロードする
	/// </summary>
	private void OnClick()
	{
		if (gameScene != null) {
			SceneManager.LoadScene (gameScene.buildIndex);
		}
	}

}
