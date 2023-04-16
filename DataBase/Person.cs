﻿using System;
using System.Data;
using System.Data.SqlClient;

namespace DataBase
{
    public class Person
    {
        public int id { get; set; }
        public string name { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string address { get; set; }
        public string numberAddress { get; set; }
        public string phone { get; set; }
        public decimal income { get; set; }
        public decimal help { get; set; }
        public int numberOfMembers { get; set; }

        public void Save()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(DbConnectionString.connectionString))
                {
                    string sql = id == 0
                        ? "INSERT INTO Persons (name, CPF, RG, address, number_address, phone, income, help, number_of_members) VALUES (@name, @CPF, @RG, @address, @number_address, @phone, @income, @help, @number_of_members); SELECT @@identity"
                        : "UPDATE Persons SET name = @name, CPF = @CPF, RG = @RG, address = @address, number_address = @number_address, phone = @phone, income = @income, help = @help, number_of_members = @number_of_members WHERE id = @id";

                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@CPF", CPF);
                    command.Parameters.AddWithValue("@RG", RG);
                    command.Parameters.AddWithValue("@address", address);
                    command.Parameters.AddWithValue("@number_address", numberAddress);
                    command.Parameters.AddWithValue("@phone", phone);
                    command.Parameters.AddWithValue("@income", income);
                    command.Parameters.AddWithValue("@help", help);
                    command.Parameters.AddWithValue("@number_of_members", numberOfMembers);
                    try
                    {
                        if (id == 0)
                            id = Convert.ToInt32(command.ExecuteScalar());
                        else
                            command.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public void Delete()
        {
            using (SqlConnection connection = new SqlConnection(DbConnectionString.connectionString))
            {
                string sql = "DELETE FROM Persons WHERE id = @id";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);
                command.CommandText = sql;
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }

        public DataTable FindById()
        {
            try
            {
                using (var connection = new SqlConnection(DbConnectionString.connectionString))
                {
                    string sql = $"SELECT * FROM Persons WHERE Persons.id = {id}";
                    var adapter = new SqlDataAdapter(sql, connection);
                    adapter.SelectCommand.CommandText = sql;
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch
            {
                throw;
            }
        }

        public DataTable FindByCPF(string CPF)
        {
            try
            {
                using (var connection = new SqlConnection(DbConnectionString.connectionString))
                {
                    string sql = $"SELECT * FROM Persons WHERE CPF = '{CPF}'";
                    var adapter = new SqlDataAdapter(sql, connection);
                    adapter.SelectCommand.CommandText = sql;
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch
            {
                throw;
            }
        }

        public DataTable FindByCpfForPerson(string CPF, int person_id)
        {
            try
            {
                using (var connection = new SqlConnection(DbConnectionString.connectionString))
                {
                    string sql = $"SELECT * FROM Persons WHERE CPF = '{CPF}' AND id <> {person_id}";
                    var adapter = new SqlDataAdapter(sql, connection);
                    adapter.SelectCommand.CommandText = sql;
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch
            {
                throw;
            }
        }

        public DataTable FindAll()
        {
            try
            {
                using (var connection = new SqlConnection(DbConnectionString.connectionString))
                {
                    string sql = "SELECT * FROM Persons";
                    var adapter = new SqlDataAdapter(sql, connection);
                    adapter.SelectCommand.CommandText = sql;
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch
            {
                throw;
            }
        }

        public DataTable FindByNameOrAddress(string data, string column)
        {
            try
            {
                using (var connection = new SqlConnection(DbConnectionString.connectionString))
                {
                    string sql = $"SELECT * FROM Persons WHERE {column} LIKE '%{data}%'";
                    var adapter = new SqlDataAdapter(sql, connection);
                    adapter.SelectCommand.CommandText = sql;
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch
            {
                throw;
            }
        }

        public DataTable FindByPersonId(int person_id)
        {
            try
            {
                using (var connection = new SqlConnection(DbConnectionString.connectionString))
                {
                    string sql = $"SELECT persons.name, Persons.CPF, Persons.RG, Persons.address, Persons.number_address, Persons.phone, Persons.income, Persons.help, Persons.number_of_members, Benefits_Received.description, Convert(VARCHAR, Benefits_Received.date_benefit, 103) AS date_benefit , Benefits_Received.person_id FROM Benefits_Received INNER JOIN Persons ON Persons.id = Benefits_Received.person_id WHERE Benefits_Received.person_id = {person_id}";
                    var adapter = new SqlDataAdapter(sql, connection);
                    adapter.SelectCommand.CommandText = sql;
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch
            {
                throw;
            }
        } 
        
        public DataTable FindAllPersonAndBenefits() { 
            try
            {
                using (var connection = new SqlConnection(DbConnectionString.connectionString))
                {
                    string sql = $"SELECT persons.name, Persons.CPF, Persons.RG, Persons.address, Persons.number_address, Persons.phone, Persons.income, Persons.help, Persons.number_of_members, Benefits_Received.description, Convert(VARCHAR, Benefits_Received.date_benefit, 103) AS date_benefit , Benefits_Received.person_id FROM Benefits_Received INNER JOIN Persons ON Persons.id = Benefits_Received.person_id";
                    var adapter = new SqlDataAdapter(sql, connection);
                    adapter.SelectCommand.CommandText = sql;
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch
            {
                throw;
            }
        }
        
        public DataTable FindAllPersonAndBenefitsByNameOrAddress(string data, string column) { 
            try
            {
                using (var connection = new SqlConnection(DbConnectionString.connectionString))
                {
                    string sql = $"SELECT persons.name, Persons.CPF, Persons.RG, Persons.address, Persons.number_address, Persons.phone, Persons.income, Persons.help, Persons.number_of_members, Benefits_Received.description, Convert(VARCHAR, Benefits_Received.date_benefit, 103) AS date_benefit , Benefits_Received.person_id FROM Benefits_Received INNER JOIN Persons ON Persons.id = Benefits_Received.person_id where {column} LIKE '%{data}%'";
                    var adapter = new SqlDataAdapter(sql, connection);
                    adapter.SelectCommand.CommandText = sql;
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}