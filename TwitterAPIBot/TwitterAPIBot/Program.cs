// See https://aka.ms/new-console-template for more information

using System;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

var cliente = MetodosGenerales.CrearCliente();
var usuarioAutenticado = await cliente.Users.GetAuthenticatedUserAsync();

Console.WriteLine(usuarioAutenticado);

var tweetsIDs = MetodosGenerales.LeerFichero("./tweetsIDs.txt");

var parametros = new SearchTweetsParameters("prueba")
{
    SearchType = SearchResultType.Recent,
    Since = DateTime.Today.AddDays(-3),
    Query = ""
};

if (tweetsIDs != null && tweetsIDs.Any())
{
    parametros.SinceId= (long) Convert.ToDouble(tweetsIDs.Last());
}

var tweetsIterator = cliente.Search.GetSearchTweetsIterator(parametros);

var tweets = new List<ITweet>();

while (!tweetsIterator.Completed){
    var page = await tweetsIterator.NextPageAsync();
    tweets.AddRange(page);
}

IEnumerable<ITweet> filterTweets = MetodosGenerales.filtrarTweets(tweets);



foreach (var Filteredtweet in filterTweets)
{
    var tweet = await cliente.Tweets.GetTweetAsync(Filteredtweet.Id);
    var retweeters = await cliente.Tweets.GetRetweeterIdsAsync(tweet);

    if (!tweetsIDs.Contains(Filteredtweet.Id.ToString()) 
            && !retweeters.Contains(usuarioAutenticado.Id)) {
   
        switch (tweet.FullText)
        {
            case var s when tweet.FullText.Contains(MetodosGenerales.COMANDO_GATOS) : 
                MetodosGenerales.mandarTweetVideo(cliente, tweet, "video");
                break;
            case var s when tweet.FullText.Contains(MetodosGenerales.COMANDO_ALABORAR): 
                MetodosGenerales.mandarTweetVideo(cliente, tweet, "videoALaborar");
                break;
            case var s when tweet.FullText.Contains(MetodosGenerales.COMANDO_ADIOSMASTER):
                MetodosGenerales.mandarTweetVideo(cliente, tweet, "videoABuenoAdiosMaster");
                break;
            case var s when tweet.FullText.Contains(MetodosGenerales.COMANDO_GANG):
                MetodosGenerales.mandarTweetVideo(cliente, tweet, "videoGang");
                break;
            case var s when tweet.FullText.Contains(MetodosGenerales.COMANDO_RACERO):
                MetodosGenerales.mandarTweetImagen(cliente, tweet, "racero.jpg");
                break;
            case var s when tweet.FullText.Contains(MetodosGenerales.COMANDO_GARBANZOS):
                MetodosGenerales.mandarTweetImagen(cliente, tweet, "garbanzos.jpeg");
                break;
            case var s when tweet.FullText.Contains(MetodosGenerales.COMANDO_ESTOYCANSADO):
                MetodosGenerales.mandarTweetVideo(cliente, tweet, "videoZapatillas");
                break;
            case var s when tweet.FullText.Contains(MetodosGenerales.COMANDO_KRATOS):
                MetodosGenerales.mandarTweetVideo(cliente, tweet, "videoKratos");
                break;
            case var s when tweet.FullText.Contains(MetodosGenerales.COMANDO_TAMEIMPALA):
                MetodosGenerales.mandarTweetVideo(cliente, tweet, "videoTameImpala");
                break;
            case var s when tweet.FullText.Contains(MetodosGenerales.COMANDO_RATATOUILLE):
                MetodosGenerales.mandarTweetVideo(cliente, tweet, "videoRatatouille");
                break;
            case var s when tweet.FullText.Contains(MetodosGenerales.COMANDO_MOSTRO):
                MetodosGenerales.mandarTweetImagen(cliente, tweet, "mostro.jpeg");
                break;
            case var s when tweet.FullText.Contains(MetodosGenerales.COMANDO_BACKTOWORK):
                MetodosGenerales.mandarTweetImagen(cliente, tweet, "backtowork.jpeg");
                break;
        }
    }

}


System.Threading.Thread.Sleep(10000);


