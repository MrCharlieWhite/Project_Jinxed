using System;
using UnityEngine;

public class Death : MonoBehaviour
{

    public Transform trapCheckPosition;
    public Vector2 trapCheckSize = new Vector2(0.4f, 0.02f);
    public LayerMask trapLayer;

    Animator anim;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        TrapCheck();
    }

    private void TrapCheck()
    {
        if (Physics2D.OverlapBox(trapCheckPosition.position, trapCheckSize, 0, trapLayer))
        {
            anim.SetBool("isDead", true);

        }
        else
        {
            anim.SetBool("isDead", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(trapCheckPosition.position, trapCheckSize);
    }

    void ControlLock()
    {
        
    }
    
}