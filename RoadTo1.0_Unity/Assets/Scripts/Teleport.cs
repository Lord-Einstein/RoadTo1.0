using UnityEngine;

public class Teleport : MonoBehaviour
{
    public string destinationScene;
    public GameObject LevelChanger;
    LevelChanger Lvl;
    private void Start()
    {

        Lvl = LevelChanger.GetComponent<LevelChanger>();
       
        
    }

    public void Fade()
    {
        Lvl.desti = destinationScene;
        Lvl.FadeToLevel();
    }

    void OnTriggerEnter2D()
    {
        Lvl.desti = destinationScene;
        Lvl.FadeToLevel();
    }

}


