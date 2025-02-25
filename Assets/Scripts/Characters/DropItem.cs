using UnityEngine;

// This class holds a drop table and handles dropping items
public class DropItem : MonoBehaviour
{
    // The chance to drop any item at all
    public float chanceToNotDrop = 0.5f;

    // Reference to the drop table
    private DropTable dropTable;

    private void Start()
    {
        dropTable = GetComponent<DropTable>();
    }

    // Roll for a drop and drop a random item from the drop table
    public void Drop()
    {
        float dropRoll = Random.value;

        if (Debug.isDebugBuild)
        {
            Debug.Log($"{gameObject.name} rolled {dropRoll:F2} / {chanceToNotDrop:F2} to drop an item.");
        }

        if (dropRoll < chanceToNotDrop)
        {
            if (Debug.isDebugBuild)
            {
                Debug.Log($"{gameObject.name} did not drop an item.");
            }
            return;
        }

        GameObject item = dropTable.GetItem();
        if (item != null)
        {
            Instantiate(item, transform.position, Quaternion.identity);
        }
        
        if (Debug.isDebugBuild)
        {
            Debug.Log($"{gameObject.name} dropped {item.name}");
        }
    }
}
