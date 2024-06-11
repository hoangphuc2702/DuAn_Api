using DuAn_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

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


            return Ok(listPrograms);
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
                                    "(imageId, programId, imgLink)  values " +
                                    "(@imageId, @programId, @imgLink);", conn);
                    cmd.Parameters.AddWithValue("@imageId", imageId);
                    cmd.Parameters.AddWithValue("@programId", programId);
                    cmd.Parameters.AddWithValue("@imgLink", image.ImgLink);

                    cmd.ExecuteNonQuery();
                    imageId++;
                }

                cmd = new SqlCommand("Set IDENTITY_INSERT IMG off", conn);
                cmd.ExecuteNonQuery();


                // Select program with images
                cmd = new SqlCommand("Select p.*, i.imgLink, i.imageId from PROGRAM p " +
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
                            ImgLink = imageRow["imgLink"].ToString()
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
        public async Task<IActionResult> UpdateProgram(int programId, string programName, DateTime startDate, DateTime endDate)
        {
            try
            {
                conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

                conn.Open();

                cmd = new SqlCommand("Update PROGRAM " +
                                    "set programName = @programName, " +
                                    "startDate = @startDate, " +
                                    "endDate = @endDate " +
                                    "where programId = @programId", conn);
                cmd.Parameters.AddWithValue("@programId", programId);
                cmd.Parameters.AddWithValue("@programName", programName);
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);

                cmd.ExecuteNonQuery();


                cmd = new SqlCommand("Select * from PROGRAM where programName = @programName", conn);
                cmd.Parameters.AddWithValue("@programName", programName);

                DataTable dt = new DataTable();

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
        [Route("GetImageByName/{name}")]
        public async Task<IActionResult> GetImageByName(string name)
        {
            List<string> images = new List<string>();

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
                    string imageLink = dt.Rows[i]["imgLink"].ToString();
                    images.Add(imageLink);
                }

                conn.Close();

                return Ok(images);
            }
            catch
            {
                throw;
            }
        }
    }
}
