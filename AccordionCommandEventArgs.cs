// Decompiled with JetBrains decompiler
// Type: AjaxControlToolkit.AccordionCommandEventArgs
// Assembly: AjaxControlToolkit, Version=3.0.30930.28736, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e
// MVID: B0EEFC76-0092-471B-AB62-F3DDC8240D71
// Assembly location: C:\TFS\UFD\Development\DevNet4.8\PCI\source\Pegasus.NET\Lib\AjaxControlToolkit.dll

using System.Web.UI.WebControls;

namespace AjaxControlToolkit
{
  public class AccordionCommandEventArgs : CommandEventArgs
  {
    private AccordionContentPanel _container;

    internal AccordionCommandEventArgs(
      AccordionContentPanel container,
      string commandName,
      object commandArg)
      : base(commandName, commandArg)
    {
      this._container = container;
    }

    public AccordionContentPanel Container => this._container;
  }
}
