using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlock : MonoBehaviour
{
    private Animator myAnimator;
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }
    IEnumerator BreakTime()
    {
        myAnimator.SetBool("CheckPlayer", true);
        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(5f);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        myAnimator.SetBool("CheckPlayer", false);
        yield break;
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if(_collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            StartCoroutine(BreakTime());
        }
    }
}
