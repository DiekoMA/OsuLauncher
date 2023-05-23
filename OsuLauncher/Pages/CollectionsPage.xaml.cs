namespace OsuLauncher.Pages;

public partial class CollectionsPage : Page
{
    public CollectionsPage()
    {
        InitializeComponent();
        
        try
        {
            /*CollectionDb db;

            var stream = Path.Combine(ConfigHelper.GetStringItem("preferences", "gamedir"), "collection.db");
            db = CollectionDb.Read(stream);
            
            foreach (var collection in db.Collections)
            {
                CollectionsList.Items.Add(collection.Name);
            }
            */

            //CollectionsList.ItemsSource = db.Collections;
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
    }

    private void CollectionsPage_OnLoaded(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("This has been loaded");
    }
}