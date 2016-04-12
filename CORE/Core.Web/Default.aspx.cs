using System;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Web.Templates;
using FDIT.Core.Seguridad;
using FDIT.Core.Sistema;
using FDIT.Core.Web;

public partial class Default : BaseXafPage, IRefreshable
{
    public override Control InnerContentPlaceHolder
    {
        get { return Content; }
    }

    public void Refresh()
    {
        OnCustomizeTemplateContent(this, new CustomizeTemplateContentEventArgs(TemplateContent));
    }

    protected override ContextActionsMenu CreateContextActionsMenu()
    {
        return new ContextActionsMenu(this, "Edit", "RecordEdit", "ObjectsCreation", "ListView", "Reports");
    }

    protected override void OnInit(EventArgs e)
    {
        CustomizeTemplateContent += OnCustomizeTemplateContent;

        base.OnInit(e);
    }

    private void OnCustomizeTemplateContent(object sender, CustomizeTemplateContentEventArgs e2)
    {
        /*if (!(e2.TemplateContent is DefaultVerticalTemplateContent1))
            return;

        try
        {
            var content = (DefaultVerticalTemplateContent1) e2.TemplateContent;
            var empresaActual = CoreAppLogonParameters.Instance.EmpresaActual();

            if (empresaActual.ImagenLogo != null)
            {
                using (var stream = new MemoryStream())
                {
                    empresaActual.ImagenLogo.Save(stream, ImageFormat.Png);
                    content.ASPxBinaryImage1.ContentBytes = stream.ToArray();
                }
            }

            content.SubheaderRow.Style[HtmlTextWriterStyle.BackgroundColor] = empresaActual.ColorFondo ?? "";
        }
        catch (Exception)
        {
        }*/
    }
}