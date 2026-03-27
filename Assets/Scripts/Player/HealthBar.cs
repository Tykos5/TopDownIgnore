using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour // handles players health bar UI
{

    public Slider slider;

    public void SetMaxHealth(int health) // initializes starting value
    {
        slider.maxValue = health;
        slider.value = health;
    }
    public void SetHealth(int health) // updated slider to show correct amount of hp, called when taking dmg
    {
        slider.value = health;
    }
}
