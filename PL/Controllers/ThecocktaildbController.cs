using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace PL.Controllers
{
    public class ThecocktaildbController : Controller
    {
        [HttpGet]
        public ActionResult GetCocktail()
        {
            
            ML.Result resultProducto = new ML.Result();
            resultProducto.Objects = new List<Object>();
            ML.Cocktail cocktail = new ML.Cocktail();
            string s = cocktail.Nombre;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://www.thecocktaildb.com/api/json/");

                var responseTask = client.GetAsync("v1/1/search.php?s=" + s);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Cocktail>();
                    readTask.Wait();


                    foreach (var resultItem in readTask.Result.drinks)
                    {
                        ML.Cocktail resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cocktail>(resultItem.ToString());

                        resultProducto.Objects.Add(resultItemList);
                    }

                }
            }
           
            cocktail.drinks = resultProducto.Objects;//Mandar a llamar a la vista, mostrar la vista(HTML)
            return View(cocktail);
        }
        [HttpPost]
        public ActionResult GetCocktail(ML.Cocktail cocktail)
        {

            ML.Result resultProducto = new ML.Result();
            resultProducto.Objects = new List<Object>();

            string n = cocktail.Nombre;
            string i = cocktail.Ingrediente;
            if(i == null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://www.thecocktaildb.com/api/json/");

                    var responseTask = client.GetAsync("v1/1/search.php?s=" + n);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<ML.Cocktail>();
                        readTask.Wait();


                        foreach (var resultItem in readTask.Result.drinks)
                        {
                            ML.Cocktail resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cocktail>(resultItem.ToString());

                            resultProducto.Objects.Add(resultItemList);
                        }

                    }
                }

            }
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://www.thecocktaildb.com/api/json/");

                    var responseTask = client.GetAsync("v1/1/filter.php?i=" + i);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<ML.Cocktail>();
                        readTask.Wait();


                        foreach (var resultItem in readTask.Result.drinks)
                        {
                            ML.Cocktail resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cocktail>(resultItem.ToString());

                            resultProducto.Objects.Add(resultItemList);
                        }

                    }
                }

            }

            cocktail.drinks = resultProducto.Objects;
            return View(cocktail);
        }
    }
}