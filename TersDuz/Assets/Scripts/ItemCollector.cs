using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int gold_coin = 0;

    private SpriteRenderer sprite;
    private Color oldColor;

    [SerializeField] private Text coinsText;

    [SerializeField] private AudioSource collectionSoundEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gold Coin"))
        {
            collectionSoundEffect.Play();
            Destroy(collision.gameObject);
            gold_coin++;
            coinsText.text = "Coins: " + gold_coin;

            sprite.color = Color.Lerp(oldColor,Color.white, 0.1f);
        }
    }

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();   
        
    }

    private void Update()
    {
        oldColor = sprite.color;
    }



}
