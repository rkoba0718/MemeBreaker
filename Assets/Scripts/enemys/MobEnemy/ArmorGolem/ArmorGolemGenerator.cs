using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ArmorGolemエネミーを生成するクラス
 */
public class ArmorGolemGenerator : MonoBehaviour
{
    public GameObject armorGolemPrefab;　//armorGolemPrefab
    GameController gameController; // gameController取得のための変数

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        StartCoroutine(Generate());
    }

    /*
     * 関数名:Generate
     * ArmorGolemEnemyを生成する関数
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
                yield return new WaitForSeconds(0.5f);
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
     * ArmorGolemエネミーの生成を定義
     * x = [-8.7f, 8.7f]でランダムに生成
     */
    void Spawn()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-8.15f, 8.15f), transform.position.y, transform.position.z);
        Instantiate(armorGolemPrefab, spawnPosition, transform.rotation);
    }
}
