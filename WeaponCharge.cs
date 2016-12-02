using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XInputDotNetPure;
public class WeaponCharge : MonoBehaviour
{
    public GameObject player;
    public GameObject myArrow;
    public GameObject animator;
    public Slider weaponSlider;
    public Image weaponSliderFill;
    private Fire fireScript;
    public const float WeaponChargeMaxValue = 3f;
    public float weaponChargeValue;
    private bool changeSpeed = false; // Delete
    bool hasArrow = false;
    bool isRolling = false;
    public bool isCharging;
    int numberController = 1;
    Color32 colorSlider;

    float timerWeapon3rdCharge = 0;
    float timeYouCanHoldArrow = 2f;
    // Use this for initialization
    void Start()
    {
        fireScript = GetComponent<Fire>();
       

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if(!GetComponentInParent<PlayerController>().isDead && !GetComponentInParent<PlayerController>().isPoweredUp)
        {
            isRolling = GetComponentInParent<PlayerController>().isRolling;
            numberController = GetComponentInParent<PlayerController>().numberController;

            if (player.GetComponent<PlayerController>().state.Triggers.Right != 0 && myArrow == null)
            {
                ChargeWeapon();
                player.GetComponent<PlayerController>().SetHalfSpeed();
                if (weaponChargeValue >= WeaponChargeMaxValue)
                {
                    timerWeapon3rdCharge += Time.deltaTime;
                    if (timerWeapon3rdCharge > timeYouCanHoldArrow)
                    {
                        player.GetComponent<PlayerController>().SetNormalSpeed();
                        isCharging = false;
                        //animator.GetComponent<AnimController>().ResetWeaponChargeTimer();
                        myArrow = fireScript.DoFire(weaponChargeValue);
                        timerWeapon3rdCharge = 0;
                        //Resets everything else 
                        weaponChargeValue = 0;
                        UpdateSlider();
                    }
                }
            }
            else
            {
                if (isCharging && myArrow == null)
                {
                    changeSpeed = false;
                    //Changes speed of player back to normal
                    player.GetComponent<PlayerController>().SetNormalSpeed();
                    isCharging = false;
                    //animator.GetComponent<AnimController>().ResetWeaponChargeTimer();
                    //Fires the arrow
                    if (weaponChargeValue >= 1)
                    {
                        myArrow = fireScript.DoFire(weaponChargeValue);
                    }
                    //Resets everything else 
                    weaponChargeValue = 0;
                    UpdateSlider();
                }
            }
            if (player.GetComponent<PlayerController>().state.Triggers.Right != 0 && myArrow == null && isRolling == true)
            {
                myArrow = fireScript.DoFire(1.1f);
                player.GetComponent<PlayerController>().SetNormalSpeed();
            }
        }
        
    }

    public GameObject MyArrow
    {
        get { return myArrow; }
        set { myArrow = value; }
    }

    void ChargeWeapon()
    {
        if (weaponChargeValue < WeaponChargeMaxValue)
        {
            weaponChargeValue = weaponChargeValue + Time.deltaTime;
            isCharging = true;
        }
        UpdateSlider();
    }

    public void UpdateSlider()
    {
        ChangeSliderColor();
        weaponSlider.value = weaponChargeValue/ WeaponChargeMaxValue;
    }

    void ChangeSliderColor()
    {
        if (weaponChargeValue < (WeaponChargeMaxValue * 1 / 3))
        {
            colorSlider = Color.white;
        }
        if (weaponChargeValue >= (WeaponChargeMaxValue * 1 / 3))
        {
            colorSlider = Color.green;
        }
        if (weaponChargeValue >= (WeaponChargeMaxValue * 2 / 3))
        {
            colorSlider = Color.yellow;
        }
        if (weaponChargeValue >= (WeaponChargeMaxValue * 3 / 3))
        {
            colorSlider = Color.red;
        }
        colorSlider.a = 50;
        weaponSliderFill.color = colorSlider;
    }

    void OnGUI()
    {
        // GUILayout.Label("trigger:" + weaponChargeValue);
    }
}