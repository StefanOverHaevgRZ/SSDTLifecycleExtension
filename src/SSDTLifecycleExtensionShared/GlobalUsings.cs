// Global using directives

global using System;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.ComponentModel.Composition;
global using System.ComponentModel.Design;
global using System.ComponentModel;
global using System.Diagnostics.CodeAnalysis;
global using System.Globalization;
global using System.IO;
global using System.Linq;
global using System.Runtime.CompilerServices;
global using System.Runtime.InteropServices;
global using System.Threading.Tasks;
global using System.Threading;
global using System.Windows;
global using System.Windows.Data;
global using System.Windows.Input;
global using System.Windows.Media;
global using Microsoft.SqlServer.Dac;
global using Microsoft.SqlServer.Dac.Model;
global using Microsoft.VisualStudio.PlatformUI;
global using Microsoft.VisualStudio.Shell.Interop;
global using Microsoft.VisualStudio.Shell;
global using Microsoft.VisualStudio.Text;
global using Microsoft.VisualStudio.Text.Classification;
global using Microsoft.VisualStudio.Utilities;
global using Microsoft.VisualStudio;
global using Microsoft.Win32;
global using SSDTLifecycleExtension.Commands;
global using SSDTLifecycleExtension.DataAccess;
global using SSDTLifecycleExtension.MVVM;
global using SSDTLifecycleExtension.Shared.Contracts.Factories;
global using SSDTLifecycleExtension.Shared.Contracts.DataAccess;
global using SSDTLifecycleExtension.Shared.Contracts.Services;
global using SSDTLifecycleExtension.Shared.Contracts;
global using SSDTLifecycleExtension.Shared.Events;
global using SSDTLifecycleExtension.Shared.Models;
global using SSDTLifecycleExtension.Shared.ScriptModifiers;
global using SSDTLifecycleExtension.Shared.Services;
global using SSDTLifecycleExtension.Shared.WorkUnits;
global using SSDTLifecycleExtension.ViewModels;
global using SSDTLifecycleExtension.Windows;
global using Unity;
global using Unity.Lifetime;
global using Unity.Resolution;
global using DefaultConstraint = SSDTLifecycleExtension.Shared.Contracts.DefaultConstraint;
global using Task = System.Threading.Tasks.Task;