using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private bool isOpen = false;

    private Animator anim;
    private Collider2D collider;

    private void Start()
    {
        anim = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("Open", isOpen);
        collider.enabled = !isOpen;
    }
}
