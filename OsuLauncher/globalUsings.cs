﻿//Internal Dependencies
global using API;
global using API.Objects;
global using OsuLauncher;
global using OsuLauncher.Views;
global using OsuLauncher.Models;
global using OsuLauncher.Helpers;
global using OsuLauncher.Dialogs;
global using OsuLauncher.Services;
global using User = API.Objects.User;

//Native Dependencies
global using System;
global using System.IO;
global using System.Linq;
global using System.Threading.Tasks;
global using System.ComponentModel;
global using System.Threading;
global using System.Windows;
global using System.Security.Cryptography;
global using System.Text.Json;
global using System.Windows.Input;
global using System.Net;
global using System.Collections.ObjectModel;
global using System.Net.Http;
global using System.Windows.Media;
global using System.Reflection;
global using System.Windows.Media.Imaging;
global using System.Text.Json.Serialization;
global using System.Diagnostics;
global using System.Globalization;
global using Path = System.IO.Path;
global using System.Windows.Controls;
global using System.Collections.Generic;
global using System.Text.RegularExpressions;
global using MessageBox = System.Windows.MessageBox;
global using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;
global using AutoUpdaterDotNET;

//External Dependencies
global using Serilog;
global using DiscordRPC;
global using CollectionManager;
global using DiscordRPC.Logging;
global using HandyControl.Themes;
global using HandyControl.Controls;
global using CollectionManager.DataTypes;
global using CollectionManager.Modules.FileIO;
global using Window = HandyControl.Controls.Window;
global using Microsoft.Extensions.Configuration;
global using DotNetConfig;
global using Downloader;
global using CommunityToolkit.Mvvm;
global using CommunityToolkit.Mvvm.Input;
global using CommunityToolkit.Mvvm.ComponentModel;
/*global using OsuMemoryReader = OsuMemoryDataProvider;
global using OsuMemoryDataProvider.OsuMemoryModels;
global using OsuMemoryDataProvider.OsuMemoryModels.Direct;*/