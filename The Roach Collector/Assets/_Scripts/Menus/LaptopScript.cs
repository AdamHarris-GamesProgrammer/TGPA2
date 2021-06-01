using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaptopScript : MonoBehaviour
{
    private Canvas activeCanvas;
    private GameObject activePanel;

    // Canvas's
    [SerializeField] Canvas menuCanvas;
    [SerializeField] Canvas shopCanvas;
    [SerializeField] Canvas blackMarketCanvas;
    [SerializeField] Canvas messageCanvas;
    [SerializeField] Canvas infoCanvas;

    [SerializeField] GameObject initialPanel;


    //Buttons
    [Header("Buttons")]
    [SerializeField] Button loginButton;
    [SerializeField] Button shopButton;
    [SerializeField] Button infoButton;


    // Start is called before the first frame update
    void Start()
    {
        activeCanvas = menuCanvas;
        activeCanvas.gameObject.active = true;
        activePanel = initialPanel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaptopLogin()
    {
        shopButton.gameObject.active = true;
        infoButton.gameObject.active = true;
        loginButton.gameObject.active = false;

    }

    public void SwitchCanvas(Canvas canvasToSwitch)
    {
        activeCanvas.gameObject.active = false;
        activeCanvas = canvasToSwitch;
        activeCanvas.gameObject.active = true;
        
    }

    public void SwitchToSellCanvas(Canvas canvas) {
        SwitchCanvas(canvas);
        FindObjectOfType<SellItems>().LoadItems();
    }

    public void SwitchShopPanel(GameObject panelToSwitch)
    {
        
        activePanel.gameObject.active = false;
        activePanel = panelToSwitch;
        activePanel.gameObject.active = true;
        

    }

}

