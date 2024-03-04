using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * PlayerShipのWeapon（武器）の動きなどの機能を持つクラス
 */
public class PlayerWeapon : MonoBehaviour
{
    private float limit = 6.5f; //範囲制限

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 10, 0) * Time.deltaTime;
        if (Mathf.Abs(transform.position.y) > limit)
        {
            Destroy(gameObject);
        }
    }

    /*
     * 関数名:OnTriggerEnter2D
     * EnemyWeaponとの衝突処理
     * EnemyWeaponとPlayerWeaponの両方を消す
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyWeapon") == true)
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        } else if (collision.CompareTag("DeathblowWeapon") == true)
        {
            Destroy(gameObject);
        }
    }
}
