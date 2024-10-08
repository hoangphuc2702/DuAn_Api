﻿using DuAn_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace DuAn_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramController : ControllerBase
    {
        private readonly IConfiguration configuration;
        SqlConnection conn;
        SqlCommand cmd;
        public ProgramController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllPrograms")]
        public async Task<IActionResult> GetAllPrograms()
        {
            List<ProgramModel> listPrograms = new List<ProgramModel>();

            conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            cmd = new SqlCommand("Select * from PROGRAM", conn);
            DataTable dt = new DataTable();

            conn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ProgramModel program = new ProgramModel();
                program.programId = int.Parse(dt.Rows[i]["programId"].ToString());
                program.programName = dt.Rows[i]["programName"].ToString();
                program.startDate = DateTime.Parse(dt.Rows[i]["startDate"].ToString());
                program.endDate = DateTime.Parse(dt.Rows[i]["endDate"].ToString());
                listPrograms.Add(program);
            }

            #region
            //List<ProgramModel> listPrograms = new List<ProgramModel>();

            //conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            //cmd = new SqlCommand("Select p.*, i.imgLink, i.imageId from PROGRAM p " +
            //                "left join IMG i on p.programId = i.programId", conn);

            //DataTable dt = new DataTable();

            //conn.Open();
            //SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            //adapter.Fill(dt);

            //Dictionary<int, ProgramModel> programDict = new Dictionary<int, ProgramModel>();

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    int programId = int.Parse(dt.Rows[i]["programId"].ToString());

            //    if (!programDict.ContainsKey(programId))
            //    {
            //        ProgramModel program = new ProgramModel();
            //        program.programId = programId;
            //        program.programName = dt.Rows[i]["programName"].ToString();
            //        program.startDate = DateTime.Parse(dt.Rows[i]["startDate"].ToString());
            //        program.endDate = DateTime.Parse(dt.Rows[i]["endDate"].ToString());
            //        program.Images = new List<ImageModel>();
            //        programDict[programId] = program;
            //    }

            //    if (dt.Rows[i]["imgLink"] != DBNull.Value)
            //    {
            //        programDict[programId].Images.Add(new ImageModel
            //        {
            //            ImageId = int.Parse(dt.Rows[i]["imageId"].ToString()),
            //            ProgramId = programId,
            //            ImgLink = dt.Rows[i]["imgLink"].ToString()
            //        });
            //    }
            //}

            //listPrograms = programDict.Values.ToList();

            //conn.Close();

            //return Ok(listPrograms);
            #endregion


            return Ok(listPrograms);
        }

        [HttpGet]
        [Route("GetAllImages")]
        public async Task<IActionResult> GetAllImages()
        {
            List<ImageModel> listImages = new List<ImageModel>();

            conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            cmd = new SqlCommand("Select * from IMG", conn);
            DataTable dt = new DataTable();

            conn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ImageModel image = new ImageModel();
                image.ImageId = int.Parse(dt.Rows[i]["imageId"].ToString());
                image.ProgramId = int.Parse(dt.Rows[i]["programId"].ToString());
                image.ImgLink = dt.Rows[i]["imgLink"].ToString();
                image.ImageName = dt.Rows[i]["imageName"].ToString();
                image.Priority = int.Parse(dt.Rows[i]["priority"].ToString());
                image.ImageDes = dt.Rows[i]["imagDes"].ToString();
                listImages.Add(image);
            }

            return Ok(listImages);
        }

        [HttpGet]
        [Route("GetAllNamePrograms")]
        public async Task<IActionResult> GetAllNamePrograms()
        {
            List<string> listNamePrograms = new List<string>();

            conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            cmd = new SqlCommand("Select * from PROGRAM", conn);
            DataTable dt = new DataTable();

            conn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string programName = dt.Rows[i]["programName"].ToString();
                listNamePrograms.Add(programName);
            }

            return Ok(listNamePrograms);
        }

        [HttpGet]
        [Route("GetProgramById/{id}")]
        public async Task<IActionResult> GetProgramById(int id)
        {
            try
            {
                conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
                cmd = new SqlCommand("Select * " +
                                "from PROGRAM " +
                                "where programId = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);

                DataTable dt = new DataTable();

                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                cmd.ExecuteNonQuery();

                if (dt.Rows.Count == 0)
                {
                    return NotFound();
                }

                DataRow row = dt.Rows[0];

                ProgramModel program = new ProgramModel
                {
                    programId = int.Parse(row["programId"].ToString()),
                    programName = row["programName"].ToString(),
                    startDate = DateTime.Parse(row["startDate"].ToString()),
                    endDate = DateTime.Parse(row["endDate"].ToString())
                };

                conn.Close();

                return Ok(program);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("GetProgramByName/{name}")]
        public async Task<IActionResult> GetProgramByName(string name)
        {
            try
            {
                conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
                cmd = new SqlCommand("Select * " +
                                "from PROGRAM " +
                                "where programName = @Name", conn);
                cmd.Parameters.AddWithValue("@Name", name);

                DataTable dt = new DataTable();

                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                cmd.ExecuteNonQuery();

                if (dt.Rows.Count == 0)
                {
                    return NotFound();
                }

                DataRow row = dt.Rows[0];

                ProgramModel program = new ProgramModel
                {
                    programId = int.Parse(row["programId"].ToString()!),
                    programName = row["programName"].ToString(),
                    startDate = DateTime.Parse(row["startDate"].ToString()),
                    endDate = DateTime.Parse(row["endDate"].ToString())
                };

                conn.Close();

                return Ok(program);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        [Route("CreateProgram")]
        public async Task<IActionResult> CreateProgram([FromBody] ProgramModel programModel)
        {
            try
            {
                conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

                conn.Open();

                cmd = new SqlCommand("Select count(*) from PROGRAM", conn);
                cmd.ExecuteNonQuery();

                int programCount = (int)await cmd.ExecuteScalarAsync();

                // Gán programId cho chương trình mới
                int programId = programCount + 1;

                cmd = new SqlCommand("Set IDENTITY_INSERT PROGRAM on", conn);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("Insert into PROGRAM " +
                                "(programId, programName, startDate, endDate)  values " +
                                "(@programId, @programName, @startDate, @endDate);", conn);
                cmd.Parameters.AddWithValue("@programId", programId);
                cmd.Parameters.AddWithValue("@programName", programModel.programName);
                cmd.Parameters.AddWithValue("@startDate", programModel.startDate);
                cmd.Parameters.AddWithValue("@endDate", programModel.endDate);

                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("Set IDENTITY_INSERT PROGRAM off", conn);
                cmd.ExecuteNonQuery();


                cmd = new SqlCommand("Select count(*) from IMG", conn);
                cmd.ExecuteNonQuery();

                int imageCount = (int)await cmd.ExecuteScalarAsync();

                // Gán imageId cho chương trình mới
                int imageId = imageCount + 1;

                cmd = new SqlCommand("Set IDENTITY_INSERT IMG on", conn);
                cmd.ExecuteNonQuery();

                // Insert images
                foreach (var image in programModel.Images)
                {
                    cmd = new SqlCommand("Insert into IMG " +
                                    "(imageId, programId, imgLink, imageName, priority, imagDes)  values " +
                                    "(@imageId, @programId, @imgLink, @imageName, @priority, @imagDes);", conn);
                    cmd.Parameters.AddWithValue("@imageId", imageId);
                    cmd.Parameters.AddWithValue("@programId", programId);
                    cmd.Parameters.AddWithValue("@imgLink", image.ImgLink);
                    cmd.Parameters.AddWithValue("@imageName", image.ImageName);
                    cmd.Parameters.AddWithValue("@priority", image.Priority);
                    cmd.Parameters.AddWithValue("@imagDes", image.ImageDes);

                    cmd.ExecuteNonQuery();
                    imageId++;
                }

                cmd = new SqlCommand("Set IDENTITY_INSERT IMG off", conn);
                cmd.ExecuteNonQuery();


                // Select program with images
                cmd = new SqlCommand("Select p.*, i.imageId, i.imgLink, i.imageName, i.priority, i.imagDes from PROGRAM p " +
                                "left join IMG i on p.programId = i.programId " +
                                "where p.programId = @programId", conn);
                cmd.Parameters.AddWithValue("@programId", programId);


                DataTable dt = new DataTable();

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    return NotFound();
                }

                DataRow row = dt.Rows[0];

                ProgramModel resultProgram = new ProgramModel
                {
                    programId = int.Parse(row["programId"].ToString()),
                    programName = row["programName"].ToString(),
                    startDate = DateTime.Parse(row["startDate"].ToString()),
                    endDate = DateTime.Parse(row["endDate"].ToString()),
                    Images = new List<ImageModel>()
                };

                // Add images to resultProgram
                foreach (DataRow imageRow in dt.Rows)
                {
                    if (imageRow["imgLink"] != DBNull.Value)
                    {
                        resultProgram.Images.Add(new ImageModel
                        {
                            ImageId = int.Parse(imageRow["imageId"].ToString()),
                            ProgramId = int.Parse(imageRow["programId"].ToString()),
                            ImgLink = imageRow["imgLink"].ToString(),
                            ImageName = imageRow["imageName"].ToString(),
                            Priority = int.Parse(imageRow["priority"].ToString()),
                            ImageDes = imageRow["imagDes"].ToString(),
                        });
                    }
                }

                conn.Close();

                return Ok(resultProgram);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut]
        [Route("UpdateProgram")]
        public async Task<IActionResult> UpdateProgram([FromBody] ProgramModel programModel)
        {
            try
            {
                using (var conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
                {
                    conn.Open();

                    // Cập nhật thông tin chương trình
                    var cmd = new SqlCommand("UPDATE PROGRAM SET programName = @programName, startDate = @startDate, endDate = @endDate WHERE programId = @programId", conn);
                    cmd.Parameters.AddWithValue("@programId", programModel.programId);
                    cmd.Parameters.AddWithValue("@programName", programModel.programName);
                    cmd.Parameters.AddWithValue("@startDate", programModel.startDate);
                    cmd.Parameters.AddWithValue("@endDate", programModel.endDate);
                    cmd.ExecuteNonQuery();



                    // Cập nhật hoặc thêm hình ảnh mới
                    foreach (var image in programModel.Images)
                    {
                        if (image.ImageId == 0) // Nếu imageId là 0, hình ảnh mới sẽ được thêm vào
                        {
                            cmd = new SqlCommand("Select count(*) from IMG", conn);
                            cmd.ExecuteNonQuery();

                            int imageCount = (int)await cmd.ExecuteScalarAsync();

                            // Gán imageId cho chương trình mới
                            int imageId = imageCount + 1;

                            cmd = new SqlCommand("Set IDENTITY_INSERT IMG on", conn);
                            cmd.ExecuteNonQuery();


                            cmd = new SqlCommand("Insert into IMG " +
                                                "(imageId, programId, imgLink, imageName, priority, imagDes)  values " +
                                                "(@imageId, @programId, @imgLink, @imageName, @priority, @imagDes);", conn);
                            cmd.Parameters.AddWithValue("@imageId", imageId);
                            cmd.Parameters.AddWithValue("@programId", programModel.programId);
                            cmd.Parameters.AddWithValue("@imgLink", image.ImgLink);
                            cmd.Parameters.AddWithValue("@imageName", image.ImageName);
                            cmd.Parameters.AddWithValue("@priority", image.Priority);
                            cmd.Parameters.AddWithValue("@imagDes", image.ImageDes);

                            cmd.ExecuteNonQuery();


                            cmd = new SqlCommand("Set IDENTITY_INSERT IMG off", conn);
                            cmd.ExecuteNonQuery();
                        }
                        else // Nếu imageId đã tồn tại, cập nhật hình ảnh hiện có
                        {
                            cmd = new SqlCommand("UPDATE IMG SET imgLink = @imgLink, imageName = @imageName, priority = @priority, imagDes = @imagDes WHERE imageId = @imageId", conn);
                            cmd.Parameters.AddWithValue("@imageId", image.ImageId);
                            cmd.Parameters.AddWithValue("@programId", programModel.programId);
                            cmd.Parameters.AddWithValue("@imgLink", image.ImgLink);
                            cmd.Parameters.AddWithValue("@imageName", image.ImageName);
                            cmd.Parameters.AddWithValue("@priority", image.Priority);
                            cmd.Parameters.AddWithValue("@imagDes", image.ImageDes);

                            cmd.ExecuteNonQuery();
                        }

                    }



                    // Lấy chương trình cùng với hình ảnh
                    cmd = new SqlCommand("SELECT p.*, i.imgLink, i.imageId, i.imgLink, i.imageName, i.priority, i.imagDes FROM PROGRAM p LEFT JOIN IMG i ON p.programId = i.programId WHERE p.programId = @programId", conn);
                    cmd.Parameters.AddWithValue("@programId", programModel.programId);

                    var dt = new DataTable();
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }

                    if (dt.Rows.Count == 0)
                    {
                        return NotFound();
                    }

                    var resultProgram = new ProgramModel
                    {
                        programId = int.Parse(dt.Rows[0]["programId"].ToString()),
                        programName = dt.Rows[0]["programName"].ToString(),
                        startDate = DateTime.Parse(dt.Rows[0]["startDate"].ToString()),
                        endDate = DateTime.Parse(dt.Rows[0]["endDate"].ToString()),
                        Images = new List<ImageModel>()
                    };

                    // Thêm hình ảnh vào resultProgram
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["imgLink"] != DBNull.Value)
                        {
                            resultProgram.Images.Add(new ImageModel
                            {
                                ImageId = int.Parse(row["imageId"].ToString()),
                                ProgramId = int.Parse(row["programId"].ToString()),
                                ImgLink = row["imgLink"].ToString(),
                                ImageName = row["imageName"].ToString(),
                                Priority = int.Parse(row["priority"].ToString()),
                                ImageDes = row["imagDes"].ToString(),
                            });
                        }
                    }

                    return Ok(resultProgram);
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi, có thể log hoặc trả về thông báo lỗi phù hợp
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("GetImageByName/{name}")]
        public async Task<IActionResult> GetImageByName(string name)
        {
            List<ImageModel> listImages = new List<ImageModel>();

            try
            {
                conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
                cmd = new SqlCommand("SELECT * " +
                                "from IMG " +
                                "JOIN PROGRAM ON IMG.programId = PROGRAM.programId " +
                                "where PROGRAM.programName = @Name", conn);
                cmd.Parameters.AddWithValue("@Name", name);

                DataTable dt = new DataTable();

                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);

                cmd.ExecuteNonQuery();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ImageModel image = new ImageModel();
                    image.ImageId = int.Parse(dt.Rows[i]["imageId"].ToString());
                    image.ProgramId = int.Parse(dt.Rows[i]["programId"].ToString());
                    image.ImgLink = dt.Rows[i]["imgLink"].ToString();
                    image.ImageName = dt.Rows[i]["imageName"].ToString();
                    image.Priority = int.Parse(dt.Rows[i]["priority"].ToString());
                    image.ImageDes = dt.Rows[i]["imagDes"].ToString();
                    listImages.Add(image);
                }

                conn.Close();

                return Ok(listImages);
            }
            catch
            {
                throw;
            }
        }
    }
}
