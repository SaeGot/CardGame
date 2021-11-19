using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleLaneElement
{
    public string selectedCard;
    public Dictionary<int, int> handCard;
    private List<int> defaultCardList;
    public SingleLaneElement()
    {
        selectedCard = "";
        handCard = new Dictionary<int, int>();
        defaultCardList = new List<int>(new int[] { 0, 1, 2, 2, 3 });
    }

    // 카드 핸드에 세팅
    public void SetHand()
    {
        for (int n = 0; n < 5; n++)
        {
            int cardKind = defaultCardList[n];
            handCard.Add(n, cardKind);
        }
    }
}
