using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	void Start() {
		//クリアUIは隠す
		GameObject.Find("UICanvas").transform.Find("GameClear").gameObject.SetActive(false);

		//クリア時の挙動をEnemy側に登録
		Enemy.OnGameClearAction = () => {
			// Debug.Log("クリアしました（Action経由）");
			GameObject.Find("UICanvas").transform.Find("GameClear").gameObject.SetActive(true);
		};
	}

	//UGUIからコール
	public void ToTitleScene() {
		Application.LoadLevel("Title");
	}
}
