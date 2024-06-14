using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject turnOrderPanel;

    [SerializeField]
    private GameObject PlayerTurnOrderPanel;

    [SerializeField]
    private GameObject resultWindow;

    [SerializeField]
    private GameObject gameOverWindow;

    [SerializeField]
    private Image[] itemIndex;

    private Image[] items;

    [SerializeField]
    private GameObject ItemPanel;

    private int itemCount = 5;


    [SerializeField]
    private Slider enemyShieldBar;
    [SerializeField]
    private Slider enemyHpBar;
    [SerializeField]
    private Text turnOrderText;
    [SerializeField]
    private Text turnPlayersOrderText;

    Text[] turnTexts;
    Text[] PlayersturnTexts;
    private int _hp;
    private int _shield;
    public int Hp
    {
        get => _hp;

        private set => _hp = Mathf.Clamp(value, 0, _hp);
    }

    public int Shield
    {
        get => _shield;

        private set => _shield = Mathf.Clamp(value, 0, _shield);
    }


    void Start()
    {
        resultWindow.SetActive(false);
        gameOverWindow.SetActive(false);
    }

    public void InitTurnText(int num)
    {
        turnTexts = new Text[num];
        for (int i = 0; i < num; i++)
        {
            turnTexts[i] = Instantiate(turnOrderText, turnOrderPanel.transform.position, Quaternion.identity);
            turnTexts[i].transform.SetParent(turnOrderPanel.transform);
        }
    }

    public void InitTurnPlayerText(int num)
    {
        PlayersturnTexts = new Text[num];
        for (int i = 0; i < num; i++)
        {
            PlayersturnTexts[i] = Instantiate(turnPlayersOrderText, PlayerTurnOrderPanel.transform.position, Quaternion.identity);
            PlayersturnTexts[i].transform.SetParent(PlayerTurnOrderPanel.transform);
        }
    }

    public void TurnTextClear()
    {
        for (int i = 0;i < turnTexts.Length; i++)
        {
            turnTexts[i].text = "";
        }
    }
    public void TurnPlayerTextClear()
    {
        for (int i = 0; i < PlayersturnTexts.Length; i++)
        {
            PlayersturnTexts[i].text = "";
        }
    }
    public void TurnTextPrint(int num, string Name, int ActionPoint)
    {
        turnTexts[num].text = Name;
        turnTexts[num].text += " "+ ActionPoint;
    }

    public void TurnPlayerTextPrint(int num, string Name)
    {
        PlayersturnTexts[num].text = Name;
    }

    public void SetMaxHealth(int health)
    {
        enemyHpBar.maxValue = health;
        enemyHpBar.value = health;
    }

    public void SetHealth(int health)
    {
        enemyHpBar.value = health;
    }

    public void SetMaxShield(int shield)
    {
        enemyShieldBar.maxValue = shield;
        enemyShieldBar.value = shield;
    }
    public void SetShield(int shield)
    {
        enemyShieldBar.value = shield;
    }

    public void InitItem()
    {
        Debug.Log("InitItem");
        items = new Image[itemCount]; 
        for (int i = 0; i < itemCount; i++)
        {
            int itemNum = Random.Range(0, itemIndex.Length);

            items[i] = Instantiate(itemIndex[itemNum], ItemPanel.transform.position, Quaternion.identity);
            items[i].transform.SetParent(ItemPanel.transform); // ItemPanel = 전투결과창
            Debug.Log($"아이템{i} {items[i]}");
        }
    }

    public void FinishGame()
    {
        Debug.Log("finish");
        resultWindow.SetActive(true);
        InitItem();
    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        gameOverWindow.SetActive(true);
    }
    



}
