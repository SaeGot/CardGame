using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public int cardType;
    public GameObject infoText;
    private GameObject instanceInfoText;

    void Awake()
    {
        instanceInfoText = Instantiate(infoText, transform);
        Vector2 position = new Vector2(0, 100);
        instanceInfoText.transform.localPosition = position;
        instanceInfoText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Down");
            instanceInfoText.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData event_Data)
    {
        if (event_Data.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Click");
            instanceInfoText.GetComponent<Text>().text = GetInfo();
            instanceInfoText.SetActive(true);
        }
    }

    // 카드 정보 설정
    public void SetInfo(GameObject card_Instance)
    {
        infoText.GetComponent<Text>().text = GetInfo();
    }

    // 카드 정보 가져오기
    public string GetInfo()
    {
        switch (cardType)
        {
            case 0:
                return "적의 공격을 방어합니다.";
            case 1:
                return "적에게 1의 대미지를 줍니다.";
            case 2:
                return "적에게 2의 대미지를 줍니다.";
            case 3:
                return "적에게 3의 대미지를 줍니다.";
        }

        return "";
    }
}
