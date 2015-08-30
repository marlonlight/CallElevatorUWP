﻿using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CallElevator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        IHubProxy chat;
        public SynchronizationContext Context { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            makeConnection();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CallElevator();
        }

        async private void CallElevator()
        {
            try
            {
                tbstatus.Text = "Elevador foi chamado";
                await chat.Invoke("Send", "callElevator", "100");

            }
            catch (Exception ex)
            {

            }
        }

        async public void makeConnection()
        {
            try
            {
                var hubConnection = new HubConnection("http://bananasvc.azurewebsites.net");
                chat = hubConnection.CreateHubProxy("ChatHub");
                Context = SynchronizationContext.Current;
                chat.On<string, string>("broadcastMessage",
                    (name, message) =>
                        Context.Post(delegate
                        {
                            if (message.Equals("200"))
                                tbstatus.Text = "Elevador Chegou!!!";
                        }, null)
                        );
                await hubConnection.Start();
            }
            catch (Exception ex)
            {

            }
        }

    }
}
