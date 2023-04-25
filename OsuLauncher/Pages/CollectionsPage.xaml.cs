using System;
using System.IO;
using System.Windows.Controls;
using HandyControl.Controls;
using osu_database_reader.BinaryFiles;
using osu_database_reader.TextFiles;
using OsuLauncher.Helpers;

namespace OsuLauncher.Pages;

public partial class CollectionsPage : Page
{
    public CollectionsPage()
    {
        InitializeComponent();
        try
        {
            CollectionDb db;

            var stream = Path.Combine(ConfigHelper.GetStringItem("preferences", "gamedir"), "collection.db");
            db = CollectionDb.Read(stream);
            
            foreach (var collection in db.Collections)
            {
                
                CollectionsList.Items.Add(collection);
            }
            //CollectionsList.ItemsSource = db.Collections;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }
}