using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Meme3を生成するクラス
 */
public class Meme3Generator : MonoBehaviour
{
    public GameObject meme3Prefab;　//meme1Prefab
    GameController gameController; // gameController取得のための変数

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        StartCoroutine(Generate());
    }

    /*
     * 関数名:Generate
     * Meme3を生成する関数
     * scoreの値で生成を制御
     */
    private IEnumerator Generate()
    {
        while (true)
        {
            if (gameController.getScore() < 6800)
            {
                yield return null;
            }
            else if (gameController.getScore() >= 6800 && gameController.getScore() < 10200)
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
     * Meme3エネミーの生成を定義
     */
    void Spawn()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Instantiate(meme3Prefab, spawnPosition, transform.rotation);
    }
}
