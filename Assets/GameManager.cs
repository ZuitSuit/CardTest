using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    List<Sprite> cardSprites;

    const string pictureListURL = "https://picsum.photos/";
    const int cardHeight = 200;
    const int cardWidth = 150;

    int cardAmount;
    int cardSeed;

    [SerializeField] Transform cardParent;
    [SerializeField] CardContainer cardPrefab;

    [SerializeField] Transform cardCanvas;
    public Transform CardCanvas => cardCanvas;

    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameStartSetup();
    }



    public void GameStartSetup()
    {
        cardAmount = Random.Range(4, 6);
        cardSeed = Random.Range(1000, 9999);
        StartCoroutine(DownloadCardPics());
    }

    public void SpawnCards()
    {
        foreach(Sprite sprite in cardSprites)
        {
            CardContainer container = Instantiate(cardPrefab, cardParent);
            Card card = new Card(Random.Range(3, 5), Random.Range(3, 5), Random.Range(3, 5));
            container.SetupCard(card, sprite);
        }
    }

    public IEnumerator DownloadCardPics()
    {
        cardSprites = new List<Sprite>();

        Rect spriteRect = new Rect(Vector2.zero, new Vector2(cardWidth, cardHeight));

        for (int i = 0; i < cardAmount; i++)
        {

            string downloadLink = $"{pictureListURL}/seed/{cardSeed * i}/{cardWidth}/{cardHeight}";
            //Debug.Log(downloadLink);
            UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(downloadLink);

            yield return textureRequest.SendWebRequest();

            if (textureRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogWarning(textureRequest.error);
                continue;
            }
            else
            {
                Sprite cardSprite = Sprite.Create(DownloadHandlerTexture.GetContent(textureRequest), spriteRect, Vector2.one * .5f);
                cardSprites.Add(cardSprite);
            }

            Debug.Log(i + "ww");
        }

        SpawnCards();
    }
}
    


