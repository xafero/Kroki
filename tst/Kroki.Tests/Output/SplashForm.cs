using System;
using System.Collections.Generic;
using System.Text;
using Kroki.Runtime;
using System.Windows.Forms;

namespace Kroki.Example
{
    public partial class SharpSplashWnd : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
    }

    partial class SharpSplashWnd
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Closetimer = new TTimer();
            // Create until here
            SuspendLayout();
            // 
            // Closetimer
            // 
            Closetimer.Enabled = false;
            Closetimer.Interval = 4000;
            Closetimer.OnTimer = ClosetimerTimer;
            Closetimer.Left = 16;
            // 
            // SharpSplashWnd
            // 
            Left = 468;
            Top = 173;
            HorzScrollBar.Visible = false;
            VertScrollBar.Visible = false;
            BorderIcons = new[]
            {
            };
            BorderStyle = bsNone;
            Caption = "SharpSplash";
            ClientHeight = 265;
            ClientWidth = 259;
            Color = clBtnFace;
            Font.Charset = DEFAULT_CHARSET;
            Font.Color = clWindowText;
            Font.Height = -11;
            Font.rName = "MS Sans Serif";
            Font.Style = new[]
            {
            };
            FormStyle = fsStayOnTop;
            OldCreateOrder = false;
            OnActivate = FormActivate;
            OnClose = FormClose;
            OnCreate = FormCreate;
            OnDestroy = FormDestroy;
            OnShow = FormShow;
            PixelsPerInch = 96;
            TextHeight = 13;
            // Now finish the layout
            ResumeLayout(false);
            PerformLayout();
        }

        private TTimer Closetimer;
    }
}
