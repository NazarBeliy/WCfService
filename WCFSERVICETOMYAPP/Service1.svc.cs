using Npgsql;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace WCFSERVICETOMYAPP
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
   

    public class Service1 : IService1
    {
        const string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=Ergan223223;Database=postgres";

        public async Task<User[]> GettingAllUsersAsync()
        {
            List<User> users = new List<User>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM users";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            User user = new User
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                TaxId = reader.GetString(2),
                                Email = reader.GetString(3),
                                PhoneNumber = reader.GetString(4),
                                Created = reader.GetDateTime(5),
                                Updated = reader.GetDateTime(6)
                            };
                            users.Add(user);
                        }
                    }
                }
            }

            return users.ToArray();
        }

        public UserOperationResult AddingNewUser(User user)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string checkQuery = "SELECT * FROM users WHERE fullname = @fullname OR email = @Email OR taxid = @TaxId OR phonenumber = @PhoneNumber";
                    using (var checkCommand = new NpgsqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@fullname", user.Name);
                        checkCommand.Parameters.AddWithValue("@Email", user.Email);
                        checkCommand.Parameters.AddWithValue("@TaxId", user.TaxId);
                        checkCommand.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);

                        using (var reader = checkCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader["fullname"].ToString() == user.Name)
                                    return UserOperationResult.NameExists;
                                if (reader["email"].ToString() == user.Email)
                                    return UserOperationResult.EmailExists;
                                if (reader["taxid"].ToString() == user.TaxId)
                                    return UserOperationResult.TaxIdExists;
                                if (reader["phonenumber"].ToString() == user.PhoneNumber)
                                    return UserOperationResult.PhoneNumberExists;
                            }
                        }
                    }

                    string insertQuery = "INSERT INTO users (fullname, email, taxid, phonenumber, createddate, lastmodifieddate) VALUES (@fullname, @Email, @TaxId, @PhoneNumber, @Created, @Updated)";
                    using (var insertCommand = new NpgsqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@fullname", user.Name);
                        insertCommand.Parameters.AddWithValue("@Email", user.Email);
                        insertCommand.Parameters.AddWithValue("@TaxId", user.TaxId);
                        insertCommand.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
                        insertCommand.Parameters.AddWithValue("@Created", user.Created);
                        insertCommand.Parameters.AddWithValue("@Updated", user.Updated);

                        insertCommand.ExecuteNonQuery();
                    }
                    return UserOperationResult.Good;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return UserOperationResult.Good; // Повертаємо Good у разі помилки для простоти
                }
            }
        }

        public void ChargingSomeChanges(User oldUser, User newUser)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE users SET FullName = @NewFullName, TaxID = @NewTaxID, Email = @NewEmail, PhoneNumber = @NewPhoneNumber, LastModifiedDate = @LastModifiedDate WHERE FullName = @OldFullName AND TaxID = @OldTaxID AND Email = @OldEmail AND PhoneNumber = @OldPhoneNumber";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NewFullName", newUser.Name);
                        command.Parameters.AddWithValue("@NewTaxID", newUser.TaxId);
                        command.Parameters.AddWithValue("@NewEmail", newUser.Email);
                        command.Parameters.AddWithValue("@NewPhoneNumber", newUser.PhoneNumber);
                        command.Parameters.AddWithValue("@LastModifiedDate", newUser.Updated);

                        command.Parameters.AddWithValue("@OldFullName", oldUser.Name);
                        command.Parameters.AddWithValue("@OldTaxID", oldUser.TaxId);
                        command.Parameters.AddWithValue("@OldEmail", oldUser.Email);
                        command.Parameters.AddWithValue("@OldPhoneNumber", oldUser.PhoneNumber);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
        public void DeleteUser(User user)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM users WHERE fullname = @fullname AND phonenumber = @phonenumber AND email = @email AND taxid = @taxid";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@fullname", user.Name);
                        command.Parameters.AddWithValue("@phonenumber", user.PhoneNumber);
                        command.Parameters.AddWithValue("@email", user.Email);
                        command.Parameters.AddWithValue("@taxid", user.TaxId);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}

