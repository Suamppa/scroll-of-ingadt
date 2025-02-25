using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : Stats
{
    public delegate void PlayerHealthChanged(int newHealth);
    public delegate void PlayerShieldChanged(int newShield);
    public delegate void PlayerStatusEffect<T>(T effect);
    public delegate void PlayerWeapon<T>(T weapon);
    public event PlayerHealthChanged OnPlayerDamaged;
    public event PlayerHealthChanged OnPlayerHealed;
    // public event PlayerShieldChanged OnPlayerShieldGained;
    public event PlayerShieldChanged OnPlayerShieldLost;
    public event PlayerStatusEffect<TemporaryPickup> OnPlayerStatus;
    public event PlayerStatusEffect<TempShield> OnPlayerTempShield;
    public event PlayerWeapon<WeaponPickup> OnPlayerWeaponPickup;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (equippedWeapon != null)
        {
            OnPlayerWeaponPickup?.Invoke(equippedWeapon);
        }
    }

    public override void ChangeWeapon()
    {
        base.ChangeWeapon();
        OnPlayerWeaponPickup?.Invoke(equippedWeapon);
    }

    public override void TakeDamage(int incomingDamage)
    {
        if (Shield > 0)
        {
            ShieldDamage(incomingDamage);
        }
        else
        {
            HealthDamage(incomingDamage);
        }
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    protected override int HealthDamage(int incomingDamage)
    {
        int effectiveDamage = base.HealthDamage(incomingDamage);
        // Invoke the OnPlayerDamaged event
        OnPlayerDamaged?.Invoke(CurrentHealth);
        return effectiveDamage;
    }

    // Override the Die() function to add a game over screen
    public override void Die()
    {
        // Death sounds, animations, respawn logic etc. can go here

        if (Debug.isDebugBuild)
        {
            Debug.Log($"{gameObject.name} died.");
        }

        SceneManager.LoadScene("DeathScreen", LoadSceneMode.Single);
    }

    public override void Heal(int healAmount)
    {
        base.Heal(healAmount);
        // Invoke the OnPlayerHealed event
        OnPlayerHealed?.Invoke(CurrentHealth);
    }

    public override void ReduceShield(int shieldAmount)
    {
        base.ReduceShield(shieldAmount);
        // Invoke the OnPlayerShieldLost event
        OnPlayerShieldLost?.Invoke(Shield);
    }

    public override void AddEffect(TemporaryPickup effect)
    {
        base.AddEffect(effect);
        if (effect is TempShield tempShield)
        {
            // HealthBar will listen for this event
            OnPlayerTempShield?.Invoke(tempShield);
        }
        else
        {
            // StatusBar will listen for this event
            OnPlayerStatus?.Invoke(effect);
        }
    }
}
