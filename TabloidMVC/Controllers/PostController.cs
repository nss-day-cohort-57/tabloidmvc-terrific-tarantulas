using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IReactionRepository _reactionRepository;

        public PostController(IPostRepository postRepository, IReactionRepository reactionRepository, ICategoryRepository categoryRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _reactionRepository = reactionRepository;
        }

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

        public IActionResult MyPosts(int userProfileId)
        {
            userProfileId = GetCurrentUserProfileId();
            var post = _postRepository.GetCurrentUsersPosts(userProfileId);
            return View(post);
        }


        public IActionResult React(int id)
        {
            var post = _postRepository.GetPostById(id);

            if (post == null)
                return NotFound();

            PostReactionViewModel vm = new PostReactionViewModel()
            {
                allReactions = _reactionRepository.GetAll(),
                postReaction = new PostReaction()
                {
                    PostId = post.Id,
                    UserProfileId = GetCurrentUserProfileId()
                }
            };
            return View(vm);
        }


        [HttpPost]
        public IActionResult React(int id, int reactId)
        {
            var userId = GetCurrentUserProfileId();
            _postRepository.AddPostReaction(new PostReaction() { PostId = id,ReactionId = reactId,UserProfileId = userId});
            return RedirectToAction("Details", new { id = id });
        }


        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }

            var reactions = _reactionRepository.GetReactionsByPost(id);

            PostViewModel pvm = new PostViewModel()
            {
                post = post,
                reactions = reactions,
                currentuserId = GetCurrentUserProfileId()

        };

            return View(pvm);
        }

        public IActionResult Delete(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);
            return View(post);
        }

        [HttpPost]
        public IActionResult Delete(int id, Post post)
        {
            try
            {
                _postRepository.Delete(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(post);
            }
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = false;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            } 
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        public IActionResult Edit(int id)
        {
            Post post = _postRepository.GetPostById(id);

            if (post == null)
            {
                return NotFound();
            }

            List<Category> categories = _categoryRepository.GetAll();

            PostCreateViewModel vm = new PostCreateViewModel()
            {
                Post = post,
                CategoryOptions = categories
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PostCreateViewModel vm)
        {
            try
            {
                _postRepository.UpdatePost(vm.Post);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(vm);
            }
        }
    }
}
