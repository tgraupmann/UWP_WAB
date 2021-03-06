﻿using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Security.Authentication.Web;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWP_WAB
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            ApplicationView.PreferredLaunchViewSize = new Size(600, 400);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        private async Task<string> SafeWebAuth(string startURL, string endURL)
        {
            try
            {
                WebAuthenticationResult webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                                    WebAuthenticationOptions.None,
                                    new Uri(startURL),
                                    new Uri(endURL));
                if (webAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    return webAuthenticationResult.ResponseData.ToString();
                }
            }
            catch (Exception ex)
            {
                return string.Format("Exception: {0}", ex);
            }

            return null;
        }

        private async Task OpenWAB(string url)
        {
            try
            {
                string result = await SafeWebAuth(url, "https://twitch.tv");
                if (result == null)
                {
                    _mTxtResult.Text = "User closed WAB";
                }
                else
                {
                    _mTxtResult.Text = result;
                }
            }
            catch (Exception ex)
            {
                _mTxtResult.Text = string.Format("Exception: {0}", ex);
            }
        }

        private async void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            await OpenWAB(_mTxtUrl.Text);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _mTxtUrl.Text = string.Format("https://sample.com/oath2?guid={0}", Guid.NewGuid());
        }
    }
}
