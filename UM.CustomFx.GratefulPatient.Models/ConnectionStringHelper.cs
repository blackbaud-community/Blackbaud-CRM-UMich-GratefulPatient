using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace UM.CustomFx.GratefulPatient.Models
{
    public class ConnectionStringHelper
    {
        const string APPNAME = "EntityFramework";
        const string PROVIDER = "System.Data.SqlClient";

        public ConnectionStringHelper(){}
        
        /// <summary>
        /// Use the default BBInfinity Connection string
        /// </summary>
        /// <param name="isEF"></param>
        /// <returns></returns>
        public static string GetConnectionString(bool isEF = false)
        {
            //build the base string based on default CRM connection string
            //gets the current url
            string currentUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            NameValueCollection qsColl = HttpUtility.ParseQueryString(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);

            builder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings[qsColl["databaseName"]].ConnectionString);
           
            builder.MultipleActiveResultSets = true;
            builder.ApplicationName = APPNAME;

            if (isEF)//use this to build metadata
            {
                EntityConnectionStringBuilder efBuilder = new EntityConnectionStringBuilder();
                efBuilder.Provider = PROVIDER;
                efBuilder.Metadata = "res://*/CRMData.csdl|res://*/CRMData.ssdl|res://*/CRMData.msl";
                efBuilder.ProviderConnectionString = builder.ConnectionString;

                return efBuilder.ToString();
            }
            else
            {
                return builder.ToString();
            }
        }

        /// <summary>
        /// Assumes use of the EF and builds connection string based on query string
        /// </summary>
        /// <param name="entityModel">Name of the entity model to connect to</param>
        /// <returns>EF Connections string for DB connection</returns>
        public static string GetConnectionString(string entityModel)
        {
            //build the base string based on default CRM connection string
            //gets the current url
            string currentUrl = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            NameValueCollection qsColl = HttpUtility.ParseQueryString(System.Web.HttpContext.Current.Request.Url.AbsoluteUri);

            builder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings[qsColl["databaseName"]].ConnectionString);

            builder.MultipleActiveResultSets = true;
            builder.ApplicationName = APPNAME;

            EntityConnectionStringBuilder efBuilder = new EntityConnectionStringBuilder();
            efBuilder.Provider = PROVIDER;
            efBuilder.Metadata = "res://*/" + entityModel + ".csdl|res://*/" + entityModel + ".ssdl|res://*/" + entityModel + ".msl";
            efBuilder.ProviderConnectionString = builder.ConnectionString;

            return efBuilder.ToString();
        }
    }
}
