using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
namespace WebInfo
{
	public class CacheManager : IHttpHandler
	{

		public bool IsReusable
		{
            get { return false; }
		}

		public void ProcessRequest(HttpContext context)
		{
            IDictionaryEnumerator en = context.Cache.GetEnumerator();
			var table = new StringBuilder();

            string chaveCache = context.Request.QueryString["cachekey"];
            if (chaveCache != null) 
            {
                context.Cache.Remove(chaveCache);
            }
            
            // Total de Bytes
            long sizeTotal = 0;           

			// Cria Headers da table
            table.Append("<p>");
            table.Append("<strong>Total de bytes:</strong> {totalCacheBytes}");
            table.Append("</p>");
			table.Append("<table>");
				table.Append("<thead>");
					table.Append("<tr>");
						table.Append("<th>");
						table.Append("<strong>Chave do Cache</strong>");
						table.Append("</th>");
						table.Append("<th>");
						table.Append("<strong>Tamanho (Em Bytes)</strong>");
						table.Append("</th>");
                        table.Append("<th>");
                        table.Append("Ações");
                        table.Append("</th>");
					table.Append("</tr>");
				table.Append("</thead>");
                table.Append("<tbody>");
			
				
				while (en.MoveNext())
				{
					long size = 0;
                    using (Stream s = new MemoryStream()) {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(s, en.Value);
                        size = s.Length;
                        sizeTotal += s.Length;
                    }

                    table.Append("<tr>");
                    table.Append("<td>");
                    table.Append(en.Key.ToString());
                    table.Append("</td>");
                    table.Append("<td>");
                    table.Append(size.ToString());
                    table.Append("</td>");
                    table.Append("<td>");
                    table.Append(String.Format("<a href='/CacheManager.axd?cachekey={0}'>Excluir</a>", en.Key.ToString()));
                    table.Append("</td>");
                    table.Append("</tr>");
				}

                table.Append("</tbody>");
                table.Append("</table>");
                


			context.Response.Write(table.ToString().Replace("{totalCacheBytes}", sizeTotal.ToString()));
		}

	}
    
}
