using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Batエネミーを生成するクラス
 */
public class BatGenerator : MonoBehaviour
{
    public GameObject batPrefab;　//batPrefab
    GameController gameController; // gameController取得のための変数

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        StartCoroutine(Generate());
    }

    /*
     * 関数名:Generate
     * BatEnemyを生成する関数
     * scoreの値で生成を制御
     */
    private IEnumerator Generate()
    {
        while (true)
        {
            if (gameController.getScore() < 4800)
            {
                yield return null;
            }
            else if (gameController.getScore() >= 4800 && gameController.getScore() < 6800)
            {
                yield return new WaitForSeconds(3f);
                Spawn();
                yield return new WaitForSeconds(4f);
            }
            else
            {
                break;
            }
        }
    }

    /*
     * 関数名:Spawn
     * Batエネミーの生成を定義
     * y = [0f, 3f]でランダムに生成
     */
    void Spawn()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, Random.Range(0f, 3f), transform.position.z);
        Instantiate(batPrefab, spawnPosition, transform.rotation);
    }
}
