using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameCanvasController : MonoBehaviour,IGameStep
{
	private const int WIN_POINT = 11;
	private const float GAME_MAX_TIME = 60.0f;
	private const string PAUSE_TEXT = "STOP";
	private const string DRAW_TEXT = "DRAW"; 
	private const string RESULT_TEXT_FORMAT = "{0}\nWINNER!"; 
	private const string TIME_TEXT_FORMAT = "{0}:{1}"; 

	private readonly Vector2 FIRST_PLAYER1_POSITION = 
		new Vector2(-195.0f,-10.0f);
	private readonly Vector2 FIRST_PLAYER2_POSITION = 
		new Vector2(195.0f,-10.0f);

	private enum Step
	{
		Wait,
		Init,
		Start,
		Playing,
		Pause,
		GameSet,
		AddCount,
		ReStart
	};

	[SerializeField]
	Player player1;
	[SerializeField]
	Player player2;
	[SerializeField]
	Text time;
	[SerializeField]
	Text player1Count;
	[SerializeField]
	Text player2Count;
	[SerializeField]
	Text statusText;

	private Boll boll = null;
	private Step step = Step.Init;
	private bool canPause;
	private bool doNextStep;
	private float gameTime = 0.0f;
	private bool isPause = false;
	void Start ()
	{
		//step = Step.Init;
		this.gameTime = 0.0f;
	}
	
	void Update ()
	{
		StateMacine ();
		TimeUpdate ();
	}

	/// <summary>
	/// 制限時間の更新
	/// </summary>
	void TimeUpdate()
	{
		gameTime -= Time.deltaTime;
		if (gameTime <= 0.0f) {
			this.step = Step.GameSet;
		}
	}

	#region Step関数

	/// <summary>
	/// 初期化
	/// </summary>
	public void OnInitiarize ()
	{
		if (this.boll != null) {
			this.boll.Delete ();
			this.boll = null;
		}
		this.gameTime = 0.0f;
		this.player1.Initiarize (FIRST_PLAYER1_POSITION);
		this.player2.Initiarize (FIRST_PLAYER2_POSITION);
		this.boll = Boll.Create ();
		this.boll.gameObject.transform.SetParent (this.transform);
		this.boll.gameObject.transform.localScale = Vector3.one;
		this.boll.Initialize ();

		this.isPause = false;
		this.canPause = false;
		this.doNextStep = true;
		this.step = Step.Start;
		this.gameTime = GAME_MAX_TIME;
		this.statusText.gameObject.transform.parent.gameObject.SetActive (false);
	}

	public void OnStart()
	{
		this.canPause = true;
		this.step = Step.Playing;
	}

	public void OnPlaying ()
	{
		this.doNextStep = false;
		this.boll.OnUpdate (Time.deltaTime);
		this.player1.OnUpdate (Time.deltaTime);
		this.player2.OnUpdate (Time.deltaTime);
		this.player1Count.text = this.player1.count.ToString();
		this.player2Count.text = this.player2.count.ToString();

		int timeSec = (int)this.gameTime;
		float decimalTime = this.gameTime - (float)timeSec;
		this.time.text = string.Format(TIME_TEXT_FORMAT,
			timeSec,
			decimalTime.ToString().PadLeft(4,'0').Substring(2,2));
	}

	public void OnPause()
	{
		this.doNextStep = false;
		this.step = Step.Wait;
		this.isPause = true;
		this.statusText.text = PAUSE_TEXT;
		this.statusText.gameObject.transform.parent.gameObject.SetActive (true);
	}

	public void OnGameSet ()
	{
		this.step = Step.Wait;
		this.doNextStep = false;
		this.canPause = false;
		Player winner = GetWinner ();
		if (winner == null) {
			this.statusText.text = DRAW_TEXT;
		} else {
			this.statusText.text = 
				string.Format(RESULT_TEXT_FORMAT,winner.playerName);
		}
		this.statusText.gameObject.transform.parent.gameObject.SetActive (true);
	}

	public void OnAddCount ()
	{
		boll.Initialize ();
		if (IsPointMax ()) {
			this.step = Step.GameSet;
		} else {
			this.step = Step.Start;
		}
	}

	/// <summary>
	/// ポーズ画面から再開
	/// </summary>
	public void OnReStart()
	{
		this.statusText.gameObject.transform.parent.gameObject.SetActive (false);
		this.step = Step.Playing;
		this.isPause = false;
	}

	/// <summary>
	/// 次のステップに進まないフラグが立つまでステップを更新し続ける
	/// </summary>
	public void StateMacine()
	{
		//一回はステートマシンを呼ぶ
		do
		{
			switch (this.step) {
			case Step.Wait : break;
			case Step.Init : OnInitiarize(); break;
			case Step.Start : OnStart(); break;
			case Step.Playing : OnPlaying(); break;
			case Step.Pause : OnPause(); break;
			case Step.GameSet : OnGameSet(); break;
			case Step.AddCount : OnAddCount(); break;
			case Step.ReStart : OnReStart(); break;
			}
		}while(doNextStep);
	}


	#endregion

	#region Click

	public void OnClickTop()
	{
		SceneManager.LoadScene ("Title");
	}

	public void OnClickReset()
	{
		this.OnInitiarize ();
	}

	public void OnClickStop()
	{
		if (canPause) {
			if (isPause) {
				OnReStart ();
			} else {
				OnPause ();
			}
		}
	}

	#endregion

	/// <summary>
	/// 勝利プレイヤーの取得
	/// </summary>
	/// <returns>The winner.</returns>
	private Player GetWinner() {
		if(this.player1.count == this.player2.count)
		{
			return null;
		}
		return (this.player1.count > this.player2.count) ? player1 : player2;
	}

	private bool IsPointMax()
	{
		return (player1.count >= WIN_POINT || player2.count >= WIN_POINT);
	}
}
