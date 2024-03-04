using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *　Meme1の処理を定義しているクラス
 */
public class Meme1 : MonoBehaviour
{
    public Transform kinPointLeft; // 眷属の出現位置
    public Transform kinPointCenter; // 眷属の出現位置
    public Transform kinPointRight; // 眷属の出現位置
    public Transform weaponPoint; // Weaponの発射位置
    public GameObject kinPrefab; // 眷属のPrefab
    public GameObject explotionPrefab; // 爆発エフェクトのPrefab
    public GameObject damagePrefab; // ダメージエフェクト用のPrefab
    public Meme1Weapon weaponPrefab; // Meme1のWeaponPrefab

    GameController gameController; // gameController取得のための変数
    GameObject playerShip; // playerShip取得のための変数
    private float limit = 3f; // Meme1の移動場所の制限
    private int point = 700; // 撃破ポイント
    private int hp = 100; // hp
    private bool invincibleFlag = false; // 無敵状態用フラグ
    private int kinDestroyCounter = 0; // 眷属の撃破カウンター
    int currentHP;
    int damage;

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
        StartCoroutine(CPU());
    }

    // Update is called once per frame
    void Update()
    {

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
     * Meme1の行動を定義
     * 移動した後、眷属を生成、その後は眷属の撃破数によって異なる攻撃をする
     * ・撃破数0の場合：ShotCurveを実行
     * ・撃破数1か2の場合：PlayerAimShotとShotCurveを交互に実行
     * ・全ての眷属を撃破した場合：PlayerAimShotとShotCurveをランダム値で交互に実行
     */
    private IEnumerator CPU()
    {
        while (Mathf.Abs(transform.position.y) > limit)
        {
            transform.position -= new Vector3(0, 2 * Time.deltaTime, 0);
            yield return null; //1フレーム待つ
        }

        yield return KinSpawn();

        while (true)
        {
            if (kinDestroyCounter == 0)
            {
                yield return ShotNCurve(6);
                yield return new WaitForSeconds(4f);
            }
            else if (kinDestroyCounter < 3)
            {
                PlayerAimShot(5);
                yield return new WaitForSeconds(2.5f);
                yield return ShotNCurve(12);
                yield return new WaitForSeconds(2.5f);
            }
            else
            {
                int random = Random.Range(6, 16);
                PlayerAimShot(random);
                yield return new WaitForSeconds(0.25f);
                yield return ShotNCurve(random);
                yield return new WaitForSeconds(0.5f);
            }
        }

    }

    /*
     * 関数名:KinSpawn
     * Meme1の眷属を生成する関数
     * 
     */
    private IEnumerator KinSpawn()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(kinPrefab, kinPointCenter.position, transform.rotation);

        yield return new WaitForSeconds(0.5f);
        Instantiate(kinPrefab, kinPointLeft.position, transform.rotation);

        yield return new WaitForSeconds(0.25f);
        Instantiate(kinPrefab, kinPointRight.position, transform.rotation);
    }

    /*
     * 関数名:ShotCurve
     * 引数：int型
     * 波状攻撃を行う処理
     * n個、Weaponを発射し、波状攻撃を行う
     */
    private IEnumerator ShotNCurve(int n)
    {
        float degree = Mathf.PI / n;
        for (int i = 0; i <= n; i++)
        {
            float shotAngle = -i * degree;
            Shot(shotAngle - Mathf.PI/2f, 2f);
            Shot(-shotAngle - Mathf.PI / 2f, 2f);
            yield return new WaitForSeconds(0.1f);
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
            Shot(shotAngle + offset, 2f);
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
        Meme1Weapon weapon = Instantiate(weaponPrefab, weaponPoint.position, weaponPoint.rotation);
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

        yield return new WaitForSeconds(0.5f);

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
            if (kinDestroyCounter < 3)
            {
                Destroy(collision.gameObject);
                return;
            }

            if (invincibleFlag)
            {
                Destroy(collision.gameObject);
                return;
            }

            ReduceHP(damage);
            if (currentHP > 0)
            {
                StartCoroutine(DamageEffect(collision.gameObject));
            } else
            {
                Instantiate(explotionPrefab, transform.position, transform.rotation);
                gameController.AddScore(point);
                Destroy(gameObject);
                Destroy(collision.gameObject);
            }

        }
    }
}
