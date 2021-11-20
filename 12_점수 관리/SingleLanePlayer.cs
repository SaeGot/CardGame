using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SingleLanePlayer : MonoBehaviour
{
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

            singleLaneElement.selectedCard = "";
        }
        else
        {
            Debug.Log("Card not selected");
        }
    }

    public void SetHand(GameObject card, GameObject canvas)
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
}
