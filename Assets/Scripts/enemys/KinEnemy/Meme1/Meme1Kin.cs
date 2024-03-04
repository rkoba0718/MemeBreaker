using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Meme1の眷属の定義
 */
public class Meme1Kin : MonoBehaviour
{
    public Transform weaponPoint; // weaponの発射位置
    public Meme1KinWeapon weaponPrefab; // weaponのPrefab
    public GameObject  kinExplotionPrefab; // Golemエネミーの爆発Prefab
    public GameObject damagePrefab; // ダメージエフェクト用のPrefab
    PlayerShip playerShip; // playerShip取得のための変数
    GameController gameController; // gameController取得のための変数
    Meme1 meme1; // meme1取得のための変数

    private int point = 150; // 撃破ポイント
    private int hp = 30; // HP
    private float angle = 0; // 8の字移動に使用
    private bool invincibleFlag = false; // 無敵状態用フラグ
    private Vector3 initPosition;
    int currentHP;

    // Start is called before the first frame update
    void Start()
    {
        playerShip = GameObject.Find("PlayerShip") ?
            GameObject.Find("PlayerShip").GetComponent<PlayerShip>() :
            null; // KinEnemy生成前にPlayerShipが爆発した時に処理落ちするのを防ぐ
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        meme1 = GameObject.Find("Meme1(Clone)").GetComponent<Meme1>();
        currentHP = hp;
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
     * Meme1の眷属を8の字に移動させる処理
     * 
     */
    void Move()
    {
        transform.position = new Vector3(
                initPosition.x + 4f * Mathf.Sin(angle),
                initPosition.y + 0.25f * Mathf.Sin(angle * 2),
                0
            );
        angle += Time.deltaTime * 0.8f;
    }

    /*
     * 関数名:ReduceHP
     * HPを減少させる関数
     *
     */
    public void ReduceHP(int damage)
    {
        currentHP -= damage;
    }

    /*
     * 関数名:Attack
     * Meme1の眷属の攻撃方法を定義
     * コルーチンとして実行され、各秒数後にWaveShotNToMを実行
     */
    private IEnumerator Attack()
    {
        while (true)
        {
            yield return WaveShotNToM(3, 2, 4);
            yield return new WaitForSeconds(1.5f);
            yield return WaveShotNToM(4, 0, 6);
            yield return new WaitForSeconds(1f);
        }
    }

    /*
     * 関数名:WaveShotNToM
     * Meme1の眷属の攻撃処理
     * w回ShotNToMを実行
     */
    private IEnumerator WaveShotNToM(int w, int n, int m)
    {
        for (int i = 0; i < w; i++)
        {
            ShotNToM(n, m);
            yield return new WaitForSeconds(0.5f);
        }
    }

    /*
     * 関数名:ShotNToM
     * m-n+1個のWeaponを30度ずつずらして発射する関数
     * 
     */
    void ShotNToM(int n, int m)
    {
        for (int i = n; i <= m; i++)
        {
            float shotAngle = -i * Mathf.PI / 6f;
            Shot(shotAngle, 2f);
        }
    }

    /*
     * 関数名:Shot
     * WeaponをshotAngleの角度かつ、speedの速さで発射する関数
     * 
     */
    void Shot(float shotAngle, float speed)
    {
        Meme1KinWeapon weapon = Instantiate(weaponPrefab, weaponPoint.position, weaponPoint.rotation);
        weapon.Setting(shotAngle, speed);
    }

    /*
     * 関数名:DamageEffect
     * ダメージを受けた時の処理
     * 色を赤に変え、ダメージエフェクトを発生し、0.25f間無敵状態にする。その後、色を戻し、無敵状態を解除
     */
    private IEnumerator DamageEffect(GameObject collision)
    {
        invincibleFlag = true;

        gameObject.GetComponent<SpriteRenderer>().color = new Color32(185, 10, 10, 255);
        Instantiate(damagePrefab, collision.transform.position, collision.transform.rotation);
        Destroy(collision);

        yield return new WaitForSeconds(0.25f);

        gameObject.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);

        GameObject damageEffect = GameObject.Find("EnemyExplotion(Clone)");
        if (damageEffect != null)
        {
            Destroy(damageEffect);
        }

        invincibleFlag = false;
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
            if (invincibleFlag)
            {
                Destroy(collision.gameObject);
                return;
            }
            ReduceHP(playerShip.weaponDamage);
            if (currentHP > 0)
            {
                StartCoroutine(DamageEffect(collision.gameObject));
            }
            else
            {
                Instantiate(kinExplotionPrefab, transform.position, transform.rotation);
                gameController.AddScore(point);
                meme1.AddDestoryCounter();
                Destroy(gameObject);
                Destroy(collision.gameObject);
            }
        }
    }
}
