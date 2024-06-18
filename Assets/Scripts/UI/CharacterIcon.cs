using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CharacterIcon : MonoBehaviour, IPointerClickHandler
{
    private Image _image;
    public DataManager.Player DefaultData { get; private set; }
    public UserDataManager.OwnedCharacter UserData { get; private set; }

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
        DefaultData = DataManager.Instance.GetPlayerData(id);
        UserData = UserDataManager.Instance.UserData.ownedCharacter.FirstOrDefault(x => x.characterID == id);
    }
}