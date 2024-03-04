using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meme2Kin : MonoBehaviour
{
    public Transform weaponPoint; // weaponの発射位置
    public Meme2KinWeapon weaponPrefab; // weaponのPrefab
    public GameObject kinExplotionPrefab; // Golemエネミーの爆発Prefab
    public GameObject damagePrefab; // ダメージエフェクト用のPrefab
    PlayerShip playerShip; // playerShip取得のための変数
    GameController gameController; // gameController取得のための変数
    Meme2 meme2; // meme2取得のための変数

    private int point = 200; // 撃破ポイント
    private int hp = 50; // HP
    private float asteroidAngle = 0; // アステロイド移動に使用
    private float lissajousAngle = 0; // リサジュー移動に使用
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
        meme2 = GameObject.Find("Meme2(Clone)").GetComponent<Meme2>();
        currentHP = hp;
        initPosition = transform.position;

        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        if (meme2.kinDestroyCounter < 2)
        {
            MoveAsteroid();
        } else
        {
            MoveLissajous();
        }
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
     * 関数名:MoveAsteroid
     * Meme2の眷属をアステロイド曲線で移動させる関数
     * 
     */
    void MoveAsteroid()
    {
        transform.position = new Vector3(
                initPosition.x + 2.8f * Mathf.Pow(Mathf.Cos(asteroidAngle * 1.5f), 3),
                initPosition.y + 1.8f * Mathf.Pow(Mathf.Sin(asteroidAngle * 1.5f), 3),
                0
            );
        asteroidAngle += Time.deltaTime * 0.65f;
    }

    /*
     * 関数名:MoveAsteroid
     * Meme2の眷属をリサジュー曲線で移動させる関数
     * 
     */
    void MoveLissajous()
    {
        transform.position = new Vector3(
                initPosition.x + 2.8f * Mathf.Sin(2f * lissajousAngle),
                initPosition.y + 2f * Mathf.Sin(3f * lissajousAngle),
                0
            );
        lissajousAngle += Time.deltaTime * 0.7f;
    }

    /*
     * 関数名:Attack
     * Meme2の眷属の攻撃方法を定義
     */
    private IEnumerator Attack()
    {
        while (true)
        {
            if (meme2.kinDestroyCounter < 2)
            {
                int random = Random.Range(3, 8);
                yield return WavePlayerAimShotN(random, 3);
                yield return WaveShot(3, 8);
                yield return new WaitForSeconds(3f);
            } else
            {
                int wave = Random.Range(5, 10);
                int n = Random.Range(2, 7);
                yield return WavePlayerAimShotN(wave, n);
                yield return new WaitForSeconds(1f);
                yield return WaveShot(wave, 12);
                float interval = Random.Range(0.1f, 1f);
                yield return new WaitForSeconds(interval);
            }
        }
    }

    /*
     * 関数名:WavePlayerAimShotN
     * 
     */
    private IEnumerator WavePlayerAimShotN(int w, int n)
    {
        for (int i = 0; i < w; i++)
        {
            yield return PlayerAimShot(n);
            yield return new WaitForSeconds(0.5f);
        }
    }

    /*
     * 関数名:PlayerAimShot
     * 引数：int型
     * PlayerShipを狙い、攻撃を行う関数
     */
    private IEnumerator PlayerAimShot(int n)
    {
        for (int i = 0; i < n; i++)
        {
            Vector3 diffPosition = playerShip != null
            ? playerShip.transform.position - transform.position
            : transform.position;
            float shotAngle = Mathf.Atan2(diffPosition.y, diffPosition.x);
            Shot(shotAngle, 5f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    /*
     * 関数名:WaveShot
     * Meme2の眷属の攻撃処理
     * w回ShotNを実行
     */
    private IEnumerator WaveShot(int w, int n)
    {
        for (int i = 0; i < w; i++)
        {
            ShotN(n);
            yield return new WaitForSeconds(1f);
        }
    }

    /*
     * 関数名:ShotN
     * n個のWeaponをPI/n度ずつずらして発射する関数
     * 
     */
    void ShotN(int n)
    {
        for (int i = 0; i <= n; i++)
        {
            float shotAngle = i * Mathf.PI / n;
            Shot(shotAngle, 1f);
            Shot(-shotAngle, 1f);
        }
    }

    /*
     * 関数名:Shot
     * WeaponをshotAngleの角度かつ、speedの速さで発射する関数
     * 
     */
    private void Shot(float shotAngle, float speed)
    {
        Meme2KinWeapon weapon = Instantiate(weaponPrefab, weaponPoint.position, weaponPoint.rotation);
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
                meme2.AddDestoryCounter();
                Destroy(gameObject);
                Destroy(collision.gameObject);
            }
        }
    }
}
