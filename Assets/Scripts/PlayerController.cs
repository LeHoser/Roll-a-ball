using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    public TextMeshProUGUI pointsText;
    public GameObject winTextObject;
    public GameObject notEnoughPointsText;
    public GameObject gameOverMan;

    private Rigidbody rb;
    [SerializeField]
    private int playerScore;
    private float movementX;
    private float movementY;
    private float timeShown;

    void Start()
    {
        playerScore = 0;
        
        rb = GetComponent<Rigidbody>();

        winTextObject.SetActive(false);

        notEnoughPointsText.SetActive(false);

        gameOverMan.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetPoints()
    {
        pointsText.text = "Count: " + playerScore.ToString();

        if(playerScore >= 18)
        {
            winTextObject.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0, movementY);

        rb.AddForce(movement * playerSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            playerScore += 1;
            SetPoints();
        }

        if(other.CompareTag("GoToZone2"))
        {
            transform.position = new Vector3(-6.16f, -0.3611679f, 70.16f);
            rb.velocity = Vector3.zero;
        }

        if(other.CompareTag("GoToZone1"))
        {
            transform.position = new Vector3(0, -0.4f, 35.27f);
            rb.velocity = Vector3.zero;
        }

        if(other.CompareTag("GoToZone3") && playerScore >= 12)
        {
            transform.position = new Vector3(43.2f, 30.5f, 104.1f);
        }

        if(other.CompareTag("GoToZone3") && playerScore <= 11)
        {
            notEnoughPointsText.SetActive(true);
            StartCoroutine(NotEnoughPoints(5, notEnoughPointsText));
        }

        if(other.CompareTag("FallZone"))
        {
            transform.position = new Vector3(0, -.4f, 0);
            rb.velocity = Vector3.zero;
        }

        if(other.CompareTag("GameOverZone"))
        {
            gameOverMan.SetActive(true);
            StartCoroutine(GameOverMan(3, gameOverMan));
            StartCoroutine(GameClose(3));
        }
    }

    IEnumerator NotEnoughPoints(int seconds, GameObject text)
    {
        yield return new WaitForSeconds(seconds);
        text.SetActive(false);
    }

    IEnumerator GameOverMan(int seconds, GameObject text)
    {
        yield return new WaitForSeconds(seconds);
        text.SetActive(false);
    }

    IEnumerator GameClose(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Application.Quit();
    }
}
