using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorAutomatic : MonoBehaviour
{
    public Animator DoorAnim;
    public bool mustClose = false;
    public bool mustOpen = false;

    public void CloseTheDoor()
    {
        DoorAnim.SetBool("wantToPass", false);
        mustOpen = false;
        mustClose = true;
    }

    public void OpenTheDoor()
    {
        DoorAnim.SetBool("wantToPass", true);
        mustOpen = true;
        mustClose = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!mustClose) DoorAnim.SetBool("wantToPass", true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!mustClose) DoorAnim.SetBool("wantToPass", false);
    }

}
