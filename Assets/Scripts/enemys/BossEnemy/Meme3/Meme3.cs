using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meme3 : MonoBehaviour
{
    public Transform kinPoint; // 眷属の出現位置
    public GameObject kinPrefab; // 眷属のPrefab
    public Transform weaponPoint; // Weaponの発射位置
    public GameObject explotionPrefab; // 爆発エフェクトのPrefab
    public GameObject damagePrefab; // ダメージエフェクト用のPrefab
    public Meme3Weapon weaponPrefab; // Meme3のWeaponPrefab
    public Meme3Weapon beamPrefab; // Meme3のBeamPrefab
    public Meme3Weapon deathblowPrefab; // Meme3のDeathblowPrefab
    public int kinDestroyCounter = 0; // 眷属の撃破カウンター

    GameController gameController; // gameController取得のための変数
    GameObject playerShip; // playerShip取得のための変数
    private float limit = 2.8f; // Meme2の移動場所の制限
    private int point = 2500; // 撃破ポイント
    private float hp = 400; // hp
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
     * Meme3の行動を定義
     * 移動した後、眷属を生成、その後は眷属の撃破数によって異なる攻撃をする
     */
    private IEnumerator CPU()
    {
        yield return MoveDown();
        yield return new WaitForSeconds(0.5f);
        KinSpawn(1);
        yield return new WaitForSeconds(0.5f);
        yield return MoveUp();

        while (true)
        {
            if (kinDestroyCounter < 1)
            {
                yield return null;
            }
            else
            {
                yield return MoveDown();
                invincibleFlag = false;

                yield return Attack(currentHP / hp);

                KinSpawn(2);
                yield return MoveUp();
                break;
            }
        }

        while (true)
        {
            if (kinDestroyCounter < 2)
            {
                yield return null;
            }
            else
            {
                yield return MoveDown();
                invincibleFlag = false;

                yield return Attack(currentHP / hp);

                KinSpawn(3);
                yield return MoveUp();
                break;
            }
        }
    }

    /*
     * 関数名:MoveUp
     * Meme3を上に移動させる関数
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
     * Meme3を下に移動させる関数
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
    void KinSpawn(int amount)
    {
        if (amount == 1)
        {
            Instantiate(kinPrefab, kinPoint.position, transform.rotation);
        } else if (amount == 2)
        {
            Vector3 spawnPoint1 = new Vector3(kinPoint.position.x + 4, kinPoint.position.y + 2, kinPoint.position.z);
            Vector3 spawnPoint2 = new Vector3(kinPoint.position.x - 4, kinPoint.position.y + 2, kinPoint.position.z);
            Instantiate(kinPrefab, spawnPoint1, transform.rotation);
            Instantiate(kinPrefab, spawnPoint2, transform.rotation);
        } else if (amount == 3)
        {
            Vector3 spawnPoint1 = new Vector3(kinPoint.position.x, kinPoint.position.y, kinPoint.position.z);
            Vector3 spawnPoint2 = new Vector3(kinPoint.position.x + 4, kinPoint.position.y + 2, kinPoint.position.z); ;
            Vector3 spawnPoint3 = new Vector3(kinPoint.position.x - 4, kinPoint.position.y + 2, kinPoint.position.z);
            Instantiate(kinPrefab, spawnPoint1, transform.rotation);
            Instantiate(kinPrefab, spawnPoint2, transform.rotation);
            Instantiate(kinPrefab, spawnPoint3, transform.rotation);
        } else
        {
            return;
        }
    }

    private IEnumerator Attack(float percentage)
    {
        if (percentage > 0.8f)
        {
            moveFlag = false;
            while (currentHP / hp > 0.8)
            {
                yield return WavePlayerAimShotN(8, 5, weaponPrefab);
                yield return new WaitForSeconds(1f);
            }
        }
        else
        {
            while (currentHP / hp > 0.5)
            {
                int n = Random.Range(10, 20);
                yield return WaveShot(3, n, weaponPrefab);
                moveFlag = false;
                yield return new WaitForSeconds(1f);
                yield return WavePlayerAimShotN(3, 5, beamPrefab);
                yield return new WaitForSeconds(0.5f);
            }
            while (currentHP / hp > 0.2)
            {
                int wave = Random.Range(4, 10);
                int n = Random.Range(3, 15);
                yield return WavePlayerAimShotN(wave, n, weaponPrefab);
                yield return new WaitForSeconds(0.5f);
                yield return WaveShot(2, 10, weaponPrefab);
                yield return new WaitForSeconds(1f);
            }
            while (currentHP / hp > 0)
            {
                yield return WavePlayerAimShotN(3, 8, weaponPrefab);
                yield return new WaitForSeconds(1f);
                yield return WavePlayerAimShotN(2, 5, beamPrefab);
                yield return new WaitForSeconds(2f);
                Deathblow();
                yield return new WaitForSeconds(3f);
            }
        }

    }

    /*
     * 関数名:WaveShotNToM
     * 
     */
    private IEnumerator WavePlayerAimShotN(int w, int n, Meme3Weapon prefab)
    {
        for (int i = 0; i < w; i++)
        {
            PlayerAimShot(n, prefab);
            yield return new WaitForSeconds(0.3f);
        }
    }

    /*
     * 関数名:PlayerAimShot
     * 引数：int型
     * PlayerShipを狙い、攻撃を行う関数
     * n個のWeaponをPlayerに向かって同時に発射する
     */
    private void PlayerAimShot(int n, Meme3Weapon prefab)
    {
        Vector3 diffPosition = playerShip != null
            ? playerShip.transform.position - transform.position
            : transform.position;
        float shotAngle = Mathf.Atan2(diffPosition.y, diffPosition.x);

        for (int i = 0; i < n; i++)
        {
            float offset = (i - n / 2f) * ((Mathf.PI / 2f) / n);
            Shot(shotAngle + offset, 8f, prefab);
        }
    }

    /*
     * 関数名:WaveShot
     * Meme3の攻撃処理
     * w回ShotNを実行
     */
    private IEnumerator WaveShot(int w, int n, Meme3Weapon prefab)
    {
        for (int i = 0; i < w; i++)
        {
            ShotN(n, prefab);
            yield return new WaitForSeconds(0.75f);
        }
    }

    /*
     * 関数名:ShotN
     * n個のWeaponをPI/n度ずつずらして発射する関数
     * 
     */
    void ShotN(int n, Meme3Weapon prefab)
    {
        for (int i = 0; i <= n; i++)
        {
            float shotAngle = i * Mathf.PI / n;
            Shot(shotAngle, 1f, prefab);
            Shot(-shotAngle, 1f, prefab);
        }
    }

    /*
     * 関数名:Shot
     * WeaponをshotAngleの角度かつ、speedの速さで発射する関数
     * 
     */
    private void Shot(float shotAngle, float speed, Meme3Weapon prefab)
    {
        Meme3Weapon weapon = Instantiate(prefab, weaponPoint.position, weaponPoint.rotation);
        weapon.Setting(shotAngle, speed);
    }

    void Deathblow()
    {
        Vector3 diffPosition = playerShip != null
            ? playerShip.transform.position - transform.position
            : transform.position;
        float shotAngle = Mathf.Atan2(diffPosition.y, diffPosition.x);
        Meme3Weapon weapon = Instantiate(deathblowPrefab, weaponPoint.position, weaponPoint.rotation);
        weapon.Setting(shotAngle, 10f);
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
            else if (currentHP == 0)
            {
                Instantiate(explotionPrefab, transform.position, transform.rotation);
                gameController.AddScore(point);
                Destroy(gameObject);
                Destroy(collision.gameObject);
            }

        }
    }
}
