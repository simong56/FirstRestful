using BlogApi.Models;
using BlogApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace BlogApi.Controllers
{ 
    [Route("blogger")]
    [ApiController]
    public class BloggerController : ControllerBase
    {
        public string ConnectionString = "server=localhost;database=blog;uid=root;password=";

        [Route("allBloggers")]
        [HttpGet]
        public object GetAllBloggers()
        {
            List<Blogger> bloggers = new List<Blogger>();
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = @"SELECT * FROM `blogger`";
            var cmd = new MySqlCommand(sql, connector);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var blogger = new Blogger
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    Age = reader.GetInt32(3),
                    Password = reader.GetString(4),
                    RegistrationTime = reader.GetDateTime(5)
                };
                bloggers.Add(blogger);
            }

            connector.Close();
            return new { response = bloggers };
        }

        [Route("bloggerById")]
        [HttpGet]
        public object GetBloggerById(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = @"SELECT * FROM `blogger` WHERE `id` = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            var reader = cmd.ExecuteReader();
            reader.Read();
            var blogger = new Blogger
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Age = reader.GetInt32(3),
                Password = reader.GetString(4),
                RegistrationTime = reader.GetDateTime(5)
            };
            connector.Close();
            return new { response = blogger };
        }

        [Route("addNewBlogger")]
        [HttpPost]
        public object AddNewBlogger(AddNewBloggerDto addNewBloggerDto)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql =
                @"INSERT INTO `blogger`(`name`, `email`, `age`, `password`, `registrationTime`) VALUES (@name,@email,@age,@password,@registrationTime)";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@name", addNewBloggerDto.Name);
            cmd.Parameters.AddWithValue("@email", addNewBloggerDto.Email);
            cmd.Parameters.AddWithValue("@age", addNewBloggerDto.Age);
            cmd.Parameters.AddWithValue("@password", addNewBloggerDto.Password);
            cmd.Parameters.AddWithValue("@registrationTime", DateTime.Now);
            cmd.ExecuteNonQuery();
            connector.Close();
            return new { result = addNewBloggerDto };
        }

        [Route("loginWithEmail")]
        [HttpPost]
        public object LoginWithEmail(LoginWithEmailDto loginWithEmailDto)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = @"SELECT * FROM `blogger` WHERE `email` = @email AND `password` = @password";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@email", loginWithEmailDto.Email);
            cmd.Parameters.AddWithValue("@password", loginWithEmailDto.Password);
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var bloggerId = reader.GetInt32(0);
                connector.Close();
                return new { success = true, result = bloggerId };
            }

            connector.Close();
            return new { success = false };
        }

        [Route("deleteBlogger")]
        [HttpDelete]
        public object DeleteBlogger(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = @"DELETE FROM `blogger` WHERE `id` = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            int rowsAffected = cmd.ExecuteNonQuery();

            return new { success = rowsAffected > 0, rowsAffected };
        }

        [Route("updateBlogger")]
        [HttpPut]
        public object UpdateBlogger(UpdateBloggerDto updateBloggerDto)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql =
                @"UPDATE `blogger` SET `name`=@name,`email`=@email,`age`=@age,`password`=@password WHERE `id` = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@name", updateBloggerDto.Name);
            cmd.Parameters.AddWithValue("@email", updateBloggerDto.Email);
            cmd.Parameters.AddWithValue("@age", updateBloggerDto.Age);
            cmd.Parameters.AddWithValue("@password", updateBloggerDto.Password);
            cmd.Parameters.AddWithValue("@id", updateBloggerDto.IdToUpdate);
            int affectedRows = cmd.ExecuteNonQuery();

            if (affectedRows > 0)
            {
                return new { success = true, result = updateBloggerDto };
            }

            return new { success = false };

        }

        [Route("getBloggerCount")]
        [HttpGet]
        public object GetBloggerCount()
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = @"SELECT COUNT(*) FROM `blogger`";
            var cmd = new MySqlCommand(sql, connector);
            var reader = cmd.ExecuteReader();
            reader.Read();
            return reader.GetInt32(0);
        }

        [Route("getAllAlphabetical")]
        [HttpGet]
        public object GetAllAlphabetical()
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            List<object> bloggers = new List<object>();
            string sql = @"SELECT `name`, `email` FROM `blogger` ORDER BY email;";
            var cmd = new MySqlCommand(sql, connector);

            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var blogger = new
                {
                    Name = reader.GetString(0),
                    Email = reader.GetString(1)
                };
                bloggers.Add(blogger);
            }

            return bloggers;
        }

        [Route("getPostsOfBlogger")]
        [HttpGet]
        public object GetPostsOfBlogger(int bloggerId)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            List<Blogpost> blogposts = new List<Blogpost>();
            string sql = @"SELECT * FROM `blogpost` WHERE `bloggerId` = @bloggerId;";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@bloggerId", bloggerId);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var blogpost = new Blogpost
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Content = reader.GetString(2),
                    PostTime = reader.GetDateTime(3),
                    UpdateTime = reader.GetDateTime(4),
                    BloggerId = reader.GetInt32(5)
                };
                blogposts.Add(blogpost);
            }
            return new { response = blogposts };
        }

        [Route("getPostCountOfBlogger")]
        [HttpGet]
        public object GetPostCountOfBlogger(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = @"SELECT COUNT(*) FROM `blogpost` WHERE `bloggerId` = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            var reader = cmd.ExecuteReader();
            reader.Read();
            return new
            {
                response = reader.GetInt32(0)
            };
        }
    }

    [Route("blogpost")]
    [ApiController]
    public class BlogpostController : ControllerBase
    {
        public string ConnectionString = "server=localhost;database=blog;uid=root;password=";

        [Route("allBlogposts")]
        [HttpGet]
        public object GetAllBlogposts()
        {
            List<Blogpost> blogposts = new List<Blogpost>();
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = @"SELECT * FROM `blogpost`";
            var cmd = new MySqlCommand(sql, connector);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var blogpost = new Blogpost
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Content = reader.GetString(2),
                    PostTime = reader.GetDateTime(3),
                    UpdateTime = reader.GetDateTime(4),
                    BloggerId = reader.GetInt32(5)
                };
                blogposts.Add(blogpost);
            }
            
            connector.Close();
            return new { response = blogposts };
        }

        [Route("blogpostById")]
        [HttpGet]
        public object GetBlogpostById(int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = @"SELECT * FROM `blogpost` where `id` = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            var reader = cmd.ExecuteReader();
            reader.Read();
            var blogpost = new Blogpost
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Content = reader.GetString(2),
                PostTime = reader.GetDateTime(3),
                UpdateTime = reader.GetDateTime(4),
                BloggerId = reader.GetInt32(5)
            };
            connector.Close();
            return new { response = blogpost };
        }

        [Route("getBlogpostCount")]
        [HttpGet]
        public object GetBlogpostCount()
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = @"SELECT COUNT(*) FROM `blogpost`";
            var cmd = new MySqlCommand(sql, connector);
            var reader = cmd.ExecuteReader();
            reader.Read();
            return new
            {
                response = reader.GetInt32(0)
            };
        }

        [Route("addNewBlogpost")]
        [HttpPost]
        public object AddNewBlogpost(AddNewBlogpostDto addNewBlogpostDto)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            string sql = @"INSERT INTO `blogpost`(`title`, `content`, `bloggerId`) VALUES (@title,@content,@bloggerId)";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@title", addNewBlogpostDto.Title);
            cmd.Parameters.AddWithValue("@content", addNewBlogpostDto.Content);
            cmd.Parameters.AddWithValue("@bloggerId", addNewBlogpostDto.BloggerId);
            cmd.ExecuteNonQuery();
            connector.Close();
            return new { result = addNewBlogpostDto };
        }

        
    }

}
