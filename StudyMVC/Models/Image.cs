using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace StudyMVC.Models
{
    public class Image
    {
        [Key]
        public int ImageID { get; set; }
        [Required]
        public int PersonID { get; set; }

        public byte[] ImageData { get; set; }
        
        public string ImageMimeType { get; set; }
    }
    public class ImageContext : DbContext
    {
        public ImageContext() : base() { }
        public DbSet<Image> ImageDB { get; set; }
    }

    public static class MoreExtensionMethods
    {
        public static IEnumerable<Image> FindByPersonID(
            this IEnumerable<Image> images, int pid)
        {
            return (from i in images where i.PersonID == pid select i);
        }
    public static void DeleteImageByPerson(
        this ImageContext images, int pid){
        foreach (Image i in images.ImageDB){
                if (i.PersonID == pid) images.ImageDB.Remove(i);
            }
            images.SaveChanges();
        }
    }
    }