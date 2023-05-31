using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseProject.DAL.Models.EF;

namespace CourseProject.BLL.Repositories
{
    public class SellerRepository : Repository<Seller>
    {
        private const string CREATE_QUERY = "INSERT INTO Seller(SellerName, SellerSurname, Phone, Email) Values(@SellerName, @SellerSurname, @Phone, @Email)";
        private const string DELETE_QUERY = "DELETE FROM Seller WHERE Id = @Id";
        private const string UPDATE_QUERY = "UPDATE Seller SET SellerName = @SellerName, SellerSurname = @SellerSurname, Phone = @Phone, Email = @Email WHERE Id = @Id";
        private const string GET_BY_ID_QUERY = "SELECT s.Id, s.SellerName, s.SellerSurname, s.Phone, s.Email FROM Seller s WHERE s.Id = @Id";
        private const string GET_QUERY = "SELECT s.Id, s.SellerName, s.SellerSurname, s.Phone, s.Email FROM Seller s";
        private const string GET_BY_NAME_QUERY = "SELECT s.Id, s.SellerName, s.SellerSurname, s.Phone, s.Email FROM Seller s WHERE s.SellerName LIKE '%' + @SellerName + '%'";

        public SellerRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public SellerRepository() { }

        public override void Create(Seller entity)
        {
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@SellerName", entity.SellerName),
                new SqlParameter("@SellerSurname", entity.SellerSurname),
                new SqlParameter("@Phone", entity.Phone),
                new SqlParameter("@Email", entity.Email)
            };
            ExecuteScalarCommand(CREATE_QUERY, parameters);
        }

        public Seller GetSellerName(string name)
        {
            Seller seller = new Seller();
            using (var connection = new SqlConnection(con))
            {
                SqlParameter parameter = new SqlParameter("@SellerName", name);
                connection.Open();
                using (var command = new SqlCommand(GET_BY_NAME_QUERY, connection))
                {
                    command.Parameters.Add(parameter);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            seller.Id = Convert.ToInt32(reader["Id"]);
                            seller.SellerName = Convert.ToString(reader["SellerName"]);
                            seller.SellerSurname = Convert.ToString(reader["SellerSurname"]);
                            seller.Phone = Convert.ToString(reader["Phone"]);
                            seller.Email = Convert.ToString(reader["Email"]);
                        }
                    }
                }
                connection.Close();
            }
            return seller;
        }

        public override void Delete(Seller entity)
        {
            var parameters = new SqlParameter[] { new SqlParameter("@Id", entity.Id) };
            ExecuteCommand(DELETE_QUERY, parameters);
        }

        public override Seller Get(int id)
        {
            Seller seller = new Seller();
            using (var connection = new SqlConnection(con))
            {
                SqlParameter parameter = new SqlParameter("@Id", id);
                connection.Open();
                using (var command = new SqlCommand(GET_BY_ID_QUERY, connection))
                {
                    command.Parameters.Add(parameter);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            seller.Id = Convert.ToInt32(reader["Id"]);
                            seller.SellerName = Convert.ToString(reader["SellerName"]);
                            seller.SellerSurname = Convert.ToString(reader["SellerSurname"]);
                            seller.Phone = Convert.ToString(reader["Phone"]);
                            seller.Email = Convert.ToString(reader["Email"]);
                        }
                    }
                }
                connection.Close();
            }
            return seller;
        }

        public override IEnumerable<Seller> Get()
        {
            List<Seller> sellers = new List<Seller>();

            using (var connection = new SqlConnection(con))
            {
                connection.Open();
                using (var command = new SqlCommand(GET_QUERY, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Seller seller = new Seller();
                            seller.Id = Convert.ToInt32(reader["Id"]);
                            seller.SellerName = Convert.ToString(reader["SellerName"]);
                            seller.SellerSurname = Convert.ToString(reader["SellerSurname"]);
                            seller.Phone = Convert.ToString(reader["Phone"]);
                            seller.Email = Convert.ToString(reader["Email"]);

                            sellers.Add(seller);
                        }
                    }
                }
            }
            return sellers;
        }

        public override void Update(Seller entity)
        {
            var parameters = new SqlParameter[] {
            new SqlParameter("@Id", entity.Id),
            new SqlParameter("@SellerName", entity.SellerName),
            new SqlParameter("@SellerSurname", entity.SellerSurname),
            new SqlParameter("@Phone", entity.Phone),
            new SqlParameter("@Email", entity.Email),
            };
            ExecuteCommand(UPDATE_QUERY, parameters);

        }
    }
}
