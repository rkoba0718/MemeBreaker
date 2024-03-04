using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatEnemy : MonoBehaviour
{
    public RatWeapon weaponPrefab; // weaponのPrefab
    public GameObject ratExplotionPrefab; // Ratエネミーの爆発Prefab

    private float limit = 10.5f; //範囲制限

    private int point = 70; // 撃破ポイント

    GameController gameController; // gameController取得のための変数
    GameObject playerShip; // playerShip取得のための変数

    private float angle = 0; // サイクロイド移動に使用
    private Vector3 initPosition;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        playerShip = GameObject.Find("PlayerShip");
        initPosition = transform.position;

        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    /*
     * 関数名:Move
     * RatEnemyを移動させる処理
     * サイクロイドを描くように移動
     */
    private void Move()
    {
        transform.position = new Vector3(
                initPosition.x +  0.4f * (angle - Mathf.Sin(0.9f * angle)),
                initPosition.y + 0.4f * (1 - Mathf.Cos(0.9f * angle)),
                0
            );
        angle += Time.deltaTime * 8f;
        if (Mathf.Abs(transform.position.x) > limit)
        {
            Destroy(gameObject);
        }
    }

    /*
     * 関数名:Attack
     * RatEnemyの攻撃方法を定義
     * PlayerAimShotを一定時間おきに実行
     */
    private IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Shot(2 * Mathf.PI *  5/ 8, 3f);
            yield return new WaitForSeconds(2f);
        }
    }

    /*
     * 関数名:Shot
     * 引数：(float型, float型)
     * 与えられた角度と速度でWeaopnを発射する
     */
    private void Shot(float shotAngle, float speed)
    {
        RatWeapon weapon = Instantiate(weaponPrefab, transform.position, transform.rotation);
        weapon.Setting(shotAngle, speed);
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
            Instantiate(ratExplotionPrefab, transform.position, transform.rotation);
            gameController.AddScore(point);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
