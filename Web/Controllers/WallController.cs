using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TDDInlämningsuppgift;
using TDDInlämningsuppgift.Data;
using TDDInlämningsuppgift.Model;
using Web.Dto;

namespace Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WallController : ControllerBase
    {
        public ApplicationDb database = new ApplicationDb();
        public SocialNetworkEngine engine => new SocialNetworkEngine(database);
        [HttpGet]
        [Route("getusers")]
        public async Task<List<PostDto>> GetUsers()
        {
            var users = engine.ViewUserWall("Alice");
            var postDtos = new List<PostDto>();

            foreach (var user in users)
            {
                var postDto = new PostDto
                {
                    //Message = user.Posters.FirstOrDefault().Message,
                    //Created = user.Posters.FirstOrDefault().Date,
                    User = new UserDto
                    {
                        Id = user.Id,
                        Name = user.Username
                    }
                };

                postDtos.Add(postDto);
            }
            return postDtos;
        }
    }
}
