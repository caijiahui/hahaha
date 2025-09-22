using advt.Data;
using advt.Entity;
using Newtonsoft.Json;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.SS.Formula.Functions;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace advt.CMS.Models.ExamModel
{
    public class ExamConfirmModel
    {
        public List<UserInfos> LstUserInfos { get; set; }
        public List<string> ListWorkPlace { get; set; }
        public List<string> LExamType { get; set; }
        public List<string> LDepartCode { get; set; }
        public List<string> LSubject { get; set; }
        public ExamConfirmModel() : base()
        {
            LstUserInfos = new List<UserInfos>();
            ListWorkPlace = new List<string>();
            LExamType = new List<string>();
            LDepartCode = new List<string>();
            LSubject = new List<string>();
        }
        /// <summary>
        /// 签到点名页面的数据（区域，部门...）
        /// </summary>
        /// <param name="data"></param>

        public void GetExamInfo(SearchHrData data)
        {
            var ddate = Convert.ToDateTime(data.ExamDate);
            var examdetail = ddate.AddDays(1);
            var lst = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { IsStop =0, IsExam = "false", IsStartExam=false });
            if (!string.IsNullOrEmpty(data.WrokPlace))
            { lst = lst.Where(x => x.OrgName == data.WrokPlace).ToList(); }
            if (!string.IsNullOrEmpty(data.TypeName))
            {
                if (data.TypeName != "全部")
                    lst = lst.Where(x => x.TypeName == data.TypeName).ToList();
            }
            if (!string.IsNullOrEmpty(data.SubjectName))
            { lst = lst.Where(x => x.TypeName == data.SubjectName).ToList(); }
            if (!string.IsNullOrEmpty(data.ExamDate))
            { lst = lst.Where(x => x.ExamDate>= ddate && x.ExamDate< examdetail).ToList(); }
            if (!string.IsNullOrEmpty(data.DepartCode))
            { lst = lst.Where(x => x.DepartCode == data.DepartCode).ToList(); }
            if (!string.IsNullOrEmpty(data.UserCode))
            {   data.UserCode = data.UserCode.ToUpper();
                lst = lst.Where(x => x.UserCode.Contains(data.UserCode)).ToList(); }
            if (!string.IsNullOrEmpty(data.UserName))
            { lst = lst.Where(x => x.UserName.Contains(data.UserName)).ToList(); }
            if (!string.IsNullOrEmpty(data.DutyType))
            { lst = lst.Where(x => x.DutyType.Contains(data.DutyType)).ToList(); }

            LstUserInfos = lst.Select(y => new UserInfos
            {
                DepartCode = y.DepartCode,
                UserCode = y.UserCode,
                UserName = y.UserName,
                SubjectName = y.SubjectName,
                TypeName = y.TypeName,
                OrgName = y.OrgName,
                IsStartExam = y.IsStartExam,
                ExamDate=y.ExamDate,
                ID = y.ID,
                DutyType = y.DutyType

            }).OrderByDescending(t => t.ExamDate).ToList();

            //var user = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo();
            //ListWorkPlace = user.GroupBy(x => x.OrgName).Select(y => y.Key).Distinct().ToList();//获取区域

            IDataReader reader = DatabaseProvider.GetInstance().Get_OAArea();//从OA数据库获取区域
            ListWorkPlace= SqlHelper.ConvertDataReaderToList(reader);
            LExamType.Add("全部");
            foreach (var item in Data.ExamType.Get_All_ExamType().GroupBy(x => x.TypeName).Select(y => y.Key))
            {
                LExamType.Add(item);
            }
            
        }
        public void SaveConfirmUser(int ID,string username)
        {
            var ddate =Convert.ToDateTime(DateTime.Now.ToShortDateString());
            var examdetail = ddate.AddDays(1);
            var user = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo(new { ID=ID });
            if (user.Count() > 0 && user != null)
            {
                foreach (var item in user)
                {
                    if (item.ExamDate< examdetail&&item.ExamDate> ddate)
                    {
                        item.IsStartExam = true;
                        item.StartExamUser = username;
                        item.StartExamDate = DateTime.Now;
                        Data.ExamUserDetailInfo.Update_ExamUserDetailInfo(item, null, new string[] { "ID" });
                    }                   
                }                
            }
        }
        public bool GetAdminType(string name)
        {
            bool isadmin = false;
            var use = Data.ExamUserDetailInfo.GetAuthority(name);
            if (use.Count() > 0 && use != null)
            {
                isadmin = true;
            }
            return isadmin;
        }
        public void GetDepartcode(string code)
        {
            //var lst = Data.ExamUserDetailInfo.Get_All_ExamUserDetailInfo();
            //LDepartCode = lst.Where(x=>x.OrgName==code).OrderBy(x => x.DepartCode).GroupBy(x => x.DepartCode).Select(y => y.Key).Distinct().ToList();
            IDataReader reader = DatabaseProvider.GetInstance().Get_OADept(code);//从OA数据库获取部门
            LDepartCode = SqlHelper.ConvertDataReaderToList(reader);

        }
        public void GetTypeSubject(string code)
        {
            var lst = Data.ExamSubject.Get_All_ExamSubject(new { TypeName =code});
            LSubject = lst.GroupBy(x => x.SubjectName).Select(y => y.Key).Distinct().ToList();
        }
        /// <summary>
        /// 考试前检验是否最新登录状态
        /// </summary>
        /// <param name="NewGuid"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool CheckLogStauts(string NewGuid, string userName)
        {
            Entity.sys_log log = advt.Data.sys_log.Get_sys_log(userName);
            if (log.newguid == NewGuid)
            { return true; }
            else
            { return false; }
        }

        /// <summary>
        /// 人脸识别
        /// </summary>
        /// <param name="userHrid">工号</param>
        /// <param name="UploadPhotoImage">拍照图片</param>
        /// <returns></returns>
        public string  FaceDetect(string userHrid, byte[] UploadPhotoImage )
        {
            string apiResponse = "";
            IDataReader reader = DatabaseProvider.GetInstance().Get_EmployePhoto(userHrid);//从TrainingPlatform数据库获取员工照片             
            if (reader.Read())
            {
                string temp = reader.GetValue(0).ToString();
                if (!string.IsNullOrEmpty(temp))
                {
                    byte[] b = (byte[])reader.GetValue(0);

                    HttpWebResponse response = null;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://172.21.132.66:8888/face_compare");
                    request.KeepAlive = false;
                    request.Method = "POST";
                    request.Timeout = 9000;
                    request.ContentType = "application/json;charset=utf-8";//text/xml(GET一般用这个)、application/json、application/x-www-form-urlencoded(一般POST不用json就用这个)、application/x-www-form-urlencoded;charset=utf-8

                    try
                    {
                        var requestObj = new
                        {
                            src_face_image_base64 = Convert.ToBase64String(b), //基准人脸图像（Base64编码）
                            dst_face_image_base64= Convert.ToBase64String(UploadPhotoImage)  // 待比对的人脸图像（Base64编码）
                        };
                        string requestContent = JsonConvert.SerializeObject(requestObj);
                        byte[] buffer = Encoding.UTF8.GetBytes(requestContent);
                        request.ContentLength = buffer.Length;
                        Stream outStream = request.GetRequestStream();
                        outStream.Write(buffer, 0, buffer.Length);
                        outStream.Dispose();
                        using (response = (HttpWebResponse)request.GetResponse())
                        {
                            using (StreamReader streamreader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")))
                            {
                                apiResponse = streamreader.ReadToEnd();
                                streamreader.Close();
                                streamreader.Dispose();
                            }
                            response.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        apiResponse = e.ToString();
                    }
                    finally
                    {
                        if (response != null)
                            response.Close();
                        if (request != null)
                            request.Abort();
                    }
                }
            }
            return apiResponse;
        }
        /// <summary>
        /// 保存人脸图片
        /// </summary>
        /// <param name="userHrid"></param>
        /// <param name="UploadPhotoImage"></param>
        public void SaveFaceImage(string userHrid, byte[] UploadPhotoImage, Face_Compare model)
        {
            string sqlconnstr = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["advt_pe"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(sqlconnstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "insert  [dbo].[UserPhoto] ( [UserCode], [EmployePhoto], [CreatedDate],face_sim,err_code,processing_time)values(@Var_Hrid,@Image,'" + DateTime.Now + "','"+model.face_sim+"','"+model.err_code+"','"+model.processing_time+"')";
                SqlParameter Par1 = new SqlParameter("@Var_Hrid", SqlDbType.NVarChar);
                Par1.Value = userHrid;
                cmd.Parameters.Add(Par1);
                SqlParameter Par2 = new SqlParameter("@Image", SqlDbType.Image);
                Par2.Value = UploadPhotoImage;
                cmd.Parameters.Add(Par2);
                int t = (int)(cmd.ExecuteNonQuery());
                //if (t > 0)
                //{
                //    Console.WriteLine("插入成功");
                //}
                conn.Close();
            }

        }
    }
    public class UserInfos
    { 
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string RuleName { get; set; }
        public string DepartCode { get; set; }
        public string SubjectName { get; set; }
        public string TypeName { get; set; }
        public DateTime? ExamDate { get; set; }
        public string OrgName { get; set; }
        public bool IsStartExam { get; set; }//是否签到
        public int ID { get; set; }
        public string DutyType { get; set; } //班别


    }
    public class Face_Compare
    {
        /// <summary>
        ///  人脸相似度得分（0-1，值越大相似度越高）
        /// </summary>
        public string face_sim { get; set; }
        /// <summary>
        /// 处理状态码（详见错误码表）
        /// </summary>
        public string err_code { get; set; }
        /// <summary>
        /// 模型分析耗时
        /// </summary>
        public string processing_time { get; set; }
    }

//    错误码 常量标识    说明
//0	OK 比对成功
//1	RunTimeExcept 系统运行时异常
//2	SrcImageNoFace 基准图像未检测到人脸
//3	SrcImageFaceMoreThanOne 基准图像检测到多个人脸
//4	DstImageNoFace 待比对图像未检测到人脸
//5	DstImageFaceMoreThanOne 待比对图像检测到多个人脸



}