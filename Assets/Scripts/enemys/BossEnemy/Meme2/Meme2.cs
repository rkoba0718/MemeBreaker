using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meme2 : MonoBehaviour
{
    public Transform kinPoint; // 眷属の出現位置
    public GameObject kinPrefab; // 眷属のPrefab
    public Transform weaponPoint; // Weaponの発射位置
    public GameObject explotionPrefab; // 爆発エフェクトのPrefab
    public GameObject damagePrefab; // ダメージエフェクト用のPrefab
    public Meme2Weapon weaponPrefab; // Meme2のWeaponPrefab
    public Meme2Weapon deathblowPrefab; // Meme2のDeathblowPrefab
    public int kinDestroyCounter = 0; // 眷属の撃破カウンター

    GameController gameController; // gameController取得のための変数
    GameObject playerShip; // playerShip取得のための変数
    private float limit = 2.8f; // Meme2の移動場所の制限
    private int point = 1000; // 撃破ポイント
    private float hp = 200; // hp
    private bool invincibleFlag = true; // 無敵状態用フラグ
    private bool moveFlag = false; // 無敵状態用フラグ
    float currentHP;
    int damage;
    float initPoint;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        playerShip = GameObject.Find("PlayerShip") ?
            GameObject.Find("PlayerShip") :
            null;
        damage = playerShip ?
            playerShip.GetComponent<PlayerShip>().weaponDamage :
            0;
        currentHP = hp;
        initPoint = transform.position.y;
        StartCoroutine(CPU());
    }

    /*
     * 関数名:AddDestoryCounter
     * 眷属の撃破カウンターを1増やす関数
     *
     */
    public void AddDestoryCounter()
    {
        kinDestroyCounter += 1;
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
     * 関数名:CPU
     * Meme2の行動を定義
     * 移動した後、眷属を生成、その後は眷属の撃破数によって異なる攻撃をする
     */
    private IEnumerator CPU()
    {
        yield return MoveDown();
        yield return new WaitForSeconds(0.5f);
        KinSpawn();
        yield return new WaitForSeconds(0.5f);
        yield return MoveUp();

        while (true)
        {
            if (kinDestroyCounter < 2)
            {
                yield return null;
            } else
            {
                yield return MoveDown();
                invincibleFlag = false;

                yield return Attack(currentHP / hp);

                KinSpawn();
                yield return MoveUp();
                break;
            }
        }

        while (true)
        {
            if (kinDestroyCounter < 4)
            {
                yield return null;
            } else
            {
                yield return MoveDown();
                yield return Attack(currentHP / hp);
                break;
            }
        }
    }

    /*
     * 関数名:MoveUp
     * Meme2を上に移動させる関数
     * 
     */
    private IEnumerator MoveUp()
    {
        moveFlag = true;
        while (Mathf.Abs(transform.position.y) < initPoint)
        {
            transform.position += new Vector3(0, 5 * Time.deltaTime, 0);
            yield return null;
        }
    }

    /*
     * 関数名:MoveDown
     * Meme2を下に移動させる関数
     * moveFlag
     */
    private IEnumerator MoveDown()
    {
        moveFlag = true;
        while (Mathf.Abs(transform.position.y) > limit)
        {
            transform.position -= new Vector3(0, 2 * Time.deltaTime, 0);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
    }

    /*
     * 関数名:KinSpawn
     * Meme2の眷属を生成する関数
     * 
     */
    void KinSpawn()
    {
        Vector3 spawnPoint1 = new Vector3(kinPoint.position.x + 4, kinPoint.position.y + 2.7f, kinPoint.position.z);
        Vector3 spawnPoint2 = new Vector3(kinPoint.position.x - 4, kinPoint.position.y + 2.7f, kinPoint.position.z);
        Instantiate(kinPrefab, spawnPoint1, transform.rotation);
        Instantiate(kinPrefab, spawnPoint2, transform.rotation);
    }

    private IEnumerator Attack(float percentage)
    {
        if (percentage > 0.8f)
        {
            moveFlag = false;
            while (currentHP / hp > 0.8)
            {
                yield return WavePlayerAimShotN(8, 5);
                yield return new WaitForSeconds(1f);
            }
        } else
        {
            while (currentHP / hp > 0.5)
            {
                int n = Random.Range(10, 20);
                yield return WaveShot(3, n);
                moveFlag = false;
                yield return new WaitForSeconds(1f);
            }
            while (currentHP / hp > 0.2)
            {
                int wave = Random.Range(4, 10);
                int n = Random.Range(3, 15);
                yield return WavePlayerAimShotN(wave, n);
                yield return new WaitForSeconds(0.5f);
                yield return WaveShot(2, 16);
                yield return new WaitForSeconds(1f);
            }
            while (currentHP / hp > 0)
            {
                Deathblow();
                yield return WavePlayerAimShotN(5, 8);
                yield return new WaitForSeconds(1f);
            }
        }

    }

    /*
     * 関数名:WaveShotNToM
     * 
     */
    private IEnumerator WavePlayerAimShotN(int w, int n)
    {
        for (int i = 0; i < w; i++)
        {
            PlayerAimShot(n);
            yield return new WaitForSeconds(0.3f);
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
            Shot(shotAngle + offset, 8f);
        }
    }

    /*
     * 関数名:WaveShotNToM
     * Meme2の眷属の攻撃処理
     * w回ShotNToMを実行
     */
    private IEnumerator WaveShot(int w, int n)
    {
        for (int i = 0; i < w; i++)
        {
            ShotN(n);
            yield return new WaitForSeconds(0.75f);
        }
    }

    /*
     * 関数名:ShotNToM
     * m-n+1個のWeaponを30度ずつずらして発射する関数
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

    void Deathblow()
    {
        Vector3 diffPosition = playerShip != null
            ? playerShip.transform.position - transform.position
            : transform.position;
        float shotAngle = Mathf.Atan2(diffPosition.y, diffPosition.x);
        Meme2Weapon weapon = Instantiate(deathblowPrefab, weaponPoint.position, weaponPoint.rotation);
        weapon.Setting(shotAngle, 10f);
    }

    /*
     * 関数名:Shot
     * 引数：(float型, float型)
     * Meme2Weaponを発射する関数
     * 与えられた角度と速度で発射する
     */
    private void Shot(float shotAngle, float speed)
    {
        Meme2Weapon weapon = Instantiate(weaponPrefab, weaponPoint.position, weaponPoint.rotation);
        weapon.Setting(shotAngle, speed);
    }

    /*
     * 関数名:DamageEffect
     * ダメージを受けた時の処理
     * 色を赤に変え、ダメージエフェクトを発生し、0.5f間無敵状態にする。その後、色を戻し、無敵状態を解除
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
            if (invincibleFlag || moveFlag)
            {
                Destroy(collision.gameObject);
                return;
            }

            ReduceHP(damage);
            if (currentHP > 0)
            {
                StartCoroutine(DamageEffect(collision.gameObject));
            }
            else
            {
                Instantiate(explotionPrefab, transform.position, transform.rotation);
                gameController.AddScore(point);
                Destroy(gameObject);
                Destroy(collision.gameObject);
            }

        }
    }
}
