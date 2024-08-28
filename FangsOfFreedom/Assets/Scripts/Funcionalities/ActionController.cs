using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    public bool isInCollide = false;
    public bool isInCollideDialogue = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemie"))
        {
            isInCollide = true;
        }

        if (other.gameObject.CompareTag("Dialogue"))
        {
            isInCollideDialogue = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemie") || other.gameObject.CompareTag("Dialogue"))
        {
            isInCollide = false;
        }

        if (other.gameObject.CompareTag("Dialogue"))
        {
            isInCollideDialogue = false;
        }
    }
}
