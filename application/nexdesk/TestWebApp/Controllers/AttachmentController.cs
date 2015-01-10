using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestWebApp.Models;
using WebMatrix.WebData;

namespace TestWebApp.Controllers
{
    [Authorize]
    public class AttachmentController : Controller
    {
        BlobStorageService bss = new BlobStorageService();
        public ActionResult Index()
        {
            CloudBlobContainer blobContainer = bss.GetBlobContainer();
            List<string> blobs = new List<string>();
            foreach (var blobItem in blobContainer.ListBlobs())
            {
                blobs.Add(blobItem.Uri.ToString());
            }
            return View(blobs);
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase attachedFile)
        {
            if (attachedFile.ContentLength > 0)
            {
                CloudBlobContainer blobContainer = bss.GetBlobContainer();
                
                CloudBlockBlob blob = blobContainer.GetBlockBlobReference(attachedFile.FileName);
                blob.UploadFromStream(attachedFile.InputStream);
            }
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id,int ticketId)
        {
            try
            {
                HelpDeskEntities hDesk = new HelpDeskEntities();
                User current_user = hDesk.Users.SingleOrDefault(u => u.id == WebSecurity.CurrentUserId);
                Attachment attachment = hDesk.Attachments.SingleOrDefault(a => a.id == id); //find the attachment from url
                if (attachment.uploader_id != current_user.id && current_user.webpages_Roles.SingleOrDefault(r => r.RoleName == "SuperUser") == null)
                {
                    return RedirectToAction("Details/" + ticketId, "Ticket", new { msg = "You dont have permission to delete this file" });
                }
                else
                {
                    //Delete old picture from blob container
                    CloudBlobContainer blobContainer = bss.GetBlobContainer();//get the container
                    string fileName = Path.GetFileName(attachment.url);
                    CloudBlockBlob blob = blobContainer.GetBlockBlobReference(fileName); // get the blob from given fileName
                    hDesk.Attachments.Remove(attachment);
                    hDesk.SaveChanges();
                    blob.Delete(); //delete from storage   
                    return RedirectToAction("Details/" + ticketId, "Ticket", new { msg = "You have successfully deleted the file" });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Details/" + ticketId, "Ticket", new { msg = ex.Message});

            }

        }

    }
}