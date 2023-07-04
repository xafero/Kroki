using System;
using System.Collections.Generic;
using System.Text;
using Kroki.Runtime;

namespace Kroki.Example
{
    public class TSharpCenterWnd : TForm
    {
        public TPanel pnlSettingTree;
        public TPanel pnlMain;
        public TPanel PnlButtons;
        public TPngSpeedButton btnSave;
        public TPngSpeedButton btnCancel;
        public TPanel pnlContent;
        public TSharpETabList tlToolbar;
        public TSharpERoundPanel pnlToolbar;
        public TSharpEListBoxEx lbTree;
        public TPngImageList picMain;
        public TPngImageList pilIcons;
        public TPanel pnlLivePreview;
        public TImage32 imgLivePreview;
        public TSharpEPageControl pnlPluginContainer;
        public TScrollBox sbPlugin;
        public TPanel pnlPlugin;
        public TSharpEPageControl pnlEditContainer;
        public TSharpERoundPanel pnlEditPluginContainer;
        public TPanel pnlEditPlugin;
        public TPanel pnlEditToolbar;
        public TPngSpeedButton btnEditCancel;
        public TPngSpeedButton btnEditApply;
        public TTimer Timer1;
        public TPanel pnlTitle;
        public TPageControl pcToolbar;
        public TTabSheet tabHistory;
        public TTabSheet tabImport;
        public TPngSpeedButton btnImport;
        public TLabel Label1;
        public TEdit edImportFilename;
        public TTabSheet tabFav;
        public TSharpEListBoxEx lbHistory;
        public TTabSheet tabExport;
        public TLabel lblDescription;
        public TApplicationEvents ApplicationEvents1;
        public TSharpERoundPanel pnlHelp;
        public TPanel pnlHelpContent;
        public TMainMenu MainMenu1;
        public TScrollBox pnlSbHelpContent;
        public TSharpERoundPanel pnlHelpToggle;
        public TPngSpeedButton btnHelp;
        public TTimer HelpRefreshTimer;
        public void FormCloseQuery(object Sender, bool CanClose)
        {
            CanClose = true;
        }

        public void tlPluginTabsTabChange(object ASender, int ATabIndex, bool AChange)
        {
            LockWindowUpdate(Self.Handle);
            try
            {
            }
            finally
            {
                LockWindowUpdate(0);
            }
        }

        public void tlEditItemTabClick(object ASender, int ATabIndex)
        {
            LockWindowUpdate(Self.Handle);
            try
            {
                FSelectedTabID = ATabIndex;
                SCM.LoadEdit(FSelectedTabID);
            }
            finally
            {
                LockWindowUpdate(0);
            }
        }

        public void btnEditApplyClick(object Sender)
        {
            LockWindowUpdate(Self.Handle);
            try
            {
                SCM.ApplyEdit(FSelectedTabID);
            }
            finally
            {
                LockWindowUpdate(0);
            }
        }

        public void tlEditItemTabChange(object ASender, int ATabIndex, bool AChange)
        {
            if (SCM.CheckEditState)
            {
                AChange = false;
                exit();
            }

            switch (ATabIndex)
            {
                case Integer(tidAdd):
                    pilIcons.PngImages.Items(10).Background = pnlEditToolbar.Color;
                    btnEditApply.PngImage = pilIcons.PngImages.Items(10).PngImage;
                    break;
                case Integer(tidEdit):
                    pilIcons.PngImages.Items(0).Background = pnlEditToolbar.Color;
                    btnEditApply.PngImage = pilIcons.PngImages.Items(0).PngImage;
                    break;
                case Integer(tidDelete):
                    pilIcons.PngImages.Items(2).Background = pnlEditToolbar.Color;
                    btnEditApply.PngImage = pilIcons.PngImages.Items(2).PngImage;
                    break;
                default:
                    break;
            }
        }

        public void tlToolbarTabClick(object ASender, int ATabIndex)
        {
            if (SCM.PluginHost.Editing)
            {
                exit();
            }

            switch (ATabIndex)
            {
                case 0:
                    btnHomeClick(null);
                    break;
                case 2:
                    btnBackClick(null);
                    break;
                default:
                    break;
            }
        }

        public void tlToolbarTabChange(object ASender, int ATabIndex, bool AChange)
        {
            if (pnlToolbar == null)
            {
                exit();
            }

            if (SCM.CheckEditState)
            {
                AChange = false;
                exit();
            }

            switch (ATabIndex)
            {
                case Integer(tidHome):
                    pnlToolbar.Hide();
                    break;
                case Integer(tidFavourite):
                    pnlToolbar.Show();
                    tabFav.show();
                    break;
                case Integer(tidHistory):
                    pnlToolbar.Hide();
                    break;
                case Integer(tidImport):
                    pnlToolbar.Show();
                    tabImport.Show();
                    break;
                case Integer(tidExport):
                    pnlToolbar.Show();
                    tabExport.Show();
                    break;
                default:
                    break;
            }
        }

        public void btnEditCancelClick(object Sender)
        {
            LockWindowUpdate(Self.Handle);
            try
            {
                SCM.CancelEdit(FSelectedTabID);
            }
            finally
            {
                LockWindowUpdate(0);
                UpdateSize();
            }
        }

        public void btnFavouriteClick(object Sender)
        {
        }

        public void btnCancelClick(object Sender)
        {
            LockWindowUpdate(Self.Handle);
            try
            {
                FCancelClicked = true;
                SCM.PluginHost.Editing = false;
                SCM.PluginHost.Warning = false;
                SCM.Unload();
                SCM.Reload();
            }
            finally
            {
                FCancelClicked = false;
                LockWindowUpdate(0);
            }
        }

        public void btnSaveClick(object Sender)
        {
            if (SCM.CheckEditState)
            {
                exit();
            }

            LockWindowUpdate(Self.Handle);
            try
            {
                SCM.PluginHost.Editing = false;
                SCM.PluginHost.Warning = false;
                SCM.Save();
            }
            finally
            {
                LockWindowUpdate(0);
                PnlButtons.Hide();
            }
        }

        public void btnBackClick(object Sender)
        {
            TSharpCenterHistoryItem tmpItem = default;
            tmpItem = null;
            if (SCM.History.Count != 0)
            {
                SCM.History.DeleteItem(TSharpCenterHistoryItem(SCM.History.Last));
                tmpItem = TSharpCenterHistoryItem(SCM.History.Last);
            }

            if (tmpItem != null)
            {
                SCM.ExecuteCommand(tmpItem.Command, tmpItem.Param, tmpItem.PluginID, tmpItem.HelpFile, tmpItem.TabIndex);
                SCM.History.Delete(SCM.History.IndexOf(tmpItem));
                SetToolbarTabVisible(tidHistory, !(SCM.History.Count == 0));
            }
        }

