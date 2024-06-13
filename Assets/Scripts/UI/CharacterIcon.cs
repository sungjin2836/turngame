using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CharacterIcon : MonoBehaviour, IPointerClickHandler
{
    private Image _image;
    public DataManager.Player Data { get; private set; }

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CharacterList.SelectCharacter(this);
    }

    public void GetCharacterData(int id)
    {
        if (_image) _image.sprite = Resources.Load<Sprite>($"Images/DoNotShare/{id}");
        Data = DataManager.Instance.GetPlayerData(id);
    }
}