using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleLanePlayer : MonoBehaviour
{
    public GameObject card;
    public GameObject canvas;
    SingleLaneElement singleLaneElement;

    void Awake()
    {
        singleLaneElement = new SingleLaneElement();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClickCard()
    {
        string selected_card_name = EventSystem.current.currentSelectedGameObject.name;
        singleLaneElement.selectedCard = selected_card_name;
        Debug.Log(selected_card_name + " Selected");
    }

    public void ClickEndTurn()
    {
        if (singleLaneElement.selectedCard != "")
        {
            string selected_card_name = singleLaneElement.selectedCard;
            GameObject selected_object = GameObject.Find(selected_card_name);
            // ���õ� ī�� �̵�
            selected_object.transform.position = new Vector3(0, 0, 0);
            Debug.Log(selected_card_name + " Moved to (0,0,0)");
            // ���õ� ī���� Ÿ�� ��������
            GameObject text_object = selected_object.transform.Find("Text").gameObject;
            int card_damage = int.Parse(text_object.GetComponent<Text>().text);
            // ������
            singleLaneElement.life -= card_damage;
            GameObject.Find("Score").GetComponent<Text>().text = singleLaneElement.life.ToString();
            Debug.Log("Life damaged " + card_damage);
            Debug.Log("Current Life " + singleLaneElement.life);
            // ���õ� ī�� �տ��� ����
            string card_num = selected_card_name.Substring(selected_card_name.Length - 1);
            singleLaneElement.handCard.Remove(int.Parse(card_num));
            // �տ� ���� ī�� ����
            int num_of_remain_cards = singleLaneElement.handCard.Count;
            Debug.Log("Card ID = " + card_num + " removed from hand");
            Debug.Log(num_of_remain_cards + " Cards remained at hand");

            string object_name = selected_object.name;
            try
            {
                Destroy(selected_object);
                Debug.Log(object_name + " : Destroyed");
            }
            catch
            {
                Debug.LogError(object_name + " : Destroying Failed");
            }
            // �տ� ī�尡 �����̸� �� �б�
            if (num_of_remain_cards <= 1)
            {
                ClearHand();
                SetHand();
            }

            singleLaneElement.selectedCard = "";
        }
        else
        {
            Debug.Log("Card not selected");
        }
    }

    public void SetHand()
    {
        singleLaneElement.SetHand();
        Vector2 position = new Vector2(0, -200);
        int position_x = -3 * 250;
        foreach (var item in singleLaneElement.handCard)
        {
            GameObject temp = Instantiate(card, canvas.transform);
            //���� ������Ʈ ��ġ ����
            position_x += 250;
            position.x = position_x;
            temp.transform.localPosition = position;
            //�ؽ�Ʈ ����
            temp.transform.Find("Text").GetComponent<Text>().text = item.Value.ToString();
            //������Ʈ�� ����
            string card_name = "Card_" + item.Key;
            temp.name = card_name;
            //��ưŬ�� ����
            temp.GetComponent<Button>().onClick.AddListener(delegate { ClickCard(); });

            Debug.Log(card_name + " Created at x :" + position_x.ToString());
        }
    }

    // �� �б�
    public void ClearHand()
    {
        string card_name;
        string object_name;
        foreach (var item in singleLaneElement.handCard)
        {
            card_name = "Card_" + item.Key;
            GameObject game_object = GameObject.Find(card_name).gameObject;
            object_name = game_object.name;
            // ī�� ������Ʈ ����
            try
            {
                Destroy(game_object);
                Debug.Log(object_name + " : Destroyed");
            }
            catch
            {
                Debug.LogError(object_name + " : Destroying Failed");
            }
        }
        // �տ��� ī�� �б�
        singleLaneElement.ClearHand();
        Debug.Log("Hand Cleared");
    }
}
