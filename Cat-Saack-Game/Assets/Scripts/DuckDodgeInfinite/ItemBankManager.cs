using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBankManager : MonoBehaviour
{
    public static ItemBankManager Instance;

    public int maxBankSize = 5;

    List<Collectible> collectibleBank = new List<Collectible>();
    List<Enemy> enemyBank = new List<Enemy>();

    void Awake()
    {
        Instance = this;
    }
    
    // Attemps to add an object to an item bank. If object is not of the supported types or the buffer is full return false
    public static bool AddObjectToBank(GameObject aObject)
    {
        bool success = false;
        if (aObject.GetComponent<Collectible>() != null)
        {
            if (Instance.collectibleBank.Count < Instance.maxBankSize)
            {
                Instance.collectibleBank.Add(aObject.GetComponent<Collectible>());
                success = true;
            }
        }
        if (aObject.GetComponent<Enemy>() != null)
        {
            if (Instance.enemyBank.Count < Instance.maxBankSize)
            {
                Instance.enemyBank.Add(aObject.GetComponent<Enemy>());
                success = true;
            }
        }
        // if operation successful move it to the bank and set it to be inactive
        if (success)
        {
            aObject.transform.position = MinigameManagerDuck.Instance.itemStorageBank;
            aObject.gameObject.SetActive(false);
        }

        return success;
    }

    // If given type has a bank and the bank has at least one object in it
    // Grab a random object from the bank and return it, and remove it from the bank
    public static GameObject GrabObjectFromBank<T>() where T : MonoBehaviour
    {
        List<T> bank = null;

        // which bank to use based on type
        if (typeof(T) == typeof(Collectible))
        {
            bank = Instance.collectibleBank as List<T>;
        }
        else if (typeof(T) == typeof(Enemy))
        {
            bank = Instance.enemyBank as List<T>;
        }

        if (bank == null || bank.Count == 0)
        {
            return null;
        }

        // select random object from the bank
        int randomIndex = Random.Range(0, bank.Count);
        T grabbedObject = bank[randomIndex];

        bank.RemoveAt(randomIndex);

        GameObject obj = grabbedObject.gameObject;
        return obj;
    }
}