        public void FormResize(object Sender)
        {
            UpdateSize();
            UpdateLivePreview();
            if (pnlHelp.Visible)
            {
                HelpRefreshTimer.Enabled = false;
                HelpRefreshTimer.Enabled = true;
            }
        }

        public void lbTree_MouseUp(object Sender, TMouseButton Button, TShiftState Shift, int X, int Y)
        {
            ClickItem();
        }

        public void btnHomeClick(object Sender)
        {
            InitToolbar();
            SCM.BuildNavRoot();
        }

        public void sbPluginResize(object Sender)
        {
            if (sbPlugin.VertScrollBar.IsScrollBarVisible)
            {
                sbPlugin.Padding.Right = 6;
            }
            else
            {
                sbPlugin.Padding.Right = 0;
            }
        }

        public void lbTreeClickItem(object Sender, int ACol, TSharpEListItem AItem)
        {
            lbTree.Repaint();
            if (lbTree.ItemIndex == -1)
            {
                exit();
            }

            if (SCM.CheckEditState)
            {
                exit();
            }

            ClickItem();
            Application.ProcessMessages();
        }

        public void FormMouseWheel(object Sender, TShiftState Shift, int WheelDelta, TPoint MousePos, bool Handled)
        {
            uint msg = default;
            uint code = default;
            int i = default;
            int n = default;
            TPoint CPos = default;
            if (!GetCursorPosSecure(CPos))
            {
                exit();
            }

            if (WindowFromPoint(CPos) == sbPlugin.Handle)
            {
                Handled = true;
                if (ssShift.In(Shift))
                {
                    msg = WM_HSCROLL;
                }
                else
                {
                    msg = WM_VSCROLL;
                }

                if (WheelDelta > 0)
                {
                    code = SB_LINEUP;
                }
                else
                {
                    code = SB_LINEDOWN;
                }

                n = Mouse.WheelScrollLines;
                for (i = 1; i <= n; i++)
                {
                    sbPlugin.Perform(msg, code, 0);
                }

                sbPlugin.Perform(msg, SB_ENDSCROLL, 0);
            }
        }

        public void lbTreeGetCellText(object Sender, int ACol, TSharpEListItem AItem, string AColText)
        {
            TSharpCenterManagerItem tmp = default;
            TColor col = default;
            tmp = TSharpCenterManagerItem(AItem.Data);
            if (tmp == null)
            {
                exit();
            }

            if (lbTree.SelectedItem == AItem)
            {
                col = SCM.Theme.NavBarSelectedItemText;
            }
            else
            {
                col = SCM.Theme.NavBarItemText;
            }

            switch (ACol)
            {
                case 0:
                    AColText = format("<font color=\"%s\">%s", new[] { colortoString(col), tmp.Caption });
                    break;
                case 1:
                    AColText = format("<font color=\"%s\">%s", new[] { colortoString(col), tmp.Status });
                    break;
                default:
                    break;
            }
        }

        public void lbTreeGetCellColor(object Sender, TSharpEListItem AItem, TColor AColor)
        {
            if (AItem.ID == lbTree.ItemIndex)
            {
                AColor = SCM.Theme.NavBarSelectedItem;
            }
        }

        public void tlToolbarBtnClick(object ASender, int ABtnIndex)
        {
            if (SCM.PluginHost.Editing)
            {
                exit();
            }

            switch (ABtnIndex)
            {
                case 0:
                    btnHomeClick(null);
                    break;
                case 1:
                    btnBackClick(null);
                    break;
                default:
                    break;
            }
        }

        public void FormShow(object Sender)
        {
            {
                SCM.PluginHost.PluginOwner = pnlPlugin;
                SCM.PluginHost.EditOwner = pnlEditPlugin;
                SCM.PngImageList = picMain;
            }

            AssignPluginEvents();
            AssignPluginHostEvents();
            InitWindow();
            InitToolbar();
            InitCommandLine();
        }

        public void pnlPluginContainerTabClick(object ASender, int ATabIndex)
        {
            LockWindowUpdate(Self.Handle);
            try
            {
                SCM.ClickTab(ATabIndex);
                SCM.PluginTabIndex = ATabIndex;
                TSharpCenterHistoryItem(SCM.History.Last).TabIndex = ATabIndex;
                UpdateSize();
            }
            finally
            {
                LockWindowUpdate(0);
            }
        }

