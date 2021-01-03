﻿using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Website.Models.Account;

namespace Website.Repository
{
    public class AccountRepository : IAccountRepository
    {

        private readonly IConfiguration _config;

        public AccountRepository(IConfiguration config)
        {
            _config = config;
        }

        // Open up a type, once we do establish a connection to sql server
        public async Task<IdentityResult> CreateAsync(ApplicationUserIdentity user, CancellationToken cancellationToken)
        {
            
            cancellationToken.ThrowIfCancellationRequested();

            var dataTable = new DataTable();
            dataTable.Columns.Add("Username" , typeof(string));
            dataTable.Columns.Add("NormalizedUsername", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("NormalizedEmail", typeof(string));
            dataTable.Columns.Add("Fullname", typeof(string));
            dataTable.Columns.Add("PasswordHash", typeof(string));

            dataTable.Rows.Add(
                user.Username,
                user.NormalizedUsername,
                user.Email,
                user.NormalizedEmail,
                user.Fullname,
                user.PasswordHash
                );

            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConection")))
            {
                // if asp.net core says we should cancel it then go with it
                await connection.OpenAsync(cancellationToken);
                
                // continue with inserting new account
                await connection.ExecuteAsync("Account_Insert", new { Account = dataTable.AsTableValuedParameter("dbo.AccountType")} , commandType: CommandType.StoredProcedure);
            }
            // return success code
            return IdentityResult.Success;
        }

        public async Task<ApplicationUserIdentity> GetByUsernameAsync(string normalizedUsername, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            ApplicationUserIdentity applicationUser;
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                applicationUser = await connection.QuerySingleOrDefaultAsync<ApplicationUserIdentity>(
                    "Account_GetByusername", new { NormalizedUsername = normalizedUsername },
                    commandType: CommandType.StoredProcedure
                    );
            }

            return applicationUser;

        }
    }
}
