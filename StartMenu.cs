using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class StartMenu : NetworkBehaviour
{
    //REDO ALL OD THIS SHIT
    private UnityEngine.UI.Button hostButtonL;
    private UnityEngine.UI.Button hostButtonR;

    private UnityEngine.UI.Button offlineButtonL;
    private UnityEngine.UI.Button offlineButtonR;

    private UnityEngine.UI.Button joinButtonL;
    private UnityEngine.UI.Button joinButtonR;

    private EventSystem eventSystem;
    private UnityEngine.Networking.NetworkManager networkManager;
    // Use this for initialization
    void Start()
    {
        //enable 72hz mode
        OVRManager.display.displayFrequency = 72.0f;
        OVRManager.tiledMultiResLevel = OVRManager.TiledMultiResLevel.LMSHigh;


        var manager = GameObject.Find("NetworkManager");
        networkManager = manager.GetComponent<UnityEngine.Networking.NetworkManager>();

        var canvasL = transform.Find("CanvasL");
        hostButtonL = canvasL.Find("Host").GetComponent<UnityEngine.UI.Button>();
        offlineButtonL = canvasL.Find("Offline").GetComponent<UnityEngine.UI.Button>();
        joinButtonL = canvasL.Find("Join").GetComponent<UnityEngine.UI.Button>();

        //right ones are just the image not button
        var canvasR = transform.Find("CanvasR");
        hostButtonR = canvasR.Find("Host").GetComponent<UnityEngine.UI.Button>();
        offlineButtonR = canvasR.Find("Offline").GetComponent<UnityEngine.UI.Button>();
        joinButtonR = canvasR.Find("Join").GetComponent<UnityEngine.UI.Button>();

        //joinButtonR.Select();
        //joinButtonL.Select();

        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        eventSystem.SetSelectedGameObject(joinButtonL.gameObject);
        UpdateRightButtons();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (eventSystem.currentSelectedGameObject == joinButtonL.gameObject)
            {
                //join multiplayer server
                networkManager.networkAddress = "192.168.1.104";
                
                networkManager.StartClient();
                Destroy(GameObject.Find("Pre-Player Player"));

            }
            else if (eventSystem.currentSelectedGameObject == hostButtonL.gameObject)
            {
                //host multiplayer server
                networkManager.StartHost();
                Destroy(GameObject.Find("Pre-Player Player"));
            }
            else if (eventSystem.currentSelectedGameObject == offlineButtonL.gameObject)
            {
                //play offline
            }
        }
        else if (OVRInput.GetDown(OVRInput.Button.One))
        {
            if (OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad).y < 0)
            {
                if (eventSystem.currentSelectedGameObject == joinButtonL.gameObject)
                {
                    eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);
                    eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(hostButtonL.gameObject);
                }
                else if (eventSystem.currentSelectedGameObject == hostButtonL.gameObject)
                {
                    eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);
                    eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(offlineButtonL.gameObject);
                }
                else if (eventSystem.currentSelectedGameObject == offlineButtonL.gameObject)
                {
                    eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);
                    eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(joinButtonL.gameObject);
                }
            }
            else
            {
                if (eventSystem.currentSelectedGameObject == joinButtonL.gameObject)
                {
                    eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);
                    eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(offlineButtonL.gameObject);
                }
                else if (eventSystem.currentSelectedGameObject == hostButtonL.gameObject)
                {
                    eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);
                    eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(joinButtonL.gameObject);
                }
                else if (eventSystem.currentSelectedGameObject == offlineButtonL.gameObject)
                {
                    eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(null);
                    eventSystem.GetComponent<EventSystem>().SetSelectedGameObject(hostButtonL.gameObject);
                }
            }
            UpdateRightButtons();
        }
    }

    void UpdateRightButtons()
    {
        var hostBlock = hostButtonR.colors;
        var offlineBlock = offlineButtonR.colors;
        var joinBlock = joinButtonR.colors;

        if (eventSystem.currentSelectedGameObject == hostButtonL.gameObject)
        {
            hostBlock.normalColor = hostButtonL.colors.highlightedColor;
            joinBlock.normalColor = joinButtonL.colors.normalColor;
            offlineBlock.normalColor = offlineButtonL.colors.normalColor;

        }
        else if (eventSystem.currentSelectedGameObject == offlineButtonL.gameObject)
        {
            hostBlock.normalColor = hostButtonL.colors.normalColor;
            joinBlock.normalColor = joinButtonL.colors.normalColor;
            offlineBlock.normalColor = offlineButtonL.colors.highlightedColor;
        }
        else if (eventSystem.currentSelectedGameObject == joinButtonL.gameObject)
        {
            hostBlock.normalColor = hostButtonL.colors.normalColor;
            joinBlock.normalColor = joinButtonL.colors.highlightedColor;
            offlineBlock.normalColor = offlineButtonL.colors.normalColor;
        }
        joinButtonR.colors = joinBlock;
        offlineButtonR.colors = offlineBlock;
        hostButtonR.colors = hostBlock;
    }
}
