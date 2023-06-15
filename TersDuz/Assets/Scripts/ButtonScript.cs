using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    private Animator anim1;
    public Animator anim2;
    void Start()
    {
        anim1 = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "MainPlayer")
        {
            anim1.SetTrigger("Trigger");
            anim2.SetTrigger("Trigger");
        }
    }
}
