﻿using System.Windows.Forms;
using RossCode.SkypeLight.Core.Eventing;
using RossCode.SkypeLight.Core.Eventing.Events;
using RossCode.SkypeLight.Core.Factories;
using RossCode.SkypeLight.UI.Properties;

namespace RossCode.SkypeLight.UI
{
    public class SkypeLightApplicationContext : ApplicationContext
    {
        private System.ComponentModel.IContainer components;
		private NotifyIcon skypeLightNotifyIcon;
		private ContextMenu skypeLightNotifyIconContextMenu;
		private MenuItem exitContextMenuItem;
		private MenuItem showCallStatusMenuItem;
        private CallStatus callStatus;

        public SkypeLightApplicationContext()
        {
            InitializeContext();
            InitializeCallIndicators();
            InitializeCallMonitors();
        }

        private void InitializeContext()
        {
			components = new System.ComponentModel.Container();
			
			InitializeNotifyIcon();
		    InitializeNotifyIconContextMenu();
        }

        private void InitializeNotifyIcon()
        {
            skypeLightNotifyIcon = new NotifyIcon(components);
            skypeLightNotifyIconContextMenu = new ContextMenu();
			
            skypeLightNotifyIcon.ContextMenu = skypeLightNotifyIconContextMenu;
            skypeLightNotifyIcon.DoubleClick += (sender, e) => ShowCallStatusView();
            skypeLightNotifyIcon.Icon = Resources.UnknownCallStatusIcon;
            skypeLightNotifyIcon.Text = "SkypeLight";
            skypeLightNotifyIcon.Visible = true;
        }

        private void InitializeNotifyIconContextMenu()
        {
            showCallStatusMenuItem = new MenuItem();
            exitContextMenuItem = new MenuItem();

            skypeLightNotifyIconContextMenu.MenuItems.AddRange(new[] { showCallStatusMenuItem, exitContextMenuItem });

            var menuIndex = 0;
            showCallStatusMenuItem.Index = menuIndex;
            showCallStatusMenuItem.Text = "Show &Status";
            showCallStatusMenuItem.DefaultItem = true;
            showCallStatusMenuItem.Click += (sender, e) => ShowCallStatusView();
            menuIndex++;

            exitContextMenuItem.Index = menuIndex;
            exitContextMenuItem.Text = "&Exit";
            exitContextMenuItem.Click += (sender, e) => ExitThread();
        }

        private void ShowCallStatusView()
        {
            new CallStatusForm(callStatus).Show();
        }

        private void InitializeCallMonitors()
        {
            var skypeService = ServiceFactory.GetSkypeService();
            skypeService.Process();
        }

        private void InitializeCallIndicators()
        {
            DomainEvents.Register<CallStatusChanged>(args =>
                {
                    callStatus = args.CallStatus;
                    skypeLightNotifyIcon.Icon = args.CallStatus == CallStatus.NotOnCall 
                        ? Resources.NoCallStatusIcon 
                        : Resources.OnCallStatusIcon;
                });
        }
    }
}