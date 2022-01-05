using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Models;
using Models.DataHelper;
using System.Data;
using System.Collections.Generic;

namespace Xjp2Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImgUploadController : Controller
    {
        private readonly StreetContext _context;
        private XjpRepository _repository = null;
       
        private static IWebHostEnvironment _hostingEnvironment;
        

        public ImgUploadController(IWebHostEnvironment hostingEnvironment, StreetContext xjpContext)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = xjpContext;
            _repository = new XjpRepository(_context);
        }

        /// <summary>
        /// 单文件上传（ajax，Form表单都适用）
        /// </summary>
        /// <returns></returns>
        [HttpPost("[action]")]
        public JsonResult SingleFileUpload()
        {  
            var formFile = Request.Form.Files[0];//获取请求发送过来的文件
            var currentDate = DateTime.Now;
            //var webRootPath = _hostingEnvironment.WebRootPath;//>>>相当于HttpContext.Current.Server.MapPath("") 
            var webRootPath = "F://houkunkun//XJP";
            try
            {
                var filePath = $"/UploadFile/{currentDate:yyyyMMdd}/";
                var IISPath = webRootPath + filePath;

                //创建每日存储文件夹
                if (!Directory.Exists(IISPath))
                {
                    Directory.CreateDirectory(IISPath);
                }

                if (formFile != null)
                {
                    //文件后缀
                    var fileExtension = Path.GetExtension(formFile.FileName);//获取文件格式，拓展名

                    //判断文件大小
                    var fileSize = formFile.Length;

                    if (fileSize > 1024 * 1024 * 10) //10M TODO:(1mb=1024X1024b)
                    {
                        return new JsonResult(new { isSuccess = false, resultMsg = "上传的文件不能大于10M" });
                    }

                    //保存的文件名称(以名称和保存时间命名)
                    var saveName = formFile.FileName.Substring(0, formFile.FileName.LastIndexOf('.')) + "_" + currentDate.ToString("HHmmss") + fileExtension;

                    //文件保存
                    using (var fs = System.IO.File.Create(webRootPath + filePath + saveName))
                    {
                        formFile.CopyTo(fs);
                        fs.Flush();
                    }

                    //完整的文件路径
                    var completeFilePath = Path.Combine(filePath, saveName);
                    return new JsonResult(new { isSuccess = true, returnMsg = "上传成功", completeFilePath = completeFilePath });
                }
                else
                {
                    return new JsonResult(new { isSuccess = false, resultMsg = "上传失败，未检测上传的文件信息~" });
                }

            }
            catch (Exception ex)
            {
                return new JsonResult(new { isSuccess = false, resultMsg = "文件保存失败，异常信息为：" + ex.Message });
            }
        }



        //path保存
        [HttpPost("[action]")]
        public void SaveImgPath([FromBody] SavepathRowParam rowdata)
        {
            _repository.SaveImgPath(rowdata);
        }
    }
}
