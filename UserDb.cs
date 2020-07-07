
using System;
using System.Collections.Generic;
using System.ComponentModel;

internal class UserDb
{
    internal class UserInfo
    {
        public int totalKarma = 0;
        public int bestKarma = 0;
        public HashSet<DateTime> dates = new HashSet<DateTime>();
    }
    Dictionary<string, UserInfo> userData = new Dictionary<string, UserInfo>();

    public void Add(Post post)
    {
        AddSingle(post);

        if (post.comments != null)
        {
            foreach (var elem in post.comments)
            {
                Add(elem);
            }
        }
    }

    private void AddSingle(Post post)
    {
        if (post.author == null)
        {
            return;
        }

        if (post.author == "[deleted]")
        {
            return;
        }

        var ui = userData.TryGetValue(post.author);
        if (ui == null)
        {
            ui = new UserInfo();
            userData[post.author] = ui;
        }

        ui.totalKarma += post.ups - 1;  // don't count the self-vote
        ui.bestKarma = Math.Max(ui.bestKarma, post.ups);
        ui.dates.Add(post.created.Date);
    }

    public IEnumerable<string> AuthorizedUsers()
    {
        foreach (var kvp in userData)
        {
            int ct = 0;

            if (kvp.Value.totalKarma >= Config.Global.validity_totalKarma)
            {
                ++ct;
            }

            if (kvp.Value.bestKarma >= Config.Global.validity_bestKarma)
            {
                ++ct;
            }

            if (kvp.Value.dates.Count >= Config.Global.validity_uniqueDates)
            {
                ++ct;
            }

            if (ct != 3)
            {
                continue;
            }

            yield return kvp.Key;
        }
    }
}
