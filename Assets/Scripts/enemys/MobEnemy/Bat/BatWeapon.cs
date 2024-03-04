using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * BatエネミーのWeaponの定義をしているクラス
 */
public class BatWeapon : MonoBehaviour
{
    private float xLimit = 10f; //範囲制限
    private float yLimit = 6.5f; //範囲制限
    float dx;
    float dy;
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(dx, dy, 0) * Time.deltaTime;
        if (Mathf.Abs(transform.position.x) > xLimit || Mathf.Abs(transform.position.y) > yLimit)
        {
            Destroy(gameObject);
        }
    }

    /*
     * 関数名:Setting
     * Weaponの発射角度と速度の設定をする関数
     * 
     */
    public void Setting(float angle, float speed)
    {
        dx = Mathf.Cos(angle) * speed;
        dy = Mathf.Sin(angle) * speed;
    }
}
