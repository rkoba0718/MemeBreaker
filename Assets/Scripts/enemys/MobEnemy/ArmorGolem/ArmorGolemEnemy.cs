using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ArmorGolemエネミーの定義をしているクラス
 */
public class ArmorGolemEnemy : MonoBehaviour
{
    public ArmorGolemWeapon weaponPrefab; // weaponのPrefab
    public GameObject armorGolemExplotionPrefab; // armorGolemエネミーの爆発Prefab 
    private float limit = 6.5f; //範囲制限

    private int point = 100; // 撃破ポイント

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
        transform.position -= new Vector3(0.02f * Mathf.Cos(Mathf.PI / 2f * Time.time), 1.5f * Time.deltaTime, 0);
        if (Mathf.Abs(transform.position.y) > limit)
        {
            Destroy(gameObject);
        }
    }

    /*
     * 関数名:Attack
     * armorGolemEnemyの攻撃方法を定義
     * PlayerAimShotを一定時間おきに実行
     */
    private IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Shot(2 * Mathf.PI * 3 / 4 - Mathf.PI / 6, 5f);
            Shot(2 * Mathf.PI * 3 / 4, 5f);
            Shot(2 * Mathf.PI * 3 / 4 + Mathf.PI / 6, 5f);
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
        ArmorGolemWeapon weapon = Instantiate(weaponPrefab, transform.position, transform.rotation);
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
            Instantiate(armorGolemExplotionPrefab, transform.position, transform.rotation);
            gameController.AddScore(point);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
