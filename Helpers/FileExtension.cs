namespace Diana.Mvc.Helpers;

public static class FileExtension
{

    public static bool IsValidSize(this IFormFile file, float kb = 20)
       //iFormFile byte qaytarir deye bizde kb ile yoxlamaq isteyirik deye 1000e vurduq
       // burada basqa extensionda istifade ede bilerikki fayllarin olcusunu azaltsin
       => file.Length <= kb * 1024;

    public static bool IsCorrectType(this IFormFile file, string contentType = "image")
    => file.ContentType.Contains(contentType);

    //IWebHostEnvironmenti burda cagira bilmedik cunki ctor yoxdu ctor olsada extenson classdi deye static ctor olur ondada cagirmaq olmur 
    public static async Task<string> SaveAsync(this IFormFile file, string path)
    {
        string extension = Path.GetExtension(file.FileName); // extensionun goturduk filenin
        string fileName = Path.GetFileName(file.FileName).Length > 32 ? file.FileName.Substring(0, 32) : Path.GetFileNameWithoutExtension(file.FileName);
        fileName = Path.Combine(path, Path.GetRandomFileName() + fileName + extension);
        //(File.exists) buraya elave olunmalidir!!! : qovluqda bu adda fayl varmi ? cunki random olasa bir yerde ust uste dusecek o zaman da bir birlerini ovveride edecekler
        using (FileStream fs = File.Create(Path.Combine(PathConstants.RootPath, fileName)))
        {
            await file.CopyToAsync(fs);
        }
        return fileName;
    }
}
