// Decompiled with JetBrains decompiler
// Type: AjaxControlToolkit.Accordion
// Assembly: AjaxControlToolkit, Version=3.0.30930.28736, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e
// MVID: B0EEFC76-0092-471B-AB62-F3DDC8240D71
// Assembly location: C:\TFS\UFD\Development\DevNet4.8\PCI\source\Pegasus.NET\Lib\AjaxControlToolkit.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AjaxControlToolkit
{
  [PersistChildren(true)]
  [Designer("AjaxControlToolkit.AccordionDesigner, AjaxControlToolkit")]
  [ParseChildren(true)]
  [ToolboxData("<{0}:Accordion runat=server></{0}:Accordion>")]
  [ToolboxBitmap(typeof (Accordion), "Accordion.Accordion.ico")]
  public class Accordion : WebControl
  {
    internal const string ItemCountViewStateKey = "_!ItemCount";
    private AccordionExtender _extender;
    private AccordionPaneCollection _panes;
    private object _dataSource;
    private ITemplate _headerTemplate;
    private ITemplate _contentTemplate;
    private bool _initialized;
    private bool _pagePreLoadFired;
    private bool _requiresDataBinding;
    private bool _throwOnDataPropertyChange;
    private DataSourceView _currentView;
    private bool _currentViewIsFromDataSourceID;
    private bool _currentViewValid;
    private DataSourceSelectArguments _arguments;
    private IEnumerable _selectResult;
    private EventWaitHandle _selectWait;

    public event EventHandler<AccordionItemEventArgs> ItemCreated;

    public event EventHandler<AccordionItemEventArgs> ItemDataBound;

    public event CommandEventHandler ItemCommand;

    public Accordion()
      : base(HtmlTextWriterTag.Div)
    {
    }

    private AccordionExtender AccordionExtender
    {
      get
      {
        if (this._extender == null)
        {
          this._extender = new AccordionExtender();
          this._extender.ID = this.ID + "_AccordionExtender";
          this._extender.TargetControlID = this.ID;
          this.Controls.AddAt(0, (Control) this._extender);
        }
        return this._extender;
      }
    }

    [DefaultValue(500)]
    [Browsable(true)]
    [Category("Behavior")]
    [Description("Length of the transition animation in milliseconds")]
    public int TransitionDuration
    {
      get => this.AccordionExtender.TransitionDuration;
      set => this.AccordionExtender.TransitionDuration = value;
    }

    [Browsable(true)]
    [Description("Number of frames per second used in the transition animation")]
    [DefaultValue(15)]
    [Category("Behavior")]
    public int FramesPerSecond
    {
      get => this.AccordionExtender.FramesPerSecond;
      set => this.AccordionExtender.FramesPerSecond = value;
    }

    [DefaultValue(false)]
    [Browsable(true)]
    [Category("Behavior")]
    [Description("Whether or not to use a fade effect in the transition animations")]
    public bool FadeTransitions
    {
      get => this.AccordionExtender.FadeTransitions;
      set => this.AccordionExtender.FadeTransitions = value;
    }

    [Description("Default CSS class for Accordion Pane Headers")]
    [Category("Appearance")]
    [Browsable(true)]
    public string HeaderCssClass
    {
      get => this.AccordionExtender.HeaderCssClass;
      set => this.AccordionExtender.HeaderCssClass = value;
    }

    [Category("Appearance")]
    [Browsable(true)]
    [Description("Default CSS class for the selected Accordion Pane Headers")]
    public string HeaderSelectedCssClass
    {
      get => this.AccordionExtender.HeaderSelectedCssClass;
      set => this.AccordionExtender.HeaderSelectedCssClass = value;
    }

    [Browsable(true)]
    [Category("Appearance")]
    [Description("Default CSS class for Accordion Pane Content")]
    public string ContentCssClass
    {
      get => this.AccordionExtender.ContentCssClass;
      set => this.AccordionExtender.ContentCssClass = value;
    }

    [Browsable(true)]
    [DefaultValue(AutoSize.None)]
    [Category("Behavior")]
    [Description("Determine how the growth of the Accordion will be controlled")]
    public AutoSize AutoSize
    {
      get => this.AccordionExtender.AutoSize;
      set => this.AccordionExtender.AutoSize = value;
    }

    [DefaultValue(0)]
    [Browsable(true)]
    [Category("Behavior")]
    [Description("Index of the AccordionPane to be displayed")]
    public int SelectedIndex
    {
      get => this.AccordionExtender.SelectedIndex;
      set => this.AccordionExtender.SelectedIndex = value;
    }

    [Category("Behavior")]
    [Browsable(true)]
    [DefaultValue(true)]
    [Description("Whether or not clicking the header will close the currently opened pane (leaving all the Accordion's panes closed)")]
    public bool RequireOpenedPane
    {
      get => this.AccordionExtender.RequireOpenedPane;
      set => this.AccordionExtender.RequireOpenedPane = value;
    }

    [Browsable(true)]
    [DefaultValue(false)]
    [Category("Behavior")]
    [Description("Whether or not we suppress the client-side click handlers of any elements in the header sections")]
    public bool SuppressHeaderPostbacks
    {
      get => this.AccordionExtender.SuppressHeaderPostbacks;
      set => this.AccordionExtender.SuppressHeaderPostbacks = value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public AccordionPaneCollection Panes
    {
      get
      {
        if (this._panes == null)
          this._panes = new AccordionPaneCollection(this);
        return this._panes;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override ControlCollection Controls => base.Controls;

    [DefaultValue(null)]
    [Browsable(false)]
    [TemplateContainer(typeof (AccordionContentPanel))]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public virtual ITemplate HeaderTemplate
    {
      get => this._headerTemplate;
      set => this._headerTemplate = value;
    }

    [Browsable(false)]
    [TemplateContainer(typeof (AccordionContentPanel))]
    [DefaultValue(null)]
    [PersistenceMode(PersistenceMode.InnerProperty)]
    public virtual ITemplate ContentTemplate
    {
      get => this._contentTemplate;
      set => this._contentTemplate = value;
    }

    [Bindable(true)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Category("Data")]
    [DefaultValue(null)]
    public virtual object DataSource
    {
      get => this._dataSource;
      set
      {
        switch (value)
        {
          case null:
          case IListSource _:
          case IEnumerable _:
            this._dataSource = value;
            this.OnDataPropertyChanged();
            break;
          default:
            throw new ArgumentException("Can't bind to value that is not an IListSource or an IEnumerable.");
        }
      }
    }

    [Category("Data")]
    [DefaultValue("")]
    [IDReferenceProperty(typeof (DataSourceControl))]
    public virtual string DataSourceID
    {
      get => this.ViewState[nameof (DataSourceID)] is string str ? str : string.Empty;
      set
      {
        this.ViewState[nameof (DataSourceID)] = (object) value;
        this.OnDataPropertyChanged();
      }
    }

    [Category("Data")]
    [DefaultValue("")]
    public virtual string DataMember
    {
      get => this.ViewState[nameof (DataMember)] is string str ? str : string.Empty;
      set
      {
        this.ViewState[nameof (DataMember)] = (object) value;
        this.OnDataPropertyChanged();
      }
    }

    protected bool IsBoundUsingDataSourceID => !string.IsNullOrEmpty(this.DataSourceID);

    protected bool RequiresDataBinding
    {
      get => this._requiresDataBinding;
      set => this._requiresDataBinding = value;
    }

    protected DataSourceSelectArguments SelectArguments
    {
      get
      {
        if (this._arguments == null)
          this._arguments = this.CreateDataSourceSelectArguments();
        return this._arguments;
      }
    }

    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);
      if (this.Page == null)
        return;
      this.Page.PreLoad += new EventHandler(this.OnPagePreLoad);
      if (this.IsViewStateEnabled || !this.Page.IsPostBack)
        return;
      this.RequiresDataBinding = true;
    }

    private void OnPagePreLoad(object sender, EventArgs e)
    {
      this._initialized = true;
      if (this.Page != null)
      {
        this.Page.PreLoad -= new EventHandler(this.OnPagePreLoad);
        if (!this.Page.IsPostBack)
          this.RequiresDataBinding = true;
        if (this.Page.IsPostBack && this.IsViewStateEnabled && this.ViewState["_!ItemCount"] == null)
          this.RequiresDataBinding = true;
        this._pagePreLoadFired = true;
      }
      this.EnsureChildControls();
    }

    protected override void OnLoad(EventArgs e)
    {
      this._initialized = true;
      this.ConnectToDataSourceView();
      if (this.Page != null && !this._pagePreLoadFired && this.ViewState["_!ItemCount"] == null)
      {
        if (!this.Page.IsPostBack)
          this.RequiresDataBinding = true;
        else if (this.IsViewStateEnabled)
          this.RequiresDataBinding = true;
      }
      base.OnLoad(e);
    }

    protected override void CreateChildControls()
    {
      base.CreateChildControls();
      if (this.AccordionExtender != null && this.ViewState["_!ItemCount"] != null)
        this.CreateControlHierarchy(false);
      this.ClearChildViewState();
      foreach (Control pane in this.Panes)
      {
        ControlCollection controls = pane.Controls;
      }
    }

    protected override void OnPreRender(EventArgs e)
    {
      this.EnsureDataBound();
      base.OnPreRender(e);
      if (this.AutoSize != AutoSize.None)
      {
        this.Style[HtmlTextWriterStyle.Overflow] = "hidden";
        this.Style[HtmlTextWriterStyle.OverflowX] = "auto";
      }
      foreach (AccordionPane pane in this.Panes)
      {
        if (pane.HeaderCssClass == this.HeaderSelectedCssClass)
          pane.HeaderCssClass = string.Empty;
        if (!string.IsNullOrEmpty(this.HeaderCssClass) && string.IsNullOrEmpty(pane.HeaderCssClass))
          pane.HeaderCssClass = this.HeaderCssClass;
        if (!string.IsNullOrEmpty(this.ContentCssClass) && string.IsNullOrEmpty(pane.ContentCssClass))
          pane.ContentCssClass = this.ContentCssClass;
      }
      int selectedIndex = this.AccordionExtender.SelectedIndex;
      int index = selectedIndex >= 0 && selectedIndex < this.Panes.Count || !this.AccordionExtender.RequireOpenedPane ? selectedIndex : 0;
      if (index < 0 || index >= this.Panes.Count)
        return;
      AccordionContentPanel contentContainer = this.Panes[index].ContentContainer;
      if (contentContainer != null)
        contentContainer.Collapsed = false;
      if (string.IsNullOrEmpty(this.HeaderSelectedCssClass))
        return;
      this.Panes[index].HeaderCssClass = this.HeaderSelectedCssClass;
    }

    public override Control FindControl(string id)
    {
      Control control = base.FindControl(id);
      if (control == null)
      {
        foreach (Control pane in this.Panes)
        {
          control = pane.FindControl(id);
          if (control != null)
            break;
        }
      }
      return control;
    }

    internal void ClearPanes()
    {
      for (int index = this.Controls.Count - 1; index >= 0; --index)
      {
        if (this.Controls[index] is AccordionPane)
          this.Controls.RemoveAt(index);
      }
    }

    private DataSourceView ConnectToDataSourceView()
    {
      if (this._currentViewValid && !this.DesignMode)
        return this._currentView;
      if (this._currentView != null && this._currentViewIsFromDataSourceID)
        this._currentView.DataSourceViewChanged -= new EventHandler(this.OnDataSourceViewChanged);
      dataSource = (IDataSource) null;
      string dataSourceId = this.DataSourceID;
      if (!string.IsNullOrEmpty(dataSourceId))
      {
        Control control = this.NamingContainer.FindControl(dataSourceId);
        if (control == null)
          throw new HttpException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "DataSource '{1}' for control '{0}' doesn't exist", (object) this.ID, (object) dataSourceId));
        if (!(control is IDataSource dataSource))
          throw new HttpException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "'{1}' is not a data source for control '{0}'.", (object) this.ID, (object) dataSourceId));
      }
      if (dataSource == null)
        return (DataSourceView) null;
      if (this.DataSource != null)
        throw new InvalidOperationException("DataSourceID and DataSource can't be set at the same time.");
      DataSourceView view = dataSource.GetView(this.DataMember);
      if (view == null)
        throw new InvalidOperationException(string.Format((IFormatProvider) CultureInfo.CurrentCulture, "DataSourceView not found for control '{0}'", (object) this.ID));
      this._currentViewIsFromDataSourceID = this.IsBoundUsingDataSourceID;
      this._currentView = view;
      if (this._currentView != null && this._currentViewIsFromDataSourceID)
        this._currentView.DataSourceViewChanged += new EventHandler(this.OnDataSourceViewChanged);
      this._currentViewValid = true;
      return this._currentView;
    }

    public override void DataBind()
    {
      if (this.IsBoundUsingDataSourceID && this.DesignMode && this.Site == null)
        return;
      this.RequiresDataBinding = false;
      this.OnDataBinding(EventArgs.Empty);
    }

    protected override void OnDataBinding(EventArgs e)
    {
      base.OnDataBinding(e);
      if (this.DataSource == null && !this.IsBoundUsingDataSourceID)
        return;
      this.ClearPanes();
      this.ClearChildViewState();
      this.CreateControlHierarchy(true);
      this.ChildControlsCreated = true;
    }

    protected virtual void CreateControlHierarchy(bool useDataSource)
    {
      int capacity = -1;
      IEnumerable enumerable = (IEnumerable) null;
      List<AccordionPane> accordionPaneList = new List<AccordionPane>();
      if (!useDataSource)
      {
        object obj = this.ViewState["_!ItemCount"];
        if (obj != null)
        {
          capacity = (int) obj;
          if (capacity != -1)
          {
            List<object> objectList = new List<object>(capacity);
            for (int index = 0; index < capacity; ++index)
              objectList.Add((object) null);
            enumerable = (IEnumerable) objectList;
            accordionPaneList.Capacity = capacity;
          }
        }
      }
      else
      {
        enumerable = this.GetData();
        capacity = 0;
        if (enumerable is ICollection collection)
          accordionPaneList.Capacity = collection.Count;
      }
      if (enumerable != null)
      {
        int index = 0;
        foreach (object dataItem in enumerable)
        {
          AccordionPane child = new AccordionPane();
          child.ID = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}_Pane_{1}", (object) this.ID, (object) index.ToString((IFormatProvider) CultureInfo.InvariantCulture));
          this.Controls.Add((Control) child);
          this.CreateItem(dataItem, index, AccordionItemType.Header, child.HeaderContainer, this.HeaderTemplate, useDataSource);
          this.CreateItem(dataItem, index, AccordionItemType.Content, child.ContentContainer, this.ContentTemplate, useDataSource);
          accordionPaneList.Add(child);
          ++capacity;
          ++index;
        }
      }
      if (!useDataSource)
        return;
      this.ViewState["_!ItemCount"] = (object) (enumerable != null ? capacity : -1);
    }

    private void CreateItem(
      object dataItem,
      int index,
      AccordionItemType itemType,
      AccordionContentPanel container,
      ITemplate template,
      bool dataBind)
    {
      if (template == null)
        return;
      AccordionItemEventArgs args = new AccordionItemEventArgs(container, itemType);
      this.OnItemCreated(args);
      container.SetDataItemProperties(dataItem, index, itemType);
      template.InstantiateIn((Control) container);
      if (!dataBind)
        return;
      container.DataBind();
      this.OnItemDataBound(args);
    }

    protected void EnsureDataBound()
    {
      try
      {
        this._throwOnDataPropertyChange = true;
        if (!this.RequiresDataBinding || string.IsNullOrEmpty(this.DataSourceID))
          return;
        this.DataBind();
      }
      finally
      {
        this._throwOnDataPropertyChange = false;
      }
    }

    protected virtual IEnumerable GetData()
    {
      this._selectResult = (IEnumerable) null;
      DataSourceView dataSourceView = this.ConnectToDataSourceView();
      if (dataSourceView != null)
      {
        this._selectWait = new EventWaitHandle(false, EventResetMode.AutoReset);
        dataSourceView.Select(this.SelectArguments, new DataSourceViewSelectCallback(this.DoSelect));
        this._selectWait.WaitOne();
      }
      else if (this.DataSource != null)
        this._selectResult = this.DataSource as IEnumerable;
      return this._selectResult;
    }

    protected virtual DataSourceSelectArguments CreateDataSourceSelectArguments() => DataSourceSelectArguments.Empty;

    private void DoSelect(IEnumerable data)
    {
      this._selectResult = data;
      this._selectWait.Set();
    }

    protected override bool OnBubbleEvent(object source, EventArgs args)
    {
      bool flag = false;
      if (args is AccordionCommandEventArgs args1)
      {
        this.OnItemCommand(args1);
        flag = true;
      }
      return flag;
    }

    protected virtual void OnDataPropertyChanged()
    {
      if (this._throwOnDataPropertyChange)
        throw new HttpException("Invalid data property change");
      if (this._initialized)
        this.RequiresDataBinding = true;
      this._currentViewValid = false;
    }

    protected virtual void OnDataSourceViewChanged(object sender, EventArgs args) => this.RequiresDataBinding = true;

    protected virtual void OnItemCommand(AccordionCommandEventArgs args)
    {
      if (this.ItemCommand == null)
        return;
      this.ItemCommand((object) this, (CommandEventArgs) args);
    }

    protected virtual void OnItemCreated(AccordionItemEventArgs args)
    {
      if (this.ItemCreated == null)
        return;
      this.ItemCreated((object) this, args);
    }

    protected virtual void OnItemDataBound(AccordionItemEventArgs args)
    {
      if (this.ItemDataBound == null)
        return;
      this.ItemDataBound((object) this, args);
    }
  }
}
