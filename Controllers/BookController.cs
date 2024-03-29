
using Bookstore.Data;
using Bookstore.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Bookstore.Controllers{

    public class BookController : Controller{

        private readonly ApplicationDbContext _context;
         private readonly IWebHostEnvironment _webHostEnvironment;
        
        public BookController(ApplicationDbContext context, IWebHostEnvironment _webHost)
        {
            _context = context;
            _webHostEnvironment = _webHost;
        }

        [Authorize]
        public IActionResult Index(int page = 1 )
        {   
            if(page<1)page=1;
            var RealPage = page - 1;
            try{
                var Books = (_context.Books!=null) ? 
                                _context.Books.Include(b=>b.Category)
                                        .Include(b=>b.Borrowings)
                                                // .ToList()
                                                    .OrderBy(b=>b.Id)
                                                        .Skip((RealPage*10))
                                                            .Take(10)
                                                                .ToList() : new List<Book>();
                setPaginationConfig(page,10.0);

                return View(Books);
            }catch(Exception){
                return View();
            }
        }
        [Authorize]
        public IActionResult Create()
        {
            var categories = _context.Categories.ToList();
            ViewData["categories"] = categories;
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Book book)
        {
            
            
            try{
                if(ModelState.IsValid)
                {
                    if(book.CoverFile != null && book.CoverFile.Length > 0)
                    {
                        // Salvar o arquivo no servidor ou em um serviço de armazenamento (por exemplo, Azure Blob Storage)
                        // Aqui, você pode usar um método para salvar o arquivo e obter o caminho do arquivo.
                        string? name = await SaveFileAndGetPath(book.CoverFile);
                        if(name == null){
                            TempData["ErrorMessage"] = "Ficheiro não suportado.";
                            return RedirectToAction("Create");
                        }
                        book.Cover = name; 
                    }

                    _context.Books.Add(book);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Livro Adicionado";
                    return RedirectToAction("Index");
                }else{
                    TempData["ErrorMessage"] = "Não foi possível adicionar  o livro, tente novamente.";
                    return View();
                }
            }catch(Exception)
            {
                TempData["ErrorMessage"] = "Não foi possível adicionar  o livro, tente novamente.";
                return View();
                
            }
        }

        [Authorize]
        public IActionResult Details(int? id)
        {
            if(id == null)
            return NotFound();

            try{
                var Book = _context.Books.Include(b=>b.Borrowings).Include(b=>b.Category).ToList().Find(b=> b.Id == id);
                if(Book != null)
                {
                    return View(Book);
                }
                TempData["ErrorMessage"] = "Livro nao encontrado.";
                return RedirectToAction("Index");
            }catch(Exception)
            {
                TempData["ErrorMessage"] = "Não foi possível adicionar  o livro, tente novamente.";
                return View();
            }
        }

        [Authorize]
        public IActionResult Edit(int? id)
        {
            if(id == null)
            return NotFound();

            try{
                var Book = _context.Books.Include(b=>b.Borrowings).Include(b=>b.Category).ToList().Find(b=> b.Id == id);
                if(Book != null)
                {
                    var categories = _context.Categories.ToList();
                    ViewData["categories"] = categories;
                    return View(Book);
                }
                TempData["ErrorMessage"] = "Livro nao encontrado.";
                return RedirectToAction("Index");
            }catch(Exception)
            {
                TempData["ErrorMessage"] = "Ocorreu um erro, tente novamente.";
                return View();
            }
        }


         [HttpPost]
         [Authorize]
        public async Task<IActionResult> Edit(int id,Book book)
        {
            
            
            try{
                if(ModelState.IsValid)
                {
                    if(book.CoverFile != null && book.CoverFile.Length > 0)
                    {
                        // Salvar o arquivo no servidor ou em um serviço de armazenamento (por exemplo, Azure Blob Storage)
                        // Aqui, você pode usar um método para salvar o arquivo e obter o caminho do arquivo.
                        string? name = await SaveFileAndGetPath(book.CoverFile);
                        if(name == null){
                            TempData["ErrorMessage"] = "Ficheiro não suportado.";
                            return RedirectToAction("Edit",id);
                        }
                        book.Cover = name; 
                    }

                    _context.Books.Update(book);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Livro Actualizado";
                    return RedirectToAction("Edit",id);
                }else{
                    TempData["ErrorMessage"] = "Não foi possível actualizar  o livro, tente novamente.";
                    return View();
                }
            }catch(Exception)
            {
                TempData["ErrorMessage"] = "Não foi possível actualizar  o livro, tente novamente.";
                return View();
                
            }
        }


        [HttpPost]
        [Authorize]
        public IActionResult Delete(int? id)
        {

            try{
                if(id == null)return NotFound();
                var Book = _context.Books.Find(id);

                if (Book == null) return NotFound();
                _context.Books.Remove(Book);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Livro Removido";
                return RedirectToAction("Index");
            }catch(Exception e)
            {
                TempData["ErrorMessage"] = "Ocorreu um problema tente outra vez";
                return RedirectToAction("Index");
            }

        }

        // HELPERS

        private async Task<string?> SaveFileAndGetPath(IFormFile file)
        {
            string [] AllowedTypes = new string[]{"image/jpeg","image/gif","image/jpg","image/png"};
                // Implemente a lógica para salvar o arquivo no servidor ou em um serviço de armazenamento
                // Retorne o caminho do arquivo após o salvamento

                // Exemplo básico: Salvar o arquivo no diretório wwwroot/uploads
                if(!AllowedTypes.Contains(file.ContentType)){
                    
                    return null;
                }
                Console.WriteLine("CONTENT TYPE: "+file.ContentType);
                var uploadsDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsDirectory))
                Directory.CreateDirectory(uploadsDirectory);

                var fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                var filePath = Path.Combine(uploadsDirectory, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Retorna o caminho do arquivo salvo
                return "/uploads/" + fileName;
        }

        private void setPaginationConfig(int page, double size)
        {
                double pages = _context.Books.Count() / size;
                ViewData["total_pages"] = Math.Ceiling(pages);
                Console.WriteLine("TOTAL PAGES = " +ViewData["total_pages"] );
                ViewData["actual_page"] = page;
                ViewData["next_page"] = page + 1;
                ViewData["prev_page"] = page==1?page:page - 1;
        }
    }
}