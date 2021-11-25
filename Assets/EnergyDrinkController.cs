using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrinkController : MonoBehaviour
{
    [SerializeField] private float Speed;
    private Rigidbody2D body;
    private GameObject player;
    private GameObject energyDrink;
    private AudioSource audioSource;
   

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        energyDrink = GameObject.Find("EnergyDrink");
        audioSource = player.GetComponent<AudioSource>();
    }

    private void Update()
    {

    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerController>().BoostSpeed(9);
            audioSource.Play();
            Destroy(gameObject);
        }
    }
}
