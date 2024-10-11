using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseItem : MonoBehaviour {
    public int ItemIndex;
    public bool IsButton = true;
    
    private void Start() {
        GetComponent<Button>().onClick.AddListener(() => {
            Purchaser.instance.PurchaseItem(ItemIndex);


        });
    }
}
