using Amazon.S3;
using AttributeRouting.Web.Mvc;
using PhotoUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoUp.Controllers
{
    public class PhotoController : Controller
    {
        [GET("/")]
        public ActionResult Index()
        {
            return View(model: Photo.Load());
        }

        [GET("/photo")]
        public ActionResult New()
        {
            return View();
        }

        [POST("/photo")]
        public ActionResult Create(Photo photo)
        {
            var photoFile = Request.Files["photo-file"];

            photo.User = Session["user"] as string;
            photo.PhotoUrl = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(photoFile.FileName);

            using (var s3 = new AmazonS3Client())
            {
                s3.PutObject(new Amazon.S3.Model.PutObjectRequest
                {
                    CannedACL = Amazon.S3.Model.S3CannedACL.PublicRead,
                    BucketName = "betamore-photoup",
                    InputStream = photoFile.InputStream,
                    Key = photo.PhotoUrl
                });
            }

            Photo.Create(photo);

            return RedirectToAction("Index");
        }
    }
}
