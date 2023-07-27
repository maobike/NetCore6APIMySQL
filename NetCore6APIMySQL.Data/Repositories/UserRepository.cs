using Dapper;
using MySql.Data.MySqlClient;
using NetCore6APIMySQL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCore6APIMySQL.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MySQLConfiguration _connectionString;
        // Contructor para recivir el string de conexion a mysql
        public UserRepository(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        // Retorna la conexion
        protected MySqlConnection dbConnection()
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        
        /**
         * Elimina un usuario
         */
        public async Task<bool> DeleteUser(User user)
        {
            var db = dbConnection();

            var sql = @" Delete from users Where id = @Id";

            var result = await db.ExecuteAsync(sql, new { user.Id });

            return result > 0;
        }

        /*
         * Obtener todos los usuarios
         */
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var db = dbConnection();
            var sql = @" Select id, name, email, password, phone from users";
            return await db.QueryAsync<User>(sql, new { });
        }

        /**
         * Retorna un usuario por Id
         */
        public async Task<User> GetUserById(int id)
        {
            var db = dbConnection();
            var sql = @" Select id, name, email, password, phone from users where id = @Id";
            return await db.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        /**
         * Crea o inserta un nuevo usuario
         */
        public async Task<bool> InsertUser(User user)
        {
            var db = dbConnection();
            var sql = @" Insert into users (name, email, password, phone) 
                        values (@name, @email, @password, @phone) ";
            var result = await db.ExecuteAsync(sql, new { user.Name, user.Email, user.Password, user.Phone });

            return result > 0;
        }

        /**
         * Edita un usuario
         */
        public async Task<bool> UpdateUser(User user)
        {
            var db = dbConnection();
            var sql = @" Update users set 
                            name = @Name, 
                            email = @Email, 
                            password = @Password, 
                            phone = @Phone 
                        Where id = @Id ";
            var result = await db.ExecuteAsync(sql, new 
            { user.Name, user.Email, user.Password, user.Phone, user.Id });

            return result > 0;
        }
    }
}
