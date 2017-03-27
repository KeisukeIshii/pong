using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boll : MonoBehaviour {

	public const float MOVE_SPEED = 100f;
	public const float MOVE_SPEED_OFFSET = 20f;
	private const string PREFAB_PATH = "Prefabs/Boll";
	private readonly Vector2 FIRST_POSITION = new Vector2(0.0f,20.0f);

	[SerializeField]
	private RectTransform rectTrans = null; 

	//移動速度
	private float deltaX = 0;
	private float deltaY = 0;

	/// <summary>
	/// 初期化
	/// </summary>
	public void Initialize()
	{
		rectTrans.anchoredPosition = FIRST_POSITION;
		//X方向には必ず動く必要あり
		deltaX = GetInitSpeed(MOVE_SPEED_OFFSET);
		//Y方向は関係なし
		deltaY = GetInitSpeed(MOVE_SPEED/2.0f);
	}

	/// <summary>
	/// ゲームコントローラ側で呼び出す更新関数
	/// </summary>
	/// <param name="deltaTime">Delta time.</param>
	public void OnUpdate (float deltaTime)
	{
		Vector2 position = new Vector2 (rectTrans.anchoredPosition.x + deltaX * deltaTime,
			                   rectTrans.anchoredPosition.y + deltaY * deltaTime);
		rectTrans.anchoredPosition = position;
	}

	public static Boll Create()
	{
		GameObject　instance = Instantiate(Resources.Load(PREFAB_PATH)) as GameObject;
		Boll boll = instance.GetComponent<Boll> ();
		return boll;
	}

	public void Delete()
	{
		Destroy (this.gameObject);
	}

	#region 移動関係

	/// <summary>
	/// 方向のしゅとく
	/// </summary>
	/// <returns>The dict.</returns>
	private float GetDict()
	{
		float dict = (float)(Random.Range (0, 2) == 0 ? -1 : 1);
		return dict;
	}

	/// <summary>
	/// 初期スピードの取得
	/// </summary>
	/// <returns>The init speed.</returns>
	/// <param name="offset">スピードの最小絶対値</param>
	private float GetInitSpeed(float offset = 0.0f)
	{
		return (offset + Random.Range (0.0f, MOVE_SPEED - offset))
			*GetDict();
	}

	/// <summary>
	/// 反射
	/// </summary>
	/// <param name="speed">Speed.</param>
	private float Refrect(float speed)
	{
		return speed * -1.0f;
	}

	#endregion

	#region あたり判定(Collider2Dから呼ばれる)

	void OnCollisionEnter2D(Collision2D coll)
	{
		//Debug.Log ("Collison");
		switch (coll.gameObject.tag) {
		case "Player":
			this.deltaX = this.Refrect (this.deltaX) * 1.05f;
			break;
		case "WallHorizontal":
			this.deltaY = this.Refrect (this.deltaY);
			break;
		case "DeadWall":
			coll.gameObject.GetComponent<DeadWall> ().AddCount ();
			break;
		}
	}

	#endregion

}
