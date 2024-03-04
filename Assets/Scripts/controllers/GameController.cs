using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/*
 * Game表示画面の決定やスコア管理を行うクラス
 */
public class GameController : MonoBehaviour
{
    public GameObject operationText; // 操作方法のテキスト
    public GameObject gameOverText; // ゲームオーバーテキスト
    public TextMeshProUGUI scoreText; // スコアテキスト
    public TextMeshProUGUI lifeText; // スコアテキスト
    int score = 0; // 初期スコア
    int life;

    // Start is called before the first frame update
    void Start()
    {
        operationText.SetActive(true);
        gameOverText.SetActive(false);
        scoreText.text = "SCORE:" + score;
        life = GameObject.Find("PlayerShip").GetComponent<PlayerShip>().life;
        lifeText.text = "LIFE:" + life;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOverText.activeSelf == true && Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene("GameScene");
        }
    }

    /*
     * 関数名:getScore
     * 戻り値:int型
     * 現在のscoreの値を返す
     */
    public int getScore()
    {
        return score;
    }

    /* 
     * 関数名:AddScore
     * 引数:int
     * scoreに引数で受け取ったポイントを足す
     */
    public void AddScore(int point)
    {
        score += point;
        scoreText.text = "SCORE:" + score;
    }

    public int getLife()
    {
        return life;
    }

    public void ReduceLife()
    {
        if (life > 0)
        {
            life -= 1;
        }
        lifeText.text = "LIFE:" + life;
    }

    /*
     * 関数名:GameOver
     * ゲームオーバーのテキストを表示する
     * 
     */
    public void GameOver()
    {
        gameOverText.SetActive(true);
    }
}