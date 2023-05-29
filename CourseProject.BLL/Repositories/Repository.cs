using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseProject.DAL.Models.EF;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;

namespace CourseProject.BLL.Repositories
{
        public abstract class Repository<T>
        {
            protected const string CONNECTION_KEY = "DB:connection";
            protected IConfiguration _configuration;
            protected string con = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Dasha\\OneDrive\\Рабочий стол\\Сourse work\\CourseProject\\CourseProject.DAL\\Database\\TestDB.mdf";
            
            public Repository()
            {

            }
            public Repository(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            public abstract T Get(int id);

            public abstract IEnumerable<T> Get();

            public abstract void Update(T entity);

            public abstract void Delete(T entity);

            public abstract void Create(T entity);

            protected void ExecuteCommand(string query, SqlParameter[] parameters)
            {
                using (var connection = new SqlConnection(con))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddRange(parameters);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }

            protected object ExecuteScalarCommand(string query, SqlParameter[] parameters)
            {
                object result;
                using (var connection = new SqlConnection(con))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddRange(parameters);
                        result = command.ExecuteScalar();
                    }
                    connection.Close();
                }
                return result;
            }
        }
}

