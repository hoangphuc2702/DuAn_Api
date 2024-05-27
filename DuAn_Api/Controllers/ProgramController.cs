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
                cmd = new SqlCommand("Select * from PROGRAM where programId = @Id", conn);
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
    }
}
