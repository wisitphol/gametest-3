using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Photon.Pun;


public class DisplayCard2 : MonoBehaviour
{
    public List<Card> displayCard = new List<Card>();
    public int displayId;

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform originalParent;

    public int Id;
    public string LetterType;
    public string ColorType;
    public string AmountType;
    public string FontType;
    public int Point;
    public Sprite Spriteimg;
    private Text IdText;
    public Image ArtImage;
    public bool cardBack;
    public static bool staticCardBack;
    public GameObject Board;
    public int numberOfCardInDeck;
    private int retryCount = 0;  // กำหนดตัวนับการเช็คซ้ำ
    private int maxRetries = 3;  // กำหนดจำนวนครั้งสูงสุดที่ต้องการเช็คซ้ำ
    private float retryDelay = 0.5f;  // ระยะเวลารอระหว่างการเช็คซ้ำ (0.5 วินาที)



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckAndLoadCardData());
    }

    private IEnumerator CheckAndLoadCardData()
    {
        while (retryCount < maxRetries)
        {
            if (CardData.cardList != null && CardData.cardList.Count > 0)
            {
                // โหลดข้อมูลหาก CardData.cardList ไม่เป็น null และมีข้อมูล
                numberOfCardInDeck = Deck2.deckSize;

                if (displayId >= 0 && displayId < CardData.cardList.Count)
                {
                    displayCard.Add(CardData.cardList[displayId]);
                    yield break;  // จบ Coroutine เมื่อโหลดข้อมูลสำเร็จ
                }
                else
                {
                    Debug.LogError("displayId is out of range.");
                    yield break;
                }
            }

            retryCount++;
            Debug.LogWarning("Retry loading CardData.cardList: Attempt " + retryCount);
            yield return new WaitForSeconds(retryDelay);  // รอเวลาตาม retryDelay ก่อนตรวจสอบใหม่
        }

        // หากตรวจสอบครบจำนวนครั้งแล้วยังไม่มีข้อมูล จะแสดง error
        Debug.LogError("CardData.cardList is null or empty.");
    }

    // Update is called once per frame
    void Update()
    {
        if (displayCard.Count > 0)
        {
            DisplayCardData(displayCard[0]);

            if (Board == null)
            {
                Board = GameObject.Find("Boardzone");
            }

            if (this.transform.parent == Board.transform.parent)
            {
                cardBack = false;
            }

            staticCardBack = cardBack;

            if (this.tag == "Clone" && numberOfCardInDeck > 0)
            {
                if (numberOfCardInDeck <= Deck2.staticDeck.Count && numberOfCardInDeck > 0)
                {
                    displayCard[0] = Deck2.staticDeck[numberOfCardInDeck - 1];
                    numberOfCardInDeck--;
                    Deck2.deckSize--;
                    cardBack = false;
                    this.tag = "Untagged";
                }
                else
                {
                    Debug.LogError("numberOfCardInDeck is out of range.");
                }
            }
        }
        else
        {
            // ตรวจสอบซ้ำก่อนจะแสดง error
            if (retryCount < maxRetries)
            {
                retryCount++;
                Debug.LogWarning("Retry loading displayCard: Attempt " + retryCount);
                return;  // ข้ามการทำงานในรอบนี้และรอรอบถัดไป
            }

            // หากตรวจสอบซ้ำครบ maxRetries ครั้งแล้วยังไม่มีข้อมูลให้แสดง error
            Debug.LogError("displayCard is empty.");
        }
    }

    public void DisplayCardData(Card card)
    {
        if (card != null && displayCard.Count > 0)
        {
            // ตรวจสอบขนาดของ displayCard ก่อนเข้าถึงดัชนี
            if (displayCard.Count > 0)
            {
                Id = displayCard[0].Id;
                LetterType = displayCard[0].LetterType;
                ColorType = displayCard[0].ColorType;
                AmountType = displayCard[0].AmountType;
                FontType = displayCard[0].FontType;
                Point = displayCard[0].Point;
                Spriteimg = displayCard[0].Spriteimg;

                //IdText.text = " " + Id;
                ArtImage.sprite = Spriteimg;
            }
            else
            {
                Debug.LogError("displayCard is empty.");
            }
        }
        else
        {
            Debug.LogError("Card is null or displayCard is empty.");
        }
    }

    public void SetBlocksRaycasts(bool blocksRaycasts)
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = blocksRaycasts;
        }
        else
        {
            Debug.LogError("CanvasGroup component missing.");
        }
    }

}
