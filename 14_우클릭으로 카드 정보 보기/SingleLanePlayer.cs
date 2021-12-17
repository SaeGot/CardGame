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
            // 선택된 카드 이동
            MoveCard(selected_object, selected_card_name);
            // 선택된 카드의 타입 가져오기
            int card_damage = GetCardType(selected_object);
            // 데미지
            DamageLife(card_damage);
            // 선택된 카드 손에서 제거
            int num_of_remain_cards = RemoveCard(selected_object, selected_card_name);
            // 손에 카드가 한장이면 손 털기
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
            //현재 오브젝트 위치 설정
            position_x += 250;
            position.x = position_x;
            temp.transform.localPosition = position;
            //카드 타입 설정
            temp.GetComponent<Card>().cardType = item.Value;
            //텍스트 설정
            temp.transform.Find("Text").GetComponent<Text>().text = item.Value.ToString();
            //오브젝트명 변경
            string card_name = "Card_" + item.Key;
            temp.name = card_name;
            //버튼클릭 설정
            temp.GetComponent<Button>().onClick.AddListener(delegate { ClickCard(); });
            //카드 정보 설정
            temp.GetComponent<Card>().SetInfo();

            Debug.Log(card_name + " Created at x :" + position_x.ToString());
        }
    }

    // 손 털기
    public void ClearHand()
    {
        string card_name;
        string object_name;
        foreach (var item in singleLaneElement.handCard)
        {
            card_name = "Card_" + item.Key;
            GameObject game_object = GameObject.Find(card_name).gameObject;
            object_name = game_object.name;
            // 카드 오브젝트 제거
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
        // 손에서 카드 털기
        singleLaneElement.ClearHand();
        Debug.Log("Hand Cleared");
    }

    // 선택된 카드 이동
    private void MoveCard(GameObject card_Object, string card_Name)
    {
        card_Object.transform.position = new Vector3(0, 0, 0);
        Debug.Log(card_Name + " Moved to (0,0,0)");
    }

    // 선택된 카드의 타입 가져오기
    private int GetCardType(GameObject card_Object)
    {
        Card card_component = card_Object.GetComponent<Card>();
        int card_type = card_component.cardType;

        return card_type;
    }

    // 데미지
    private void DamageLife(int damage)
    {
        singleLaneElement.life -= damage;
        GameObject.Find("Score").GetComponent<Text>().text = singleLaneElement.life.ToString();
        Debug.Log("Life damaged " + damage);
        Debug.Log("Current Life " + singleLaneElement.life);
    }

    // 선택된 카드 손에서 제거
    private int RemoveCard(GameObject card_Object, string card_Name)
    {
        string card_num = card_Name.Substring(card_Name.Length - 1);
        singleLaneElement.handCard.Remove(int.Parse(card_num));
        // 손에 남은 카드 개수
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
