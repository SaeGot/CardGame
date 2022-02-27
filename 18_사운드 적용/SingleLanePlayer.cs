using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleLanePlayer : MonoBehaviour
{
    public GameObject card;
    public int handPositionY;
    public int battlePositionY;
    public int maxLife;
    private SingleLaneElement singleLaneElement;
    private bool opponent;

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

    // ���� ���� �� �ʱ�ȭ
    public void Initialize(bool _opponent)
    {
        opponent = _opponent;
        SetHand();
        SetLife(maxLife);
    }

    public void ClickCard()
    {
        if (EventSystem.current.currentSelectedGameObject)
        {
            string selected_card_name = EventSystem.current.currentSelectedGameObject.name;
            singleLaneElement.selectedCard = selected_card_name;
            transform.Find(selected_card_name).GetComponent<AudioSource>().Play();
            Debug.Log(selected_card_name + " Selected");
        }
    }

    // ���� �غ�
    public void Ready()
    {
        string selected_card_name = singleLaneElement.selectedCard;
        GameObject selected_object = transform.Find(selected_card_name).gameObject;
        // ���õ� ī�� �̵�
        Vector2 position = new Vector2(0, battlePositionY);
        MoveCard(selected_object, selected_card_name, position);
    }

    // ����� ��������
    public int GetDamage(SingleLanePlayer opponent)
    {
        string selected_card_name = singleLaneElement.selectedCard;
        GameObject selected_object = transform.Find(selected_card_name).gameObject;

        string opponent_selected_card_name = opponent.singleLaneElement.selectedCard;
        GameObject opponent_selected_object = opponent.transform.Find(opponent_selected_card_name).gameObject;
        int opponent_card_type = GetCardType(opponent_selected_object);

        int card_damage = GetCardType(selected_object);
        if (card_damage == 1) { }
        else if(opponent_card_type == 0)
        {
            card_damage = 0;
        }
        else if (card_damage == 0 && opponent_card_type != 1)
        {
            card_damage = opponent_card_type;
        }
        else if (card_damage <= opponent_card_type)
        {
            card_damage = 0;
        }

        return card_damage;
    }

    // ���� ����
    public void Fight(int damage)
    {
        DamageLife(damage);
        string selected_card_name = singleLaneElement.selectedCard;
        GameObject selected_object = transform.Find(selected_card_name).gameObject;
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

    // ���õ� ī�� ���� Ȯ��
    public bool CheckSelectedCard()
    {
        if (singleLaneElement.selectedCard != "")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetHand()
    {
        singleLaneElement.SetHand();
        Vector2 position = new Vector2(0, handPositionY);
        int position_x = -3 * 250;
        foreach (var item in singleLaneElement.handCard)
        {
            GameObject temp = Instantiate(card, transform);
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
            if (opponent)
            {
                temp.GetComponent<Button>().enabled = false;
            }
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
            GameObject game_object = transform.Find(card_name).gameObject;
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

    // ���� ī�� ��������
    public List<int> GetRemainCards()
    {
        List<int> list = new List<int>();
        foreach (var card in singleLaneElement.handCard)
        {
            list.Add(card.Key);
        }

        return list;
    }

    // ü�� ����
    public void SetLife(int life)
    {
        singleLaneElement.life = life;
        transform.Find("Score").GetComponent<Text>().text = life.ToString();
    }

    // ü�� ��������
    public int GetLife()
    {
        return singleLaneElement.life;
    }

    // �ΰ����� ����
    public void AISelectCard()
    {
        //���� ī�� Key �� ����Ʈ
        List<int> card_keys = new List<int>(singleLaneElement.handCard.Keys);
        //���� ī�� ����
        int remain_cards_count = card_keys.Count;
        //���� ����
        int random_num = Random.Range(0, remain_cards_count);
        //ī�� ���� ����
        int selected_card = card_keys[random_num];
        singleLaneElement.selectedCard = "Card_" + selected_card.ToString();
    }

    // ���õ� ī�� �̵�
    private void MoveCard(GameObject card_Object, string card_Name, Vector2 position)
    {
        card_Object.transform.localPosition = position;
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
        transform.Find("Score").GetComponent<Text>().text = singleLaneElement.life.ToString();
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
        string player_name = card_Object.transform.parent.name;
        try
        {
            Destroy(card_Object);
            Debug.Log(player_name + " " + object_name + " : Destroyed");
        }
        catch
        {
            Debug.LogError(player_name + " " + object_name + " : Destroying Failed");
        }

        return num_of_remain_cards;
    }
}
