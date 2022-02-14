using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

/// <summary>
/// Contains shop menu logic and UI code
/// </summary>
public class ShopMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Purchase> purchaseList = new List<Purchase>();
    public GameObject purchaseButtonPrefab;
    public PlayerInventory inventory; //if multiple players need to check which player instead of assigning one in inspector

    public RectTransform buttonPanelTransform;

    List<GameObject> currentPurchaseButtons = new List<GameObject>(); //prob as button better
    List<GameObject> currentPurchaseLayouts = new List<GameObject>();

    public GameObject horizontalLayoutPrefab;
    public GameObject itemDescriptionPrefab;

    [SerializeField]
    AudioClip purchaseClip;

    public List<ItemUIHolder> itemHolderList;

    public PurchasePopup popUp;


    private void Awake()
    {
        //InitializeShop();
    }

    private void Start()
    {
        GlobalReferences.instance.shopMenu = this;
        gameObject.SetActive(false);
    }

    public void InitializeShopOld()
    {
        if (purchaseList.Count == 0)
        {

        }
        foreach(var purchase in purchaseList)
        {
            GameObject hLayout = Instantiate(horizontalLayoutPrefab);
            hLayout.transform.SetParent(buttonPanelTransform);

            GameObject buttonGameObject =Instantiate(purchaseButtonPrefab);
            Button b = buttonGameObject.GetComponent<Button>();
            b.onClick.AddListener(delegate { TryPurchase(purchase, purchaseList); });
            buttonGameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = purchase.price.ToString();
            buttonGameObject.transform.SetParent(hLayout.transform, false);

            GameObject descriptionObject = Instantiate(itemDescriptionPrefab);
            descriptionObject.GetComponent<TMPro.TextMeshProUGUI>().text = purchase.description;
            descriptionObject.transform.SetParent(hLayout.transform, false);

            hLayout.transform.localScale = Vector3.one;
            currentPurchaseLayouts.Add(hLayout);
            currentPurchaseButtons.Add(buttonGameObject);
        }
        OnEnterShop(purchaseList);
    }

    public void InitializeShop()
    {
        int i = 0;
        foreach(var purchase in purchaseList)
        {
            itemHolderList[i].itemHolderGameObject.SetActive(true);
            itemHolderList[i].cost.text = purchaseList[i].price.ToString();
            itemHolderList[i].buyButton.onClick.AddListener(delegate { SetUpPopUp(purchase, purchaseList); });
            itemHolderList[i].decription.text = purchaseList[i].description;
            itemHolderList[i].itemSprite.sprite = purchase.purchaseLogo;
            currentPurchaseButtons.Add(itemHolderList[i].buyButton.gameObject);
            i++;
        }
    }


    public void OnEnterShop(in List<Purchase> _purchaseList)
    {
        for(int i=0; i < _purchaseList.Count;i++)
        {
            var purchase = _purchaseList[i];
            if (!CanBuy(purchase.price))
            {
                var image = currentPurchaseButtons[i].GetComponent<Image>();
                float h, s, v; //v = darkness value, s = saturation, h = hue aka color
                Color.RGBToHSV(purchaseButtonPrefab.GetComponent<Image>().color, out h, out s, out v);
                v *= 0.5f;
                image.color = Color.HSVToRGB(h, s, v);
            }
        }
    }

    public Color DarkenColor(Color color, float multiplier)
    {
        float h, s, v; //v = darkness value, s = saturation, h = hue aka color
        Color.RGBToHSV(color, out h, out s, out v);
        v *= multiplier;
        Debug.Log(v);
        return Color.HSVToRGB(h, s, v);
    }

    public bool CanBuy(int price)
    {
        if(inventory.GetCoinCount() >= price)
        {
            return true;
        }
        return false;
    }

    public void TryPurchase(Purchase purchase, List<Purchase> _purchaseList)
    {
        if(inventory.GetCoinCount() >= purchase.price)
        {
            Debug.Log("trying to purchase");
            //maybe add to inventory first, then from inventory do w/e
            purchase.item.OnReceived();
            inventory.IncrementCoinCount(-purchase.price);
            int index = _purchaseList.IndexOf(purchase);
          //  Debug.Log("trying to buy " + purchase.item.itemName);
            //currentPurchaseButtons[index].GetComponent<Button>().onClick.RemoveAllListeners();
            itemHolderList[index].buyButton.onClick.RemoveAllListeners();
            var buttonImage = itemHolderList[index].buyButton.GetComponent<Image>();
            buttonImage.color = Color.red;
            PopUpSetActive(false);
            AudioManager.instance.PlaySound(AudioType.SFXPurchaseItem);
            // buttonImage.color = DarkenColor(buttonImage.color,0.5f);

            // Destroy(currentPurchaseLayouts[index]);
            // currentPurchaseLayouts.RemoveAt(index);
            //purchaseList.RemoveAt(index);
            // Destroy(currentPurchaseButtons[index]);
            //currentPurchaseButtons.RemoveAt(index);
            //animation, sounds and effects here
        }
    }

    public void SetUpPopUp(Purchase purchase, List<Purchase> _purchaseList)
    {
        if (inventory.GetCoinCount() < purchase.price)
        {
            AudioManager.instance.PlaySound(AudioType.SFXFailPurchase);
            PopUpSetActive(false);
            return;
        }
        PopUpSetActive(true);
        popUp.confirmButton.onClick.RemoveAllListeners();
        popUp.confirmButton.onClick.AddListener(delegate { TryPurchase(purchase, purchaseList); });
    }

    public void ClearShopUIOld()
    {
        for(int i=0; i< currentPurchaseLayouts.Count;i++)
        {
            Destroy(currentPurchaseLayouts[i].gameObject);
        }
        currentPurchaseLayouts.Clear();
        for(int i=0; i< currentPurchaseButtons.Count; i++)
        {
            Destroy(currentPurchaseButtons[i].gameObject);
        }
        currentPurchaseButtons.Clear();
    }

    public void ClearShopUI()
    {
        foreach(var holder in itemHolderList)
        {
            holder.itemHolderGameObject.SetActive(false);
            holder.buyButton.onClick.RemoveAllListeners();
            holder.buyButton.GetComponent<Image>().color = Color.white;
        }
    }

    /// <summary>
    /// currently just pauses and unpauses time. could also make it so time isn't paused but player input is disabled in shop menu, animations e.g. playing still.
    /// </summary>
    public void ToggleShopMenu()
    {
        if (Time.timeScale == 0)
        {
            buttonPanelTransform.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Time.timeScale = 1;
            inventory.GetComponent<PlayerController>().EnableCombatActions();
        }
        else
        {
            inventory.GetComponent<PlayerController>().RemoveCombatActions();
            buttonPanelTransform.gameObject.SetActive(true);
            gameObject.SetActive(true);
            Time.timeScale = 0;
            if (currentPurchaseButtons.Count>0)EventSystem.current.SetSelectedGameObject(currentPurchaseButtons[0]);
        }
    }

    public void PopUpSetActive(bool active)
    {
        popUp.popup.SetActive(active);
    }

}

[System.Serializable]
public class ItemUIHolder
{
    public GameObject itemHolderGameObject;
    public Button buyButton;
    public Image itemSprite;
    public TMPro.TextMeshProUGUI decription;
    public TMPro.TextMeshProUGUI cost;
}

[System.Serializable]
public class PurchasePopup
{
    public GameObject popup;
    public Button confirmButton;
    public Button refuseButton;
}
