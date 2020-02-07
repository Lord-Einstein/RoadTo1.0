
/* Code angelehnt an folgendes Tutorial:
 * https://youtu.be/_1pz_ohupPs
 * 
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum BattleState { START, PROCESSING, PLAYERTURN, ENEMYTURN, WON, LOST }

public enum SelectedPlayer { KIM, BEKE, MARTIN}

public class BattleSystem : MonoBehaviour
{
    public string destinationScene;
    public GameObject LevelChanger;
    LevelChanger Lvl;

    [Header("Prefabs")]
    public GameObject bossPrefab;
    public GameObject topPrefab;
    public GameObject midPrefab;
    public GameObject botPrefab;

    [Header("Battlestations")]
    public Transform bossBattlestation;
    public Transform topBattlestation;
    public Transform midBattlestation;
    public Transform botBattlestation;
    

    [Header("Units")]
    Unit bossUnit;
    Unit[] Units = new Unit[3];

    [Header("HUDs")]
    public BattleHUD bossHUD;
    public BattleHUD[] playerHUDs = new BattleHUD[3];



    Animator[] anims = new Animator[3];

    [Header("Arrows")]
    GameObject[] arrows = new GameObject[3];

    [Header("PlayerButtons")]
    Button[] PlayerButtons = new Button[3];

    [Header("ActionsForPlayers")]
    GameObject ActionWindow;
    Button[] ActionButtons = new Button[2];
    public GameObject SpecialAttack;

    public SelectedPlayer player;

    [Space(10)]
    public TextMeshProUGUI dialogueText;

    public BattleState state;

    bool isSpecialUsed = false;
    bool isBossEnraged = false;

    void Start()
    {
        state = BattleState.START;

        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        Lvl = LevelChanger.GetComponent<LevelChanger>();

        #region Instatiate Objects
        GameObject bossGO =  Instantiate(bossPrefab, bossBattlestation);
        bossUnit = bossGO.GetComponent<Unit>();
        
        GameObject topGO = Instantiate(topPrefab, topBattlestation);
        Units[2] = topGO.GetComponent<Unit>();
        anims[2] = topPrefab.GetComponentInChildren<Animator>();

        GameObject midGO = Instantiate(midPrefab, midBattlestation);
        Units[1] = midGO.GetComponent<Unit>();
        anims[1] = midPrefab.GetComponentInChildren<Animator>();

        GameObject botGO = Instantiate(botPrefab, botBattlestation);
        Units[0] = botGO.GetComponent<Unit>();
        anims[0] = botPrefab.GetComponentInChildren<Animator>();


        #endregion

        
        #region SetupArrows

        arrows[0] = GameObject.Find("ArrowKim");
        arrows[1] = GameObject.Find("ArrowBeke");
        arrows[2] = GameObject.Find("ArrowMartin");

        arrows[0].SetActive(false);
        arrows[1].SetActive(false);
        arrows[2].SetActive(false);

        #endregion

        #region SetupPlayerButtons

        PlayerButtons[0] = GameObject.Find("PlayerUI/BPlayer1").GetComponent<Button>();
        PlayerButtons[1] = GameObject.Find("PlayerUI/BPlayer2").GetComponent<Button>();
        PlayerButtons[2] = GameObject.Find("PlayerUI/BPlayer3").GetComponent<Button>();

        PlayerButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = Units[0].unitName;
        PlayerButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = Units[1].unitName;
        PlayerButtons[2].GetComponentInChildren<TextMeshProUGUI>().text = Units[2].unitName;

        #endregion

        #region SetupActionButtons

        ActionWindow = GameObject.Find("ActionsUI/Actions");

        ActionButtons[0] = GameObject.Find("ActionsUI/Actions/Action1").GetComponent<Button>();
        ActionButtons[1] = GameObject.Find("ActionsUI/Actions/Action2").GetComponent<Button>();
        
        SpecialAttack.SetActive(false);

        ActionWindow.SetActive(false);
        #endregion
        

        #region SetHUDs
        bossHUD.SetHUD(bossUnit);

        playerHUDs[0].SetHUD(Units[0]);
        playerHUDs[1].SetHUD(Units[1]);        
        playerHUDs[2].SetHUD(Units[2]);
        #endregion

        dialogueText.text = "" + bossUnit.unitName + " erscheint...";

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;

        

        PlayerTurn();
    }

    public void Fade()
    {
        Lvl.desti = destinationScene;
        Lvl.FadeToLevel();
    }

    
    public void OnKlickPlayer1()
    {
        if(state!= BattleState.PLAYERTURN)
        {
            return;
        }

        player = SelectedPlayer.KIM;

        arrows[0].SetActive(true);
        arrows[1].SetActive(false);
        arrows[2].SetActive(false);

        ActionWindow.SetActive(true);

        ActionButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Bedeutsame BCG-Backpfeife";
        ActionButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Atemberaubende ABC-Analyse-Attacke";
    }

    public void OnKlickPlayer2()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        player = SelectedPlayer.BEKE;

        arrows[0].SetActive(false);
        arrows[1].SetActive(true);
        arrows[2].SetActive(false);

        ActionButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Kampf *Kunst*";
        ActionButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Hexadezimale Herxerei";

        ActionWindow.SetActive(true);
    }

    public void OnKlickPlayer3()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        
        player = SelectedPlayer.MARTIN;


        arrows[0].SetActive(false);
        arrows[1].SetActive(false);
        arrows[2].SetActive(true);

        ActionButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Krasser Kampf-Code";
        ActionButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Perfekt Perfektionistischer Programm Punch";

        ActionWindow.SetActive(true);
    }
    



    IEnumerator PlayerAttack(int dmg)
    {
        bool isDead = bossUnit.TakeDamage(dmg);

        bossHUD.SetHP(bossUnit.currentHP);

        dialogueText.text = "Attacke erfolgreich!";
        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

        

    }

    
    IEnumerator EnemyTurn()
    {
        int rand = Random.Range(0, 3);
        
        bool isDead = false;

        if (bossUnit.currentHP <= bossUnit.maxHP / 2 && !isBossEnraged )
        {
            isBossEnraged = true;
            dialogueText.text = "Da geht aber noch mehr!\nDas können sie Besser";

        }

        yield return new WaitForSeconds(2f);

        dialogueText.text = bossUnit.unitName + " greift an!";

        

        yield return new WaitForSeconds(2f);

        

        if (!isBossEnraged)
        {
            isDead = Units[rand].TakeDamage(bossUnit.damageAtk1);
        }
        else
        {
            isDead = Units[rand].TakeDamage(bossUnit.damageAtk2);
        }
        

        playerHUDs[rand].SetHP(Units[rand].currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }else {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
        
    }
    IEnumerator EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "Wir haben  gewonnen";
            yield return new WaitForSeconds(2f);
            Fade();
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "Wir haben verloren";
            yield return new WaitForSeconds(2f);
            destinationScene = "Loose Screen";
            Fade();

        }
        

    }
    void PlayerTurn()
    {
        int totalMaxHP = Units[0].maxHP + Units[1].maxHP + Units[2].maxHP;
        int totalCurHP = Units[0].currentHP + Units[1].currentHP + Units[2].currentHP;

        if(totalCurHP <= (totalMaxHP *0.6f) && !isSpecialUsed)
        {
            SpecialAttack.SetActive(true);
        }

        dialogueText.text = "Charakter wählen!";

        
    }

    public void OnAttackButton1()
    {
               int dmg = 0;

        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        switch (player)
        {
            case SelectedPlayer.KIM:
                dmg = Units[0].damageAtk1;
                anims[0].SetTrigger("Attack");
                Debug.Log("KAttack1");
                break;
            case SelectedPlayer.BEKE:
                dmg = Units[1].damageAtk1;
                break;
            case SelectedPlayer.MARTIN:
                dmg = Units[2].damageAtk1;
                break;
        }
        state = BattleState.PROCESSING;
        StartCoroutine(PlayerAttack(dmg));
    }
    public void OnAttackButton2()
    {
        int dmg = 0;

        if (state != BattleState.PLAYERTURN)
        {
            return;
        }
        switch (player)
        {
            case SelectedPlayer.KIM:
                dmg = Units[0].damageAtk2;
                Debug.Log("KAttack2");
                anims[0].SetTrigger("Attack");
                break;
            case SelectedPlayer.BEKE:
                dmg = Units[1].damageAtk2;
                break;
            case SelectedPlayer.MARTIN:
                dmg = Units[2].damageAtk2;
                break;
        }
        state = BattleState.PROCESSING;
        StartCoroutine(PlayerAttack(dmg));
    }

    
    public void OnSpecialAttack()
    {
        if (state != BattleState.PLAYERTURN || isSpecialUsed)
        {
            return;
        }

        int dmg = bossUnit.currentHP - 10;

        isSpecialUsed = true;
        state = BattleState.PROCESSING;
        StartCoroutine(PlayerAttack(dmg));
        SpecialAttack.SetActive(false);
    }
   
}
