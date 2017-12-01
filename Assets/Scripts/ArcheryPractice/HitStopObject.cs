using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitStopObject : MonoBehaviour
{
    ScoreCalculation scoreCalculation;
    [SerializeField]
    Text text;

    [SerializeField]
    ScoreTotal scoreTotal;
    

    void Start()
    {
        Debug.Log("hoge");
        scoreCalculation = GetComponent<ScoreCalculation>();
        
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

            //AudioManager.Instance.PlaySE("弓矢・矢が刺さる01");

            var get_score = scoreCalculation.getScore(collision.gameObject);
           if (get_score == 0){
                bul.rig.isKinematic = false;
                bul.rig.useGravity = true;
            }
            text.text = "Score : " + get_score.ToString();
            scoreTotal.addScore(get_score);
        }

    }

}
