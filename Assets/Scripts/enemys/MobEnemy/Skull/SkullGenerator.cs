using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Skullエネミーを生成するクラス
 */
public class SkullGenerator : MonoBehaviour
{
    public GameObject skullPrefab;　//skullPrefab
    GameController gameController; // gameController取得のための変数

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        StartCoroutine(Generate());
    }

    /*
     * 関数名:Generate
     * SkullEnemyを生成する関数
     * scoreの値で生成を制御
     */
    private IEnumerator Generate()
    {
        while (true)
        {
            if (gameController.getScore() < 1650)
            {
                yield return null;
            }
            else if (gameController.getScore() >= 1650 && gameController.getScore() < 2000)
            {
                yield return new WaitForSeconds(2f);
                Spawn();
                yield return new WaitForSeconds(1f);
            }
            else if (gameController.getScore() >=3800  && gameController.getScore() < 4800)
            {
                yield return new WaitForSeconds(1f);
                Spawn();
                yield return new WaitForSeconds(1f);
            }
            else if (gameController.getScore() >= 4800 && gameController.getScore() < 5800)
            {
                yield return new WaitForSeconds(2f);
                Spawn();
                yield return new WaitForSeconds(2f);
            }
            else
            {
                break;
            }
        }
    }

    /*
     * 関数名:Spawn
     * skullエネミーの生成を定義
     * x = [-8.7f, 8.7f]でランダムに生成
     */
    void Spawn()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-8.15f, 8.15f), transform.position.y, transform.position.z);
        Instantiate(skullPrefab, spawnPosition, transform.rotation);
    }
}