        public void ApplicationEvents1Message(tagMSG Msg, bool Handled)
        {
            TWinControl actrl = default;
            Handled = false;
            if ((Msg.Message == WM_KEYDOWN))
            {
                if (IsWindow(SCM.EditWndHandle))
                {
                    actrl = TForm(GetControlByHandle(SCM.EditWndHandle)).ActiveControl;
                    if (actrl != null)
                    {
                        Handled = true;
                        if (Msg.wParam == VK_TAB)
                        {
                            SendMessage(SCM.EditWndHandle, Msg.message, Msg.wParam, Msg.lParam);
                        }
                        else
                        {
                            if ((Msg.wParam == VK_LEFT) || (Msg.wParam == VK_RIGHT) || (Msg.wParam == VK_UP) || (Msg.wParam == VK_DOWN))
                            {
                                SendMessage(actrl.Handle, Msg.message, Msg.wParam, Msg.lParam);
                            }
                            else
                            {
                                Handled = false;
                            }
                        }
                    }
                }
                else
                {
                    if (IsWindow(SCM.PluginWndHandle))
                    {
                        actrl = TForm(GetControlByHandle(SCM.PluginWndHandle)).ActiveControl;
                        if (actrl != null)
                        {
                            Handled = true;
                            if (Msg.wParam == VK_TAB)
                            {
                                SendMessage(SCM.PluginWndHandle, Msg.message, Msg.wParam, Msg.lParam);
                            }
                            else
                            {
                                if ((Msg.wParam == VK_LEFT) || (Msg.wParam == VK_RIGHT) || (Msg.wParam == VK_UP) || (Msg.wParam == VK_DOWN))
                                {
                                    SendMessage(actrl.Handle, Msg.message, Msg.wParam, Msg.lParam);
                                }
                                else
                                {
                                    Handled = false;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void FormDestroy(object Sender)
        {
            int i = default;
            for (i = lbTree.Count - 1; i >= 0; i--)
            {
                TObject(lbTree(i).data).Free();
            }

            SCM.Unload();
            FreeAndNil(SCM);
        }

        public void btnHelpClick(object Sender)
        {
            ToggleHelp(false);
        }

        public void HelpRefreshTimerTimer(object Sender)
        {
            HelpRefreshTimer.Enabled = false;
            ToggleHelp(true);
        }

        private bool FCancelClicked;
        private int FSelectedTabID;
        private bool FForceShow;
        private void UpdateLivePreview()
        {
            TBitmap32 bmp = default;
            if (SCM == null)
            {
                exit();
            }

            try
            {
                if ((SCM.PluginHasPreviewSupport))
                {
                    bmp = TBitmap32.Create;
                    try
                    {
                        bmp.DrawMode = dmBlend;
                        bmp.CombineMode = cmMerge;
                        bmp.SetSize(pnlLivePreview.Width, 50);
                        SCM.Plugin.PreviewInterface.UpdatePreview(bmp);
                        imgLivePreview.Color = SCM.Theme.Background;
                        pnlLivePreview.Color = SCM.Theme.Background;
                        imgLivePreview.Bitmap.SetSize(bmp.Width, bmp.Height);
                        imgLivePreview.Bitmap.Clear(color32(SCM.Theme.Background));
                        bmp.DrawTo(imgLivePreview.Bitmap, 0, 0);
                        pnlLivePreview.Height = bmp.Height;
                        pnlLivePreview.Margins.Top = 10;
                        pnlLivePreview.Margins.Bottom = 15;
                    }
                    finally
                    {
                        bmp.Free();
                    }
                }
                else
                {
                    imgLivePreview.Bitmap.SetSize(0, 0);
                    pnlLivePreview.Height = 0;
                    pnlLivePreview.Margins.Top = 0;
                    pnlLivePreview.Margins.Bottom = 0;
                }
            }
            finally
            {
            }
        }

        private void WMTerminateMessage(TMessage Msg)
        {
            Close();
        }

        private void WMSettingsUpdate(TMessage Msg)
        {
            ISharpETheme Theme = default;
            exit();
            if (new[]
            {
                TSU_UPDATE_ENUM(msg.WParam)
            }

            <= new[]
            {
                suSkinFont,
                suSkinFileChanged,
                suTheme,
                suIconSet,
                suScheme
            }

            )
            {
                Theme = GetCurrentTheme;
                switch (msg.WParam)
                {
                    case Integer(suSkinFont):
                        Theme.LoadTheme(new[] { tpSkinFont });
                        break;
                    case Integer(suSkinFileChanged):
                        Theme.LoadTheme(new[] { tpSkinScheme });
                        break;
                    case Integer(suTheme):
                        Theme.LoadTheme(ALL_THEME_PARTS);
                        break;
                    case Integer(suScheme):
                        Theme.LoadTheme(new[] { tpSkinScheme });
                        break;
                    case Integer(suIconSet):
                        Theme.LoadTheme(new[] { tpIconSet });
                        break;
                    default:
                        break;
                }
            }
        }

        private void ClickItem()
        {
            TSharpCenterManagerItem tmpItem = default;
            TSharpCenterHistoryItem tmpHistory = default;
            if (lbTree.ItemIndex == -1)
            {
                exit();
            }

            SCM.PluginTabIndex = 0;
            tmpItem = TSharpCenterManagerItem(lbTree.Item(lbTree.ItemIndex).Data);
            if (tmpItem == null)
            {
                exit();
            }

            switch (tmpItem.ItemType)
            {
                case itmNone:
                    break;
                case itmFolder:
                    SCM.BuildNavFromPath(tmpItem.Path);
                    SetToolbarTabVisible(tidHistory, true);
                    break;
                case itmSetting:
                    SCM.BuildNavFromFile(tmpItem.Filename);
                    SetToolbarTabVisible(tidHistory, true);
                    break;
                case itmDll:
                    tmpHistory = SCM.History.Item(SCM.History.IndexOf(SCM.History.Last));
                    tmpHistory.Command = sccLoadDll;
                    tmpHistory.PluginID = tmpItem.PluginID;
                    tmpHistory.Param = tmpItem.Filename;
                    tmpHistory.HelpFile = tmpItem.HelpFile;
                    SCM.Unload();
                    SCM.Load(tmpItem.Filename, tmpItem.PluginID, tmpItem.HelpFile);
                    break;
                default:
                    break;
            }
        }

        private void InitCommandLine()
        {
            TSCC_COMMAND_ENUM enumCommandType = default;
            string sPluginID = default;
            string sSection = default;
            string sName = default;
            string sParam = default;
            SendDebugMessage("SharpCenter", cmdline, 0);
            if (GetCommandLineParams(enumCommandType, sSection, sName, sPluginID))
            {
                sParam = SharpApi.GetCenterDirectory + sSection + "\\" + sName + SharpApi.GetCenterConfigExt;
                if ((enumCommandType == sccLoadSetting) && (!FileExists(sParam)))
                {
                    sParam = SharpApi.GetCenterDirectory + sSection;
                }

                if ((enumCommandType == sccLoadDll) && (!FileExists(sParam)))
                {
                    sParam = SharpApi.GetCenterDirectory + sSection + "\\DLL\\" + sName + ".dll";
                }

                SCM.ExecuteCommand(enumCommandType, sParam, sPluginID, "", 0);
            }
            else
            {
                SCM.BuildNavRoot();
            }
        }

        private void DoDoubleBufferAll(TComponent AComponent)
        {
            int i = default;
            if (Assigned(AComponent))
            {
                if (AComponent is TWinControl)
                {
                    TWinControl(AComponent).DoubleBuffered = true;
                }

                for (i = 0; i <= AComponent.ComponentCount - 1; i++)
                {
                    DoDoubleBufferAll(AComponent.Components(i));
                }
            }
        }

        private void UpdateConfigHeader()
        {
            TSharpCenterManagerItem tmp = default;
            if (lbTree.SelectedItem != null)
            {
                tmp = TSharpCenterManagerItem(lbTree.SelectedItem.Data);
                if (tmp != null)
                {
                    if ((ExtractFileExt(tmp.Filename) == ".dll"))
                    {
                        lblDescription.Caption = tmp.Description;
                        lblDescription.Hint = tmp.Description;
                        pnlTitle.Visible = (tmp.Description != "");
                    }
                }
            }
        }

        private void UpdateEditButtons()
        {
            if ((SCM.PluginHost.Editing))
            {
                btnEditApply.Visible = true;
                switch (FSelectedTabID)
                {
                    case integer(tidAdd):
                        btnEditApply.Caption = "Add";
                        break;
                    case integer(tidEdit):
                        btnEditApply.Caption = "Apply";
                        break;
                    case integer(tidDelete):
                        btnEditApply.Caption = "Delete";
                        btnEditApply.Visible = true;
                        break;
                    default:
                        break;
                }

                btnEditCancel.Caption = "Cancel";
            }
            else
            {
                btnEditApply.Visible = false;
                switch (FSelectedTabID)
                {
                    case integer(tidAdd):
                        btnEditApply.Caption = "Add";
                        if (FForceShow)
                        {
                            btnEditApply.Visible = true;
                            FForceShow = false;
                        }

                        break;
                    case integer(tidEdit):
                        btnEditApply.Caption = "Edit";
                        break;
                    case integer(tidDelete):
                        btnEditApply.Caption = "Delete";
                        btnEditApply.Visible = true;
                        break;
                    default:
                        break;
                }

                btnEditCancel.Caption = "Close";
            }
        }

        private void AssignPluginHostEvents()
        {
            SCM.PluginHost.OnSettingsChanged = SetHostSettingsChangedEvent;
            SCM.PluginHost.OnSetEditTab = SetEditTabEvent;
            SCM.PluginHost.OnSetEditTabVisibility = SetEditTabVisibilityEvent;
            SCM.PluginHost.OnRefreshSize = RefreshSizeEvent;
            SCM.PluginHost.OnRefreshPreview = RefreshPreviewEvent;
            SCM.PluginHost.OnRefreshTheme = RefreshThemeEvent;
            SCM.PluginHost.OnRefreshPluginTabs = RefreshPluginTabsEvent;
            SCM.PluginHost.OnRefreshAll = RefreshAllEvent;
            SCM.PluginHost.OnRefreshValidation = RefreshValidation;
            SCM.PluginHost.OnSetEditing = SetEditingEvent;
            SCM.PluginHost.OnSetWarning = SetWarningEvent;
            SCM.PluginHost.OnSetButtonVisibility = SetButtonVisibilityEvent;
            SCM.PluginHost.OnRefreshTitle = RefreshTitleEvent;
            SCM.PluginHost.OnSave = SaveEvent;
            SCM.PluginHost.OnThemeEditForm = AssignThemeToEditFormEvent;
            SCM.PluginHost.OnThemePluginForm = AssignThemeToPluginFormEvent;
        }

        private void AssignPluginEvents()
        {
            SCM.OnRefreshTheme = RefreshThemeEvent;
            SCM.OnInitNavigation = InitNavEvent;
            SCM.OnAddNavItem = AddItemEvent;
            SCM.OnSetHomeTitle = SetHomeTitleEvent;
            SCM.OnLoadPlugin = LoadPluginEvent;
            SCM.OnUnloadPlugin = UnloadPluginEvent;
            SCM.OnLoadEdit = LoadEditEvent;
            SCM.OnApplyEdit = ApplyEditEvent;
            SCM.OnCancelEdit = CancelEditEvent;
            SCM.OnAddPluginTabs = AddPluginTabsEvent;
            SCM.OnUpdateTheme = UpdateThemeEvent;
        }

        public void GetCopyData(TMessage Msg)
        {
            TsharpE_DataStruct tmpMsg = default;
            TSCC_COMMAND_ENUM command = default;
            List<String> strlTokens = default;
            string sSection = default;
            string sName = default;
            string sParam = default;
            tmpMsg = PSharpE_DataStruct(PCopyDataStruct(msg.lParam).lpData);
            strlTokens = TStringlist.Create;
            try
            {
                StrTokenToStrings(tmpMsg.Parameter, "|", strlTokens);
                sSection = "";
                if (strlTokens.Count > 0)
                {
                    sSection = strlTokens(0);
                }

                sName = "";
                if (strlTokens.Count > 1)
                {
                    sName = strlTokens(1);
                }
            }
            finally
            {
                strlTokens.Free();
            }

            sParam = SharpApi.GetCenterDirectory + sSection + "\\" + sName + SharpApi.GetCenterConfigExt;
            command = CenterCommandAsEnum(tmpMsg.Command);
            SCM.ExecuteCommand(command, sParam, tmpMsg.PluginID, "", 0);
            if (command == sccLoadSetting)
            {
                ForceForegroundWindow(Handle);
            }
        }

        public void InitWindow()
        {
            SetVistaFonts(Self);
            DoDoubleBufferAll(pnlTitle);
            DoDoubleBufferAll(lbTree);
            DoDoubleBufferAll(pnlContent);
            DoDoubleBufferAll(pnlEditToolbar);
            Self.ParentBackground = false;
            Self.DoubleBuffered = true;
            FSelectedTabID = 0;
            FCancelClicked = false;
            PnlButtons.Hide();
            pnlEditContainer.Visible = false;
            pnlLivePreview.Height = 0;
            pnlPluginContainer.Visible = false;
            pnlEditContainer.TabIndex = -1;
            pnlTitle.Visible = false;
            pnlToolbar.Hide();
            pnlHelpToggle.Top = pnlTitle.Top + pnlTitle.Height - pnlHelpToggle.Height;
            pnlHelp.Top = pnlTitle.Top + pnlTitle.Height - 1;
            pnlHelp.Width = pnlPluginContainer.Width;
            pnlHelp.Left = pnlPluginContainer.Left;
            pnlHelpToggle.Left = pnlPluginContainer.Left + pnlPluginContainer.Width - pnlHelpToggle.Width;
        }

        public void InitToolbar()
        {
            SetToolbarTabVisible(tidImport, false);
            SetToolbarTabVisible(tidExport, false);
            SetToolbarTabVisible(tidHistory, false);
            pnlToolbar.Visible = false;
            tlToolbar.TabIndex = 0;
            SCM.PluginTabIndex = 0;
        }

        public void UpdateSize()
        {
            int h = default;
            if (SCM == null)
            {
                exit();
            }

            if (SCM.PluginWndHandle != 0)
            {
                h = GetControlByHandle(SCM.PluginWndHandle).Height;
                pnlPlugin.Height = h;
                GetControlByHandle(SCM.PluginWndHandle).Width = pnlPlugin.Width;
            }

            if ((SCM.EditWndHandle != 0) && (IsWindow(SCM.EditWndHandle)))
            {
                pnlEditContainer.Minimized = false;
                pnlEditPluginContainer.Visible = true;
                pnlEditContainer.Height = 65 + GetControlByHandle(SCM.EditWndHandle).Height;
                GetControlByHandle(SCM.EditWndHandle).Width = pnlEditPlugin.Width;
            }
            else
            {
                pnlEditContainer.Minimized = true;
                pnlEditPluginContainer.Visible = false;
            }
        }

        public void ClearHelp()
        {
            int n = default;
            for (n = pnlSbHelpContent.ComponentCount - 1; n >= 0; n--)
            {
                pnlSbHelpContent.Components(n).Free();
            }
        }

        public void ToggleHelp(bool Update)
        {
            string s = default;
            bool fullsize = default;
            bool b = default;
            TJclSimpleXML XML = default;
            int n = default;
            TLabel lbItem = default;
            TImage32 imgItem = default;
            TBitmap32 Bmp = default;
            int i = default;
            TControl lastItem = default;
            TControl currentItem = default;
            string HelpDir = default;
            int nextMargin = default;
            fullsize = false;
            lastItem = null;
            LockWindowUpdate(Handle);
            try
            {
                if ((!pnlHelp.Visible) || (Update))
                {
                    if (Update)
                    {
                        ClearHelp();
                    }

                    pnlHelp.Visible = true;
                    pnlHelpToggle.Color = SCM.Theme.EditControlBackground;
                    pnlHelpToggle.BorderColor = SCM.Theme.Border;
                    if (length(trim(SCM.ActiveHelpFile)) > 0)
                    {
                        if (FileExists(SCM.ActiveHelpFile))
                        {
                            HelpDir = IncludeTrailingBackslash(ExtractFileDir(SCM.ActiveHelpFile));
                            XML = TJclSimpleXML.Create;
                            if (LoadXMLFromSharedFile(XML, SCM.ActiveHelpFile, true))
                            {
                                nextMargin = 0;
                                fullsize = (CompareText("full", XML.Root.Properties.Value("size", "fixed")) == 0);
                                for (n = 0; n <= XML.Root.Items.Count - 1; n++)
                                {
                                    {
                                        XML.Root.Items.currentItem = null;
                                        if ((CompareText(Item(n).Name, "Image") == 0))
                                        {
                                            s = SharpApi.GetCenterDirectory + "Images\\" + Item(n).Value;
                                            if (FileExists(s))
                                            {
                                                imgItem = TImage32.Create(pnlSbHelpContent);
                                                imgItem.AutoSize = true;
                                                imgItem.ParentColor = true;
                                                Bmp = TBitmap32.Create;
                                                try
                                                {
                                                    if ((CompareText(".png", ExtractFileExt(s)) == 0))
                                                    {
                                                        GR32_PNG.LoadBitmap32FromPNG(Bmp, s, b);
                                                    }
                                                    else
                                                    {
                                                        Bmp.LoadFromFile(s);
                                                    }

                                                    imgItem.Bitmap.SetSize(Bmp.Width, Bmp.Height);
                                                    imgItem.Bitmap.Clear(color32(pnlSbHelpContent.Color));
                                                    Bmp.DrawMode = dmBlend;
                                                    Bmp.CombineMode = cmMerge;
                                                    Bmp.DrawTo(imgItem.Bitmap, 0, 0);
                                                }
                                                finally
                                                {
                                                    Bmp.Free();
                                                }

                                                imgItem.Parent = pnlSbHelpContent;
                                                if ((CompareText("none", Item(n).Properties.Value("span", "none")) == 0))
                                                {
                                                    if ((lastItem != null))
                                                    {
                                                        imgItem.Top = lastItem.Top + lastItem.Height;
                                                    }
                                                    else
                                                    {
                                                        imgItem.Top = 0;
                                                    }

                                                    if ((CompareText("left", Item(n).Properties.Value("align", "left")) == 0))
                                                    {
                                                        imgItem.Left = 4 + nextMargin;
                                                    }

                                                    imgItem.Left = imgItem.Left + Item(n).Properties.IntValue("mleft", 0);
                                                    imgItem.Top = imgItem.Top + Item(n).Properties.IntValue("mtop", 0);
                                                    nextMargin = imgItem.Left + imgItem.Width;
                                                }
                                                else
                                                {
                                                    imgItem.Align = alTop;
                                                    if ((CompareText("left", Item(n).Properties.Value("align", "left")) == 0))
                                                    {
                                                        imgItem.BitmapAlign = baTopLeft;
                                                    }
                                                    else
                                                    {
                                                        imgItem.BitmapAlign = baCenter;
                                                    }

                                                    imgItem.Margins.Left = 4 + nextMargin;
                                                    imgItem.Margins.Right = 4;
                                                    imgItem.Margins.Top = 2;
                                                    imgItem.MArgins.Bottom = 2;
                                                    imgItem.AlignWithMargins = true;
                                                    nextMargin = 0;
                                                    currentItem = imgItem;
                                                }

                                                imgItem.Show();
                                            }
                                        }

                                        if ((CompareText(Item(n).Name, "Label") == 0))
                                        {
                                            lbItem = TLabel.Create(pnlSbHelpContent);
                                            lbItem.Parent = pnlSbHelpContent;
                                            lbItem.Align = alTop;
                                            lbItem.Margins.Left = 4 + nextMargin;
                                            lbItem.Margins.Right = 4;
                                            lbItem.Margins.Top = 2;
                                            lbItem.Margins.Bottom = 0;
                                            lbItem.AlignWithMargins = true;
                                            lbItem.AutoSize = true;
                                            lbItem.WordWrap = true;
                                            i = Item(n).Properties.IntValue("Indent", 0);
                                            lbItem.Margins.Left = lbItem.Margins.Left + i * 8;
                                            lbItem.Caption = Item(n).Value;
                                            if (Item(n).Properties.BoolValue("bold", false))
                                            {
                                                lbItem.Font.Style = lbItem.Font.Style + new[]
                                                {
                                                    fsBold
                                                };
                                            }

                                            if (Item(n).Properties.BoolValue("italic", false))
                                            {
                                                lbItem.Font.Style = lbItem.Font.Style + new[]
                                                {
                                                    fsItalic
                                                };
                                            }

                                            if (Item(n).Properties.BoolValue("underline", false))
                                            {
                                                lbItem.Font.Style = lbItem.Font.Style + new[]
                                                {
                                                    fsUnderline
                                                };
                                            }

                                            lbItem.Show();
                                            currentItem = lbItem;
                                            nextMargin = 0;
                                        }
                                        else
                                        {
                                            if ((CompareText(Item(n).Name, "Sep") == 0))
                                            {
                                                lbItem = TLabel.Create(pnlSbHelpContent);
                                                lbItem.Parent = pnlSbHelpContent;
                                                lbItem.Align = alTop;
                                                lbItem.Margins.Left = 4 + nextMargin;
                                                lbItem.Margins.Right = 4;
                                                lbItem.Margins.Top = 0;
                                                lbItem.Margins.Bottom = 0;
                                                lbItem.AlignWithMargins = true;
                                                lbItem.Caption = "";
                                                if (CompareText(Item(n).Properties.Value("Size", "Normal"), "Small") == 0)
                                                {
                                                    lbItem.Height = 4;
                                                }
                                                else
                                                {
                                                    if (CompareText(Item(n).Properties.Value("Size", "Normal"), "Big") == 0)
                                                    {
                                                        lbItem.Height = 16;
                                                    }
                                                    else
                                                    {
                                                        lbItem.Height = 10;
                                                    }
                                                }

                                                lbItem.Show();
                                                currentItem = lbItem;
                                                nextMargin = 0;
                                            }
                                        }

                                        if ((currentItem != null) && (lastItem != null) && (nextMargin == 0))
                                        {
                                            currentItem.Top = lastItem.Top + lastItem.Height;
                                        }

                                        if ((currentItem != null))
                                        {
                                            lastItem = currentItem;
                                        }
                                    }
                                }
                            }

                            XML.Free();
                        }
                    }

                    if (fullsize)
                    {
                        pnlSbHelpContent.Tag = 1;
                    }
                    else
                    {
                        pnlSbHelpContent.Tag = 0;
                    }

                    UpdateHelpHeight();
                    pnlSbHelpContent.Height = pnlSbHelpContent.Height + 1;
                    pnlSbHelpContent.Height = pnlSbHelpContent.Height - 1;
                }
                else
                {
                    pnlHelp.Visible = false;
                    pnlHelpToggle.Color = SCM.Theme.Background;
                    pnlHelpToggle.BorderColor = SCM.Theme.Background;
                    ClearHelp();
                }
            }
            finally
            {
                LockWindowUpdate(0);
            }
        }

        public void UpdateHelpHeight()
        {
            TControl lastitem = default;
            int h = default;
            if (pnlSbHelpContent.ComponentCount == 0)
            {
                exit();
            }

            lastitem = TControl(pnlSbHelpContent.Components(pnlSbHelpContent.ComponentCount - 1));
            if (pnlSbHelpContent.Tag == 1)
            {
                h = Height - pnlHelp.Top - pnlButtons.Height;
                if (lastItem != null)
                {
                    pnlHelp.Height = Min(h, lastItem.Top + lastItem.Height + pnlSbHelpContent.Top + pnlHelpContent.Top + 8);
                }
                else
                {
                    pnlHelp.Height = h;
                }
            }
            else
            {
                h = lastItem.Top + lastItem.Height + pnlSbHelpContent.Top + pnlHelpContent.Top + 8;
                if (h < 224)
                {
                    pnlHelp.Height = h;
                }
                else
                {
                    pnlHelp.Height = 224;
                }
            }
        }

        public bool GetCommandLineParams(TSCC_COMMAND_ENUM enumCommandType, string sSection, string sName, string sPluginID)
        {
            List<String> strlTokens = default;
            string sApiParam = default;
            int n = default;
            return false;
            n = Pos("-api", ParamStr(1));
            if (n != 0)
            {
                sApiParam = ParamStr(2);
                strlTokens = TStringlist.Create;
                try
                {
                    StrTokenToStrings(sApiParam, "|", strlTokens);
                    enumCommandType = sccLoadSetting;
                    if (strlTokens.Count > 0)
                    {
                        enumCommandType = TSCC_COMMAND_ENUM(StrToInt(strlTokens(0)));
                    }

                    sSection = "";
                    if (strlTokens.Count > 1)
                    {
                        sSection = strlTokens(1);
                    }

                    sName = "";
                    if (strlTokens.Count > 2)
                    {
                        sName = strlTokens(2);
                    }

                    sPluginID = "";
                    if (strlTokens.Count > 3)
                    {
                        sPluginID = strlTokens(3);
                    }

                    StrReplace(sPluginID, "{CurrentTheme}", GetCurrentTheme.Info.Name);
                }
                finally
                {
                    strlTokens.Free();
                    return true;
                }
            }
        }

        public void EnabledWM(TMessage Msg)
        {
            SendMessage(self.Handle, msg.Msg, msg.WParam, msg.LParam);
        }

        public int SelectedTabID { get; set; }

        public void SetToolbarTabVisible(TTabID ATabID, bool AVisible)
        {
            {
                switch (ATabID)
                {
                    case tidHome:
                        Item(0).Visible = AVisible;
                        break;
                    case tidFavourite:
                        Item(1).Visible = AVisible;
                        break;
                    case tidHistory:
                        tlToolbar.Buttons.Item(1).Visible = AVisible;
                        break;
                    case tidImport:
                        Item(3).Visible = AVisible;
                        break;
                    case tidExport:
                        Item(4).Visible = AVisible;
                        break;
                    default:
                        break;
                }
            }

            tlToolbar.TabIndex = 0;
            tlToolbar.Invalidate();
        }

        public void LoadPluginEvent(object Sender)
        {
            int i = default;
            LockWindowUpdate(Self.Handle);
            try
            {
                pnlToolbar.Hide();
                FSelectedTabID = 0;
                if ((SCM.Plugin.ConfigMode == SharpApi.scmLive))
                {
                    PnlButtons.Hide();
                }
                else
                {
                    if (SCM.PluginHost.Editing)
                    {
                        PnlButtons.Show();
                    }
                }

                if ((SCM.PluginHasEditSupport))
                {
                    pnlEditContainer.Minimized = true;
                    pnlEditContainer.Visible = true;
                    pnlEditPluginContainer.Visible = false;
                    pnlEditContainer.TabList.TabIndex = -1;
                }
                else
                {
                    pnlEditContainer.Visible = false;
                }

                for (i = 0; i <= Pred(lbTree.Count); i++)
                {
                    if (CompareText(TSharpCenterManagerItem(lbTree.Item(i).Data).Filename, SCM.Plugin.Dll) == 0)
                    {
                        lbTree.ItemIndex = i;
                        if (Sender != null)
                        {
                            TSharpCenterManagerItem(lbTree.Item(i).Data).Description = TSharpCenterManager(Sender).Plugin.Plugindata.Description;
                        }

                        break();
                    }
                }

                UpdateConfigHeader();
                if (pnlHelp.Visible)
                {
                    ToggleHelp(false);
                }

                pnlHelpToggle.Visible = false;
                if (length(trim(SCM.ActiveHelpFile)) > 0)
                {
                    if (FileExists(SCM.ActiveHelpFile))
                    {
                        pnlHelpToggle.Visible = true;
                    }
                }
            }
            finally
            {
                pnlPluginContainer.Show();
                pnlPluginContainer.Height = pnlPluginContainer.Height + 1;
                pnlPluginContainer.Height = pnlPluginContainer.Height - 1;
                SCM.PluginHost.Refresh(rtAll);
                UpdateSize();
                sbPlugin.SetFocus();
                SetToolbarTabVisible(tidHistory, SCM.History.Count > 1);
                LockWindowUpdate(0);
            }
        }

        public void UnloadPluginEvent(object Sender)
        {
            LockWindowUpdate(Self.Handle);
            try
            {
                if (((PnlButtons.Visible) && !(FCancelClicked)))
                {
                    SCM.Save();
                }

                if ((scm.PluginHasEditSupport))
                {
                    SCM.Plugin.EditInterface.CloseEdit(false);
                }
            }
            finally
            {
                pnlEditContainer.TabList.TabIndex = -1;
                pnlLivePreview.Height = 0;
                pnlEditContainer.Visible = false;
                pnlPluginContainer.Hide();
                pnlTitle.Hide();
                PnlButtons.Hide();
                LockWindowUpdate(0);
            }
        }

        public void AddItemEvent(TSharpCenterManagerItem AItem, int AIndex)
        {
            TSharpEListItem tmp = default;
            tmp = lbTree.AddItem(AItem.Caption, AItem.IconIndex);
            tmp.AddSubItem(AItem.Status);
            tmp.Data = AItem;
        }

        public void AddPluginTabsEvent(object Sender)
        {
            int i = default;
            string s = default;
            TTabItem tabItem = default;
            for (i = pnlPluginContainer.TabList.Count - 1; i >= 0; i--)
            {
                pnlPluginContainer.TabList.TabItem(i).Free();
            }

            pnlPluginContainer.TabList.Clear();
            if (SCM.PluginTabs.Count <= 1)
            {
                pnlPluginContainer.TabList.Hide();
                pnlPluginContainer.UpdateSize();
                sbPlugin.Margins.Top = 6;
                exit();
            }
            else
            {
                pnlPluginContainer.TabList.Show();
                pnlPluginContainer.UpdateSize();
                sbPlugin.Margins.Top = 32;
            }

            try
            {
                s = "";
                for (i = 0; i <= Pred(SCM.PluginTabs.Count); i++)
                {
                    tabItem = pnlPluginContainer.TabItems.Add;
                    tabItem.Caption = SCM.PluginTabs(i);
                }

                pnlPluginContainer.TabIndex = SCM.PluginTabIndex;
            }
            finally
            {
                sbPlugin.Invalidate();
            }
        }

        public void UpdateThemeEvent(object Sender)
        {
            TWinControl ctrl = default;
            LockWindowUpdate(Self.Handle);
            try
            {
                ctrl = null;
                if (SCM.EditWndHandle != 0)
                {
                    if (GetControlByHandle(SCM.EditWndHandle) != null)
                    {
                        ctrl = TForm(GetControlByHandle(SCM.EditWndHandle)).ActiveControl;
                    }
                }

                SCM.RefreshTheme();
                Scm.Plugin.PluginInterface.Refresh(SCM.Theme, SCM.PluginHost.Editing);
                lbTree.Enabled = !(SCM.PluginHost.Editing);
                UpdateEditButtons();
                if (ctrl != null)
                {
                    ctrl.SetFocus();
                }
            }
            finally
            {
                LockWindowUpdate(0);
            }
        }

        public void InitNavEvent(object Sender)
        {
            int i = default;
            for (i = lbTree.Items.Count - 1; i >= 0; i--)
            {
                TSharpCenterManagerItem(TSharpEListItem(lbTree.Items.Objects(i)).Data).Free();
                TSharpEListItem(lbTree.Items.Objects(i)).Free();
                lbTree.Items.Objects(i) = null;
            }

            lbTree.Clear();
        }

        public void LoadEditEvent(object Sender)
        {
            LockWindowUpdate(Self.Handle);
            try
            {
                UpdateSize();
                SCM.PluginHost.Refresh(rtAll);
            }
            finally
            {
                LockWindowUpdate(0);
            }
        }

        public void ApplyEditEvent(object Sender)
        {
            tlEditItemTabClick(pnlEditContainer.TabList, pnlEditContainer.TabList.TabIndex);
        }

        public void CancelEditEvent(Tobject Sender)
        {
            LockWindowUpdate(Self.Handle);
            try
            {
                if (Sender == null)
                {
                    pnlEditContainer.TabList.TabIndex = -1;
                    pnlEditContainer.Minimized = true;
                    pnlEditPluginContainer.Visible = false;
                }
                else
                {
                    pnlEditPluginContainer.Visible = true;
                    tlEditItemTabClick(pnlEditContainer.TabList, pnlEditContainer.TabList.TabIndex);
                }
            }
            finally
            {
                LockWindowUpdate(0);
            }
        }

        public void SavePluginEvent(object Sender)
        {
            PnlButtons.Hide();
        }

        public void RefreshThemeEvent(object Sender)
        {
            TColor colBackground = default;
            lbTree.Color = SCM.Theme.Background;
            if (SCM.PluginHost.Warning)
            {
                lbTree.Colors.ItemColor = SCM.Theme.NavBarItem;
                lbTree.Colors.ItemColorSelected = SCM.Theme.NavBarItem;
            }
            else
            {
                lbTree.Colors.ItemColor = SCM.Theme.NavBarItem;
                lbTree.Colors.ItemColorSelected = SCM.Theme.NavBarSelectedItem;
            }

            lbTree.Colors.DisabledColor = SCM.Theme.Background;
            lblDescription.Font.Color = SCM.Theme.BackgroundText;
            pnlSettingTree.Color = SCM.Theme.Background;
            pnlMain.Color = SCM.Theme.Background;
            pnlContent.Color = SCM.Theme.Background;
            Self.Color = SCM.Theme.Background;
            tlToolbar.BkgColor = SCM.Theme.Background;
            pnlTitle.Color = SCM.Theme.Background;
            if (SCM.PluginHost.Warning)
            {
                colBackground = SCM.Theme.EditBackgroundError;
            }
            else
            {
                colBackground = SCM.Theme.EditBackground;
            }

            pnlEditContainer.Color = colBackground;
            pnlEditContainer.PageBackgroundColor = colBackground;
            pnlEditContainer.BackgroundColor = colBackground;
            pnlEditContainer.TabBackgroundColor = SCM.Theme.Background;
            pnlEditContainer.BackgroundColor = colBackground;
            pnlEditContainer.TabColor = SCM.Theme.Background;
            pnlEditContainer.TabSelColor = colBackground;
            pnlEditContainer.TabCaptionColor = SCM.Theme.BackgroundText;
            pnlEditContainer.TabCaptionSelColor = SCM.Theme.EditBackgroundText;
            pnlEditContainer.BorderColor = SCM.Theme.Border;
            pnlEditToolbar.Color = colBackground;
            pnlEditPluginContainer.Color = colBackground;
            pnlEditPluginContainer.BackgroundColor = SCM.Theme.Background;
            pnlEditPlugin.color = colBackground;
            btnEditCancel.Font.Color = SCM.Theme.EditBackgroundText;
            btnEditApply.Font.Color = SCM.Theme.EditBackgroundText;
            sbPlugin.Color = SCM.Theme.PluginBackground;
            pnlPluginContainer.PageBackgroundColor = SCM.Theme.PluginBackground;
            pnlPluginContainer.BackgroundColor = SCM.Theme.Background;
            pnlPluginContainer.TabBackgroundColor = SCM.Theme.Background;
            pnlPluginContainer.TabColor = SCM.Theme.PluginTab;
            pnlPluginContainer.TabSelColor = SCM.Theme.PluginSelectedTab;
            pnlPluginContainer.TabCaptionColor = SCM.Theme.PluginTabText;
            pnlPluginContainer.TabCaptionSelColor = SCM.Theme.PluginTabSelectedText;
            pnlPluginContainer.BorderColor = SCM.Theme.Border;
            pnlPluginContainer.DoubleBuffered = true;
            pnlContent.DoubleBuffered = true;
            pnlMain.DoubleBuffered = true;
            Self.DoubleBuffered = true;
            PnlButtons.Color = SCM.Theme.Background;
            btnSave.Font.Color = SCM.Theme.BackgroundText;
            btnCancel.Font.Color = SCM.Theme.BackgroundText;
            pnlHelp.Color = SCM.Theme.EditControlBackground;
            pnlHelp.BorderColor = SCM.Theme.Border;
            pnlHelpContent.Color = SCM.Theme.EditControlBackground;
            if (pnlHelp.Visible)
            {
                pnlHelpToggle.Color = SCM.Theme.EditControlBackground;
                pnlHelpToggle.BorderColor = SCM.Theme.Border;
            }
            else
            {
                pnlHelpToggle.Color = SCM.Theme.Background;
                pnlHelpToggle.BorderColor = SCM.Theme.Background;
            }
        }

        public void SetHomeTitleEvent(string ADescription)
        {
            pnlTitle.Visible = true;
            lblDescription.Caption = ADescription;
        }

        public void RefreshSizeEvent(object Sender)
        {
            UpdateSize();
        }

        public void RefreshPreviewEvent(object Sender)
        {
            UpdateLivePreview();
        }

        public void RefreshPluginTabsEvent(object Sender)
        {
            SCM.LoadPluginTabs();
            lbTree.Refresh();
        }

        public void RefreshTitleEvent(object Sender)
        {
            TPluginData pluginData = default;
            TSharpCenterManagerItem tmpItem = default;
            if (lbTree.SelectedItem != null)
            {
                GetConfigPluginData(SCM.Plugin.Dllhandle, pluginData, SCM.Plugin.PluginInterface.PluginHost.PluginId);
                tmpItem = TSharpCenterManagerItem(lbTree.SelectedItem.Data);
                tmpItem.Status = pluginData.Status;
                lbTree.Refresh();
            }

            UpdateConfigHeader();
        }

        public void RefreshValidation(object sender)
        {
            if (scm.PluginHasValidationSupport)
            {
                SCM.RefreshValidation();
            }
        }

        public void RefreshAllEvent(object Sender)
        {
            RefreshPreviewEvent(null);
            RefreshSizeEvent(null);
            RefreshTitleEvent(null);
        }

        public void SaveEvent(object Sender)
        {
            SCM.Save();
        }

        public void AssignThemeToPluginFormEvent(TForm AForm, bool AEditing)
        {
            exit();
        }

        public void AssignThemeToEditFormEvent(TForm AForm, bool AEditing)
        {
            exit();
        }

        public void SetHostSettingsChangedEvent(object Sender)
        {
            PnlButtons.Show();
        }

        public void SetEditTabEvent(TSCB_BUTTON_ENUM ATab)
        {
            SCM.PluginHost.Editing = false;
            SCM.PluginHost.Warning = false;
            if (pnlEditContainer.Minimized)
            {
                FSelectedTabID = -1;
            }
            else
            {
                switch (ATab)
                {
                    case scbAddTab:
                        pnlEditContainer.TabIndex = cEdit_Add;
                        FSelectedTabID = cEdit_Add;
                        SCM.LoadEdit(FSelectedTabID);
                        break;
                    case scbEditTab:
                        pnlEditContainer.TabIndex = cEdit_Edit;
                        FSelectedTabID = cEdit_Edit;
                        SCM.LoadEdit(FSelectedTabID);
                        break;
                    default:
                        break;
                }
            }

            UpdateThemeEvent(null);
        }

        public void SetEditTabVisibilityEvent(TSCB_BUTTON_ENUM ATab, bool AVisible)
        {
            switch (ATab)
            {
                case scbAddTab:
                    pnlEditContainer.TabItems.Item(cEdit_Add).Visible = AVisible;
                    break;
                case scbEditTab:
                    pnlEditContainer.TabItems.Item(cEdit_Edit).Visible = AVisible;
                    break;
                default:
                    break;
            }
        }

        public void SetButtonVisibilityEvent(TSCB_BUTTON_ENUM ATab, bool AVisible)
        {
            switch (ATab)
            {
                case scbImport:
                    SetToolbarTabVisible(tidImport, AVisible);
                    break;
                case scbExport:
                    SetToolbarTabVisible(tidExport, AVisible);
                    break;
                case scbConfigure:
                    btnEditApply.Visible = AVisible;
                    btnEditApply.Caption = "Add";
                    btnEditApply.PngImage = pilIcons.PngImages.Items(10).PngImage;
                    FForceShow = true;
                    break;
                default:
                    break;
            }
        }

        public void SetEditingEvent(bool AValue)
        {
            UpdateEditButtons();
        }

        public void SetWarningEvent(bool AValue)
        {
            SCM.RefreshTheme();
        }
    }
}
