using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;
using MdXaml;
using OsuAPI;
using OsuAPI.Objects;

namespace OsuLauncher.Pages;

public partial class WikiPage : Page
{
    private OsuClient _client;
    public WikiPage()
    {
        InitializeComponent();
        var tokenLocation = Path.Combine(Directory.GetCurrentDirectory(), "token.secret");
        if (File.Exists(tokenLocation))
        {
            _client = new OsuClient(File.ReadAllText(tokenLocation));
        }

        var wikitest = _client.GetWikiAsync("en", "Game_mode_osu!");
        Markdown engine = new Markdown();

        FlowDocument document = engine.Transform(wikitest.Result.Content);

        WikiViewer.Document = document;
    }
}