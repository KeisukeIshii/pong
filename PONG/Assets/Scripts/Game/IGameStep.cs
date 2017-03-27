using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameStep
{
	void OnInitiarize ();
	void OnStart();
	void OnPlaying();
	void OnPause();
	void OnGameSet ();
	void OnAddCount ();
	void OnReStart();
	void StateMacine();
}