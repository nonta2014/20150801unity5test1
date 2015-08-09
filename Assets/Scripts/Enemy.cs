using UnityEngine;
using System.Collections;

// public class Enemy : MonoBehaviour {
public class Enemy : Token {

	//生存数（ゲームマネージャに読ませる）
	public static int Count = 0;

	RectTransform rectTf;
	public static System.Action OnGameClearAction = null;

	// Use this for initialization
	void Start() {

		Count++;

		//GameRect取得テスト
		rectTf = GameObject.Find("GameRect").GetComponent<RectTransform>();
		Debug.Log("room size : " + rectTf.rect.width + "/" + rectTf.rect.height);
		Debug.Log("world size : " + GetWorldMin().x + "/" + GetWorldMin().y + "/" + GetWorldMax().x + "/" + GetWorldMax().y);
		//さて……画面外判定に使われているカメラは、例の2.4というマジックナンバーが来た。。
		// これを、GameRectの大きさ(800x600)の値で処理するにはどうしたらいいかな？
		// マジックナンバーを使えばなんてことはない。3.2:2.4なんだよな。

		// サイズを設定
		SetSize(SpriteWidth / 2, SpriteHeight / 2);
		// ランダムな方向に移動する
		// 方向をランダムに決める
		float dir = Random.Range(0, 359);
		// 速さは2
		float spd = 2;
		SetVelocity(dir, spd);
	}

	// Update is called once per frame
	void Update() {

		// カメラの左下・右上座標をカベとして取得
		// Vector2 min = GetWorldMin();
		// Vector2 max = GetWorldMax();

		//ゲームサイズを規定（カメラサイズに合わせたオブジェクト座標）
		//あと、GetWorldMin/Maxがやっていたように自分のサイズを考慮しています。
		Vector2 min = new Vector2(-3.2f + _width, -2.4f + _height);
		Vector2 max = new Vector2(3.2f - _width, 2.4f - _height);

		if (X < min.x || max.x < X)
		{
			// 画面外に出たので、X移動量を反転する
			VX *= -1;
			// 画面内に移動する
			ClampGameRect(min, max);
		}
		if (Y < min.y || max.y < Y)
		{
			// 画面外に出たので、Y移動量を反転する
			VY *= -1;
			// 画面内に移動する
			ClampGameRect(min, max);
		}

	}

	void ClampGameRect(Vector2 min, Vector2 max) {
		// ClampScreen(); return ; //元コード互換テスト。これはカメラサイズに依存していてGameRectを考慮していない。

		Vector2 pos = transform.position;
		// 画面内に収まるように制限をかける.
		pos.x = Mathf.Clamp(pos.x, min.x, max.x);
		pos.y = Mathf.Clamp(pos.y, min.y, max.y);

		// プレイヤーの座標を反映.
		transform.position = pos;
	}

	/// クリックされた
	public void OnMouseDown()
	{
		// パーティクルを生成
		for (int i = 0; i < 32; i++)
		{
			Particle.Add(X, Y);
		}
		// 破棄する
		DestroyObj();
		Count--;

		//クリア判定、アクション実行
		if (Count == 0) {
			// Debug.Log("クリアしました");
			if (OnGameClearAction != null) OnGameClearAction();
		}
	}
}
