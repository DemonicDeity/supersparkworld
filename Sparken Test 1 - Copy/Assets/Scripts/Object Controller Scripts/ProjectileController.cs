using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the creation of Projectiles
public class ProjectileController : MonoBehaviour {

    public GameObject lightningBall; // Will create a lightningball if it is attached
    public GameObject sparkShine; // Will create a sparkshine if it is attached
    public GameObject seismicTremorWave; // Will create a seismictremorwave if it is attached
    public GameObject lightningConductor;
    Vector2 offset; // Variable offset for each projectile
    Vector2 velocity; // Variable velocity of the projectile
    Vector2 knockBackSenderSelf; // Sender for knockback to the projectile creator

    public void fireThunderballDrop()
    {
        offset = new Vector2(0.4f, 0.0f); // Sets offset for the lightningball
        velocity = new Vector2(5f*transform.localScale.x, -5f); // Sets velocity for the lightningball
        //Creates a lightningball with the correct position and velocity
        GameObject lightningBallGO = Instantiate(lightningBall, (Vector2) transform.position + offset * transform.localScale.x, Quaternion.identity);
        lightningBallGO.GetComponent<Rigidbody2D>().velocity = velocity;

        // Sends knockback to the Sparken and pushes it backwards depending on the direction the lightningball is fired
        knockBackSenderSelf = new Vector2(-2 * transform.localScale.x, 2);
        gameObject.SendMessage("applySelfKnockBack", knockBackSenderSelf, SendMessageOptions.DontRequireReceiver);
    }
    public void fireForcefulLightningBall()
    {
        offset = new Vector2(0.4f, 0.0f); // Sets offset for the lightningball
        velocity = new Vector2(5f * transform.localScale.x, 0); // Sets velocity for the lightningball
        //Creates a lightningball with the correct position and velocity
        GameObject lightningBallGO = Instantiate(lightningBall, (Vector2)transform.position + offset * transform.localScale.x, Quaternion.identity);
        lightningBallGO.GetComponent<Rigidbody2D>().velocity = velocity;
    }

    public void createSparkShine()
    {
        offset = new Vector2(0.0f, 0.0f); // Sets offset for the sparkshine
        velocity = new Vector2(0, 0); // Sets velocity for the sparkshine
        //Creates a sparkshine with the correct position and velocity
        GameObject sparkShineGO = Instantiate(sparkShine, (Vector2)transform.position + offset * transform.localScale.x, Quaternion.identity);
        //sparkShineGO.GetComponent<Rigidbody2D>().velocity = velocity;
    }
    public void createSeismicTremor()
    {
        offset = new Vector2(0.0f, -0.3f); // Sets offset for the seismictremor
        velocity = new Vector2(0, 0); // Sets velocity for the seismictremor
        //Creates a seismictremor with the correct position and velocity
        GameObject seismicTremorWaveGO = Instantiate(seismicTremorWave, (Vector2)transform.position + offset, Quaternion.identity);
        seismicTremorWaveGO.GetComponent<SeismicTremorWaveControllerScript>().leftOrRight = (int) transform.localScale.x;
    }
    public void createLightningConductor()
    {
        offset = new Vector2(0.0f, 0.0f); // Sets offset for the seismictremor
        velocity = new Vector2(0, 0); // Sets velocity for the seismictremor
        //Creates a Lightning Conductor with the correct position and velocity
        GameObject lightningConductorGO = Instantiate(lightningConductor, (Vector2)transform.position + offset, Quaternion.identity);
    }

}
