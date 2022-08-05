using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0F;
    [SerializeField]
    private int powerUpID;

    [SerializeField]
    private AudioClip _clip;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < -6.57F)
        {
            Destroy(this.gameObject);    
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with: " + other.name);
        if (other.tag == "Player")
        {
            // access the player
            Player player = other.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 1F);
            // check if the component we're looking for, exists or not
            // after creating handle, check we actually found our component
            if (player != null)
            {
                if (powerUpID == 0)
                {
                    // enable the triple shot
                    player.TripleShotPowerUpOn();
                }
                else if (powerUpID == 1)
                {
                    // enable speed boost
                    player.SpeedBoostPowerUpOn();
                }
                else if(powerUpID == 2)
                {
                    // enable shield
                    player.EnableShields();
                }
            }     

            StartCoroutine(player.TripleShotPowerDownRoutine());

            // and in the end, we destroy ourself
            Destroy(this.gameObject);  
        }
    }
}
