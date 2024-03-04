using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Meme1を生成するクラス
 */
public class Meme1Generator : MonoBehaviour
{
    public GameObject meme1Prefab;　//meme1Prefab
    GameController gameController; // gameController取得のための変数

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        StartCoroutine(Generate());
    }

    /*
     * 関数名:Generate
     * Meme1を生成する関数
     * scoreの値で生成を制御
     */
    private IEnumerator Generate()
    {
        while (true)
        {
            if (gameController.getScore() < 160)
            {
                yield return null;
            }
            else if (gameController.getScore() >= 160 && gameController.getScore() < 1300)
            {
                yield return new WaitForSeconds(1f);
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
     * Meme1エネミーの生成を定義
     * 
     */
    void Spawn()
    {
        Vector3 spawnPosition = new Vector3(0, transform.position.y, transform.position.z);
        Instantiate(meme1Prefab, spawnPosition, transform.rotation);
    }
}
