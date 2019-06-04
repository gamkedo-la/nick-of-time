using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private bool isOpen = false;
    [SerializeField]
    private bool isLocked = false;
    [SerializeField]
    private GameObject childLock;
    [SerializeField]
    private GameObject childDoor;

    private Animator lockAnimator;
    private Animator doorAnimator;

    private void Start()
    {
        lockAnimator = childLock.GetComponent<Animator>();
        doorAnimator = childDoor.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        lockAnimator.SetBool("Locked", isLocked);
        doorAnimator.SetBool("Open", isOpen);
    }

    void OpenDoor()
    {
        isOpen = true;
        //Debug.LogWarning("Done Unlocking, opening door...");
    }
}
