using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int gold_coin = 0;

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
        }
    }

}
