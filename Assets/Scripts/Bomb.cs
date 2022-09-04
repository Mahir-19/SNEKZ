using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CollisionMan(2f);

    }
    void CollisionMan(float Dist)
    {
        RaycastHit Hit;
        for (int i = 0; i < 4; i++)
        {
            float YDir = 0;
            if (i == 1)
            {
                YDir = 90;
            }
            else if (i == 2)
            {
                YDir = 180;
            }
            if (i == 3)
            {
                YDir = -90;
            }
            //Debug.Log(YDir);
            Vector3 Dir = new Vector3(0, YDir, 0);
            if (Physics.Raycast(transform.position, Dir, out Hit, Dist))
            {
                //Debug.Log(Hit.transform.name);
                if (Hit.transform.GetComponentInParent<Part>() != null)
                {
                    Destroy(Hit.transform.GetComponentInParent<Part>().gameObject);
                }

            }

        }
    }
}
