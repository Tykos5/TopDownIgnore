using UnityEngine;

public class StaticData
{
    public static bool canSwapScene = true;

    public static int difficulty = 0; // Default difficulty level
    public static float playerHealth = 10; // Default player max health
    public static bool canSpear = false; // Whether the player can use the spear attack


    public static int zombieDamage = 1; // Default damage for zombies
    public static int zombieHealth = 3; // Default health for zombies

    public static int meleeReaperDamage = 2; // Default damage for melee reapers
    public static int meleeReaperHealth = 7; // Default health for melee reapers
    
    public static int rangedReaperDamage = 1; // Default damage for ranged reapers
    public static int rangedReaperHealth = 5; // Default health for ranged reapers
    public static int orbDespawnTime = 5; // Time in seconds before orbs despawn

    public static int rockBossHealth = 15; // Default health for rock boss
    public static int RockBossMeleeDMG = 3; // Default melee damage for rock boss
    public static int RockBossRangedDMG = 2; // Default ranged damage for rock boss
    public static int RockBossOrbDespawnTime = 6; // Time in seconds before rock boss orbs despawn
    public static float RockBossRangedCD = 5f; // Cooldown time in seconds for rock boss ranged attack

    public static int score = 0; // Player's score
}