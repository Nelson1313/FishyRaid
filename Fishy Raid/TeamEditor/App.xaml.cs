﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TeamEditor.Logic;

namespace TeamEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() {
            Ioc.Default.ConfigureServices(
                    new ServiceCollection()
                        .AddSingleton<ITeamEditorLogic, TeamEditorLogic>()
                        .AddSingleton<IMessenger>(WeakReferenceMessenger.Default)
                        .BuildServiceProvider()
                    );
        }
    }
}
