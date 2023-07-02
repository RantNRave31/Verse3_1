using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Core;
using NamedPipeWrapper;

namespace Verse3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class MainApplication : Application
    {
        private static CoreInterop.InteropServer _server;
        public static MainWindowModelView _mainWindowModelView;

        public MainApplication()
            : base()
        {

        }
        protected override void OnStartup(StartupEventArgs e)
        {
            ArsenalViewModel.domain_ = AppDomain.CurrentDomain;
            base.OnStartup(e);
            Core.Core.InitConsole();
            StartServer();
            ArsenalViewModel.StaticArsenal = new ArsenalViewModel();
            MainWindowModelView mainWindowModelView = _mainWindowModelView = new MainWindowModelView();
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel("Main Verse 3");
            _mainWindowModelView.DataContext = mainWindowViewModel;
            //serviceEquipmentViewModel.Refresh(null);
            mainWindowModelView.Show();
        }
        //Function that creates a new InteropServer in a new thread
        public static void StartServer()
        {
            Thread serverThread = new Thread(() =>
            {
                //TODO: SECURE INTEROP THREAD

                _server = new CoreInterop.InteropServer("Verse3", "Verse3");
                _server.ClientMessage += OnClientMessage;
                _server.ClientConnected += OnClientConnected;
                //_server.ClientDisconnected += OnClientDisconnected;
                //_server.Error += OnError;
            });
            serverThread.Start();
        }

        private static void OnClientConnected(object sender, EventArgs e)
        {
            CoreConsole.Log("Client connected");
            NamedPipeConnection<DataStructure, DataStructure> connection = (NamedPipeConnection<DataStructure, DataStructure>)sender;
            connection.PushMessage(new DataStructure<string>("Welcome"));
        }

        private static void OnClientMessage(object sender, DataStructure e)
        {
            CoreConsole.Log($"Client sent a message: {e.ToString()}");
        }
    }
}
