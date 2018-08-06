using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Linq;
using System.Text;
using System;
using UnityEngine;
using Donner;
using System.Text.RegularExpressions;

public class InvoiceServer : MonoBehaviour {

    WebServer ws;
    public string port;

    public GameManagerLnd gameManagerLnd;
    string externalIp;


    // Use this for initialization
    void Start () {
        ws = new WebServer(SendResponse, "http://*:"+port+"/tanks/");
        ws.Run();


        StartCoroutine(GetPublicIP());
    }
    public string SendResponse(HttpListenerRequest request)
    {
        var response = "Wrong Query, tanks?speed=1 or 2 for speedboost(10 seconds, 50 sat); /tanks?shield=1 or 2 for shield(5 seconds, 25 sat); /powerup?tanks=1 or 2 for mine at player(40 sat);";
        
        
        if (request.QueryString.AllKeys.Contains("speed"))
        {

            var s = gameManagerLnd.getPowerupInvoice("speed", int.Parse(request.QueryString["speed"]), 10f).GetAwaiter().GetResult();
            response = contentToQr(s);
        }
        else if (request.QueryString.AllKeys.Contains("shield"))
        {
            var s = gameManagerLnd.getPowerupInvoice("shield", int.Parse(request.QueryString["shield"]), 5f).GetAwaiter().GetResult();
            response = contentToQr(s);        }
        else if (request.QueryString.AllKeys.Contains("ammo"))
        {
            //response = gameManagerLnd.getReloadInvoice(int.Parse(request.QueryString["player"]), int.Parse(request.QueryString["ammo"])).GetAwaiter().GetResult();
            //response = "<HTML><script type='text/javascript' src='https://ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js'></script><script type='text/javascript' src='https://cdn.rawgit.com/jeromeetienne/jquery-qrcode/master/jquery.qrcode.min.js'></script><BODY><div id = 'qrcode' ><br>" + response + "</div ></BODY><script>$(document).ready(function () {jQuery('#qrcode').qrcode('" + response + "');});</script></HTML>";
        }else if (request.QueryString.AllKeys.Contains("mine"))
        {
            var s = gameManagerLnd.getMineInvoice(int.Parse(request.QueryString["mine"])).GetAwaiter().GetResult();
            response = contentToQr(s);
        }
        else if (request.QueryString[0] == "channel")
        {
            Debug.Log("get channel request");
            var s = gameManagerLnd.pubkey + "@" + externalIp + ":" + port;
            response = contentToQr(s);
        }

        return response;
    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnApplicationQuit()
    {


        ws.Stop();
    }

    string contentToQr(string response) {
        return "<HTML><script type='text/javascript' src='https://ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js'></script><script type='text/javascript' src='https://cdn.rawgit.com/jeromeetienne/jquery-qrcode/master/jquery.qrcode.min.js'></script><BODY><div id = 'qrcode' ><br>" + response + "</div ></BODY><script>$(document).ready(function () {jQuery('#qrcode').qrcode('" + response + "');});</script></HTML>";
    }
    IEnumerator GetPublicIP()
    {
        using (WWW www = new WWW("https://ipv4.myexternalip.com/raw"))
        {
            yield return www;
            var temp = www.text;
            Regex.Replace(temp, @"\s+", "");
            externalIp = LndHelper.RemoveWhitespace(temp);
            Debug.Log(www.text);

        }
    }
}
}


