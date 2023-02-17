using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    List<Sprite> cardSprites;
    public static GameManager Instance;

    const string pictureListURL = "https://picsum.photos/";
    const int cardHeight = 200;
    const int cardWidth = 150;

    int cardAmount;
    int cardSeed;

    [SerializeField] Transform cardParent;
    [SerializeField] CardContainer cardPrefab;

    [SerializeField] Sprite backupSprite;

    [SerializeField] Transform cardCanvas, loadingCanvas;
    public Transform CardCanvas => cardCanvas;



    [SerializeField] Image loadingBar;
    [SerializeField] TextMeshProUGUI loadingText;


    //could do a state machine but meh, this works
    GameState currentState;
    public GameState CurrentState => currentState;

    List<CardContainer> cardContainers = new List<CardContainer>();

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentState = GameState.Busy;
        loadingCanvas.gameObject.SetActive(true);
        GameStartSetup();
    }

    public void GameStartSetup()
    {
        RandomizeCards();
        StartCoroutine(DownloadCardPics());
    }

    public void RandomizeCards()
    {
        cardAmount = Random.Range(4, 6);
        cardSeed = 1 + (int)(Random.value * 10000);
    }

    public void SpawnCards()
    {
        loadingCanvas.gameObject.SetActive(false);
        StartCoroutine(SpawnCardsOverTime());
    }

    public IEnumerator DownloadCardPics()
    {
        cardSprites = new List<Sprite>();

        Rect spriteRect = new Rect(Vector2.zero, new Vector2(cardWidth, cardHeight));

        for (int i = 0; i < cardAmount; i++)
        {
            loadingText.text = $"Loading card sprites {i + 1}/{cardAmount}";

            LeanTween.cancel(loadingBar.gameObject);
            LeanTween.value(loadingBar.gameObject, loadingBar.fillAmount, (i + 1) / (float)cardAmount, .1f)
                .setOnUpdate((float newFill) =>
            {
                loadingBar.fillAmount = newFill;
            });

            string downloadLink = $"{pictureListURL}/seed/{cardSeed * i}/{cardWidth}/{cardHeight}";
            UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(downloadLink);

            yield return textureRequest.SendWebRequest();

            if (textureRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogWarning(textureRequest.error);
                // add placeholder sprite instead
                cardSprites.Add(backupSprite);
                continue;
            }
            else
            {
                Sprite cardSprite = Sprite.Create(DownloadHandlerTexture.GetContent(textureRequest), spriteRect, Vector2.one * .5f);
                cardSprites.Add(cardSprite);
            }
        }

        SpawnCards();
    }

    public IEnumerator SpawnCardsOverTime()
    {
        Vector3 scaleFrom = Vector3.zero;
        scaleFrom.z = 1f;
        foreach (Sprite sprite in cardSprites)
        {
            CardContainer container = Instantiate(cardPrefab, cardParent);
            cardContainers.Add(container);
            Card card = new Card(Random.Range(1, 10), Random.Range(1, 10), Random.Range(1, 10));
            container.SetupCard(card, sprite);
            container.transform.localScale = scaleFrom;

            LeanTween.scale(container.gameObject, Vector3.one, .2f).setEaseOutSine();
            yield return new WaitForSeconds(.5f);
        }


        //let player actually play
        currentState = GameState.PlayerTurn;
    }

    public Vector3 MiddleOfTheScreen()
    {
        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        return center;
    }

    //"GAMEPLAY"
    public void DamageAll(bool damageOnly = false)
    {
        if (currentState == GameState.Busy) return;
        currentState = GameState.Busy;
        StartCoroutine(EnemyTurn(damageOnly));
    }

    public IEnumerator EnemyTurn(bool damageOnly)
    {
        for (int i = cardContainers.Count - 1; i >= 0; i--)
        {
            //TODO possinly skip method entirely if value is 0

            if (damageOnly)
            {
                cardContainers[i].ChangeValue(ValueType.Health, Random.Range(-1, -2));
            }
            else
            {
                cardContainers[i].ChangeValue((ValueType)Random.Range(0, 3), Random.Range(-2, 9));
            }

            
            yield return new WaitUntil(() => !cardContainers[i].Used);
        }

        currentState = GameState.PlayerTurn;
    }

    public void RemoveCard(CardContainer container)
    {
        cardContainers.Remove(container);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

//obviously add more states if it's a real game, duh
public enum GameState
{
    PlayerTurn,
    Busy
}
    


