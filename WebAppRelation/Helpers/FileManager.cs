﻿
namespace BB205_Pronia.Helpers
{
    public static class FileManager
    {
        public static bool CheckLong(this IFormFile file, int length)
        {
            return file.Length < length;
        }
        public static bool CheckType(this IFormFile file, string type)
        {
            return file.ContentType.Contains(type);
        }
        public static string Upload(this IFormFile file, string envPath, string folderName)
        {
            if (!Directory.Exists(envPath + folderName))
            {
                Directory.CreateDirectory(envPath + folderName);
            }

            string fileName = file.FileName;

            if (fileName.Length > 64)
            {
                fileName = fileName.Substring(fileName.Length - 64);
            }

            fileName = Guid.NewGuid().ToString() + fileName;

            string path = envPath + folderName + fileName;
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return fileName;
        }

        public static void Delete(this string ImgUrl, string envPath, string folderName)
        {
            string path = envPath + folderName + ImgUrl;

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}