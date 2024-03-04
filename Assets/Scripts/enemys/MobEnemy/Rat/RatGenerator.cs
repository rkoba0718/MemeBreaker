using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Ratエネミーを生成するクラス
 */
public class RatGenerator : MonoBehaviour
{
    public GameObject ratPrefab;　//ratPrefab
    GameController gameController; // gameController取得のための変数

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        StartCoroutine(Generate());
    }

    /*
     * 関数名:Generate
     * RatEnemyを生成する関数
     * scoreの値で生成を制御
     */
    private IEnumerator Generate()
    {
        while (true)
        {
            if (gameController.getScore() <  3800)
            {
                yield return null;
            }
            else if (gameController.getScore() >= 3800 && gameController.getScore() < 4800)
            {
                yield return new WaitForSeconds(1f);
                Spawn();
                yield return new WaitForSeconds(1.5f);
            }
            else
            {
                break;
            }
        }
    }

    /*
     * 関数名:Spawn
     * Ratエネミーの生成を定義
     * y = [0f, 4.6f]でランダムに生成
     */
    void Spawn()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, Random.Range(0, 4.6f), transform.position.z);
        Instantiate(ratPrefab, spawnPosition, transform.rotation);
    }
}
