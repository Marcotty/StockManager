﻿using Microsoft.Extensions.DependencyInjection;
using StockManager.Services;

namespace StockManager
{
    public partial class App : Application
    {
        private readonly IStockService _stockService;
        public App(IStockService stockService)
        {
            InitializeComponent();
            _stockService = stockService;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            Console.WriteLine("App is sleeping. State saved.");
            _stockService?.SaveStockListToStockFile();
        }
        protected override void OnStart()
        {
            base.OnStart();
            Console.WriteLine("App is starting.");
            _stockService?.LoadStockListFromStockFile();
        }
    }
}