using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookAt : MonoBehaviour
{
    public Transform target;

    private void Start()
    {
        /*Vector3 newScale=transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;*/
    }
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
    }
}
