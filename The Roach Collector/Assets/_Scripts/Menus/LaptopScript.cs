using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaptopScript : MonoBehaviour
{
    private Canvas activeCanvas;
    private GameObject activePanel;

    // Canvas's
    public Canvas menuCanvas;
    public Canvas shopCanvas;
    public Canvas blackMarketCanvas;
    public Canvas messageCanvas;
    public Canvas infoCanvas;

    public GameObject initialPanel;


    //Buttons
    public Button loginButton;
    public Button shopButton;
    public Button blackMarketButton;
    public Button messageButton;
    public Button infoButton;


    


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
        blackMarketButton.gameObject.active = true;
        messageButton.gameObject.active = true;
        infoButton.gameObject.active = true;
        loginButton.gameObject.active = false;

    }

    public void SwitchCanvas(Canvas canvasToSwitch)
    {
        activeCanvas.gameObject.active = false;
        activeCanvas = canvasToSwitch;
        activeCanvas.gameObject.active = true;
    }

    public void SwitchShopPanel(GameObject panelToSwitch)
    {
        activePanel.gameObject.active = false;
        activePanel = panelToSwitch;
        activePanel.gameObject.active = true;

    }
}

