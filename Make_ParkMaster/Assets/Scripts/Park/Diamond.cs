using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    public bool isBestDiamond = false;
    private void Update()
    {
        transform.GetChild(0).Rotate(0, Time.deltaTime * 50, 0);
    }
    public void AddCoin()
    {
        if (isBestDiamond)
            GameObject.Find("Canvas").GetComponent<MenuController>().AddBestDiamond();
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }
    
}
