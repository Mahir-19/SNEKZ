using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject BulletObj;
    public GameObject HitObj;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<Part>() != null)
        {
            Destroy(collision.gameObject.GetComponentInParent<Part>().gameObject);
           

        }
        GetComponent<Rigidbody>().isKinematic = true;
        HitObj.SetActive(true);
        BulletObj.SetActive(false);
        //Destroy(gameObject, 2f);
        StartCoroutine(Disable(2f));

    }
    IEnumerator Disable(float TheT)
    {
        yield return new WaitForSeconds(TheT);
        HitObj.SetActive(false);
        BulletObj.SetActive(true);
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        StartCoroutine(Disable(4f));
    }
}
