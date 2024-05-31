using UnityEngine;

public class DebugData : MonoBehaviour
{
    public int characterID = 1002;
    public Character target;

    private void Start()
    {
        if (characterID < 2000)
        {
            GetComponent<Player>().Initialize(characterID);
        }
        else
        {
            GetComponent<Enemy>().Initialize(characterID);
        }
    }

    public void Attack()
    {
        if (GetComponent<Player>())
            GetComponent<Player>().NormalAttack(target);
        else if (GetComponent<Enemy>()) GetComponent<Enemy>().NormalAttack(target);
    }
}