using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : Enemy {
    private Vector3 targetDir;

    public float speed = 20.0f;

    public override void Start() {
        base.Start();
        targetDir = PlayerLife.playerObject.transform.position - transform.position;
        targetDir.Normalize();
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg + 45.0f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public virtual void FixedUpdate() {
        transform.position += targetDir * speed * Time.fixedDeltaTime;
    }
    
}