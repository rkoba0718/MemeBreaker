using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * GolemエネミーのWeaponの定義をしているクラス
 */
public class GolemWeapon : MonoBehaviour
{
    private float limit = 6.5f; //範囲制限
    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, 4, 0) * Time.deltaTime;
        if (Mathf.Abs(transform.position.y) > limit)
        {
            Destroy(gameObject);
        }
    }
}
