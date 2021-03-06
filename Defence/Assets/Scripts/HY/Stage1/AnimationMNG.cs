using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationMNG : MonoBehaviour
{
    public Animator tableanim;
    public Animator draweranim;
    public Animator closetanim;
    public Animator door;
    public Animator Toybox;
    public Animator window;
    public Animator bed;

   public void BedAnim()
    {
        bed.SetTrigger("isBedClosed");
    }

    public void ClosetAnim()
    {
        closetanim.SetTrigger("isClosetClosed");
    }
    
    public void ClosetAnim_Open()
    {
        closetanim.SetTrigger("isClosetClosed");
        closetanim.SetTrigger("isClosetOpened");
    }

    public void WindowAnim()
    {
        window.SetBool("isWindowClosed", true);
    }
    public void ToyboxAnim()
    {
        Toybox.SetTrigger("isBoxClosed");
    }

    public void DoorAnim()
    {
        door.SetTrigger("isDoorClosed");
    }

    public void DrawerAnim()
    {

    }
}
