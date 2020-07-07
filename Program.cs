using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Def.Config.DefaultHandlerThrowExceptions = Def.Config.DefaultExceptionBehavior.Never;

        {
            var parser = new Def.Parser();
            parser.AddFile("xml/config.xml");
            parser.Finish();
        }

        var userdb = new UserDb();
            
        foreach (var file in Directory.GetFiles(Config.Global.dataDir, "*.json", SearchOption.AllDirectories).ProgressBar())
        {
            var post = JsonConvert.DeserializeObject<Post>(File.ReadAllText(file));
            userdb.Add(post);
        }

        var authorized = userdb.AuthorizedUsers().ToArray();

        string result = "";
        result += "~author: [";
        result += string.Join(", ", authorized.OrderBy(user => user));
        result += "]";

        Dbg.Inf(result);

        authorized.Shuffle();

        Dbg.Inf("Random sample of authorized users: " + string.Join(" ", authorized.Take(20).Select(user => $"/u/{user}")));
    }
}
