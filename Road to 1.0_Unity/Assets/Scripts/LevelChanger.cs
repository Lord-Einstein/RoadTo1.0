
/* Code angelehnt an folgendes Tutorial:
 * https://youtu.be/Oadq-IrOazg
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public Animator animator;
    public string desti;
    
    public void FadeToLevel()
    {
        animator.SetTrigger("Fade_toBlack");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(desti);
    }
}
