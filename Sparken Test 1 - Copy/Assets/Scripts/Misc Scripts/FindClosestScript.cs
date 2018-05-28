using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A simple function to find the closest character of a given type in a given range from a given character
public class FindClosestScript {

    float radius; // Range to look in

    // Get the closest gameobject by object
    public GameObject GetClosestObject(GameObject user, string findingTag, float setRadius)
        {
            radius = setRadius;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(user.transform.position, radius); // Get all colliders that overlap within the range
            Collider2D closestCollider = null; // Set the closest currently to null

            foreach (Collider2D hit in colliders) {
            // Finds all colliders of the given tag
            if (hit.gameObject.tag == findingTag)
            {
                //checks if it's hitting itself
                if (hit == user.GetComponent<Collider2D>())
                {
                    continue;
                }
                //Sets it to the first found if it is null
                if (closestCollider == null)
                {
                    closestCollider = hit;
                }
                //compares distances and updates
                if (Vector3.Distance(user.transform.position, hit.transform.position) <= Vector3.Distance(user.transform.position, closestCollider.transform.position))
                {
                    closestCollider = hit;
                }
            }
     }
            // Returns the closest if it isn't null
            if (closestCollider != null)
                return closestCollider.gameObject;
            return null;
 }

    // get the closest gameobject by name
    public GameObject GetClosestObjectByName(GameObject user, string findingName, float setRadius)
    {
        radius = setRadius;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(user.transform.position, radius); // Get all colliders that overlap within the range
        Collider2D closestCollider = null; // Set the closest currently to null

        foreach (Collider2D hit in colliders)
        {
            // Finds all colliders of the given name
            if (hit.gameObject.name == findingName)
            {
                if (closestCollider == null)
                {
                    closestCollider = hit;
                }
                //compares distances and updates the closest
                if (Vector3.Distance(user.transform.position, hit.transform.position) <= Vector3.Distance(user.transform.position, closestCollider.transform.position))
                {
                    closestCollider = hit;
                }
            }
        }
        // Returns the closest if it isn't null
        if (closestCollider != null)
            return closestCollider.gameObject;
        return null;
    }
}
