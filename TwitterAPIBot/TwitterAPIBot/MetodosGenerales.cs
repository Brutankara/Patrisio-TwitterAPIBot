using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using System.Text.Json;

public static class MetodosGenerales
{
    public static string CARPETA_IMAGENES = "..//..//../images/";
    public static string CARPETA_VIDEOS = "..//..//../videos/";
    public static string CARPETA_SECURITY = "..//..//../security/";

    public static string COMANDO_GATOS = "!gatos";
    public static string COMANDO_ALABORAR = "!alaborar";
    public static string COMANDO_ADIOSMASTER  = "!adiosmaster";
    public static string COMANDO_GANG = "!gang";
    public static string COMANDO_RACERO = "!racero";
    public static string COMANDO_GARBANZOS = "!garbanzos";
    public static string COMANDO_ESTOYCANSADO = "!estoycansado";
    public static string COMANDO_KRATOS = "!kratos";
    public static string COMANDO_TAMEIMPALA = "!tameimpala";
    public static string COMANDO_RATATOUILLE = "!ratatouille";
    public static string COMANDO_MOSTRO = "!mostro";
    public static string COMANDO_BACKTOWORK = "!backtowork";
    //public static string COMANDO_ = "!";
    //public static string COMANDO_ = "!";
    //public static string COMANDO_ = "!";
    //public static string COMANDO_ = "!";
    //public static string COMANDO_ = "!";


	public static IEnumerable<ITweet> filtrarTweets(List<ITweet> tweets)
	{
		IEnumerable<ITweet> tweetsFiltrados;
		tweetsFiltrados = tweets.Where(tweetUnico =>
				!(tweetUnico.Retweeted || tweetUnico.IsRetweet)
				&&
				(
					tweetUnico.FullText.Contains(COMANDO_GATOS)
					||
					tweetUnico.FullText.Contains(COMANDO_ALABORAR)
					||
					tweetUnico.FullText.Contains(COMANDO_ADIOSMASTER)
					||
					tweetUnico.FullText.Contains(COMANDO_GANG)
                    ||
                    tweetUnico.FullText.Contains(COMANDO_RACERO)
                    ||
                    tweetUnico.FullText.Contains(COMANDO_GARBANZOS)
                    ||
                    tweetUnico.FullText.Contains(COMANDO_ESTOYCANSADO)
                    ||
                    tweetUnico.FullText.Contains(COMANDO_KRATOS)
                    ||
                    tweetUnico.FullText.Contains(COMANDO_TAMEIMPALA)
                    ||
                    tweetUnico.FullText.Contains(COMANDO_RATATOUILLE)
                    ||
                    tweetUnico.FullText.Contains(COMANDO_MOSTRO)
                    ||
                    tweetUnico.FullText.Contains(COMANDO_BACKTOWORK)

                  )
        );


        return tweetsFiltrados;
	}

	public static async void mandarTweetVideo(TwitterClient client, ITweet tweet ,string nombreVideo)
	{
        var videoBinary = File.ReadAllBytes(CARPETA_VIDEOS + nombreVideo +".mp4");
        var uploadedVideo = await client.Upload.UploadTweetVideoAsync(videoBinary);
        await client.Upload.WaitForMediaProcessingToGetAllMetadataAsync(uploadedVideo);

        var reply = await client.Tweets.PublishTweetAsync(new PublishTweetParameters("@" + tweet.CreatedBy + " ")
		{
            InReplyToTweet = tweet,
            Medias = { uploadedVideo }
		});

        //var retweet =  client.Tweets.PublishRetweetAsync(tweet);
        EscribirFichero("./tweetsIDs.txt", tweet.Id.ToString());
    }
    public static async void mandarTweetImagen(TwitterClient client, ITweet tweet, string nombreImagen)
    {
        var imagenBinary = File.ReadAllBytes(CARPETA_IMAGENES + nombreImagen);
        var uploadedImage = await client.Upload.UploadTweetImageAsync(imagenBinary);

        var reply = await client.Tweets.PublishTweetAsync(new PublishTweetParameters("@" + tweet.CreatedBy + " ")
        {
            InReplyToTweet = tweet,
            Medias = { uploadedImage }
        });

        //var retweet =  client.Tweets.PublishRetweetAsync(tweet);
        EscribirFichero("./tweetsIDs.txt", tweet.Id.ToString());
    }

    public static List<String> LeerFichero(string path)
    {   
        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }

        var fichero = File.ReadAllLines(path);
        var result = new List<string>(fichero);
        return result;
    }

    public static void EscribirFichero(string path, string texto)
    {
        List<string> list = new List<string>();
        list.Add(texto);

        File.AppendAllLines(path,list);
        //File.AppendText(path, texto);
        //File.WriteAllText(path, texto);
    }

    internal static TwitterClient CrearCliente()
    {
        string SecurityJson = File.ReadAllText(CARPETA_SECURITY + "keys.json");
        Credenciales credenciales = JsonSerializer.Deserialize<Credenciales>(SecurityJson);


        var credentialesTwitter = new TwitterCredentials(
                                            credenciales.APIKey,
                                            credenciales.APISecret,
                                            credenciales.accessToken,
                                            credenciales.accessTokenSecret
                                            );

        return new TwitterClient(credentialesTwitter);
    }
}
