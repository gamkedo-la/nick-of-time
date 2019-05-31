using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private bool isOpen = false;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Open", isOpen);
    }
}
