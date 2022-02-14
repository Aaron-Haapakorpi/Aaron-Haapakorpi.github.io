using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// used as a script to contain references to all objects, e.g. players, enemies etc for easier handling
/// currently used for references to player, combat ui and object pools for bullet particles and text
/// </summary>
public class GlobalReferences : MonoBehaviour
{
    // Start is called before the first frame update
    public static GlobalReferences instance;

    
    public List<GameObject> players = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();
    public Room currentRoom;
    public RoomGrid roomGrid;
    public GameObject rain;
    public int bossesKilled;
    public GameObject trees;
    

    //not used currently, useful if there's multiple players and need easy references.
    public List<PlayerScriptHolder> playerTest = new List<PlayerScriptHolder>();

    //not currently used
    public Dictionary<string, GameObject> canvasElements;

    [HideInInspector]
    public ParticleSystem sparkTest;
    [HideInInspector]
    public ObjectPool sparkPool;
    [HideInInspector]
    public ObjectPool laserSparkPool;
    [HideInInspector]
    public ObjectPool laserParticlePool;
    [HideInInspector]
    public ObjectPool bossLaserSparkPool;
    [HideInInspector]
    public ObjectPool bossLaserParticlePool;

    public CombatUiReferences combatUi;

    public DeathUI deathUI;

    public PauseMenu pauseMenu;

    [HideInInspector]
    public ObjectPool damageCombatTextPool;

    public ShopMenu shopMenu;

    [HideInInspector]
    public PlayerInventory inventory;

    private const float combatTextMinLimit = 1;

    private float currentCombatTextDamage = 0;

 
    private void Awake()
    {
        instance = this;
        
    }

    public void InstantiateCombatText(string text,Vector3 pos)
    {
        var t = damageCombatTextPool.GetNext();
        if (t.activeSelf == true)
        {
            var newText = Instantiate(t);
            newText.SetActive(false);
            damageCombatTextPool.AddObject(newText);
        }
        t.GetComponent<DamageCombatText>().SetText(text);
        t.transform.position = pos + UnityEngine.Random.insideUnitSphere;
        t.SetActive(true);
    }

    public void InstantiateCombatTextAdditive(float damage, Vector3 pos)
    {
        currentCombatTextDamage += damage;
        if (currentCombatTextDamage > 1)
        {
            var t = damageCombatTextPool.GetNext();
            if (t.activeSelf == true)
            {
                var newText = Instantiate(t);
                newText.SetActive(false);
                damageCombatTextPool.AddObject(newText);
            }
            t.GetComponent<DamageCombatText>().SetText(System.Math.Round((currentCombatTextDamage), 0).ToString());
            t.transform.position = pos + UnityEngine.Random.insideUnitSphere;
            t.SetActive(true);
            currentCombatTextDamage = 0;
        }
    }

}
public class PlayerScriptHolder
{
    public Guid playerID; //might use an id to differentiate between multiple players
    public PlayerShooting shooting;
    public PlayerMovement movement;
    public PlayerInventory inventory;
    public PlayerDamageable damageable;
    public PlayerAnimation animation;
    public PlayerController controller;
}

[System.Serializable]
public class CombatUiReferences
{
    public TMPro.TextMeshProUGUI clipBulletCountText;
    public TMPro.TextMeshProUGUI bulletDividerText;
    public TMPro.TextMeshProUGUI reserveBulletCountText;
    public Image bulletFillImage;
    public Image reserveBulletFillImage;
    public TMPro.TextMeshProUGUI healthCountText;
    public Image healthFillImage;
    public TMPro.TextMeshProUGUI healthDividerText;
    public TMPro.TextMeshProUGUI coinCountText;
    public TMPro.TextMeshProUGUI keyCountText;
    public Image weaponCooldownImage;
    public Image weaponUIImage;
}
