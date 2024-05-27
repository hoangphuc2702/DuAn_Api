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

        public ProgramController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        SqlConnection conn;
        SqlCommand cmd;

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
        [Route("GetProgramById")]
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

        [HttpPost]
        [Route("CreateProgram")]
        public async Task<IActionResult> CreateProgram(String programName, DateTime startDate, DateTime endDate)
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
                cmd.Parameters.AddWithValue("@programName", programName);
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);

                cmd.ExecuteNonQuery();


                cmd = new SqlCommand("Set IDENTITY_INSERT PROGRAM off", conn);
                cmd.ExecuteNonQuery();


                cmd = new SqlCommand("Select * from PROGRAM " +
                                "where programName = @programName", conn);
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

        [HttpPut]
        [Route("UpdateProgram")]
        public async Task<IActionResult> UpdateProgram(int programId, String programName, DateTime startDate, DateTime endDate)
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
    }
}
