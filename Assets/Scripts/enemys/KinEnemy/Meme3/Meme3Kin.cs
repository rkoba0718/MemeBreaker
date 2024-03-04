using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meme3Kin : MonoBehaviour
{
    public Transform weaponPoint; // weaponの発射位置
    public Meme3KinWeapon weaponPrefab; // weaponのPrefab
    public Meme3KinWeapon beamPrefab; // weaponのPrefab
    public GameObject kinExplotionPrefab; // Golemエネミーの爆発Prefab
    public GameObject damagePrefab; // ダメージエフェクト用のPrefab
    PlayerShip playerShip; // playerShip取得のための変数
    GameController gameController; // gameController取得のための変数
    Meme3 meme3; // meme3取得のための変数

    private int point = 300; // 撃破ポイント
    private int hp = 80; // HP
    private float roseAngle = 0; // バラ曲線移動に使用
    private float eightAngle = 0; // 外サイクロイド移動に使用
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
        meme3 = GameObject.Find("Meme3(Clone)").GetComponent<Meme3>();
        currentHP = hp;
        initPosition = transform.position;

        StartCoroutine(Attack());
    }

    // Update is called once per frame
    void Update()
    {
        if (meme3.kinDestroyCounter < 1)
        {
            MoveRoseCurve(4f, 1f);
        }
        else
        {
            MoveEight();
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
     * Meme3の眷属をアステロイド曲線で移動させる関数
     * 
     */
    void MoveRoseCurve(float a1, float a2)
    {
        transform.position = new Vector3(
                initPosition.x + 2.5f * (Mathf.Sin(a1 * roseAngle * 0.2f) * Mathf.Cos(roseAngle * 0.2f)),
                initPosition.y + 2.5f * (Mathf.Sin(a2 * roseAngle * 0.2f) * Mathf.Sin(roseAngle * 0.2f)),
                0
            );
        roseAngle += Time.deltaTime * 5.5f;
    }

    /*
     * 関数名:MoveEight
     * Meme3の眷属を8の字で移動させる関数
     * 
     */
    void MoveEight()
    {
        transform.position = new Vector3(
                initPosition.x + 4f * Mathf.Sin(eightAngle),
                initPosition.y + 0.35f * Mathf.Sin(eightAngle * 2),
                0
            );
        eightAngle += Time.deltaTime * 0.8f;
    }

    /*
     * 関数名:Attack
     * Meme3の眷属の攻撃方法を定義
     */
    private IEnumerator Attack()
    {
        while (true)
        {
            if (meme3.kinDestroyCounter < 1)
            {
                int wave = Random.Range(3, 5);
                int n = Random.Range(2, 4);
                yield return WavePlayerAimShotN(wave, n, weaponPrefab);
                yield return new WaitForSeconds(1f);
                yield return WaveShot(wave, 6, weaponPrefab);
                float interval = Random.Range(0.1f, 1f);
                yield return new WaitForSeconds(interval);
            }
            else
            {
                int wave = Random.Range(4, 5);
                int n = Random.Range(5, 10);
                yield return WaveShot(wave, n, weaponPrefab);
                yield return new WaitForSeconds(2f);
                Shot(2 * Mathf.PI * 2 / 3, 7f, beamPrefab);
                Shot(2 * Mathf.PI * 3 / 4, 7f, beamPrefab);
                Shot(2 * Mathf.PI * 5 / 6, 7f, beamPrefab);
                yield return new WaitForSeconds(2f);
            }
        }
    }

    /*
     * 関数名:WavePlayerAimShotN
     * 
     */
    private IEnumerator WavePlayerAimShotN(int w, int n, Meme3KinWeapon prefab)
    {
        for (int i = 0; i < w; i++)
        {
            yield return PlayerAimShot(n, prefab);
            yield return new WaitForSeconds(0.5f);
        }
    }

    /*
     * 関数名:PlayerAimShot
     * 引数：int型, string
     * PlayerShipを狙い、攻撃を行う関数
     */
    private IEnumerator PlayerAimShot(int n, Meme3KinWeapon prefab)
    {
        for (int i = 0; i < n; i++)
        {
            Vector3 diffPosition = playerShip != null
            ? playerShip.transform.position - transform.position
            : transform.position;
            float shotAngle = Mathf.Atan2(diffPosition.y, diffPosition.x);
            Shot(shotAngle, 5f, prefab);
            yield return new WaitForSeconds(0.1f);
        }
    }

    /*
     * 関数名:WaveShot
     * Meme3の眷属の攻撃処理
     * w回ShotNを実行
     */
    private IEnumerator WaveShot(int w, int n, Meme3KinWeapon prefab)
    {
        for (int i = 0; i < w; i++)
        {
            ShotN(n, prefab);
            yield return new WaitForSeconds(1f);
        }
    }

    /*
     * 関数名:ShotN
     * n個のWeaponをPI/n度ずつずらして発射する関数
     * 
     */
    void ShotN(int n, Meme3KinWeapon prefab)
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
    private void Shot(float shotAngle, float speed, Meme3KinWeapon prefab)
    {
        Meme3KinWeapon weapon = Instantiate(prefab, weaponPoint.position, weaponPoint.rotation);
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
                meme3.AddDestoryCounter();
                Destroy(gameObject);
                Destroy(collision.gameObject);
            }
        }
    }
}
