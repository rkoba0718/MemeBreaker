using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Meme2を生成するクラス
 */
public class Meme2Generator : MonoBehaviour
{
    public GameObject meme2Prefab;　//meme1Prefab
    GameController gameController; // gameController取得のための変数

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        StartCoroutine(Generate());
    }

    /*
     * 関数名:Generate
     * Meme2を生成する関数
     * scoreの値で生成を制御
     */
    private IEnumerator Generate()
    {
        while (true)
        {
            if (gameController.getScore() < 2000)
            {
                yield return null;
            }
            else if (gameController.getScore() >= 2000 && gameController.getScore() < 3800)
            {
                yield return new WaitForSeconds(5f);
                Spawn();
                break;
            }
            else
            {
                break;
            }
        }
    }

    /*
     * 関数名:Spawn
     * Meme2エネミーの生成を定義
     */
    void Spawn()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Instantiate(meme2Prefab, spawnPosition, transform.rotation);
    }
}
