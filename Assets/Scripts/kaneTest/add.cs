using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class add : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //GetComponent<Rigidbody>().AddForce(new Vector3(0, 100, 500));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision collision)
    {
        //衝突判定
        if (collision.gameObject.tag == "Target")
        {
            //相手のタグがBallならば、自分を消す
            //collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //collision.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            //collision.gameObject.GetComponent<Rigidbody>().useGravity = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, -transform.localScale.z / 2 + collision.gameObject.transform.position.z);
            var ri = GetComponent<Rigidbody>();
            transform.rotation = Quaternion.identity;
            Destroy(ri);
        }
    }
}
