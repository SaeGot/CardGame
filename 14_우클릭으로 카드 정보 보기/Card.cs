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

    // ī�� ���� ����
    public void SetInfo(GameObject card_Instance)
    {
        infoText.GetComponent<Text>().text = GetInfo();
    }

    // ī�� ���� ��������
    public string GetInfo()
    {
        switch (cardType)
        {
            case 0:
                return "���� ������ ����մϴ�.";
            case 1:
                return "������ 1�� ������� �ݴϴ�.";
            case 2:
                return "������ 2�� ������� �ݴϴ�.";
            case 3:
                return "������ 3�� ������� �ݴϴ�.";
        }

        return "";
    }
}
