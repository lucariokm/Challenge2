using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveManeuver : MonoBehaviour
{
    public Vector2 manuvertime;
    public Vector2 manuverwait;
    public Vector2 startWait;
    public Boundary boundary;
    public float tilt;
    
    private float targetManuver;
    private float currentspeed;
    public float dodge;
    public float smoothing;

    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody>();
        StartCoroutine(Evade());
        currentspeed = rb.velocity.z;
    }

    IEnumerator Evade()
    {
        yield return new WaitForSeconds (Random.Range (startWait.x, startWait.y));
        
        while(true)
        {
            targetManuver = Random.Range (1,dodge)* -Mathf.Sign (transform.position.x);
            yield return new WaitForSeconds(Random.Range (manuvertime.x, manuvertime.y));
            targetManuver = 0;
            yield return new WaitForSeconds(Random.Range(manuverwait.x, manuverwait.y));
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float newManuver = Mathf.MoveTowards (rb.velocity.x, targetManuver, Time.deltaTime* smoothing);
        rb.velocity = new Vector3 (newManuver, 0.0f, currentspeed);
        rb.position = new Vector3 
        (
            Mathf.Clamp (rb.position.x, boundary.xMin,boundary.xMax),
            0.0f,
            Mathf.Clamp (rb.position.z, boundary.zMin,boundary.zMax)
        );
        rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
    }
}
