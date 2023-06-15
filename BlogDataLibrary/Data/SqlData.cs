﻿using BlogDataLibrary.Database;
using BlogDataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDataLibrary.Data
{
    public interface ISqlData
    {
        void AddPost(PostModel post);
        UserModel Authenticate(string username, string password);
        List<ListPostModel> ListPosts();
        void Register(string username, string firstname, string lastname, string password);
        ListPostModel ShowPostDetails(int id);
    }

    public class SqlData : ISqlData
    {

        private ISqlDataAccess _db;
        private const string connectionStringName = "SqlDb";
        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }
        public UserModel Authenticate(string username, string password)
        {
            UserModel result = _db.LoadData<UserModel, dynamic>("dbo.spUsers_Authenticate",
                                                               new { username, password },
                                                                connectionStringName,
                                                                true).FirstOrDefault();

            return result;
        }

        public void Register(string username, string firstname, string lastname, string password)
        {
            _db.SaveData<dynamic>(
                "dbo.spUsers_Register",
                new { username, firstname, lastname, password },
                connectionStringName,
                true);
        }

        public void AddPost(PostModel post)
        {
            _db.SaveData("spPosts_Insert", new { post.UserId, post.Title, post.Body, post.DateCreated },
                connectionStringName, true);
        }

        public List<ListPostModel> ListPosts()
        {
            return _db.LoadData<ListPostModel, dynamic>("dbo.spPosts_List", new { }, connectionStringName, true).ToList();
        }


        public ListPostModel ShowPostDetails(int id)
        {
            return _db.LoadData<ListPostModel, dynamic>("dbo.spPosts_Detail", new { id }, connectionStringName, true).FirstOrDefault();
        }
    }



}