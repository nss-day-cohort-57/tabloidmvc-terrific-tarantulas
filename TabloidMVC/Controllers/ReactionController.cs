using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class ReactionController : Controller
    {

        private readonly IReactionRepository _reactionRepository;

        public ReactionController(IReactionRepository reactionRepository)
        {
            _reactionRepository = reactionRepository;
        }


        // GET: ReactionController
        public ActionResult Index()
        {
            var reactions = _reactionRepository.GetAll();
            return View(reactions);
        }

        // GET: ReactionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReactionController/Create
        public ActionResult Create()
        {
            Reaction reaction = new Reaction();
            return View(reaction);
        }

        // POST: ReactionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reaction created)
        {
            try
            {
                _reactionRepository.Add(created);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReactionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReactionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReactionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReactionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
