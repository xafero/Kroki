using System;
using System.Collections.Generic;
using System.Text;
using Kroki.Runtime;
using System.Windows.Forms;

namespace Kroki.Example
{
    public partial class frmSettingsWnd : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
    }

    partial class frmSettingsWnd
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
            dlgOpen = new TOpenDialog();
            ssmConfig = new TSharpESwatchManager();
            SharpECenterHeader1 = new TSharpECenterHeader();
            cboMonitor = new TComboBox();
            pnlMonitorList = new TPanel();
            pnlMonitor = new TSharpERoundPanel();
            SharpECenterHeader11 = new TSharpECenterHeader();
            imgGradient = new TImage32();
            SharpECenterHeader4 = new TSharpECenterHeader();
            SharpECenterHeader5 = new TSharpECenterHeader();
            SharpECenterHeader3 = new TSharpECenterHeader();
            secGradColor = new TSharpEColorEditorEx();
            sgbGradEndTrans = new TSharpeGaugeBox();
            sgbGradStartTrans = new TSharpeGaugeBox();
            Panel11 = new TPanel();
            cboGradType = new TComboBox();
            Panel5 = new TPanel();
            pnlGrad = new TPanel();
            chkApplyGrad = new TJvXPCheckbox();
            pnlGradient = new TPanel();
            pagGradient = new TJvStandardPage();
            SharpECenterHeader12 = new TSharpECenterHeader();
            imgColor = new TImage32();
            SharpECenterHeader2 = new TSharpECenterHeader();
            sgbHue = new TSharpeGaugeBox();
            sgbSat = new TSharpeGaugeBox();
            sgbLum = new TSharpeGaugeBox();
            Panel10 = new TPanel();
            pnlColorHSL = new TPanel();
            chkApplyColor = new TJvXPCheckbox();
            pnlColor = new TPanel();
            pagColor = new TJvStandardPage();
            secWpColor = new TSharpEColorEditorEx();
            SharpECenterHeader10 = new TSharpECenterHeader();
            sgbWpTrans = new TSharpeGaugeBox();
            Panel9 = new TPanel();
            SharpECenterHeader9 = new TSharpECenterHeader();
            chkWpMirrorHoriz = new TJvXPCheckbox();
            chkWpMirrorVert = new TJvXPCheckbox();
            Panel8 = new TPanel();
            SharpECenterHeader8 = new TSharpECenterHeader();
            rdoWpAlignTile = new TJvXPCheckbox();
            rdoWpAlignCenter = new TJvXPCheckbox();
            rdoWpAlignScale = new TJvXPCheckbox();
            rdoWpAlignStretch = new TJvXPCheckbox();
            Panel7 = new TPanel();
            SharpECenterHeader7 = new TSharpECenterHeader();
            btnWpBrowse = new TButton();
            edtWpFile = new TEdit();
            pnlWallpaperFilePath = new TPanel();
            btnWpDirectoryBrowse = new TButton();
            edtWpDirectory = new TEdit();
            Panel1 = new TPanel();
            chkWpRandomize = new TJvXPCheckbox();
            sgbWpChangeInterval = new TSharpeGaugeBox();
            chkWpRecursive = new TJvXPCheckbox();
            pnlWallpaperDirectoryPath = new TPanel();
            pnlWallpaperOptions = new TPanel();
            chkAutoWallpaper = new TJvXPCheckbox();
            SharpECenterHeader6 = new TSharpECenterHeader();
            imgWallpaper = new TImage32();
            pnlWallpaper = new TPanel();
            pagWallpaper = new TJvStandardPage();
            plConfig = new TJvPageList();
            // Create until here
            SuspendLayout();
            // 
            // dlgOpen
            // 
            dlgOpen.Filter = "All Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            dlgOpen.Options = new[]
            {
                ofHideReadOnly,
                ofFileMustExist,
                ofEnableSizing
            };
            dlgOpen.Left = 56;
            dlgOpen.Top = 528;
            // 
            // ssmConfig
            // 
            ssmConfig.Swatches = null;
            ssmConfig.PopulateThemeColors = true;
            ssmConfig.Width = 601;
            ssmConfig.ShowCaptions = true;
            ssmConfig.SwatchHeight = 16;
            ssmConfig.SwatchWidth = 16;
            ssmConfig.SwatchSpacing = 4;
            ssmConfig.SwatchFont.Charset = DEFAULT_CHARSET;
            ssmConfig.SwatchFont.Color = clWindowText;
            ssmConfig.SwatchFont.Height = -11;
            ssmConfig.SwatchFont.rName = "Tahoma";
            ssmConfig.SwatchFont.Style = new[]
            {
            };
            ssmConfig.SwatchTextBorderColor = 16709617;
            ssmConfig.SortMode = sortName;
            ssmConfig.BorderColor = clBlack;
            ssmConfig.BackgroundColor = clBlack;
            ssmConfig.BackgroundTextColor = clBlack;
            ssmConfig.Left = 16;
            ssmConfig.Top = 532;
            // 
            // SharpECenterHeader1
            // 
            SharpECenterHeader1.AlignWithMargins = true;
            SharpECenterHeader1.Left = 5;
            SharpECenterHeader1.Top = 0;
            SharpECenterHeader1.Width = 632;
            SharpECenterHeader1.Height = 37;
            SharpECenterHeader1.Margins.Left = 5;
            SharpECenterHeader1.Margins.Top = 0;
            SharpECenterHeader1.Margins.Right = 5;
            SharpECenterHeader1.Margins.Bottom = 0;
            SharpECenterHeader1.Title = "Monitor";
            SharpECenterHeader1.Description = "Please select the monitor from the list below";
            SharpECenterHeader1.TitleColor = clWindowText;
            SharpECenterHeader1.DescriptionColor = clRed;
            SharpECenterHeader1.Align = alTop;
            // 
            // cboMonitor
            // 
            cboMonitor.Left = 0;
            cboMonitor.Top = 0;
            cboMonitor.Width = 327;
            cboMonitor.Height = 21;
            cboMonitor.Margins.Left = 28;
            cboMonitor.Margins.Top = 8;
            cboMonitor.Margins.Right = 8;
            cboMonitor.Margins.Bottom = 0;
            cboMonitor.Align = alLeft;
            cboMonitor.Style = csDropDownList;
            cboMonitor.Constraints.MaxWidth = 327;
            cboMonitor.DropDownCount = 12;
            cboMonitor.ItemHeight = 13;
            cboMonitor.TabOrder = 0;
            cboMonitor.OnChange = MonitorChangeEvent;
            // 
            // pnlMonitorList
            // 
            pnlMonitorList.AlignWithMargins = true;
            pnlMonitorList.Left = 5;
            pnlMonitorList.Top = 42;
            pnlMonitorList.Width = 632;
            pnlMonitorList.Height = 21;
            pnlMonitorList.Margins.Left = 5;
            pnlMonitorList.Margins.Top = 5;
            pnlMonitorList.Margins.Right = 5;
            pnlMonitorList.Margins.Bottom = 10;
            pnlMonitorList.Align = alTop;
            pnlMonitorList.AutoSize = true;
            pnlMonitorList.BevelOuter = bvNone;
            pnlMonitorList.ParentBackground = false;
            pnlMonitorList.ParentColor = true;
            pnlMonitorList.TabOrder = 0;
            pnlMonitorList.ExplicitTop = 47;
            pnlMonitorList.ExplicitHeight = 25;
            // 
            // pnlMonitor
            // 
            pnlMonitor.AlignWithMargins = true;
            pnlMonitor.Left = 0;
            pnlMonitor.Top = 0;
            pnlMonitor.Width = 642;
            pnlMonitor.Height = 82;
            pnlMonitor.Margins.Left = 0;
            pnlMonitor.Margins.Top = 0;
            pnlMonitor.Margins.Right = 0;
            pnlMonitor.Margins.Bottom = 10;
            pnlMonitor.Align = alTop;
            pnlMonitor.AutoSize = true;
            pnlMonitor.BevelOuter = bvNone;
            pnlMonitor.ParentBackground = false;
            pnlMonitor.ParentColor = true;
            pnlMonitor.TabOrder = 1;
            pnlMonitor.DrawMode = srpNormal;
            pnlMonitor.NoTopBorder = false;
            pnlMonitor.NoBottomBorder = false;
            pnlMonitor.RoundValue = 10;
            pnlMonitor.BorderColor = clBtnFace;
            pnlMonitor.Border = false;
            pnlMonitor.BackgroundColor = clWindow;
            pnlMonitor.BottomSideBorder = false;
            // 
            // SharpECenterHeader11
            // 
            SharpECenterHeader11.AlignWithMargins = true;
            SharpECenterHeader11.Left = 5;
            SharpECenterHeader11.Top = 0;
            SharpECenterHeader11.Width = 630;
            SharpECenterHeader11.Height = 37;
            SharpECenterHeader11.Margins.Left = 5;
            SharpECenterHeader11.Margins.Top = 0;
            SharpECenterHeader11.Margins.Right = 5;
            SharpECenterHeader11.Margins.Bottom = 0;
            SharpECenterHeader11.Title = "Gradient effect";
            SharpECenterHeader11.Description = "Define whether to apply a gradient effect";
            SharpECenterHeader11.TitleColor = clWindowText;
            SharpECenterHeader11.DescriptionColor = clRed;
            SharpECenterHeader11.Align = alTop;
            // 
            // imgGradient
            // 
            imgGradient.Left = 560;
            imgGradient.Top = 211;
            imgGradient.Width = 51;
            imgGradient.Height = 51;
            imgGradient.Bitmap.ResamplerClassrName = "TNearestResampler";
            imgGradient.BitmapAlign = baCenter;
            imgGradient.Scale = 1.000000000000000000;
            imgGradient.ScaleMode = smNormal;
            imgGradient.TabOrder = 2;
            imgGradient.Visible = false;
            // 
            // SharpECenterHeader4
            // 
            SharpECenterHeader4.AlignWithMargins = true;
            SharpECenterHeader4.Left = 5;
            SharpECenterHeader4.Top = 89;
            SharpECenterHeader4.Width = 630;
            SharpECenterHeader4.Height = 37;
            SharpECenterHeader4.Margins.Left = 5;
            SharpECenterHeader4.Margins.Top = 10;
            SharpECenterHeader4.Margins.Right = 5;
            SharpECenterHeader4.Margins.Bottom = 0;
            SharpECenterHeader4.Title = "Gradient visibility";
            SharpECenterHeader4.Description = "Define how visible the gradient is when applied";
            SharpECenterHeader4.TitleColor = clWindowText;
            SharpECenterHeader4.DescriptionColor = clRed;
            SharpECenterHeader4.Align = alTop;
            // 
            // SharpECenterHeader5
            // 
            SharpECenterHeader5.AlignWithMargins = true;
            SharpECenterHeader5.Left = 5;
            SharpECenterHeader5.Top = 168;
            SharpECenterHeader5.Width = 630;
            SharpECenterHeader5.Height = 37;
            SharpECenterHeader5.Margins.Left = 5;
            SharpECenterHeader5.Margins.Top = 10;
            SharpECenterHeader5.Margins.Right = 5;
            SharpECenterHeader5.Margins.Bottom = 0;
            SharpECenterHeader5.Title = "Gradient colour";
            SharpECenterHeader5.Description = "Define the gradient colours";
            SharpECenterHeader5.TitleColor = clWindowText;
            SharpECenterHeader5.DescriptionColor = clRed;
            SharpECenterHeader5.Align = alTop;
            SharpECenterHeader5.ExplicitTop = 173;
            // 
            // SharpECenterHeader3
            // 
            SharpECenterHeader3.AlignWithMargins = true;
            SharpECenterHeader3.Left = 5;
            SharpECenterHeader3.Top = 10;
            SharpECenterHeader3.Width = 630;
            SharpECenterHeader3.Height = 37;
            SharpECenterHeader3.Margins.Left = 5;
            SharpECenterHeader3.Margins.Top = 10;
            SharpECenterHeader3.Margins.Right = 5;
            SharpECenterHeader3.Margins.Bottom = 0;
            SharpECenterHeader3.Title = "Gradient type";
            SharpECenterHeader3.Description = "Define the gradient effect you wish to apply to the wallpaper";
            SharpECenterHeader3.TitleColor = clWindowText;
            SharpECenterHeader3.DescriptionColor = clRed;
            SharpECenterHeader3.Align = alTop;
            // 
            // secGradColor
            // 
            secGradColor.AlignWithMargins = true;
            secGradColor.Left = 1;
            secGradColor.Top = 215;
            secGradColor.Width = 634;
            secGradColor.Height = 56;
            secGradColor.Margins.Left = 1;
            secGradColor.Margins.Top = 10;
            secGradColor.Margins.Right = 5;
            secGradColor.Margins.Bottom = 0;
            secGradColor.VertScrollBar.Smooth = true;
            secGradColor.VertScrollBar.Tracking = true;
            secGradColor.Align = alTop;
            secGradColor.AutoSize = true;
            secGradColor.BevelInner = bvNone;
            secGradColor.BevelOuter = bvNone;
            secGradColor.BorderStyle = bsNone;
            secGradColor.Color = clBlack;
            secGradColor.ParentColor = false;
            secGradColor.TabOrder = 2;
            secGradColor.Items = null;
            secGradColor.SwatchManager = ssmConfig;
            secGradColor.OnUiChange = WallpaperColorUiChangeEvent;
            secGradColor.OnExpandCollapse = secColorExpandCollapse;
            secGradColor.BorderColor = clBlack;
            secGradColor.BackgroundColor = clBlack;
            secGradColor.BackgroundTextColor = clBlack;
            secGradColor.ContainerColor = clBlack;
            secGradColor.ContainerTextColor = clBlack;
            secGradColor.ExplicitTop = 230;
            // 
            // sgbGradEndTrans
            // 
            sgbGradEndTrans.AlignWithMargins = true;
            sgbGradEndTrans.Left = 167;
            sgbGradEndTrans.Top = 0;
            sgbGradEndTrans.Width = 163;
            sgbGradEndTrans.Height = 22;
            sgbGradEndTrans.Margins.Left = 4;
            sgbGradEndTrans.Margins.Top = 0;
            sgbGradEndTrans.Margins.Right = 0;
            sgbGradEndTrans.Margins.Bottom = 0;
            sgbGradEndTrans.Align = alLeft;
            sgbGradEndTrans.ParentBackground = false;
            sgbGradEndTrans.TabOrder = 1;
            sgbGradEndTrans.Min = 0;
            sgbGradEndTrans.Max = 255;
            sgbGradEndTrans.Value = 255;
            sgbGradEndTrans.Prefix = "End: ";
            sgbGradEndTrans.Suffix = "%";
            sgbGradEndTrans.Description = "Set the End Transparency";
            sgbGradEndTrans.PopPosition = ppBottom;
            sgbGradEndTrans.PercentDisplay = true;
            sgbGradEndTrans.SignDisplay = false;
            sgbGradEndTrans.MaxPercent = 100;
            sgbGradEndTrans.Formatting = "%d";
            sgbGradEndTrans.OnChangeValue = WallpaperTransChangeEvent;
            sgbGradEndTrans.BackgroundColor = clWindow;
            // 
            // sgbGradStartTrans
            // 
            sgbGradStartTrans.Left = 0;
            sgbGradStartTrans.Top = 0;
            sgbGradStartTrans.Width = 163;
            sgbGradStartTrans.Height = 22;
            sgbGradStartTrans.Align = alLeft;
            sgbGradStartTrans.ParentBackground = false;
            sgbGradStartTrans.TabOrder = 0;
            sgbGradStartTrans.Min = 0;
            sgbGradStartTrans.Max = 255;
            sgbGradStartTrans.Value = 255;
            sgbGradStartTrans.Prefix = "Start: ";
            sgbGradStartTrans.Suffix = "%";
            sgbGradStartTrans.Description = "Set the Start Transparency";
            sgbGradStartTrans.PopPosition = ppBottom;
            sgbGradStartTrans.PercentDisplay = true;
            sgbGradStartTrans.SignDisplay = false;
            sgbGradStartTrans.MaxPercent = 100;
            sgbGradStartTrans.Formatting = "%d";
            sgbGradStartTrans.OnChangeValue = WallpaperTransChangeEvent;
            sgbGradStartTrans.BackgroundColor = clWindow;
            // 
            // Panel11
            // 
            Panel11.AlignWithMargins = true;
            Panel11.Left = 5;
            Panel11.Top = 136;
            Panel11.Width = 630;
            Panel11.Height = 22;
            Panel11.Margins.Left = 5;
            Panel11.Margins.Top = 10;
            Panel11.Margins.Right = 5;
            Panel11.Margins.Bottom = 0;
            Panel11.Align = alTop;
            Panel11.BevelOuter = bvNone;
            Panel11.ParentBackground = false;
            Panel11.ParentColor = true;
            Panel11.TabOrder = 1;
            Panel11.ExplicitTop = 146;
            // 
            // cboGradType
            // 
            cboGradType.Left = 0;
            cboGradType.Top = 0;
            cboGradType.Width = 327;
            cboGradType.Height = 21;
            cboGradType.Align = alLeft;
            cboGradType.Style = csDropDownList;
            cboGradType.ItemHeight = 13;
            cboGradType.TabOrder = 0;
            cboGradType.OnSelect = cboGradTypeSelect;
            cboGradType.Items.Strings = null;
            // 
            // Panel5
            // 
            Panel5.AlignWithMargins = true;
            Panel5.Left = 5;
            Panel5.Top = 57;
            Panel5.Width = 630;
            Panel5.Height = 22;
            Panel5.Margins.Left = 5;
            Panel5.Margins.Top = 10;
            Panel5.Margins.Right = 5;
            Panel5.Margins.Bottom = 0;
            Panel5.Align = alTop;
            Panel5.BevelOuter = bvNone;
            Panel5.ParentBackground = false;
            Panel5.ParentColor = true;
            Panel5.TabOrder = 0;
            Panel5.ExplicitTop = 62;
            // 
            // pnlGrad
            // 
            pnlGrad.AlignWithMargins = true;
            pnlGrad.Left = 0;
            pnlGrad.Top = 74;
            pnlGrad.Width = 640;
            pnlGrad.Height = 271;
            pnlGrad.Margins.Left = 0;
            pnlGrad.Margins.Top = 4;
            pnlGrad.Margins.Right = 0;
            pnlGrad.Margins.Bottom = 0;
            pnlGrad.Align = alTop;
            pnlGrad.AutoSize = true;
            pnlGrad.BevelOuter = bvNone;
            pnlGrad.Color = clWindow;
            pnlGrad.TabOrder = 1;
            pnlGrad.ExplicitTop = 79;
            pnlGrad.ExplicitHeight = 286;
            // 
            // chkApplyGrad
            // 
            chkApplyGrad.AlignWithMargins = true;
            chkApplyGrad.Left = 5;
            chkApplyGrad.Top = 47;
            chkApplyGrad.Width = 630;
            chkApplyGrad.Height = 23;
            chkApplyGrad.Margins.Left = 5;
            chkApplyGrad.Margins.Top = 10;
            chkApplyGrad.Margins.Right = 5;
            chkApplyGrad.Margins.Bottom = 0;
            chkApplyGrad.Caption = "Apply gradient effects";
            chkApplyGrad.TabOrder = 0;
            chkApplyGrad.Align = alTop;
            chkApplyGrad.OnClick = ApplyGradientClickEvent;
            chkApplyGrad.ExplicitTop = 52;
            // 
            // pnlGradient
            // 
            pnlGradient.Left = 1;
            pnlGradient.Top = 1;
            pnlGradient.Width = 640;
            pnlGradient.Height = 345;
            pnlGradient.Align = alTop;
            pnlGradient.AutoSize = true;
            pnlGradient.BevelOuter = bvNone;
            pnlGradient.TabOrder = 0;
            // 
            // pagGradient
            // 
            pagGradient.Left = 0;
            pagGradient.Top = 0;
            pagGradient.Width = 642;
            pagGradient.Height = 648;
            pagGradient.Caption = "pagGradient";
            // 
            // SharpECenterHeader12
            // 
            SharpECenterHeader12.AlignWithMargins = true;
            SharpECenterHeader12.Left = 5;
            SharpECenterHeader12.Top = 0;
            SharpECenterHeader12.Width = 630;
            SharpECenterHeader12.Height = 37;
            SharpECenterHeader12.Margins.Left = 5;
            SharpECenterHeader12.Margins.Top = 0;
            SharpECenterHeader12.Margins.Right = 5;
            SharpECenterHeader12.Margins.Bottom = 0;
            SharpECenterHeader12.Title = "Colour blend";
            SharpECenterHeader12.Description = "Define whether to apply a colour blending effect";
            SharpECenterHeader12.TitleColor = clWindowText;
            SharpECenterHeader12.DescriptionColor = clRed;
            SharpECenterHeader12.Align = alTop;
            // 
            // imgColor
            // 
            imgColor.Left = 558;
            imgColor.Top = 93;
            imgColor.Width = 74;
            imgColor.Height = 59;
            imgColor.Bitmap.ResamplerClassrName = "TNearestResampler";
            imgColor.BitmapAlign = baTopLeft;
            imgColor.Scale = 1.000000000000000000;
            imgColor.ScaleMode = smNormal;
            imgColor.TabOrder = 2;
            imgColor.Visible = false;
            // 
            // SharpECenterHeader2
            // 
            SharpECenterHeader2.AlignWithMargins = true;
            SharpECenterHeader2.Left = 5;
            SharpECenterHeader2.Top = 10;
            SharpECenterHeader2.Width = 630;
            SharpECenterHeader2.Height = 37;
            SharpECenterHeader2.Margins.Left = 5;
            SharpECenterHeader2.Margins.Top = 10;
            SharpECenterHeader2.Margins.Right = 5;
            SharpECenterHeader2.Margins.Bottom = 0;
            SharpECenterHeader2.Title = "HSL Colour Adjust";
            SharpECenterHeader2.Description = "Change the values below to adjust the Hue, Saturation and Luminosity";
            SharpECenterHeader2.TitleColor = clWindowText;
            SharpECenterHeader2.DescriptionColor = clRed;
            SharpECenterHeader2.Align = alTop;
            // 
            // sgbHue
            // 
            sgbHue.Left = 0;
            sgbHue.Top = 0;
            sgbHue.Width = 137;
            sgbHue.Height = 22;
            sgbHue.Margins.Left = 0;
            sgbHue.Margins.Top = 8;
            sgbHue.Margins.Right = 8;
            sgbHue.Margins.Bottom = 0;
            sgbHue.Align = alLeft;
            sgbHue.ParentBackground = false;
            sgbHue.TabOrder = 0;
            sgbHue.Min = -255;
            sgbHue.Max = 255;
            sgbHue.Value = 0;
            sgbHue.Prefix = "Hue: ";
            sgbHue.Description = "Change Hue";
            sgbHue.PopPosition = ppBottom;
            sgbHue.PercentDisplay = false;
            sgbHue.SignDisplay = false;
            sgbHue.MaxPercent = 100;
            sgbHue.Formatting = "%d";
            sgbHue.OnChangeValue = HSLColorChangeEvent;
            sgbHue.BackgroundColor = clWindow;
            // 
            // sgbSat
            // 
            sgbSat.AlignWithMargins = true;
            sgbSat.Left = 141;
            sgbSat.Top = 0;
            sgbSat.Width = 137;
            sgbSat.Height = 22;
            sgbSat.Margins.Left = 4;
            sgbSat.Margins.Top = 0;
            sgbSat.Margins.Right = 0;
            sgbSat.Margins.Bottom = 0;
            sgbSat.Align = alLeft;
            sgbSat.ParentBackground = false;
            sgbSat.TabOrder = 1;
            sgbSat.Min = -255;
            sgbSat.Max = 255;
            sgbSat.Value = 0;
            sgbSat.Prefix = "Sat: ";
            sgbSat.Description = "Change Saturation";
            sgbSat.PopPosition = ppBottom;
            sgbSat.PercentDisplay = false;
            sgbSat.SignDisplay = false;
            sgbSat.MaxPercent = 100;
            sgbSat.Formatting = "%d";
            sgbSat.OnChangeValue = HSLColorChangeEvent;
            sgbSat.BackgroundColor = clWindow;
            // 
            // sgbLum
            // 
            sgbLum.AlignWithMargins = true;
            sgbLum.Left = 282;
            sgbLum.Top = 0;
            sgbLum.Width = 137;
            sgbLum.Height = 22;
            sgbLum.Margins.Left = 4;
            sgbLum.Margins.Top = 0;
            sgbLum.Margins.Right = 0;
            sgbLum.Margins.Bottom = 0;
            sgbLum.Align = alLeft;
            sgbLum.ParentBackground = false;
            sgbLum.TabOrder = 2;
            sgbLum.Min = -255;
            sgbLum.Max = 255;
            sgbLum.Value = 0;
            sgbLum.Prefix = "Lum: ";
            sgbLum.Description = "Change Luminance";
            sgbLum.PopPosition = ppBottom;
            sgbLum.PercentDisplay = false;
            sgbLum.SignDisplay = false;
            sgbLum.MaxPercent = 100;
            sgbLum.Formatting = "%d";
            sgbLum.OnChangeValue = HSLColorChangeEvent;
            sgbLum.BackgroundColor = clWindow;
            // 
            // Panel10
            // 
            Panel10.AlignWithMargins = true;
            Panel10.Left = 5;
            Panel10.Top = 57;
            Panel10.Width = 630;
            Panel10.Height = 22;
            Panel10.Margins.Left = 5;
            Panel10.Margins.Top = 10;
            Panel10.Margins.Right = 5;
            Panel10.Margins.Bottom = 0;
            Panel10.Align = alTop;
            Panel10.BevelOuter = bvNone;
            Panel10.ParentBackground = false;
            Panel10.ParentColor = true;
            Panel10.TabOrder = 0;
            Panel10.ExplicitTop = 62;
            // 
            // pnlColorHSL
            // 
            pnlColorHSL.AlignWithMargins = true;
            pnlColorHSL.Left = 0;
            pnlColorHSL.Top = 74;
            pnlColorHSL.Width = 640;
            pnlColorHSL.Height = 79;
            pnlColorHSL.Margins.Left = 0;
            pnlColorHSL.Margins.Top = 4;
            pnlColorHSL.Margins.Right = 0;
            pnlColorHSL.Margins.Bottom = 0;
            pnlColorHSL.Align = alTop;
            pnlColorHSL.AutoSize = true;
            pnlColorHSL.BevelOuter = bvNone;
            pnlColorHSL.Color = clWindow;
            pnlColorHSL.TabOrder = 1;
            pnlColorHSL.ExplicitTop = 79;
            pnlColorHSL.ExplicitHeight = 84;
            // 
            // chkApplyColor
            // 
            chkApplyColor.AlignWithMargins = true;
            chkApplyColor.Left = 5;
            chkApplyColor.Top = 47;
            chkApplyColor.Width = 630;
            chkApplyColor.Height = 23;
            chkApplyColor.Margins.Left = 5;
            chkApplyColor.Margins.Top = 10;
            chkApplyColor.Margins.Right = 5;
            chkApplyColor.Margins.Bottom = 0;
            chkApplyColor.Caption = "Apply colour blending effects";
            chkApplyColor.TabOrder = 0;
            chkApplyColor.Align = alTop;
            chkApplyColor.OnClick = ApplyColorClickEvent;
            chkApplyColor.ExplicitTop = 52;
            // 
            // pnlColor
            // 
            pnlColor.Left = 1;
            pnlColor.Top = 1;
            pnlColor.Width = 640;
            pnlColor.Height = 163;
            pnlColor.Align = alTop;
            pnlColor.AutoSize = true;
            pnlColor.BevelOuter = bvNone;
            pnlColor.TabOrder = 0;
            // 
            // pagColor
            // 
            pagColor.Left = 0;
            pagColor.Top = 0;
            pagColor.Width = 642;
            pagColor.Height = 648;
            pagColor.Caption = "pagColor";
            // 
            // secWpColor
            // 
            secWpColor.AlignWithMargins = true;
            secWpColor.Left = 1;
            secWpColor.Top = 502;
            secWpColor.Width = 634;
            secWpColor.Height = 32;
            secWpColor.Margins.Left = 1;
            secWpColor.Margins.Top = 10;
            secWpColor.Margins.Right = 5;
            secWpColor.Margins.Bottom = 0;
            secWpColor.VertScrollBar.Smooth = true;
            secWpColor.VertScrollBar.Tracking = true;
            secWpColor.Align = alTop;
            secWpColor.AutoSize = true;
            secWpColor.BevelInner = bvNone;
            secWpColor.BevelOuter = bvNone;
            secWpColor.BorderStyle = bsNone;
            secWpColor.Color = clBlack;
            secWpColor.ParentColor = false;
            secWpColor.TabOrder = 11;
            secWpColor.Items = null;
            secWpColor.SwatchManager = ssmConfig;
            secWpColor.OnUiChange = WallpaperColorUiChangeEvent;
            secWpColor.OnExpandCollapse = secColorExpandCollapse;
            secWpColor.BorderColor = clBlack;
            secWpColor.BackgroundColor = clBlack;
            secWpColor.BackgroundTextColor = clBlack;
            secWpColor.ContainerColor = clBlack;
            secWpColor.ContainerTextColor = clBlack;
            // 
            // SharpECenterHeader10
            // 
            SharpECenterHeader10.AlignWithMargins = true;
            SharpECenterHeader10.Left = 5;
            SharpECenterHeader10.Top = 455;
            SharpECenterHeader10.Width = 630;
            SharpECenterHeader10.Height = 37;
            SharpECenterHeader10.Margins.Left = 5;
            SharpECenterHeader10.Margins.Top = 10;
            SharpECenterHeader10.Margins.Right = 5;
            SharpECenterHeader10.Margins.Bottom = 0;
            SharpECenterHeader10.Title = "Background colour";
            SharpECenterHeader10.Description = "Define the wallpaper background colour";
            SharpECenterHeader10.TitleColor = clWindowText;
            SharpECenterHeader10.DescriptionColor = clRed;
            SharpECenterHeader10.Align = alTop;
            // 
            // sgbWpTrans
            // 
            sgbWpTrans.Left = 0;
            sgbWpTrans.Top = 0;
            sgbWpTrans.Width = 327;
            sgbWpTrans.Height = 22;
            sgbWpTrans.Margins.Left = 28;
            sgbWpTrans.Margins.Top = 8;
            sgbWpTrans.Margins.Right = 8;
            sgbWpTrans.Margins.Bottom = 0;
            sgbWpTrans.Align = alLeft;
            sgbWpTrans.ParentBackground = false;
            sgbWpTrans.TabOrder = 0;
            sgbWpTrans.Min = 0;
            sgbWpTrans.Max = 255;
            sgbWpTrans.Value = 0;
            sgbWpTrans.Prefix = "Transparency: ";
            sgbWpTrans.Suffix = "%";
            sgbWpTrans.Description = "Change Wallpaper Transparency";
            sgbWpTrans.PopPosition = ppBottom;
            sgbWpTrans.PercentDisplay = true;
            sgbWpTrans.SignDisplay = false;
            sgbWpTrans.MaxPercent = 100;
            sgbWpTrans.Formatting = "%d";
            sgbWpTrans.OnChangeValue = WallpaperTransChangeEvent;
            sgbWpTrans.BackgroundColor = clWindow;
            // 
            // Panel9
            // 
            Panel9.AlignWithMargins = true;
            Panel9.Left = 5;
            Panel9.Top = 423;
            Panel9.Width = 630;
            Panel9.Height = 22;
            Panel9.Margins.Left = 5;
            Panel9.Margins.Top = 10;
            Panel9.Margins.Right = 5;
            Panel9.Margins.Bottom = 0;
            Panel9.Align = alTop;
            Panel9.BevelOuter = bvNone;
            Panel9.ParentBackground = false;
            Panel9.ParentColor = true;
            Panel9.TabOrder = 4;
            // 
            // SharpECenterHeader9
            // 
            SharpECenterHeader9.AlignWithMargins = true;
            SharpECenterHeader9.Left = 5;
            SharpECenterHeader9.Top = 376;
            SharpECenterHeader9.Width = 630;
            SharpECenterHeader9.Height = 37;
            SharpECenterHeader9.Margins.Left = 5;
            SharpECenterHeader9.Margins.Top = 10;
            SharpECenterHeader9.Margins.Right = 5;
            SharpECenterHeader9.Margins.Bottom = 0;
            SharpECenterHeader9.Title = "Wallpaper visibility";
            SharpECenterHeader9.Description = "Define how visible the wallpaper is against the background colour";
            SharpECenterHeader9.TitleColor = clWindowText;
            SharpECenterHeader9.DescriptionColor = clRed;
            SharpECenterHeader9.Align = alTop;
            // 
            // chkWpMirrorHoriz
            // 
            chkWpMirrorHoriz.Left = 0;
            chkWpMirrorHoriz.Top = 0;
            chkWpMirrorHoriz.Width = 106;
            chkWpMirrorHoriz.Height = 30;
            chkWpMirrorHoriz.Caption = "Horizontal";
            chkWpMirrorHoriz.TabOrder = 0;
            chkWpMirrorHoriz.Align = alLeft;
            chkWpMirrorHoriz.OnClick = MirrorChangeEvent;
            // 
            // chkWpMirrorVert
            // 
            chkWpMirrorVert.Left = 106;
            chkWpMirrorVert.Top = 0;
            chkWpMirrorVert.Width = 127;
            chkWpMirrorVert.Height = 30;
            chkWpMirrorVert.Caption = "Vertical";
            chkWpMirrorVert.TabOrder = 1;
            chkWpMirrorVert.Align = alLeft;
            chkWpMirrorVert.OnClick = MirrorChangeEvent;
            // 
            // Panel8
            // 
            Panel8.AlignWithMargins = true;
            Panel8.Left = 5;
            Panel8.Top = 336;
            Panel8.Width = 630;
            Panel8.Height = 30;
            Panel8.Margins.Left = 5;
            Panel8.Margins.Top = 10;
            Panel8.Margins.Right = 5;
            Panel8.Margins.Bottom = 0;
            Panel8.Align = alTop;
            Panel8.BevelOuter = bvNone;
            Panel8.ParentBackground = false;
            Panel8.ParentColor = true;
            Panel8.TabOrder = 3;
            // 
            // SharpECenterHeader8
            // 
            SharpECenterHeader8.AlignWithMargins = true;
            SharpECenterHeader8.Left = 5;
            SharpECenterHeader8.Top = 289;
            SharpECenterHeader8.Width = 630;
            SharpECenterHeader8.Height = 37;
            SharpECenterHeader8.Margins.Left = 5;
            SharpECenterHeader8.Margins.Top = 10;
            SharpECenterHeader8.Margins.Right = 5;
            SharpECenterHeader8.Margins.Bottom = 0;
            SharpECenterHeader8.Title = "Wallpaper mirroring";
            SharpECenterHeader8.Description = "Define the mirroring options for the wallpaper";
            SharpECenterHeader8.TitleColor = clWindowText;
            SharpECenterHeader8.DescriptionColor = clRed;
            SharpECenterHeader8.Align = alTop;
            // 
            // rdoWpAlignTile
            // 
            rdoWpAlignTile.Left = 271;
            rdoWpAlignTile.Top = 0;
            rdoWpAlignTile.Width = 90;
            rdoWpAlignTile.Height = 30;
            rdoWpAlignTile.Caption = "Tile";
            rdoWpAlignTile.TabOrder = 3;
            rdoWpAlignTile.Align = alLeft;
            rdoWpAlignTile.OnClick = AlignmentChangeEvent;
            // 
            // rdoWpAlignCenter
            // 
            rdoWpAlignCenter.Left = 0;
            rdoWpAlignCenter.Top = 0;
            rdoWpAlignCenter.Width = 90;
            rdoWpAlignCenter.Height = 30;
            rdoWpAlignCenter.Caption = "Center";
            rdoWpAlignCenter.TabOrder = 0;
            rdoWpAlignCenter.Checked = true;
            rdoWpAlignCenter.State = cbChecked;
            rdoWpAlignCenter.Align = alLeft;
            rdoWpAlignCenter.OnClick = AlignmentChangeEvent;
            // 
            // rdoWpAlignScale
            // 
            rdoWpAlignScale.Left = 90;
            rdoWpAlignScale.Top = 0;
            rdoWpAlignScale.Width = 90;
            rdoWpAlignScale.Height = 30;
            rdoWpAlignScale.Caption = "Scale";
            rdoWpAlignScale.TabOrder = 1;
            rdoWpAlignScale.Align = alLeft;
            rdoWpAlignScale.OnClick = AlignmentChangeEvent;
            // 
            // rdoWpAlignStretch
            // 
            rdoWpAlignStretch.Left = 180;
            rdoWpAlignStretch.Top = 0;
            rdoWpAlignStretch.Width = 91;
            rdoWpAlignStretch.Height = 30;
            rdoWpAlignStretch.Caption = "Stretch";
            rdoWpAlignStretch.TabOrder = 2;
            rdoWpAlignStretch.Align = alLeft;
            rdoWpAlignStretch.OnClick = AlignmentChangeEvent;
            // 
            // Panel7
            // 
            Panel7.AlignWithMargins = true;
            Panel7.Left = 5;
            Panel7.Top = 249;
            Panel7.Width = 630;
            Panel7.Height = 30;
            Panel7.Margins.Left = 5;
            Panel7.Margins.Top = 10;
            Panel7.Margins.Right = 5;
            Panel7.Margins.Bottom = 0;
            Panel7.Align = alTop;
            Panel7.BevelOuter = bvNone;
            Panel7.ParentBackground = false;
            Panel7.ParentColor = true;
            Panel7.TabOrder = 2;
            // 
            // SharpECenterHeader7
            // 
            SharpECenterHeader7.AlignWithMargins = true;
            SharpECenterHeader7.Left = 5;
            SharpECenterHeader7.Top = 202;
            SharpECenterHeader7.Width = 630;
            SharpECenterHeader7.Height = 37;
            SharpECenterHeader7.Margins.Left = 5;
            SharpECenterHeader7.Margins.Top = 10;
            SharpECenterHeader7.Margins.Right = 5;
            SharpECenterHeader7.Margins.Bottom = 0;
            SharpECenterHeader7.Title = "Wallpaper allignment";
            SharpECenterHeader7.Description = "Define the alignment for the wallpaper";
            SharpECenterHeader7.TitleColor = clWindowText;
            SharpECenterHeader7.DescriptionColor = clRed;
            SharpECenterHeader7.Align = alTop;
            // 
            // btnWpBrowse
            // 
            btnWpBrowse.AlignWithMargins = true;
            btnWpBrowse.Left = 329;
            btnWpBrowse.Top = 0;
            btnWpBrowse.Width = 67;
            btnWpBrowse.Height = 22;
            btnWpBrowse.Margins.Top = 0;
            btnWpBrowse.Margins.Right = 0;
            btnWpBrowse.Margins.Bottom = 0;
            btnWpBrowse.Align = alLeft;
            btnWpBrowse.Caption = "Browse";
            btnWpBrowse.TabOrder = 1;
            btnWpBrowse.OnClick = btnWpBrowseClick;
            // 
            // edtWpFile
            // 
            edtWpFile.AlignWithMargins = true;
            edtWpFile.Left = 0;
            edtWpFile.Top = 0;
            edtWpFile.Width = 326;
            edtWpFile.Height = 22;
            edtWpFile.Margins.Left = 0;
            edtWpFile.Margins.Top = 0;
            edtWpFile.Margins.Right = 0;
            edtWpFile.Margins.Bottom = 0;
            edtWpFile.Align = alLeft;
            edtWpFile.AutoSize = false;
            edtWpFile.TabOrder = 0;
            edtWpFile.OnChange = edtWallpaperChange;
            // 
            // pnlWallpaperFilePath
            // 
            pnlWallpaperFilePath.Left = 0;
            pnlWallpaperFilePath.Top = 87;
            pnlWallpaperFilePath.Width = 630;
            pnlWallpaperFilePath.Height = 22;
            pnlWallpaperFilePath.Margins.Left = 5;
            pnlWallpaperFilePath.Margins.Top = 10;
            pnlWallpaperFilePath.Margins.Right = 5;
            pnlWallpaperFilePath.Margins.Bottom = 0;
            pnlWallpaperFilePath.Align = alTop;
            pnlWallpaperFilePath.AutoSize = true;
            pnlWallpaperFilePath.BevelOuter = bvNone;
            pnlWallpaperFilePath.ParentColor = true;
            pnlWallpaperFilePath.TabOrder = 1;
            // 
            // btnWpDirectoryBrowse
            // 
            btnWpDirectoryBrowse.AlignWithMargins = true;
            btnWpDirectoryBrowse.Left = 329;
            btnWpDirectoryBrowse.Top = 0;
            btnWpDirectoryBrowse.Width = 66;
            btnWpDirectoryBrowse.Height = 22;
            btnWpDirectoryBrowse.Margins.Top = 0;
            btnWpDirectoryBrowse.Margins.Right = 0;
            btnWpDirectoryBrowse.Margins.Bottom = 0;
            btnWpDirectoryBrowse.Align = alLeft;
            btnWpDirectoryBrowse.Caption = "Browse";
            btnWpDirectoryBrowse.TabOrder = 1;
            btnWpDirectoryBrowse.OnClick = btnWpDirectoryBrowseClick;
            // 
            // edtWpDirectory
            // 
            edtWpDirectory.AlignWithMargins = true;
            edtWpDirectory.Left = 0;
            edtWpDirectory.Top = 0;
            edtWpDirectory.Width = 326;
            edtWpDirectory.Height = 22;
            edtWpDirectory.Margins.Left = 0;
            edtWpDirectory.Margins.Top = 0;
            edtWpDirectory.Margins.Right = 0;
            edtWpDirectory.Margins.Bottom = 0;
            edtWpDirectory.Align = alLeft;
            edtWpDirectory.AutoSize = false;
            edtWpDirectory.TabOrder = 0;
            edtWpDirectory.OnChange = edtWpDirectoryChange;
            // 
            // Panel1
            // 
            Panel1.Left = 0;
            Panel1.Top = 0;
            Panel1.Width = 630;
            Panel1.Height = 22;
            Panel1.Align = alTop;
            Panel1.BevelOuter = bvNone;
            Panel1.TabOrder = 0;
            // 
            // chkWpRandomize
            // 
            chkWpRandomize.Left = 0;
            chkWpRandomize.Top = 64;
            chkWpRandomize.Width = 112;
            chkWpRandomize.Height = 22;
            chkWpRandomize.Caption = "Randomize";
            chkWpRandomize.TabOrder = 2;
            chkWpRandomize.OnClick = chkWpRandomizeClick;
            // 
            // sgbWpChangeInterval
            // 
            sgbWpChangeInterval.Left = 186;
            sgbWpChangeInterval.Top = 64;
            sgbWpChangeInterval.Width = 137;
            sgbWpChangeInterval.Height = 22;
            sgbWpChangeInterval.ParentBackground = false;
            sgbWpChangeInterval.TabOrder = 3;
            sgbWpChangeInterval.Min = 1;
            sgbWpChangeInterval.Max = 4320;
            sgbWpChangeInterval.Value = 30;
            sgbWpChangeInterval.Prefix = "Interval: ";
            sgbWpChangeInterval.Suffix = " minutes";
            sgbWpChangeInterval.Description = "Set how often the wallpaper should change.";
            sgbWpChangeInterval.PopPosition = ppBottom;
            sgbWpChangeInterval.PercentDisplay = false;
            sgbWpChangeInterval.SignDisplay = false;
            sgbWpChangeInterval.MaxPercent = 100;
            sgbWpChangeInterval.Formatting = "%d";
            sgbWpChangeInterval.OnChangeValue = sgbWpChangeIntervalChangeValue;
            sgbWpChangeInterval.BackgroundColor = clWindow;
            // 
            // chkWpRecursive
            // 
            chkWpRecursive.Left = 0;
            chkWpRecursive.Top = 36;
            chkWpRecursive.Width = 180;
            chkWpRecursive.Height = 22;
            chkWpRecursive.Caption = "Include Subdirectories";
            chkWpRecursive.TabOrder = 1;
            chkWpRecursive.OnClick = chkWpRecursiveClick;
            // 
            // pnlWallpaperDirectoryPath
            // 
            pnlWallpaperDirectoryPath.Left = 0;
            pnlWallpaperDirectoryPath.Top = 0;
            pnlWallpaperDirectoryPath.Width = 630;
            pnlWallpaperDirectoryPath.Height = 87;
            pnlWallpaperDirectoryPath.Margins.Left = 5;
            pnlWallpaperDirectoryPath.Margins.Top = 10;
            pnlWallpaperDirectoryPath.Margins.Right = 5;
            pnlWallpaperDirectoryPath.Margins.Bottom = 0;
            pnlWallpaperDirectoryPath.Align = alTop;
            pnlWallpaperDirectoryPath.BevelOuter = bvNone;
            pnlWallpaperDirectoryPath.ParentColor = true;
            pnlWallpaperDirectoryPath.TabOrder = 0;
            // 
            // pnlWallpaperOptions
            // 
            pnlWallpaperOptions.AlignWithMargins = true;
            pnlWallpaperOptions.Left = 5;
            pnlWallpaperOptions.Top = 83;
            pnlWallpaperOptions.Width = 630;
            pnlWallpaperOptions.Height = 109;
            pnlWallpaperOptions.Margins.Left = 5;
            pnlWallpaperOptions.Margins.Top = 10;
            pnlWallpaperOptions.Margins.Right = 5;
            pnlWallpaperOptions.Margins.Bottom = 0;
            pnlWallpaperOptions.Align = alTop;
            pnlWallpaperOptions.AutoSize = true;
            pnlWallpaperOptions.BevelOuter = bvNone;
            pnlWallpaperOptions.ParentBackground = false;
            pnlWallpaperOptions.ParentColor = true;
            pnlWallpaperOptions.TabOrder = 1;
            // 
            // chkAutoWallpaper
            // 
            chkAutoWallpaper.AlignWithMargins = true;
            chkAutoWallpaper.Left = 5;
            chkAutoWallpaper.Top = 47;
            chkAutoWallpaper.Width = 630;
            chkAutoWallpaper.Height = 23;
            chkAutoWallpaper.Margins.Left = 5;
            chkAutoWallpaper.Margins.Top = 10;
            chkAutoWallpaper.Margins.Right = 5;
            chkAutoWallpaper.Caption = "Enable automatic wallpaper changing";
            chkAutoWallpaper.TabOrder = 0;
            chkAutoWallpaper.Align = alTop;
            chkAutoWallpaper.OnClick = chkAutoWallpaperClick;
            // 
            // SharpECenterHeader6
            // 
            SharpECenterHeader6.AlignWithMargins = true;
            SharpECenterHeader6.Left = 5;
            SharpECenterHeader6.Top = 0;
            SharpECenterHeader6.Width = 630;
            SharpECenterHeader6.Height = 37;
            SharpECenterHeader6.Margins.Left = 5;
            SharpECenterHeader6.Margins.Top = 0;
            SharpECenterHeader6.Margins.Right = 5;
            SharpECenterHeader6.Margins.Bottom = 0;
            SharpECenterHeader6.Title = "Wallpaper options";
            SharpECenterHeader6.Description = "Define the wallpaper filename or directory for the selected monitor";
            SharpECenterHeader6.TitleColor = clWindowText;
            SharpECenterHeader6.DescriptionColor = clRed;
            SharpECenterHeader6.Align = alTop;
            // 
            // imgWallpaper
            // 
            imgWallpaper.Left = 340;
            imgWallpaper.Top = 157;
            imgWallpaper.Width = 279;
            imgWallpaper.Height = 174;
            imgWallpaper.Bitmap.ResamplerClassrName = "TNearestResampler";
            imgWallpaper.BitmapAlign = baTopLeft;
            imgWallpaper.Scale = 1.000000000000000000;
            imgWallpaper.ScaleMode = smNormal;
            imgWallpaper.TabOrder = 10;
            imgWallpaper.Visible = false;
            // 
            // pnlWallpaper
            // 
            pnlWallpaper.Left = 1;
            pnlWallpaper.Top = 1;
            pnlWallpaper.Width = 640;
            pnlWallpaper.Height = 534;
            pnlWallpaper.Align = alTop;
            pnlWallpaper.AutoSize = true;
            pnlWallpaper.BevelOuter = bvNone;
            pnlWallpaper.TabOrder = 0;
            // 
            // pagWallpaper
            // 
            pagWallpaper.Left = 0;
            pagWallpaper.Top = 0;
            pagWallpaper.Width = 642;
            pagWallpaper.Height = 648;
            pagWallpaper.Caption = "pagWallpaper";
            // 
            // plConfig
            // 
            plConfig.AlignWithMargins = true;
            plConfig.Left = 0;
            plConfig.Top = 92;
            plConfig.Width = 642;
            plConfig.Height = 648;
            plConfig.Margins.Left = 0;
            plConfig.Margins.Top = 0;
            plConfig.Margins.Right = 0;
            plConfig.Margins.Bottom = 0;
            plConfig.ActivePage = pagWallpaper;
            plConfig.PropagateEnable = false;
            plConfig.Align = alClient;
            // 
            // frmSettingsWnd
            // 
            Left = 0;
            Top = 0;
            BorderStyle = bsNone;
            Caption = "frmSettingsWnd";
            ClientHeight = 740;
            ClientWidth = 642;
            Color = clWindow;
            Font.Charset = DEFAULT_CHARSET;
            Font.Color = clWindowText;
            Font.Height = -11;
            Font.rName = "Tahoma";
            Font.Style = new[]
            {
            };
            OldCreateOrder = false;
            Position = poDesigned;
            OnCreate = FormCreate;
            OnDestroy = FormDestroy;
            PixelsPerInch = 96;
            TextHeight = 13;
            // Now finish the layout
            ResumeLayout(false);
            PerformLayout();
        }

        private TJvPageList plConfig;
        private TJvStandardPage pagWallpaper;
        private TPanel pnlWallpaper;
        private TImage32 imgWallpaper;
        private TSharpECenterHeader SharpECenterHeader6;
        private TJvXPCheckbox chkAutoWallpaper;
        private TPanel pnlWallpaperOptions;
        private TPanel pnlWallpaperDirectoryPath;
        private TJvXPCheckbox chkWpRecursive;
        private TSharpeGaugeBox sgbWpChangeInterval;
        private TJvXPCheckbox chkWpRandomize;
        private TPanel Panel1;
        private TEdit edtWpDirectory;
        private TButton btnWpDirectoryBrowse;
        private TPanel pnlWallpaperFilePath;
        private TEdit edtWpFile;
        private TButton btnWpBrowse;
        private TSharpECenterHeader SharpECenterHeader7;
        private TPanel Panel7;
        private TJvXPCheckbox rdoWpAlignStretch;
        private TJvXPCheckbox rdoWpAlignScale;
        private TJvXPCheckbox rdoWpAlignCenter;
        private TJvXPCheckbox rdoWpAlignTile;
        private TSharpECenterHeader SharpECenterHeader8;
        private TPanel Panel8;
        private TJvXPCheckbox chkWpMirrorVert;
        private TJvXPCheckbox chkWpMirrorHoriz;
        private TSharpECenterHeader SharpECenterHeader9;
        private TPanel Panel9;
        private TSharpeGaugeBox sgbWpTrans;
        private TSharpECenterHeader SharpECenterHeader10;
        private TSharpEColorEditorEx secWpColor;
        private TJvStandardPage pagColor;
        private TPanel pnlColor;
        private TJvXPCheckbox chkApplyColor;
        private TPanel pnlColorHSL;
        private TPanel Panel10;
        private TSharpeGaugeBox sgbLum;
        private TSharpeGaugeBox sgbSat;
        private TSharpeGaugeBox sgbHue;
        private TSharpECenterHeader SharpECenterHeader2;
        private TImage32 imgColor;
        private TSharpECenterHeader SharpECenterHeader12;
        private TJvStandardPage pagGradient;
        private TPanel pnlGradient;
        private TJvXPCheckbox chkApplyGrad;
        private TPanel pnlGrad;
        private TPanel Panel5;
        private TComboBox cboGradType;
        private TPanel Panel11;
        private TSharpeGaugeBox sgbGradStartTrans;
        private TSharpeGaugeBox sgbGradEndTrans;
        private TSharpEColorEditorEx secGradColor;
        private TSharpECenterHeader SharpECenterHeader3;
        private TSharpECenterHeader SharpECenterHeader5;
        private TSharpECenterHeader SharpECenterHeader4;
        private TImage32 imgGradient;
        private TSharpECenterHeader SharpECenterHeader11;
        private TSharpERoundPanel pnlMonitor;
        private TPanel pnlMonitorList;
        private TComboBox cboMonitor;
        private TSharpECenterHeader SharpECenterHeader1;
        private TSharpESwatchManager ssmConfig;
        private TOpenDialog dlgOpen;
    }
}
