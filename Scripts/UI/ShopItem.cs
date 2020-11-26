using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IPointerClickHandler
{
    public Shop shop;
    public Image bg;
    public Text price;

    public int value;
    public ShopItemType type;
    public ShopItemState state;
    public ShopItemState defaultState;

    bool isUnlocked;

    public void Init()
    {
        bg = GetComponent<Image>();
        price.text = value.ToString();
        if ((int)state > 1)
            Unlocked();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        shop.OnItemEnter(this);
    }

    public void SetDefaultState()
    {
        if (isUnlocked)
            defaultState = ShopItemState.Unlocked;
        else defaultState = ShopItemState.Locked;
        state = defaultState;
    }

    public bool IsUnlocked => isUnlocked;
    public void Unlocked() 
    { 
        isUnlocked = true; 
        price.enabled = false;
    }
}
public enum ShopItemType { Ship, Trail }
public enum ShopItemState { Locked, UnlockButton, Unlocked, SelectButton, Selected }
