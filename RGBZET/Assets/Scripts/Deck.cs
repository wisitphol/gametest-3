using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Deck : MonoBehaviour
{
    public List<Card> container = new List<Card>();
    public List<Card> deck = new List<Card>();
    public int x;
    public static int deckSize;
    public static List<Card> staticDeck = new List<Card>();

    public GameObject CardInDeck;

    public GameObject CardToHand;
    public GameObject[] Clones;
    public GameObject Hand;

    // Start is called before the first frame update
    void Start()
    {
        x = 0;
        deckSize = 80;
        deck = new List<Card>(new Card[deckSize]);

        for(int i = 0; i < deckSize; i++)
        {
            x = Random.Range(0,26);
            deck[i] = CardData.cardList[x];
        }
        
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        staticDeck = deck;


        if(deckSize < 10)
        {
            CardInDeck.SetActive(false);
        }
        
        if(TurnSystem.startTurn == true)
        {
            StartCoroutine(Draw(1));
            TurnSystem.startTurn = false;
        }
    }

    IEnumerator StartGame()
    {
        for(int i = 0;i <= 3;i++)
        {
            yield return new WaitForSeconds(1);

            Instantiate(CardToHand, transform.position, transform.rotation);
        }
    }


    public void Shuffle()
    {
        for(int i = 0; i < deckSize; i++)
        {
            container[0] = deck[i];
            int randomIndex = Random.Range(i,deckSize);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = container[0];
        }
    }

    IEnumerator Draw(int x)
    {
        for(int i = 0; i < x; i++)
        {
            yield return new WaitForSeconds(1);

            Instantiate(CardToHand, transform.position, transform.rotation);
        }
    }

}
