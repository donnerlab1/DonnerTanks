using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Donner;
using System.Threading.Tasks;

public class GameManagerLnd : LndRpcBridge {

    public bool readConfig;
    public string hostname;
    public string port;
    public string certFile;

    public string pubkey { get; private set; }
    public string macaroonFile;
    string cert;
    string mac;

    public int roundBounty = 0;
    public Text bountyText;
    public Text lastPowerUpText;

    public Text[] P1Texts;

    public Text[] P2Texts;

    int p1Earnings;
    int p2Earnings;

    public TankLnd p1Donner;
    public TankLnd p2Donner;
    LndConfig config;


    async void Start () {
        if (readConfig)
        {
            config = LndHelper.ReadConfigFile(Application.dataPath + "/Resources/donner.conf");
        }
        else
        {
            config = new LndConfig { Hostname = hostname, Port = port, MacaroonFile = macaroonFile, TlsFile = certFile };
        }

        LndHelper.SetupEnvironmentVariables();

        cert = File.ReadAllText(Application.dataPath + "/Resources/" + config.TlsFile);

        mac = LndHelper.ToHex(File.ReadAllBytes(Application.dataPath + "/Resources/" + config.MacaroonFile));

        //await ConnectToLnd(hostname + ":" + port, cert);
        await ConnectToLndWithMacaroon(hostname + ":" + port, cert, mac);
        OnInvoiceSettled += new InvoiceSettledEventHandler(ReadInvoice);
        SubscribeInvoices();
        var getInfo = await GetInfo();
        pubkey = getInfo.IdentityPubkey;
    }
	

    public async Task<string> getReloadInvoice(int player, int amt)
    {

        var s = await AddInvoice(amt * 5, "reload;" + player);
        //Debug.Log("get reload invoice by" + player);
        return s;
    }

    public async Task<string> getPowerupInvoice(string type, int player, float time)
    {
        
        var s = await AddInvoice((int)(time * 5), type + ";" + player);
        //Debug.Log("get powerup invoice " + type + " for" + player);
        return s;
    }

    public async Task<string> getMineInvoice(int player) {
        var s = await AddInvoice(40, "mine;"+player);
        //Debug.Log("get Mine Invoice for player "+ player);
        return s;
    }

    
    void ReadInvoice(object sender, InvoiceSettledEventArgs e)
    {
        Debug.Log(e.Invoice.RPreimage.ToBase64());
        var info = e.Invoice.Memo.Split(';');
        //Debug.Log(info[0]);
        lastPowerUpText.text = info[0];
        lastPowerUpText.color = (info[1] == "1") ? Color.blue : Color.red;
        roundBounty += (int) e.Invoice.Value;
        switch (info[0])
        {
            case ("reload"):
                //GetComponent<Complete.GameManager>().m_Tanks[int.Parse(info[1])- 1].m_Instance.GetComponent<Complete.TankShooting>().addAmmo((int)e.Invoice.Value / 5);
                break;
            case ("speed"):
                GetComponent<Complete.GameManager>().m_Tanks[int.Parse(info[1]) - 1].m_Instance.GetComponent<TankPowerup>().ActivateSpeedBoost(e.Invoice.Value / 5);
                break;
            case ("shield"):
                GetComponent<Complete.GameManager>().m_Tanks[int.Parse(info[1]) - 1].m_Instance.GetComponent<TankPowerup>().ActivateShield(e.Invoice.Value / 5);
                break;
            case ("mine"):
                GetComponent<MineSpawner>().spawnMine(int.Parse(info[1]) - 1);
            break;


        }
        UpdateBounty();

    }

    public async void Payout(Complete.TankManager winner)
    {
        TankLnd lnd = null;
        if(winner.m_PlayerNumber == 1)
        {
            lnd = p1Donner;
            p1Earnings += roundBounty;
            P1Texts[0].text = "Earnings: " + p1Earnings;
        }else
        {
            lnd = p2Donner;
            p2Earnings += roundBounty;
            P2Texts[0].text = "Earnings: " + p2Earnings;
        }
        //var invoice = await lnd.AddInvoice((int)roundBounty, "bounty");
        //var hash = await SendPayment(invoice);
        roundBounty = 0;
        ResetAmmo();
        UpdateBounty();
    }

    public void ResetAmmo()
    {
        GetComponent<Complete.GameManager>().m_Tanks[0].m_Instance.GetComponent<Complete.TankShooting>().m_ammo = 0;
        GetComponent<Complete.GameManager>().m_Tanks[1].m_Instance.GetComponent<Complete.TankShooting>().m_ammo = 0;
    }

    void UpdateBounty()
    {
        bountyText.text = "Bounty: " + roundBounty;
    }

    void OnApplicationQuit()
    {
        Shutdown();
    }

    
}
