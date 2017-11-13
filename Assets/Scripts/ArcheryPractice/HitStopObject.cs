using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopObject : MonoBehaviour
{

    void Start()
    {
        Debug.Log("hoge");
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hogehoge");

        if (collision.gameObject.tag == "Bullet")
        {
            var bul = collision.gameObject.GetComponent<BulletTag>();

            Debug.Log("Stop Bullet");

            var size = transform.localScale;
            var pos = bul.rig.transform.position;
            bul.rig.transform.position = new Vector3(pos.x, pos.y, transform.position.z - size.z / 2);
            bul.rig.transform.rotation = Quaternion.identity;
            bul.Stop();
            bul.rig.isKinematic = true;
        }

    }

}
