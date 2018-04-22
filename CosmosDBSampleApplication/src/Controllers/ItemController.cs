namespace todo.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Models;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using System.Linq;

    public class ItemController : Controller
    {
        [ActionName("Index")]
        public async Task<ActionResult> IndexAsync()
        {
            var items = await DocumentDBRepository<Item>.GetItemsAsync(d => !d.Completed);
            return View(items);
        }

        [ActionName("KendoRead")]
        public async Task<ActionResult> KendoRead()
        {
            var items = await DocumentDBRepository<Item>.GetItemsAsync(d => !d.Completed);

            return this.Json(items, JsonRequestBehavior.AllowGet);
        }

        [ActionName("KendoCreate")]
        public async Task<ActionResult> KendoCreate(string models)
        {
            var items = JsonConvert.DeserializeObject<IEnumerable<Item>>(models);
            

            if (items != null && ModelState.IsValid)
            {
                Item item = items.FirstOrDefault();
                await DocumentDBRepository<Item>.CreateItemAsync(item);
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [ActionName("KendoUpdate")]
        public async Task<ActionResult> KendoUpdate(string models)
        {
            var items = JsonConvert.DeserializeObject<IEnumerable<Item>>(models);

            if (items != null && ModelState.IsValid)
            {
                Item item = items.FirstOrDefault();
                await DocumentDBRepository<Item>.UpdateItemAsync(item.Id, item);
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [ActionName("KendoDestroy")]
        public async Task<ActionResult> KendoDestroy(string models)
        {
            var items = JsonConvert.DeserializeObject<IEnumerable<Item>>(models);

            if (items != null && ModelState.IsValid)
            {
                Item item = items.FirstOrDefault();
                await DocumentDBRepository<Item>.DeleteItemAsync(item.Id, item.Category);
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

#pragma warning disable 1998
        [ActionName("Create")]
        public async Task<ActionResult> CreateAsync()
        {
            return View();
        }
#pragma warning restore 1998

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Id,Name,Description,Completed,Category")] Item item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<Item>.CreateItemAsync(item);

                var items = await DocumentDBRepository<Item>.GetItemsAsync(d => !d.Completed);
                return this.Json(items, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index");
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync([Bind(Include = "Id,Name,Description,Completed,Category")] Item item)
        {
            if (ModelState.IsValid)
            {
                await DocumentDBRepository<Item>.UpdateItemAsync(item.Id, item);

                var items = await DocumentDBRepository<Item>.GetItemsAsync(d => !d.Completed);
                return this.Json(items, JsonRequestBehavior.AllowGet);
                //return RedirectToAction("Index");
            }

            return View(item);
        }

        [ActionName("Edit")]
        public async Task<ActionResult> EditAsync(string id, string category)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Item item = await DocumentDBRepository<Item>.GetItemAsync(id, category);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        [ActionName("Delete")]
        public async Task<ActionResult> DeleteAsync(string id, string category)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Item item = await DocumentDBRepository<Item>.GetItemAsync(id, category);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedAsync([Bind(Include = "Id, Category")] string id, string category)
        {
            await DocumentDBRepository<Item>.DeleteItemAsync(id, category);
            return RedirectToAction("Index");
        }

        [ActionName("Details")]
        public async Task<ActionResult> DetailsAsync(string id, string category)
        {
            Item item = await DocumentDBRepository<Item>.GetItemAsync(id, category);
            return View(item);
        }
    }
}