using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace frontlook_csharp_library.FL_General
{
    public static class FL_Media
    {
        public static bool FL_CheckExtension(string extension, IFormFile file = null)
        {
            var b = file != null && file.Length > 0 && Path.GetExtension(file.FileName) == "." + extension;
            return b;
        }

        /// <summary>
        /// Checks if path is null
        /// </summary>
        /// <param name="webRootPath">Server root path</param>
        /// <param name="storePath">Path for designated media file</param>
        /// <returns>string: Path(Can Be null)</returns>
        public static string FL_GetPathCombination([CanBeNull] string webRootPath = null, [CanBeNull] string storePath = null)
        {
            if (!string.IsNullOrEmpty(storePath) && string.IsNullOrEmpty(webRootPath))
            {
                return storePath;
            }
            else if (string.IsNullOrEmpty(storePath) && !string.IsNullOrEmpty(webRootPath))
            {
                return webRootPath;
            }
            else
                return Path.Combine(webRootPath ?? string.Empty, storePath ?? string.Empty);
        }

        /// <summary>
        /// Gets File Path
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="webRootPath">Server root path</param>
        /// <param name="storePath">Path for designated media file</param>
        /// <returns>string: Path</returns>
        public static string FL_FilePath(string FileName, [CanBeNull] string webRootPath = null, [CanBeNull] string storePath = null)
        {
            var pathCombo = FL_GetPathCombination(webRootPath, storePath);
            var st = !string.IsNullOrEmpty(pathCombo)
                ? Path.Combine(pathCombo, FileName)
                : FileName;
            return st;
        }

        /// <summary>
        /// Gets File Path
        /// </summary>
        /// <param name="Extension"></param>
        /// <param name="webRootPath">Server root path</param>
        /// <param name="storePath">Path for designated media file</param>
        /// <param name="FileName"></param>
        /// <returns>string: Path</returns>
        public static string FL_FilePathWithExtension(string FileName, string Extension = null, string webRootPath = null, string storePath = null)
        {
            var st = Extension != null
                ? FL_FilePath(FileName, webRootPath, storePath) + "." + Extension
                : FL_FilePath(FileName, webRootPath, storePath);
            return st;
        }

        /// <summary>
        /// Saves Media in a specified folder
        /// </summary>
        /// <param name="SetFile">Media File</param>
        /// <param name="webRootPath">Main Server Path</param>
        /// <param name="storePath">Path for designated media file</param>
        /// <param name="FileName">Name of the file after being stored. Can be null.</param>
        /// <param name="Extension">Extension of the file</param>
        /// <returns>string</returns>
        public static async Task<string> FL_CopyMediaFileToFolderAsync(this IFormFile SetFile, [CanBeNull] string Extension = null, [CanBeNull] string FileName = null,
            [CanBeNull] string storePath = null, [CanBeNull] string webRootPath = null)
        {
            var PathDB = string.Empty;
            if (SetFile != null && SetFile.Length > 0)
            {
                //Getting FileName
                //var fileName = ContentDispositionHeaderValue.Parse(EmployeePhoto.ContentDisposition).FileName.Trim('"');

                //Assigning Unique Filename (Guid)
                var myUniqueFileName = FileName ?? Guid.NewGuid().ToString();

                //Getting file Extension
                var FileExtension = Extension != null ? "." + Extension : Path.GetExtension(SetFile.FileName.Trim('"'));

                // concatenating  FileName + FileExtension
                var newFileName = myUniqueFileName + FileExtension;

                var FinalStorePath = FL_GetPathCombination(webRootPath, storePath);

                if (!Directory.Exists(FinalStorePath))
                {
                    Directory.CreateDirectory(FinalStorePath);
                }

                // Combines two strings into a path.
                //FileName = FinalStorePath + $@"\{newFileName}";
                FileName = Path.Combine(FinalStorePath, newFileName);

                // if you want to store path of folder in database
                PathDB = storePath != null ? Path.Combine(storePath, newFileName) : newFileName;

                await FL_CopyFileAsync(SetFile, FileName);
            }
            return string.IsNullOrEmpty(PathDB) ? null : PathDB;
        }

        /// <summary>
        /// Saves Media in a specified folder in predetermined path
        /// </summary>
        /// <param name="SetFile">Media File</param>
        /// <param name="webRootPath">Main Server Path</param>
        /// <param name="FileName">Name of the file after being stored. Can be null.</param>
        /// <returns>string</returns>
        public static async Task<string> FL_CopyMediaFileAsync(this IFormFile SetFile, string FileName, string webRootPath = null)
        {
            if (SetFile == null || SetFile.Length <= 0) return string.IsNullOrEmpty(FileName) ? null : FileName;

            var newFileName = webRootPath != null ? Path.Combine(webRootPath, FileName) : FileName;

            await FL_CopyFileAsync(SetFile, newFileName);
            return string.IsNullOrEmpty(FileName) ? null : FileName;
        }

        /// <summary>
        /// Replaces Media Files Related Employees In Its Designated Folders
        /// </summary>
        /// <param name="MediaFile"></param>
        /// <param name="oldFile"></param>
        /// <param name="oldFilePath"></param>
        /// <param name="newFilePath"></param>
        /// <param name="webRootPath"></param>
        public static async Task<string> FL_RemoveAndReplaceMediaAsync(this IFormFile MediaFile, byte[] oldFile, string oldFilePath, string newFilePath, string webRootPath)
        {
            if (File.Exists(oldFilePath))
            {
                File.Delete(oldFilePath);
            }
            if (MediaFile != null && MediaFile.Length > 0)
            {
                return await MediaFile.FL_CopyMediaFileAsync(newFilePath, webRootPath);
            }

            oldFile.FL_GetFileFromMemory(Path.Combine(webRootPath, oldFilePath));
            return oldFilePath;
        }

        public static async Task<string> FL_RemoveAndReplaceMediaAsync(this IFormFile SetFile, string Extension, [CanBeNull] string OldFileName = null,
            [CanBeNull] string storePath = null, [CanBeNull] string webRootPath = null)
        {
            if (FL_CheckExtension(Extension, SetFile))
            {
                if (File.Exists(FL_FilePath(OldFileName, webRootPath)))
                {
                    File.Delete(FL_FilePath(OldFileName, webRootPath));
                }
                var e = await SetFile.FL_CopyMediaFileToFolderAsync(Extension, null, storePath, webRootPath);
                return e;
            }

            var f = OldFileName;
            return f;
        }

        /// <summary>
        /// Saves Media in a specified folder
        /// </summary>
        /// <param name="SetFile">Media File</param>
        /// <param name="webRootPath">Main Server Path</param>
        /// <param name="storePath">Path for designated media file</param>
        /// <param name="FileName">Name of the file after being stored. Can be null.</param>
        /// <param name="Extension">Extension of the file</param>
        /// <returns>string</returns>
        public static async Task<string> FL_ReplaceMediaFileToFolderAsync(this IFormFile SetFile, string FileName = null,
            string storePath = null, string webRootPath = null, string Extension = null)
        {
            var FileExtension = Extension != null ? "." + Extension : Path.GetExtension(SetFile.FileName.Trim('"'));
            var FilePath = FL_FilePath(FileName, webRootPath, storePath) + FileExtension;
            FL_DeleteFile(FilePath);
            return await FL_CopyMediaFileToFolderAsync(SetFile, FileName, storePath, webRootPath);
        }

        /// <summary>
        /// Deletes Designated Media File
        /// </summary>
        /// <param name="Extension">Extension Of the File</param>
        /// <param name="webRootPath">Main Server Path</param>
        /// <param name="storePath">Path for designated media file</param>
        /// <param name="FileName">Name of the file after being stored. Can be null.</param>
        /// <returns>string</returns>
        public static string FL_DeleteMediaFileFromFolder(this string FileName, string webRootPath = null,
            string storePath = null, string Extension = null)
        {
            var FilePath = FL_FilePathWithExtension(FileName, Extension, webRootPath, storePath);

            if ((File.Exists(FilePath)))
            {
                File.Delete(FilePath);
                return null;
            }
            return storePath != null ? Path.Combine(storePath, FileName) : FileName;
        }

        /// <summary>
        /// Copies File
        /// </summary>
        /// <param name="setFile"></param>
        /// <param name="FilePath"></param>
        public static async Task<bool> FL_CopyFileAsync(this IFormFile setFile, string FilePath)
        {
            if (setFile == null || setFile.Length <= 0) return false;
            using FileStream fs = File.Create(FilePath);
            await setFile.CopyToAsync(fs);
            fs.Flush();
            return true;
        }

        /// <summary>
        /// Replaces File
        /// </summary>
        /// <param name="setFile">File which will replace existing file</param>
        /// <param name="FilePath">File Path of the existing file</param>
        public static async Task<bool> FL_ReplaceFile(this IFormFile setFile, string FilePath)
        {
            if (setFile == null || setFile.Length <= 0) return false;
            FL_DeleteFile(FilePath);
            await FL_CopyFileAsync(setFile, FilePath);
            return true;
        }

        /// <summary>
        /// Deletes File
        /// </summary>
        /// <param name="FilePath">Path of File With Extension</param>
        public static bool FL_DeleteFile(string FilePath)
        {
            if (!File.Exists(FilePath)) return false;
            File.Delete(FilePath);
            return true;
        }

        /// <summary>
        /// Copies File To Memory
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="AltPath"></param>
        public static byte[] FL_GetFileToMemory([CanBeNull] this string FilePath, [CanBeNull] string AltPath = null)
        {
            if (File.Exists(FilePath))
            {
                return File.ReadAllBytes(FilePath);
            }

            return AltPath != null ? File.Exists(AltPath) ? File.ReadAllBytes(AltPath) : null : null;
        }

        /// <summary>
        /// Copies File To Memory
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="height"></param>
        /// <param name="AltPath"></param>
        public static byte[] FL_GetFileToMemoryWebViewSquare([CanBeNull] this string FilePath, int height, [CanBeNull] string AltPath = null)
        {
            if (File.Exists(FilePath))
            {
                return FL_BitmapToBytes(Image.FromFile(FilePath).FL_ResizeImageSquare(height));
            }

            return AltPath != null ? File.Exists(AltPath) ? FL_BitmapToBytes(Image.FromFile(AltPath).FL_ResizeImageSquare(height)) : null : null;
        }

        private static byte[] FL_BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Copies File To Memory
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="FileBytes"></param>
        public static string FL_GetFileFromMemory(this byte[] FileBytes, string FilePath)
        {
            File.WriteAllBytes(FilePath, FileBytes);
            return FilePath != null ? File.Exists(FilePath) ? FilePath : null : null;
        }
    }
}