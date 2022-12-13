using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    List<Sprite> cardSprites;

    const string pictureListURL = "https://picsum.photos/";
    const int height = 200;
    const int width = 150;

    int cardAmount;
    int cardSeed;

    void Start()
    {
        GameStartSetup();
    }


    public void GameStartSetup()
    {
        cardAmount = Random.Range(5, 10);
        cardSeed = Random.Range(1000, 9999);
        StartCoroutine(DownloadCardPics());
    }

    public IEnumerator DownloadCardPics()
    {
        cardSprites = new List<Sprite>();

        Rect spriteRect = new Rect(Vector2.zero, new Vector2(width, height));

        for (int i = 0; i < cardAmount; i++)
        {

            string downloadLink = $"{pictureListURL}/seed/{cardSeed * i}/{width}/{height}";
            Debug.Log(downloadLink);
            UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(downloadLink);

            yield return textureRequest.SendWebRequest();

            if (textureRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(textureRequest.error);
                continue;
            }
            else
            {
                Sprite cardSprite = Sprite.Create(DownloadHandlerTexture.GetContent(textureRequest), spriteRect, Vector2.one * .5f);
                cardSprites.Add(cardSprite);
            }
        }

        Debug.Log(cardSprites.Count);
    }
}
    


