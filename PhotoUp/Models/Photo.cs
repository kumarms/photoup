using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using Dapper;

namespace PhotoUp.Models
{
    public class Photo
    {
        public string User { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime Created { get; set; }

        public static void Create(Photo photo)
        {
            using (var db = Db())
            {
                db.Execute(@"
                    INSERT INTO [Photos]
                        ([user],   description,  photourl, created)
                    values
                        (@user, @description, @photourl, GETUTCDATE())
                ", photo);
            }
        }

        public static IEnumerable<Photo> Load()
        {
            using (var db = Db())
            {
                return db.Query<Photo>(@"
                    SELECT * FROM [Photos]
                    ORDER BY [Created] DESC
                ");
            }
        }

        private static DbConnection Db()
        {
            var setting = System.Configuration.ConfigurationManager.ConnectionStrings["photoup"];
            var factory = DbProviderFactories.GetFactory(setting.ProviderName);

            var conn = factory.CreateConnection();
            conn.ConnectionString = setting.ConnectionString;
            conn.Open();

            return conn;
        }
    }
}