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
    private bool AIPlayer;

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

    // 게임 시작 전 초기화
    public void Initialize(bool AI_Player)
    {
        AIPlayer = AI_Player;
        SetHand();
        SetLife(maxLife);
    }

    public void ClickCard()
    {
        if (EventSystem.current.currentSelectedGameObject)
        {
            string selected_card_name = EventSystem.current.currentSelectedGameObject.name;
            singleLaneElement.selectedCard = selected_card_name;
            SetSelectedAnimation(selected_card_name);
            transform.Find(selected_card_name).GetComponent<AudioSource>().Play();
            Debug.Log(selected_card_name + " Selected");
        }
    }

    // 전투 준비
    public void Ready()
    {
        string selected_card_name = singleLaneElement.selectedCard;
        GameObject selected_object = transform.Find(selected_card_name).gameObject;
        // 선택된 카드 이동
        Vector2 position = new Vector2(0, battlePositionY);
        MoveCard(selected_object, selected_card_name, position);
    }

    // 대미지 가져오기
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

    // 전투 시작
    public IEnumerator Fight(int damage)
    {
        string selected_card_name = singleLaneElement.selectedCard;
        GameObject selected_object = transform.Find(selected_card_name).gameObject;
        yield return StartCoroutine(Rush(selected_object));
        DamageLife(damage);
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

    // 선택된 카드 여부 확인
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
            if (AIPlayer)
            {
                temp.GetComponent<Button>().enabled = false;
            }
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
            GameObject game_object = transform.Find(card_name).gameObject;
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

    // 남은 카드 가져오기
    public List<int> GetRemainCards()
    {
        List<int> list = new List<int>();
        foreach (var card in singleLaneElement.handCard)
        {
            list.Add(card.Key);
        }

        return list;
    }

    // 체력 설정
    public void SetLife(int life)
    {
        singleLaneElement.life = life;
        transform.Find("Score").GetComponent<Text>().text = life.ToString();
    }

    // 체력 가져오기
    public int GetLife()
    {
        return singleLaneElement.life;
    }

    // 인공지능 적용
    public void AISelectCard()
    {
        //남은 카드 Key 값 리스트
        List<int> card_keys = new List<int>(singleLaneElement.handCard.Keys);
        //남은 카드 개수
        int remain_cards_count = card_keys.Count;
        //랜덤 숫자
        int random_num = Random.Range(0, remain_cards_count);
        //카드 랜덤 선택
        int selected_card = card_keys[random_num];
        singleLaneElement.selectedCard = "Card_" + selected_card.ToString();
    }

    // 카드 선택 애니메이션 해제
    public IEnumerator StopSelectedAnimation()
    {
        string selected_card_name = singleLaneElement.selectedCard;
        GameObject selected_object = transform.Find(selected_card_name).gameObject;
        selected_object.GetComponent<Card>().AnimationSet(false);

        yield return new WaitForSeconds(0.2f);
    }

    // 선택된 카드 이동
    private void MoveCard(GameObject card_Object, string card_Name, Vector2 position)
    {
        card_Object.transform.localPosition = position;
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
        transform.Find("Score").GetComponent<Text>().text = singleLaneElement.life.ToString();
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

    // 돌진 애니메이션
    private IEnumerator Rush(GameObject selected_Object)
    {
        while ((Vector2)selected_Object.transform.position != Vector2.zero)
        {
            selected_Object.transform.position = Vector2.MoveTowards(selected_Object.transform.position, Vector2.zero, 5 * Time.deltaTime);
            yield return null;
        }
    }

    // 카드 선택 애니메이션
    private void SetSelectedAnimation(string selected_CardName)
    {
        foreach (KeyValuePair<int, int> hand_card in singleLaneElement.handCard)
        {
            transform.Find("Card_" + hand_card.Key).GetComponent<Card>().AnimationSet(false);
        }
        GameObject selected_object = transform.Find(selected_CardName).gameObject;
        selected_object.GetComponent<Card>().AnimationSet(true);
    }
}
