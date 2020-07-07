
using System;

class Post
{
    public string author;
    public int ups;

    [Newtonsoft.Json.JsonConverter(typeof(Util.FloatUnixDateTimeConverter))]
    public DateTime created;

    public Post[] comments;
}
