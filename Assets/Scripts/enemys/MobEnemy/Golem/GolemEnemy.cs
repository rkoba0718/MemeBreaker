using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Golemエネミーの定義をしているクラス
 */
public class GolemEnemy : MonoBehaviour
{
    public float limit = -6.5f; //範囲制限
    public GameObject golemExplotionPrefab; // Golemエネミーの爆発Prefab 
    public GameObject weaponPrefab; // weaponのPrefab

    GameController gameController; // gameController取得のための変数
    private int point = 20; // 撃破ポイント

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        InvokeRepeating("Attack", 0.5f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, 2 * Time.deltaTime, 0);
        if (Mathf.Abs(transform.position.y) > limit)
        {
            Destroy(gameObject);
        }
    }

    /*
     * 関数名:Attack
     * GolemEnemyの攻撃処理
     * 
     */
    void Attack()
    {
        Instantiate(weaponPrefab, transform.position, transform.rotation);
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
            Instantiate(golemExplotionPrefab, transform.position, transform.rotation);
            gameController.AddScore(point);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
