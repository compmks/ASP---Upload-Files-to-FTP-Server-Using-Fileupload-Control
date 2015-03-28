using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;


public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        string albumImage;
        string link = Server.MapPath("~/tmp/albumImage/"); //temporary folder


        if (FileUpload1.HasFile) //It will not work if fileupload control is left empty
        {

            if (!Directory.Exists(link))
            {
                Directory.CreateDirectory(link);
            }
            string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
            albumImage = link + filename;
            string reportNamepd1 = filename;
            FileUpload1.SaveAs(albumImage);
            //uploading image from temporary  folder to FTP server
            Upload(albumImage);

        }
        else
        {
            albumImage = "-NA-";
        }

        
        
        //to get uploaded file url, in case you want to store the url to your database
        String fileimage = "http://virandry.com/image/" + Path.GetFileName(albumImage);

    }

    private static void Upload(string file)
    {

        String ftpurl = "ftp://ftp.virandry.com/image/"; //e.g. ftp://ftp.virandry.com/image/
        String ftpusername = "USERNAME"; // Your ftp username
        String ftppassword = "PASSWORD"; // Your ftp password

        try
        {

            string filename = Path.GetFileName(file);
            string ftpfullpath = ftpurl + filename;
            FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
            ftp.Credentials = new NetworkCredential(ftpusername, ftppassword);

            ftp.KeepAlive = true;
            ftp.UseBinary = true;
            ftp.Method = WebRequestMethods.Ftp.UploadFile;

            FileStream fs = File.OpenRead(file);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();

            Stream ftpstream = ftp.GetRequestStream();
            ftpstream.Write(buffer, 0, buffer.Length);
            ftpstream.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}