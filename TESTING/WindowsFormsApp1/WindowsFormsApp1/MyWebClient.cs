using System;
using System.Net;

public class MyWebClient : WebClient
{
    public CookieContainer CookieContainer { get; set; }

    public MyWebClient() : this(new CookieContainer())
    { }

    public MyWebClient(CookieContainer c) => CookieContainer = c;

    protected override WebRequest GetWebRequest(Uri address)
    {
        WebRequest request = base.GetWebRequest(address);
        ((HttpWebRequest)request).CookieContainer = CookieContainer;
        return request;
    }
}

