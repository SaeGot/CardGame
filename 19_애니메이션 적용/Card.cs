using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerClickHandler
{
    public int cardType;
    public GameObject infoText;
    public bool rightClickEnabled;
    private GameObject instanceInfoText;

    void Awake()
    {
        rightClickEnabled = true;
        instanceInfoText = Instantiate(infoText, transform);
        Vector2 position = new Vector2(0, 100);
        instanceInfoText.transform.localPosition = position;
        instanceInfoText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && rightClickEnabled)
        {
            instanceInfoText.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData event_Data)
    {
        if (event_Data.button == PointerEventData.InputButton.Right && rightClickEnabled)
        {
            instanceInfoText.SetActive(true);
            transform.SetAsLastSibling();
        }
    }

    // 정보 텍스트 활성 설정
    public void InfoTextSetActive(bool active, bool right_ClickEnabled)
    {
        instanceInfoText.SetActive(active);
        rightClickEnabled = right_ClickEnabled;
    }


    // 카드 정보 설정
    public void SetInfo()
    {
        instanceInfoText.GetComponent<Text>().text = GetInfo();
    }

    // 카드 정보 가져오기
    public string GetInfo()
    {
        switch (cardType)
        {
            case 0:
                return "상대의 카드숫자가 1이 아닐경우,\n상대의 카드숫자만큼 반격 및 방어합니다";
            case 1:
                return "상대에게 반드시 1의 대미지를 줍니다.";
            case 2:
                return "상대보다 큰 숫자일 경우 2의 대미지를 줍니다.";
            case 3:
                return "상대보다 큰 숫자일 경우 3의 대미지를 줍니다.";
        }

        return "";
    }

    // 카드 제시 애니메이션
    public void AnimationSet(bool selected)
    {
        GetComponent<Animator>().SetBool("Selected", selected);
    }
}
