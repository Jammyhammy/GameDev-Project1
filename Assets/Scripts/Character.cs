using UnityEngine;
using System.Collections;


public abstract class Character : MonoBehaviour
{
    public int startingHealth;
    public int currentHealth;
    public bool isDead;

    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    public virtual void Start()
    {
		currentHealth = startingHealth;
        isDead = false;
    }

    public virtual void TakeDamage (int amount)
    {
        if(isDead)
            return;
        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            isDead = true;
        }
    }
}