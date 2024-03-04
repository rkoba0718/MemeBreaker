using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Skullエネミーの定義をしているクラス
 */
public class SkullEnemy : MonoBehaviour
{
    public GameObject weaponPrefab; // weaponのPrefab
    public GameObject skullExplotionPrefab; // skullエネミーの爆発Prefab 
    private float limit = 6.5f; //範囲制限

    private int point = 70; // 撃破ポイント

    GameController gameController; // gameController取得のための変数

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0.01f * Mathf.Cos(Mathf.PI / 2f * Time.time), 1.5f * Time.deltaTime, 0);
        if (Mathf.Abs(transform.position.y) > limit)
        {
            Destroy(gameObject);
        }
    }

    /*
     * 関数名:Attack
     * skullEnemyの攻撃方法を定義
     * PlayerAimShotを一定時間おきに実行
     */
    private IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            Instantiate(weaponPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(0.15f);
            Instantiate(weaponPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(0.15f);
            Instantiate(weaponPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(2.5f);
        }
    }

    /*
     * 関数名:OnTriggerEnter2D
     * PlayerWeaponとの衝突処理
     * 爆発エフェクトを出し、スコアを加算
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerWeapon") == true)
        {
            Instantiate(skullExplotionPrefab, transform.position, transform.rotation);
            gameController.AddScore(point);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
