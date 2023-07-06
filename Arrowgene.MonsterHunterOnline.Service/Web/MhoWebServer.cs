using System.Threading.Tasks;
using Arrowgene.Logging;
using Arrowgene.WebServer;
using Arrowgene.WebServer.Route;
using Arrowgene.WebServer.Server.Kestrel;

namespace Arrowgene.MonsterHunterOnline.Service.Web;

public class MhoWebServer
{
    private static readonly ILogger Logger = LogProvider.Logger<Logger>(typeof(MhoWebServer));

    private WebService _webService;

    public MhoWebServer()
    {
        WebSetting webSetting = new WebSetting();
        _webService = new WebService(new KestrelWebServer(webSetting));
    }

    public void Start()
    {
        _webService.AddRoute(new WebRouter());
        _webService.Start();
    }
    
    public void Stop()
    {
        _webService.Stop();
    }

    private class WebRouter : WebRoute
    {
        public override string Route => "/mho/*";

        public override async Task<WebResponse> Post(WebRequest request)
        {
            string body = await request.ReadStringAsync();

            Logger.Debug(request.ToString());
            Logger.Debug(body);

            WebResponse response = new WebResponse();
            response.StatusCode = 200;
            await response.WriteAsync("{\"test}");
            return response;
        }
    }
}