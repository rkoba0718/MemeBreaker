using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Golemエネミーを生成するクラス
 */
public class GolemGenerator : MonoBehaviour
{
    public GameObject golemPrefab;　//golemPrefab
    GameController gameController; // gameController取得のための変数

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        StartCoroutine(Generate());
    }

    void Update()
    {

    }

    /*
     * 関数名:Generate
     * GolemEnemyを生成する関数
     * scoreの値で生成を制御
     */
    private IEnumerator Generate()
    {
        while (true)
        {
            if (gameController.getScore() < 160)
            {
                Spawn();
                yield return new WaitForSeconds(1.5f);
            }
            else if (gameController.getScore() >= 160 && gameController.getScore() < 1300)
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
     * Golemエネミーの生成を定義
     * x = [-8.7f, 8.7f]でランダムに生成
     */
    void Spawn()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-8.15f, 8.15f), transform.position.y, transform.position.z);

        Instantiate(golemPrefab, spawnPosition, transform.rotation);
    }
}
