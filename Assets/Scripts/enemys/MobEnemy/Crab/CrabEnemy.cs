using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Crabエネミーの定義をしているクラス
 */
public class CrabEnemy : MonoBehaviour
{
    public CrabWeapon weaponPrefab; // weaponのPrefab
    public GameObject crabExplotionPrefab; // Crabエネミーの爆発Prefab 
    private float limit = 10.5f; //範囲制限

    private int point = 50; // 撃破ポイント

    GameController gameController; // gameController取得のための変数
    GameObject playerShip; // playerShip取得のための変数

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        playerShip = GameObject.Find("PlayerShip");
        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(1.5f * Time.deltaTime, 0, 0);
        if (Mathf.Abs(transform.position.x) > limit)
        {
            Destroy(gameObject);
        }
    }

    /*
     * 関数名:Attack
     * CrabEnemyの攻撃方法を定義
     * PlayerAimShotを一定時間おきに実行
     */
    private IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            PlayerAimShot(2);
            yield return new WaitForSeconds(2.5f);
        }
    }

    /*
     * 関数名:PlayerAimShot
     * 引数：int型
     * PlayerShipを狙い、攻撃を行う関数
     * n個のWeaponをPlayerに向かって同時に発射する
     */
    private void PlayerAimShot(int n)
    {
        Vector3 diffPosition = playerShip != null
            ? playerShip.transform.position - transform.position
            : transform.position;
        float shotAngle = Mathf.Atan2(diffPosition.y, diffPosition.x);

        for (int i = 0; i < n; i++)
        {
            float offset = (i - n / 2f) * ((Mathf.PI / 2f) / n);
            Shot(shotAngle + offset, 3f);
        }
    }

    /*
     * 関数名:Shot
     * 引数：(float型, float型)
     * Meme1Weaponを発射する関数
     * 与えられた角度と速度で発射する
     */
    private void Shot(float shotAngle, float speed)
    {
        CrabWeapon weapon = Instantiate(weaponPrefab, transform.position, transform.rotation);
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
            Instantiate(crabExplotionPrefab, transform.position, transform.rotation);
            gameController.AddScore(point);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
