using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Crabエネミーを生成するクラス
 */
public class CrabGenerator : MonoBehaviour
{
    public GameObject crabPrefab;　//crabPrefab
    GameController gameController; // gameController取得のための変数

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        StartCoroutine(Generate());
    }

    /*
     * 関数名:Generate
     * CrabEnemyを生成する関数
     * scoreの値で生成を制御
     */
    private IEnumerator Generate()
    {
        while (true)
        {
            if (gameController.getScore() < 1300)
            {
                yield return null;
            }
            else if (gameController.getScore() >= 1300 && gameController.getScore() < 2000)
            {
                yield return new WaitForSeconds(1f);
                Spawn();
                yield return new WaitForSeconds(1f);
            }
            else
            {
                break;
            }
        }
    }

    /*
     * 関数名:Spawn
     * crabエネミーの生成を定義
     * y = [-1.0f, 4.6f]でランダムに生成
     */
    void Spawn()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, Random.Range(-1.0f, 4.6f), transform.position.z);
        Instantiate(crabPrefab, spawnPosition, transform.rotation);
    }
}
