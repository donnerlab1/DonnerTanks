using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Donner;
using System.IO;
using Complete;

public class TankLnd : LndRpcBridge {

    public string hostname;
    public string port;
    public string certfile;
    public string macaroonfile;
    string cert;
    string mac;


    public int m_PlayerNumber = 1;
    private string m_ReloadButton;
    public GameManagerLnd gameLnd;
    public GameObject tankInstance;


    // Use this for initialization
    async void Start () {
        

        LndHelper.SetupEnvironmentVariables();


        string name = "";
        if (m_PlayerNumber == 1)
        {
            name = "bob";
            hostname = "localhost";
            port = "10010";
        }
        else if(m_PlayerNumber == 2)
        {
            name = "charlie";
            hostname = "localhost";
            port = "10011";
        }
        cert = File.ReadAllText(Application.dataPath + "/Resources/" + name + ".cert");

        mac = LndHelper.ToHex(File.ReadAllBytes(Application.dataPath + "/Resources/" + name + "admin.macaroon"));
        //await ConnectToLnd(hostname + ":" + port, cert);
        await ConnectToLndWithMacaroon(hostname + ":" + port, cert, mac);

        gameLnd = GameObject.Find("GameManager").GetComponent<GameManagerLnd>();

    }

    public async void connectToTankLnd(int player)
    {
        
    }

    public void setPlayernumber(int playernumber)
    {
        m_PlayerNumber = playernumber;
        m_ReloadButton = "Reload" + m_PlayerNumber;
    }
    public void OnEnable()
    {
        //SetupRpc();
    }

    public async void SetupRpc()
    {
        //await ConnectToLndWithMacaroon(hostname + ":" + port, cert, mac);
    }

    // Update is called once per frame
    async void Update() {
        if (Input.GetButtonUp(m_ReloadButton))
        {
            var invoice = await gameLnd.getReloadInvoice( m_PlayerNumber, 1);
            var preimage = await SendPayment(invoice);
            Debug.Log(m_PlayerNumber + " paid for reload: " + preimage);
        }
    }
    void OnDisable()
    {
        //Shutdown();
    }
}
