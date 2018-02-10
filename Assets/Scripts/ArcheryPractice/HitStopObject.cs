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

    [SerializeField]
    ParticleSystem[] particle;

    public int particleNum = 0;

    void Start()
    {
        scoreCalculation = GetComponent<ScoreCalculation>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hogehoge");
        Debug.Log("HSO hit coll : " + collision.transform.position);

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

            var get_score = scoreCalculation.getScore(collision.gameObject);
            if (get_score == 0)
            {
                bul.rig.isKinematic = false;
                bul.rig.useGravity = true;
            }
            text.text = "Score : " + get_score.ToString();
            scoreTotal.AddScore(get_score);

            if (collision.gameObject.GetComponent<IsPushEffect>() != null)
            {
                EffectPlay(pos, get_score);
            }
        }

    }

    public void OnHitUpdateText( int score)
    {
        //text.text = "Score : " + score.ToString();
        text.text = score.ToString();
    }


    //エフェクトする
    public void EffectPlay(Vector3 pos, int get_score)
    {
        if (get_score <= 4)
        {
            particleNum = 0;
        }
        else if (get_score <= 8)
        {
            particleNum = 1;
        }
        else
        {
            particleNum = 2;
        }
        particle[particleNum].transform.position = new Vector3(pos.x, pos.y, particle[particleNum].transform.position.z);
        particle[particleNum].Play(true);
    }
}
