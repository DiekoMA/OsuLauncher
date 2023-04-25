using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Windows;
using RestSharp;

namespace OsuLauncher.Dialogs;

public partial class AuthCodeDialog : Window
{
    public AuthCodeDialog()
    {
        InitializeComponent();
        TokenSwapButton.Click += (sender, args) => InitiateTokenSwap(CodeBox.Text);
    }

    private async void InitiateTokenSwap(string code)
    {
        try
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://osu.ppy.sh/oauth/token");

                var formData = new Dictionary<string, string>
                {
                    { "client_id", "21770" },
                    { "client_secret", "CDMaHyAmbtex7f5Oxl3iUA7POwPESfCjkoP1fG3D" },
                    { "code", code },
                    { "grant_type", "authorization_code" },
                    { "redirect_uri", "http://localhost:7040" }
                };

                request.Content = new FormUrlEncodedContent(formData);

                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to swap osu! API tokens.");
                }

                var json = await response.Content.ReadAsStringAsync();
                dynamic tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                MessageBox.Show(tokenResponse.access_token.ToString());
                
                File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "token.secret"), tokenResponse.access_token.ToString());
                this.Close();
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
        }
        
    }
}