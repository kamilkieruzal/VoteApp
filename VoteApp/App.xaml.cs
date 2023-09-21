using Autofac;
using Prism.Events;
using System;
using System.Windows;
using VoteApp.DatabaseContext;
using VoteApp.Interfaces.Interfaces;
using VoteApp.Models;
using VoteApp.Services;
using VoteApp.ViewModels;
using VoteApp.Windows;

namespace VoteApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ContainerBuilder builder;

        public App()
        {
            builder = new ContainerBuilder();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<VoterService>().As<IAddService<Voter>>();
            builder.RegisterType<CandidateService>().As<IAddService<Candidate>>();
            builder.RegisterType<CandidateService>().As<ICandidateService>();
            builder.RegisterType<VoteService>().As<IVoteService>();
            builder.RegisterType<VoterService>().As<IVoterService>();
            builder.RegisterType<MessageBoxService>().As<IMessageBoxService>();
            builder.RegisterType<VoteAppDbContext>().AsSelf().SingleInstance();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<AddViewModel>().AsSelf();
            builder.RegisterType<MainWindowViewModel>().AsSelf();
            builder.RegisterType<CandidatesViewModel>().AsSelf();
            builder.RegisterType<VotersViewModel>().AsSelf();
            builder.RegisterType<VotePanelViewModel>().AsSelf();
            builder.RegisterType<AddWindow>().AsSelf();
        }

        private void VoteAppStart(object sender, EventArgs eventArgs)
        {
            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var main = scope.Resolve<MainWindow>();
                main.DataContext = scope.Resolve<MainWindowViewModel>();

                main.ShowDialog();
            }
        }
    }
}
