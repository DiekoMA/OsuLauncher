using System.Windows.Controls;
using HandyControl.Controls;
using HtmlAgilityPack;
using OsuAPI;

namespace OsuLauncher.Pages;

public partial class NewsPage : Page
{
    public NewsPage()
    {
        InitializeComponent();
        OsuClient client = new OsuClient();
        var newsListings = client.GetNewsListingsAsync();

        foreach (var newsPiece in newsListings.Result.Data)
        {
            CarouselItem newsItem = new CarouselItem();

            newsItem.Content = newsPiece.Title;
            NewsCarousel.Items.Add(newsItem);
        }
    }
}