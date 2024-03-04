using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *　自身が操作するPlayerの機能を持つクラス
 */
public class PlayerShip : MonoBehaviour
{
    public Transform weaponPoint; // 攻撃の発射位置
    public GameObject weaponPrefab; // 攻撃のレーザーPrefab
    public GameObject shipExplotionPrefab; // PlayerShipの爆発Prefab

    GameController gameController; // gameController取得のための変数
    public float xLimit = 8.15f; // x軸の範囲制限
    public float yLimit = 4.3f; // y軸の範囲制限

    public int weaponDamage = 10; // PlayerWeaponのダメージ
    public int life = 5; // PlayerのLife

    private int moveSpeed = 10; // PlayerShipの移動速度

    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Clamp();
        Attack();
    }

    /* 
     *　関数名:getWeaponDamage
     *　PlayerWeaponのダメージを返す関数
     *　
     */
    public int getWeaponDamage()
    {
        return weaponDamage;
    }

    /* 
     *　関数名:Move
     *　PlayerShipの移動方法を定義
     *　キーボードからの矢印入力を受け付け、入力方向に移動
     */
    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        transform.position += new Vector3(x, y, 0) * Time.deltaTime * moveSpeed;
    }

    /* 
     *　関数名:Clamp
     *　PlayerShipの移動範囲を定義
     *　画面外に移動するのを防ぐ
     */
    private void Clamp()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x = Mathf.Clamp(currentPosition.x, -xLimit, xLimit);
        currentPosition.y = Mathf.Clamp(currentPosition.y, -yLimit, yLimit);
        transform.position = currentPosition;
    }

    /*
     *　関数名:Attack
     *　PlayerShipの攻撃方法を定義
     *　spaceボタンが押された際に、レーザーを発射し、効果音を再生
     */
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(weaponPrefab, weaponPoint.position, transform.rotation);
            GetComponent<AudioSource>().Play();
        }
    }

    /*
     * 関数名:OnTriggerEnter2D
     * EnemyまたはEnemyWeaponとの衝突処理
     * PlayerShipを爆発し、ゲームオーバー画面を表示
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") == true ||
            collision.CompareTag("KinEnemy") == true ||
            collision.CompareTag("MemeEnemy") == true)
        {
            Instantiate(shipExplotionPrefab, transform.position, transform.rotation);
            gameController.ReduceLife();
            if (gameController.getLife() == 0)
            {
                gameController.GameOver();
                Destroy(gameObject);
                Destroy(collision.gameObject); 
            }
        }
        else if (
          collision.CompareTag("EnemyWeapon") == true ||
          collision.CompareTag("DeathblowWeapon") == true)
        {
            Instantiate(shipExplotionPrefab, transform.position, transform.rotation);
            gameController.ReduceLife();
            if (gameController.getLife() == 0)
            {
                gameController.GameOver();
                Destroy(gameObject);
            }
            Destroy(collision.gameObject);
        }
    }
}
