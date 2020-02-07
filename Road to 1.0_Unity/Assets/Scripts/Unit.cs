
/* Code angelehnt an folgendes Tutorial:
 * https://youtu.be/_1pz_ohupPs
 */

using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;

    public int damageAtk1;
    public int damageAtk2;

    public int maxHP;
    public int currentHP;

    public bool TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0) { 
            return true;
        }
        else { 
            return false;
        }
    }

}
