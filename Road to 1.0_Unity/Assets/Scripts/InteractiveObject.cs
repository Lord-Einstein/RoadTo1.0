using UnityEngine;
public class InteractiveObject : MonoBehaviour
{

    
    public GameObject TextPane;
    Animator anim;
    
    void Start()
    {
        TextPane.SetActive(false);
        anim = TextPane.GetComponent<Animator>();
    }

    /* Code angelehnt an folgendes Tutorial:
    * http://unity.grogansoft.com/speech-bubbles-and-popup-ui/
    */

    void OnTriggerEnter2D()
    {

        TurnOnMessage();

    }

    private void TurnOnMessage()
    {
        TextPane.SetActive(true);
        anim.SetTrigger("FadeIn");
    }

    void OnTriggerExit2D()
    {

        TurnOffMessage();

    }

    private void TurnOffMessage()
    {

        anim.SetTrigger("FadeOut");
    }
}