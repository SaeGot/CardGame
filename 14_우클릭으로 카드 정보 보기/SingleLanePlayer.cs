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
            MoveCard(selected_object, selected_card_name);
            // ���õ� ī���� Ÿ�� ��������
            int card_damage = GetCardType(selected_object);
            // ������
            DamageLife(card_damage);
            // ���õ� ī�� �տ��� ����
            int num_of_remain_cards = RemoveCard(selected_object, selected_card_name);
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
            //ī�� Ÿ�� ����
            temp.GetComponent<Card>().cardType = item.Value;
            //�ؽ�Ʈ ����
            temp.transform.Find("Text").GetComponent<Text>().text = item.Value.ToString();
            //������Ʈ�� ����
            string card_name = "Card_" + item.Key;
            temp.name = card_name;
            //��ưŬ�� ����
            temp.GetComponent<Button>().onClick.AddListener(delegate { ClickCard(); });
            //ī�� ���� ����
            temp.GetComponent<Card>().SetInfo();

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

    // ���õ� ī�� �̵�
    private void MoveCard(GameObject card_Object, string card_Name)
    {
        card_Object.transform.position = new Vector3(0, 0, 0);
        Debug.Log(card_Name + " Moved to (0,0,0)");
    }

    // ���õ� ī���� Ÿ�� ��������
    private int GetCardType(GameObject card_Object)
    {
        Card card_component = card_Object.GetComponent<Card>();
        int card_type = card_component.cardType;

        return card_type;
    }

    // ������
    private void DamageLife(int damage)
    {
        singleLaneElement.life -= damage;
        GameObject.Find("Score").GetComponent<Text>().text = singleLaneElement.life.ToString();
        Debug.Log("Life damaged " + damage);
        Debug.Log("Current Life " + singleLaneElement.life);
    }

    // ���õ� ī�� �տ��� ����
    private int RemoveCard(GameObject card_Object, string card_Name)
    {
        string card_num = card_Name.Substring(card_Name.Length - 1);
        singleLaneElement.handCard.Remove(int.Parse(card_num));
        // �տ� ���� ī�� ����
        int num_of_remain_cards = singleLaneElement.handCard.Count;
        Debug.Log("Card ID = " + card_num + " removed from hand");
        Debug.Log(num_of_remain_cards + " Cards remained at hand");

        string object_name = card_Object.name;
        try
        {
            Destroy(card_Object);
            Debug.Log(object_name + " : Destroyed");
        }
        catch
        {
            Debug.LogError(object_name + " : Destroying Failed");
        }

        return num_of_remain_cards;
    }
}
