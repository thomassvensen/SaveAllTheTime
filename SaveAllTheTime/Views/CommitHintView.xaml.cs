﻿using System.Reactive.Linq;
using ReactiveUI;
using SaveAllTheTime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using SaveAllTheTime.ViewModels;

namespace SaveAllTheTime.Views
{
    /// <summary>
    /// Interaction logic for CommitHintView.xaml
    /// </summary>
    public partial class CommitHintView : UserControl, IViewFor<CommitHintViewModel>
    {
        public CommitHintView()
        {
            InitializeComponent();

            this.WhenAnyObservable(x => x.ViewModel.Open.CanExecuteObservable)
                .BindTo(this, x => x.Open.Visibility);

            this.WhenAny(x => x.ViewModel.HintState, x => x.Value.ToString())
                .Subscribe(x => VisualStateManager.GoToElementState(visualRoot, x, true));

            this.WhenAnyObservable(x => x.ViewModel.RefreshStatus.ItemsInflight)
                .Select(x => x != 0 ? "Loading" : "NotLoading")
                .Subscribe(x => VisualStateManager.GoToElementState(visualRoot, x, true));

            this.BindCommand(ViewModel, x => x.Open, x => x.Open);

            this.WhenAnyObservable(x => x.ViewModel.Open)
                .Subscribe(x => Process.Start(ViewModel.ProtocolUrl));

            this.OneWayBind(ViewModel, x => x.SuggestedOpacity, x => x.RootVisual.Opacity);
        }

        public CommitHintViewModel ViewModel {
            get { return (CommitHintViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(CommitHintViewModel), typeof(CommitHintView), new PropertyMetadata(null));

        object IViewFor.ViewModel {
            get { return ViewModel; }
            set { ViewModel = (CommitHintViewModel)value; }
        }
    }
}