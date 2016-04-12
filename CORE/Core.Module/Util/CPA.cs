using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using DevExpress.Xpo;

public class CPA
{
    [NonPersistent]
    public class CPAResultItem
    {
        public string Calle;
        public int CodigoLocalidad;
        public string CPA;
        public int Tipo;
        public string Numero { get; set; }
    }

    [NonPersistent]
    public class LocalidadResultItem
    {
        public int Codigo;
        public int CodigoPostal;
        public string CodigoProvincia;
        public string Nombre;
    }

    public static IList<CPAResultItem> ObtenerCPA(int localidad, string calle, string altura)
    {
        return null;
        /*var client = new WebClient( );
    
        var response = client.UploadValues( 
                                            new NameValueCollection
                                            {
                                              { "localidad", localidad.ToString( ) },
                                              { "calle", calle },
                                              { "altura", altura }
                                            } );
        client.Dispose(  );
        var data = Encoding.Default.GetString( response );
        data = HtmlDecode( data );

        if( data.Contains( "cpa-resultado-error" ) )
          data = "";

        data = "<result>" + data + "</result>";

        var xml = new XmlDocument( );
        xml.LoadXml( data );

        var res = new List< CPAResultItem >( );
        foreach( XmlNode node in xml.DocumentElement.SelectNodes( "/result/div" ) )
        {
          var cpaRes = new CPAResultItem { CodigoLocalidad = localidad };
          res.Add( cpaRes );

          var parts = node.ChildNodes;
          cpaRes.Calle = "";
          cpaRes.CPA = "";
          cpaRes.Tipo = -1;

          if( parts.Count >= 2 )
          {
            cpaRes.Calle = parts[ 0 ].InnerText;
            cpaRes.CPA = parts[ 1 ].InnerText;
            cpaRes.Tipo = 1;

            var rx = new System.Text.RegularExpressions.Regex( @"^(.+)\s+(\d+)$" );
            var m = rx.Match( cpaRes.Calle );
            if( m.Success )
            {
              cpaRes.Calle = m.Groups[ 1 ].Value;
              cpaRes.Numero = m.Groups[ 2 ].Value;
            }
            if( cpaRes.Calle.StartsWith( "CALLE " ) )
              cpaRes.Calle = cpaRes.Calle.Substring( 6 );

            rx = new Regex( @"para la localidad\s*(.+)\s*es", RegexOptions.IgnoreCase );
            m = rx.Match( parts[ 0 ].InnerText );
            if( m.Success )
            {
              cpaRes.Calle = m.Groups[ 1 ].Value;
              cpaRes.Tipo = 2;
            }
          }
        }

        return res;*/
    }

    public static IList<LocalidadResultItem> ObtenerLocalidades(string codigoProvincia, string busqueda)
    {
        var client = new WebClient();
        client.Headers.Add("Referer", "http://www.correoargentino.com.ar/cpa");

        var response =
            client.UploadValues(
                "http://www.correoargentino.com.ar/consultas/cpa/obtener_localidades/" + codigoProvincia,
                new NameValueCollection
                {
                    {"localidad", busqueda}
                });

        client.Dispose();

        var data = Encoding.Default.GetString(response);
        data = HtmlDecode(data);

        data = "<result>" + data + "</result>";

        var xml = new XmlDocument();
        xml.LoadXml(data);

        var res = new List<LocalidadResultItem>();
        foreach (XmlNode node in xml.DocumentElement.SelectNodes("/result/select/option"))
        {
            var item = new LocalidadResultItem
            {
                CodigoProvincia = codigoProvincia,
                Codigo = -1,
                Nombre = "",
                CodigoPostal = -1
            };

            var m = Regex.Match(node.InnerText, @"(.+)\((\d+)\)");

            if (node.Attributes["value"] != null && m.Success)
            {
                item.Codigo = Convert.ToInt32(node.Attributes["value"].Value);
                item.Nombre = m.Groups[1].Value;
                item.CodigoPostal = Convert.ToInt32(m.Groups[2].Value);
            }
            res.Add(item);
        }
        return res;
    }

    public static string HtmlDecode(string data)
    {
        var ret = data;

        ret = ret.Replace("&nbsp;", " ");
        ret = ret.Replace("&aacute;", "á");
        ret = ret.Replace("&eacute;", "é");
        ret = ret.Replace("&iacute;", "í");
        ret = ret.Replace("&oacute;", "ó");
        ret = ret.Replace("&uacute;", "ú");
        ret = ret.Replace("&ntilde;", "ñ");
        ret = ret.Replace("&Aacute;", "Á");
        ret = ret.Replace("&Eacute;", "É");
        ret = ret.Replace("&Iacute;", "Í");
        ret = ret.Replace("&Oacute;", "Ó");
        ret = ret.Replace("&Uacute;", "Ú");
        ret = ret.Replace("&Ntilde;", "Ñ");
        ret = ret.Replace("&nbsp;", " ");

        return ret;
    }
}