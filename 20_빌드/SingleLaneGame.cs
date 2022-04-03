using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SingleLaneGame : MonoBehaviour
{
    public SingleLanePlayer me;
    public SingleLanePlayer you;
    bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        me.Initialize(false);
        you.Initialize(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // �� ���� Ŭ��
    public void ClickEndTurn()
    {
        if (!gameOver)
        {
            if (me.CheckSelectedCard())
            {
                SetButton(false, false);                
                StartCoroutine("Battle");
            }
            else
            {
                Debug.Log("Card not selected");
            }
        }
    }

    // �α��� �޴��� �̵�
    public void LoginMenu()
    {
        SceneManager.LoadScene("LoginMenu");
    }

    // ����
    private IEnumerator Battle()
    {        
        you.AISelectCard();
        yield return StartCoroutine(me.StopSelectedAnimation());
        me.Ready();
        you.Ready();
        yield return new WaitForSeconds(1f);

        int you_damage = me.GetDamage(you);
        int me_damage = you.GetDamage(me);
        StartCoroutine(me.Fight(me_damage));
        yield return StartCoroutine(you.Fight(you_damage));
        yield return new WaitForSeconds(0.5f);

        CheckGameOver();
        SetButton(true, true);
        yield return null;
    }

    // �������� Ȯ��
    private bool CheckGameOver()
    {
        int me_life = me.GetLife();
        int you_life = you.GetLife();

        if (me_life <= 0 || you_life <= 0)
        {
            GameObject canvas = GameObject.Find("Canvas");
            GameObject gameover_object = canvas.transform.Find("GameOver").gameObject;
            Text gameover_text = gameover_object.transform.Find("Text").GetComponent<Text>();
            if (me_life <= 0 && you_life <= 0)
            {
                gameover_text.text = "Draw";
            }
            else if (me_life <= 0)
            {
                gameover_text.text = "Lose";
            }
            else
            {
                gameover_text.text = "Win";
            }
            gameover_object.SetActive(true);
            gameOver = true;

            return true;
        }

        return false;
    }

    // ��ư Ȱ�� ����
    private void SetButton(bool left_ClickEnabled, bool right_ClickEnabled)
    {
        GameObject.Find("EndTurn").GetComponent<Button>().interactable = left_ClickEnabled;
        List<int> remain_cards = me.GetRemainCards();
        GameObject me_object = GameObject.Find("Me");
        foreach (var card in remain_cards)
        {
            string card_name = "Card_" + card.ToString();
            GameObject card_object = me_object.transform.Find(card_name).gameObject;
            card_object.GetComponent<Button>().interactable = left_ClickEnabled;
            card_object.GetComponent<Card>().InfoTextSetActive(false, right_ClickEnabled);
        }
    }
}
